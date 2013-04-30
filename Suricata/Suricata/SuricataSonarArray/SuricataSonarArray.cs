//------------------------------------------------------------------------------
//  <copyright file="ReferencePlatform2011SonarArray.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation.  All rights reserved.
//  </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Microsoft.Ccr.Core;
using Microsoft.Dss.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Robotics.PhysicalModel.Proxy;
using W3C.Soap;
using analogarray = Microsoft.Robotics.Services.AnalogSensorArray.Proxy;
using analogsensor = Microsoft.Robotics.Services.AnalogSensor.Proxy;
using dssphttp = Microsoft.Dss.Core.DsspHttp;
using sonar = Microsoft.Robotics.Services.Sonar.Proxy;
using sonararray = Microsoft.Robotics.Services.SonarSensorArray.Proxy;
using sonarsensor = Microsoft.Robotics.Services.Sonar.Proxy;
using submgr = Microsoft.Dss.Services.SubscriptionManager;
using svcbase = Microsoft.Dss.ServiceModel.DsspServiceBase;

namespace POFerro.Robotics.Suricata.SuricataSonarArray
{
    /// <summary>
    /// IrSensor enumeration
    /// </summary>
    [Description("Sonar proximity sensors.")]
    public enum SonarSensors
    {
        /// <summary>
        /// Proximity sensor 1
        /// </summary>
        LeftSonarProximityInMeters,

        /// <summary>
        /// Proximity sensor 2
        /// </summary>
        RightSonarProximityInMeters,
        
        /// <summary>
        /// Total number of sensors in the enum
        /// </summary>
        /// <remarks>Must be last element in
        /// enum.</remarks>
        SensorCount,
    }

    /// <summary>
    /// Encapsulates a pair of sonars
    /// </summary>
    [Contract(Contract.Identifier)]
    [DisplayName("SuricataSonarArray")]
	[Description("Sonares da Suricata")]
    [AlternateContract(analogarray.Contract.Identifier)]
    [AlternateContract(sonararray.Contract.Identifier)]
    public class SuricataSonarArrayService : svcbase.DsspServiceBase
    {
		/// <summary>
		/// Pose of the left sonar sensor
		/// </summary>
		private readonly Pose LeftSonarPose = new Pose { Position = new Vector3(-0.16f, 0.15f, 0.12f), Orientation = new Quaternion(0, -0.07991469f, 0, 0.996801734f) };

		/// <summary>
		/// Pose of the right sonar sensor
		/// </summary>
		private readonly Pose RightSonarPose = new Pose { Position = new Vector3(.16f, 0.15f, 0.12f), Orientation = new Quaternion(0, 0.07991469f, 0, 0.996801734f) };
		
		/// <summary>
        /// Name of the alternate service port
        /// </summary>
        private const string AlternatePort = "altPort";

        /// <summary>
        /// Sensor ports
        /// </summary>
        private sonar.SonarOperations[] sensorPorts = new sonar.SonarOperations[(int)SonarSensors.SensorCount];

		/// <summary>
		/// Sensor states
		/// </summary>
		private sonar.SonarState[] sensorStates = new sonar.SonarState[(int)SonarSensors.SensorCount];

		[ServiceState]
		[InitialStatePartner(Optional = true, ServiceUri = ServicePaths.Store + "/SuricataSonarArray.config.xml")]
		SuricataIRArrayState _state = new SuricataIRArrayState();

		/// <summary>
        /// The main port
        /// </summary>
		[ServicePort("/SuricataSonarSensorArray", AllowMultipleInstances = true)]
        private analogarray.AnalogSensorOperations mainPort = new analogarray.AnalogSensorOperations();

        /// <summary>
        /// The alternate port
        /// </summary>
		[AlternateServicePort("/SuricataSonarArray", AlternateContract = sonararray.Contract.Identifier, AllowMultipleInstances = true)]
        private sonararray.SonarSensorOperations altPort = new sonararray.SonarSensorOperations();

        /// <summary>
        /// Subscription manager port
        /// </summary>
        [SubscriptionManagerPartner]
        private submgr.SubscriptionManagerPort subMgrPort = new submgr.SubscriptionManagerPort();
        
        /// <summary>
        /// Left sonar
        /// </summary>
        [Partner("SonarLeft", Contract = sonar.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
        private sonar.SonarOperations leftSonar = new sonar.SonarOperations();

        /// <summary>
        /// Right sonar
        /// </summary>
        [Partner("SonarRight", Contract = sonar.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
        private sonar.SonarOperations rightSonar = new sonar.SonarOperations();

        /// <summary>
        /// Constructs a ReferencePlatform2011SonarArrayService
        /// </summary>
        /// <param name="creationPort">The port of creation</param>
		public SuricataSonarArrayService(DsspServiceCreationPort creationPort)
            : base(creationPort)
        {
        }

        /// <summary>
        /// HttpGet AnalogSensor Handler
        /// </summary>
        /// <param name="httpGet">HTTP Get request</param>
        /// <returns>A CCR iterator</returns>
        [ServiceHandler(ServiceHandlerBehavior.Concurrent)]
        public virtual IEnumerator<ITask> HttpGetAnalogSensorHandler(dssphttp.HttpGet httpGet)
        {
            analogarray.Get get = new analogarray.Get();
            this.mainPort.Post(get);
            yield return get.ResponsePort.Choice();
            analogarray.AnalogSensorArrayState analogState = get.ResponsePort;
            httpGet.ResponsePort.Post(new dssphttp.HttpResponseType(analogState));
            yield break;
        }

        /// <summary>
        /// HttpGet SonarSensorArray Handler
        /// </summary>
        /// <param name="httpGet">HTTP Get request</param>
        /// <returns>A CCR iterator</returns>
        [ServiceHandler(ServiceHandlerBehavior.Concurrent, PortFieldName = AlternatePort)]
        public virtual IEnumerator<ITask> HttpGetSonarSensorHandler(dssphttp.HttpGet httpGet)
        {
            sonararray.Get get = new sonararray.Get();
            this.altPort.Post(get);
            yield return get.ResponsePort.Choice();
            sonararray.SonarSensorArrayState sonararrayState = get.ResponsePort;
            httpGet.ResponsePort.Post(new dssphttp.HttpResponseType(sonararrayState));
            yield break;
        }

        /// <summary>
        /// Get the state
        /// </summary>
        /// <param name="get">The get message</param>
        /// <returns>A CCR iterator</returns>
        [ServiceHandler(ServiceHandlerBehavior.Concurrent)]
        public IEnumerator<ITask> GetHandler(analogarray.Get get)
        {
            // compose state on the fly by issuing N parallel GET requests to partners
            var getSensorState = new sonar.Get();

            foreach (var port in this.sensorPorts)
            {
                port.Post(getSensorState);
            }

            ICollection<sonar.SonarState> sensorStates = null;
            ICollection<Fault> faults = null;
            yield return getSensorState.ResponsePort.MultipleItemReceive(
                this.sensorPorts.Length,
                (s, f) =>
                {
                    sensorStates = s;
                    faults = f;
                });

            if (faults != null && faults.Count > 0)
            {
                get.ResponsePort.Post(Fault.FromCodeSubcode(FaultCodes.Receiver, DsspFaultCodes.OperationFailed));
                yield break;
            }

			var state = new analogarray.AnalogSensorArrayState();
			state.Sensors = sensorStates.Select(sensorState =>
				new analogsensor.AnalogSensorState()
				{
					HardwareIdentifier = sensorState.HardwareIdentifier,
					NormalizedMeasurement = sensorState.DistanceMeasurement,
					RawMeasurement = sensorState.DistanceMeasurement,
					RawMeasurementRange = sensorState.MaxDistance,
					Pose = sensorState.Pose,
					TimeStamp = sensorState.TimeStamp,
				}).OrderBy(s => s.HardwareIdentifier == _state.SonarLeftState.HardwareIdentifier ? SonarSensors.LeftSonarProximityInMeters : SonarSensors.RightSonarProximityInMeters)
				.ToList();

			get.ResponsePort.Post(state);
            yield break;
        }

        /// <summary>
        /// Get the state
        /// </summary>
        /// <param name="get">The get message</param>
        /// <returns>A CCR iterator</returns>
        [ServiceHandler(ServiceHandlerBehavior.Concurrent, PortFieldName = AlternatePort)]
        public IEnumerator<ITask> GetSonarArrayHandler(sonararray.Get get)
        {
            // compose state on the fly by issuing N parallel GET requests to partners
            var getSensorState = new sonar.Get();

            foreach (var port in this.sensorPorts)
            {
                port.Post(getSensorState);
            }

            ICollection<sonar.SonarState> sensorStates = null;
            ICollection<Fault> faults = null;
            yield return getSensorState.ResponsePort.MultipleItemReceive(
                this.sensorPorts.Length,
                (s, f) =>
                {
                    sensorStates = s;
                    faults = f;
                });

            if (faults != null && faults.Count > 0)
            {
                get.ResponsePort.Post(Fault.FromCodeSubcode(FaultCodes.Receiver, DsspFaultCodes.OperationFailed));
                yield break;
            }

			var state = new sonararray.SonarSensorArrayState();
			state.Sensors = sensorStates.Select(sensorState => new sonarsensor.SonarState()
							{
								HardwareIdentifier = sensorState.HardwareIdentifier,
								AngularRange = sensorState.AngularRange,
								AngularResolution = sensorState.AngularResolution,
								DistanceMeasurement = sensorState.DistanceMeasurement,
								DistanceMeasurements = sensorState.DistanceMeasurements,
								MaxDistance = sensorState.MaxDistance,
								Pose = sensorState.Pose,
								TimeStamp = sensorState.TimeStamp,
							})
							.OrderBy(s => s.HardwareIdentifier == _state.SonarLeftState.HardwareIdentifier ? SonarSensors.LeftSonarProximityInMeters : SonarSensors.RightSonarProximityInMeters)
							.ToList();
            get.ResponsePort.Post(state);
            yield break;
        }

        /// <summary>
        /// Service start routine
        /// </summary>
        protected override void Start()
        {
            this.CreateDefaultState();
            TaskQueue.Enqueue(new IterativeTask(() => this.ConfigureSensors()));
        }

        /// <summary>
        /// Configure sensors
        /// </summary>
        /// <returns>A CCR iterator</returns>
        private IEnumerator<ITask> ConfigureSensors()
        {
            int i = 0;

            // get each sensor's current state
            // and assign hardware identifiers
            var replaceSensorState = new sonar.Replace();
            var get = new sonar.Get();
            foreach (var sensor in this.sensorPorts)
            {
                this.sensorPorts[i].Post(get);
                yield return get.ResponsePort.Choice();

                sonar.SonarState sensorState = get.ResponsePort;
                if (sensorState != null)
                {
                    replaceSensorState.Body = sensorState;
                }
                else
                {
                    replaceSensorState.Body = new sonar.SonarState();
                }

				replaceSensorState.Body.HardwareIdentifier = this.sensorStates[i].HardwareIdentifier;
				replaceSensorState.Body.Pose = this.sensorStates[i].Pose;
				
				sensor.Post(replaceSensorState);
                i++;
            }

            ICollection<Fault> faults = null;
            yield return replaceSensorState.ResponsePort.MultipleItemReceive(
                this.sensorPorts.Length,
                (successes, f) =>
                {
                    faults = f;
                });

            if (faults != null && faults.Count > 0)
            {
                LogError("Failure configuring Sonar sensors");
                this.mainPort.Post(new DsspDefaultDrop());
            }

            this.StartAfterConfigure();
        }

        /// <summary>
        /// Start after configure
        /// </summary>
        private void StartAfterConfigure()
        {
            base.Start();
        }

        /// <summary>
        /// Create default state
        /// </summary>
        private void CreateDefaultState()
        {
			if (_state == null)
			{
				_state = new SuricataIRArrayState()
				{
					SonarLeftState = new sonar.SonarState()
					{
						HardwareIdentifier = 5,
						Pose = LeftSonarPose
					},
					SonarRightState = new sonar.SonarState()
					{
						HardwareIdentifier = 6,
						Pose = RightSonarPose
					},
				};
				base.SaveState(_state);
			}
			this.sensorStates[(int)SonarSensors.LeftSonarProximityInMeters] = _state.SonarLeftState;
			this.sensorStates[(int)SonarSensors.RightSonarProximityInMeters] = _state.SonarRightState;

			this.sensorPorts[(int)SonarSensors.LeftSonarProximityInMeters] = this.leftSonar;
            this.sensorPorts[(int)SonarSensors.RightSonarProximityInMeters] = this.rightSonar;
        }
    }
}