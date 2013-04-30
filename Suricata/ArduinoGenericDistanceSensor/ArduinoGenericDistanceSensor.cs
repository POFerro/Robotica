using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using Microsoft.Dss.Services.SubscriptionManager;
using System.Runtime.InteropServices;

using analogsensor = Microsoft.Robotics.Services.AnalogSensor.Proxy;
using sonar = Microsoft.Robotics.Services.Sonar.Proxy;
using infrared = Microsoft.Robotics.Services.Infrared.Proxy;

using Arduino.Firmata.Types.Proxy;
using System.Threading;

namespace ArduinoGenericDistanceSensor
{
    [Contract(Contract.Identifier)]
    [DisplayName("Arduino Generic Distance Sensor")]
	[AlternateContract(analogsensor.Contract.Identifier)]
	[AlternateContract(sonar.Contract.Identifier)]
	public class ArduinoGenericDistanceSensorService : DsspServiceBase
    {
        #region Service
        /// <summary>
        /// Service state
        /// </summary>
        [ServiceState]
        [InitialStatePartner(Optional = false, ServiceUri = "ArduinoGenericDistanceSensorService.xml")]
        ArduinoGenericDistanceSensorState _state = new ArduinoGenericDistanceSensorState();

        /// <summary>
        /// Main service port
        /// </summary>
        [ServicePort("/ArduinoGenericDistanceSensor", AllowMultipleInstances = true)]
        ArduinoGenericDistanceSensorOperations _mainPort = new ArduinoGenericDistanceSensorOperations();

		[AlternateServicePort(AlternateContract = analogsensor.Contract.Identifier)]
		analogsensor.AnalogSensorOperations _analogSensorPort = new analogsensor.AnalogSensorOperations();

		[AlternateServicePort(AlternateContract = sonar.Contract.Identifier)]
		sonar.SonarOperations _sonarPort = new sonar.SonarOperations();

		[AlternateServicePort(AlternateContract = infrared.Contract.Identifier)]
		infrared.InfraredOperations _infraredPort = new infrared.InfraredOperations();
		
		[SubscriptionManagerPartner]
        SubscriptionManagerPort _submgrPort = new SubscriptionManagerPort();

        /// <summary>
        /// ArduinoService partner
        /// </summary>
        [Partner("ArduinoService", Contract = Arduino.Proxy.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry, Optional = true)]
        Arduino.Proxy.ArduinoOperations _arduinoServicePort = new Arduino.Proxy.ArduinoOperations();
        Arduino.Proxy.ArduinoOperations _arduinoServiceNotify = new Arduino.Proxy.ArduinoOperations();

        Sensors.ISensorBase _MainSensor = null;

        #endregion

        /// <summary>
        /// Service constructor
        /// </summary>
        public ArduinoGenericDistanceSensorService(DsspServiceCreationPort creationPort)
            : base(creationPort)
        {
        }

        /// <summary>
        /// Service start
        /// </summary>
        protected override void Start()
        {
			if (_state != null)
			{
				if (_state.SensorModel == Sensors.SensorsModels.IR_Sharp_GP2Y0A21YK0F)
				{
					var sensor = new Sensors.IRSensors.Sharp_GP2Y0A21YK0F();
					_MainSensor = (Sensors.ISensorBase)sensor;
				}

				if (_state.SensorModel == Sensors.SensorsModels.Sonar_HC_SR)
				{
					var sensor = new Sensors.SonarSensors.GenericSonar();
					_MainSensor = (Sensors.ISensorBase)sensor;
				}
			}

			base.Start();
			
			_arduinoServicePort.Subscribe(_arduinoServiceNotify, typeof(Arduino.Messages.Proxy.AnalogOutputUpdate), typeof(Arduino.Messages.Proxy.DigitalOutputUpdate));
			MainPortInterleave.CombineWith(
				new Interleave(
					new TeardownReceiverGroup(),
					new ExclusiveReceiverGroup(),
					new ConcurrentReceiverGroup(
						Arbiter.Receive<Arduino.Messages.Proxy.DigitalOutputUpdate>(true, _arduinoServiceNotify, DigitalOutputUpdateHandler),
						Arbiter.Receive<Arduino.Messages.Proxy.AnalogOutputUpdate>(true, _arduinoServiceNotify, AnalogOutputUpdateHandler)
					)));

			_MainSensor.ConfigureArduino(this, _arduinoServicePort, _state);
        }

		private void DigitalOutputUpdateHandler(Arduino.Messages.Proxy.DigitalOutputUpdate message)
		{
			try
			{
				OnNewArduinoValueReport(message.Body.CurrentPin, message.Body.Value);
			}
			finally
			{
				message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
			}
		}

		private void OnNewArduinoValueReport(Pins currentPin, int value)
		{
			if (_MainSensor != null && _MainSensor.IsSensorAnalogPin(_state, currentPin))
			{
				dtUltNotif = DateTime.Now;
				_MainSensor.SetAnalogData(value);

				var messAnalog = new analogsensor.AnalogSensorState()
				{
					Pose = _state.Pose,
					TimeStamp = dtUltNotif,
					HardwareIdentifier = _state.HardwareIdentifier,
					RawMeasurementRange = (double)_MainSensor.GetDistanceMax() / 100,
					RawMeasurement = (double)_MainSensor.GetDistanceCurrent() / 100,
					NormalizedMeasurement = (double)_MainSensor.GetDistanceCurrent() / 100,
				};
				base.SendNotification(_submgrPort, new analogsensor.Replace(messAnalog));
			}
		}

		DateTime dtUltNotif = DateTime.MinValue;
        private void AnalogOutputUpdateHandler(Arduino.Messages.Proxy.AnalogOutputUpdate message)
        {
			try
			{
				OnNewArduinoValueReport(message.Body.CurrentPin, message.Body.Value);
			}
			finally
			{
				message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
			}
        }

		private analogsensor.AnalogSensorState SensorValueGet()
		{
			return new analogsensor.AnalogSensorState()
			{
				HardwareIdentifier = _state.HardwareIdentifier,
				NormalizedMeasurement = (double)_MainSensor.GetDistanceCurrent() / 100,
				RawMeasurement = (double)_MainSensor.GetDistanceCurrent() / 100,
				RawMeasurementRange = (double)_MainSensor.GetDistanceMax() / 100,
				TimeStamp = dtUltNotif,
				Pose = _state.Pose,
			};
		}

		private sonar.SonarState SonarValueGet()
		{
			var sensorState = SensorValueGet();
			return new sonar.SonarState()
			{
				DistanceMeasurement = sensorState.NormalizedMeasurement,
				HardwareIdentifier = _state.HardwareIdentifier,
				MaxDistance = sensorState.RawMeasurementRange,
				TimeStamp = sensorState.TimeStamp,
				Pose = _state.Pose,
			};
		}

		private infrared.InfraredState InfraredValueGet()
		{
			var sensorState = SensorValueGet();
			return new infrared.InfraredState()
			{
				DistanceMeasurement = sensorState.NormalizedMeasurement,
				HardwareIdentifier = _state.HardwareIdentifier,
				MaxDistance = sensorState.RawMeasurementRange,
				TimeStamp = sensorState.TimeStamp,
				Pose = _state.Pose,
				ManufacturerIdentifier = _MainSensor.GetName(),
				MinDistance = _MainSensor.GetDistanceMin()
			};
		}

		#region DSS Handlers
		/// <summary>
        /// Handles Subscribe messages
        /// </summary>
        /// <param name="subscribe">the subscribe request</param>
        [ServiceHandler]
        public void SubscribeHandler(Subscribe subscribe)
        {
            SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
        }

		[ServiceHandler(PortFieldName = "_analogSensorPort")]
		public void AnalogSensorGetHandler(analogsensor.Get get)
		{
			get.ResponsePort.Post(SensorValueGet());
		}

		[ServiceHandler(PortFieldName = "_sonarPort")]
		public void SonarGetHandler(sonar.Get get)
		{
			get.ResponsePort.Post(SonarValueGet());
		}

		[ServiceHandler(PortFieldName = "_infraredPort")]
		public void InfraredGetHandler(infrared.Get get)
		{
			get.ResponsePort.Post(InfraredValueGet());
		}

		[ServiceHandler(PortFieldName = "_analogSensorPort")]
		public void AnalogSensorHttpGetHandler(Microsoft.Dss.Core.DsspHttp.HttpGet httpget)
		{
			httpget.ResponsePort.Post(new Microsoft.Dss.Core.DsspHttp.HttpResponseType(SensorValueGet()));
		}

		[ServiceHandler(PortFieldName = "_sonarPort")]
		public void SonarHttpGetHandler(Microsoft.Dss.Core.DsspHttp.HttpGet httpget)
		{
			httpget.ResponsePort.Post(new Microsoft.Dss.Core.DsspHttp.HttpResponseType(SonarValueGet()));
		}

		[ServiceHandler(PortFieldName = "_infraredPort")]
		public void InfraredHttpGetHandler(Microsoft.Dss.Core.DsspHttp.HttpGet httpget)
		{
			httpget.ResponsePort.Post(new Microsoft.Dss.Core.DsspHttp.HttpResponseType(InfraredValueGet()));
		}

		[ServiceHandler(PortFieldName = "_analogSensorPort")]
		public void AnalogSensorReplaceHandler(analogsensor.Replace replace)
		{
			_state.HardwareIdentifier = replace.Body.HardwareIdentifier;
			_state.Pose = replace.Body.Pose;
			_MainSensor.ConfigureArduino(this, _arduinoServicePort, _state);

			replace.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		[ServiceHandler(PortFieldName = "_sonarPort")]
		public void SonarReplaceHandler(sonar.Replace replace)
		{
			_state.HardwareIdentifier = replace.Body.HardwareIdentifier;
			_state.Pose = replace.Body.Pose;
			_MainSensor.ConfigureArduino(this, _arduinoServicePort, _state);

			replace.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		[ServiceHandler(PortFieldName = "_infraredPort")]
		public void SonarReplaceHandler(infrared.Replace replace)
		{
			_state.HardwareIdentifier = replace.Body.HardwareIdentifier;
			_state.Pose = replace.Body.Pose;
			_MainSensor.ConfigureArduino(this, _arduinoServicePort, _state);

			replace.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		[ServiceHandler(PortFieldName = "_analogSensorPort")]
		public void AnalogSensorReliableSubscribeHandler(analogsensor.ReliableSubscribe reliablesubscribe)
		{
			SubscribeHelper(_submgrPort, reliablesubscribe.Body, reliablesubscribe.ResponsePort);
		}

		[ServiceHandler(PortFieldName = "_analogSensorPort")]
		public void AnalogSensorSubscribeHandler(analogsensor.Subscribe subscribe)
		{
			SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
		}

		[ServiceHandler(PortFieldName = "_sonarPort")]
		public void SonarReliableSubscribeHandler(sonar.ReliableSubscribe reliablesubscribe)
		{
			SubscribeHelper(_submgrPort, reliablesubscribe.Body, reliablesubscribe.ResponsePort);
		}

		[ServiceHandler(PortFieldName = "_sonarPort")]
		public void SonarSubscribeHandler(sonar.Subscribe subscribe)
		{
			SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
		}

		[ServiceHandler(PortFieldName = "_infraredPort")]
		public void SonarReliableSubscribeHandler(infrared.ReliableSubscribe reliablesubscribe)
		{
			SubscribeHelper(_submgrPort, reliablesubscribe.Body, reliablesubscribe.ResponsePort);
		}

		[ServiceHandler(PortFieldName = "_infraredPort")]
		public void SonarSubscribeHandler(infrared.Subscribe subscribe)
		{
			SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
		}

		#endregion
	}
}
