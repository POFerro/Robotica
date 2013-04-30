using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using submgr = Microsoft.Dss.Services.SubscriptionManager;
using System.Runtime.InteropServices;
using Microsoft.Dss.Core;
using System.Threading;
using System.Linq;

namespace Arduino
{
    [Contract(Contract.Identifier)]
    [DisplayName("Arduino Service")]
    public class ArduinoService : DsspServiceBase
    {
        #region Service
        /// <summary>
        /// Service state
        /// </summary>
        [ServiceState]
        [InitialStatePartner(Optional = true, ServiceUri = "ArduinoService.xml")]
        ArduinoState _state = new ArduinoState();

        /// <summary>
        /// Main service port
        /// </summary>
        [ServicePort("/Arduino", AllowMultipleInstances = true)]
        ArduinoOperations _mainPort = new ArduinoOperations();

        [SubscriptionManagerPartner]
        submgr.SubscriptionManagerPort _submgrPort = new submgr.SubscriptionManagerPort();
        #endregion

        Arduino.Firmata.Firmata _firmata = null;

        /// <summary>
        /// Service constructor
        /// </summary>
        public ArduinoService(DsspServiceCreationPort creationPort)
            : base(creationPort)
        {
        }

        /// <summary>
        /// Service start
        /// </summary>
        protected override void Start()
        {
            if (_state == null)
            {
                _state = new ArduinoState();

                _state.ConnectionType = ArduinoState.ConnectionTypes.SerialPort;
                _state.ComPort = 6;
                _state.ComPortRate = 57600;
				Console.WriteLine("State not found in: " + ServicePaths.Store + "/ArduinoService.xml");
            }

            Arduino.ConnectionTypes.ConnectionBase connection = null;
			try
			{
				if (_state.ConnectionType == ArduinoState.ConnectionTypes.SerialPort)
				{
					Arduino.ConnectionTypes.SerialPort.Serial conn = new Arduino.ConnectionTypes.SerialPort.Serial();
					conn.Open(_state.ComPort, _state.ComPortRate);
					connection = (Arduino.ConnectionTypes.ConnectionBase)conn;
				}

				_firmata = new Arduino.Firmata.Firmata(connection, _state.SamplingInterval);
				_firmata.AnalogOutputUpdated += _firmata_AnalogOutputUpdated;
				_firmata.DigitalOutputUpdated += _firmata_DigitalOutputUpdated;
				_firmata.FirmataStringReceived += _firmata_FirmataStringReceived;

				_firmata.WaitToStart(10000);
			}
			catch (Exception e)
			{
				LogError("Firmata failed to start", e);
				StartFailed();
				return;
			}

            _state.LastTick = DateTime.Now;

            SetArduinoConfiguration(_state);

			base.Start();

            ShowMessage("Arduino started");
        }

		void _firmata_FirmataStringReceived(string Message)
		{
			this.ShowMessage(Message);
		}

		void _firmata_DigitalOutputUpdated(Firmata.Types.Pins pin, Firmata.Types.PinDigitalValue value)
		{
			if (_firmata.getPinReportMode(pin))
			{
				ReportDigitalPinState(pin);
			}
		}

		void _firmata_AnalogOutputUpdated(Firmata.Types.Pins pin, int value)
		{
			if (_firmata.getPinReportMode(pin))
			{
				ReportAnalogPinState(pin);
			}
		}

        private void ShowMessage(string text)
        {
            LogInfo(text);
            Console.WriteLine(text);
        }

        void SetArduinoConfiguration_AnalogOutputSet(PinConfigurationAnalogOutput input)
        {
            if (input.Pin != Arduino.Firmata.Types.Pins.None)
            {
				ShowMessage(String.Format("Initial SetPinMode request. Pin = {0}, Mode = Analog", input.Pin));
				_firmata.setPinMode(input.Pin, Arduino.Firmata.Types.PinMode.Analog);
				if (!_state.PinsReporting.Contains(input.Pin)) _state.PinsReporting.Add(input.Pin);
            }
        }

        void SetArduinoConfiguration_DigitalSet(PinConfigurationDigitalPort port)
        {
            if (port.Pin != Arduino.Firmata.Types.Pins.None)
            {
				ShowMessage(String.Format("Initial SetPinMode request. Pin = {0}, Mode = {1}", port.Pin, port.Mode));
				_firmata.setPinMode(port.Pin, port.Mode);
                if (port.Mode == Arduino.Firmata.Types.PinMode.Output)
                {
					if (!_state.PinsReporting.Contains(port.Pin)) _state.PinsReporting.Add(port.Pin);
					_firmata.sendSetPinReporting(port.Pin, true);
                }
                else if (port.Mode == Arduino.Firmata.Types.PinMode.PWM)
                {
					//_firmata.analogWrite(port.Pin, port.PWMInitialValue);
                }
                else if (port.Mode == Arduino.Firmata.Types.PinMode.Input)
                {
					if (!_state.PinsReporting.Contains(port.Pin)) _state.PinsReporting.Add(port.Pin);
					_firmata.sendSetPinReporting(port.Pin, true);
				}
            }
        }

		void SetArduinoConfiguration(ArduinoState _statesource)
		{
			if (_statesource.PinsConfiguration != null)
			{
				if (_statesource.PinsConfiguration.Analog != null)
					foreach (var port in _statesource.PinsConfiguration.Analog)
					{
						SetArduinoConfiguration_AnalogOutputSet(port);
					}

				if (_statesource.PinsConfiguration.Digital != null)
					foreach (var port in _statesource.PinsConfiguration.Digital)
					{
						SetArduinoConfiguration_DigitalSet(port);
					}
			}
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

		[ServiceHandler(ServiceHandlerBehavior.Teardown)]
		public void DropHandler(DsspDefaultDrop shutdown)
		{
			_firmata.sendReset();
			_firmata.Dispose();

			shutdown.ResponsePort.Post(DefaultDropResponseType.Instance);
		}

		protected void ReportAnalogPins()
		{
			foreach (var pin in _firmata.getPinList(Arduino.Firmata.Types.PinMode.Analog))
			{
				if (_firmata.getPinReportMode(pin))
				{
					ReportAnalogPinState(pin);
				}
			}
		}

		protected void ReportDigitalPins(Arduino.Firmata.Types.PinMode mode)
		{
			foreach (var pin in _firmata.getPinList(mode))
			{
				if (_firmata.getPinReportMode(pin))
				{
					ReportDigitalPinState(pin, mode);
				}
			}
		}

		protected void ReportAnalogPinState(Arduino.Firmata.Types.Pins pin)
		{
			if ((int)pin < (int)Arduino.Firmata.Types.Pins.A0)
			{
				ReportDigitalPinState(pin);
			}
			else
			{
				var mess = new Messages.AnalogOutputUpdate();
				mess.Body.CurrentPin = pin;
				mess.Body.Value = _firmata.analogRead(pin);
				base.SendNotification(_submgrPort, mess);
			}
		}

		protected void ReportDigitalPinState(Arduino.Firmata.Types.Pins pin)
		{
			ReportDigitalPinState(pin, _firmata.getPinMode(pin));
		}

		protected void ReportDigitalPinState(Arduino.Firmata.Types.Pins pin, Arduino.Firmata.Types.PinMode mode)
		{
			var mess = new Messages.DigitalOutputUpdate();
			mess.Body.CurrentPin = pin;
			mess.Body.CurrentPinMode = mode;
			mess.Body.Value = _firmata.digitalRead(pin);
			base.SendNotification(_submgrPort, mess);
		}

		[ServiceHandler(ServiceHandlerBehavior.Exclusive)]
		public IEnumerator<ITask> SetServoModeHandler(Messages.SetServoMode message)
		{
			ShowMessage(String.Format("SetServoMode request. Pin = {0}, Min = {1}, Max = {2}, StartAngle = {3}", message.Body.Pin, message.Body.MinPulse, message.Body.MaxPulse, message.Body.StartAngle));
			_firmata.setServoMode(message.Body.Pin, message.Body.MinPulse, message.Body.MaxPulse, message.Body.StartAngle);

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
			yield break;
		}

		[ServiceHandler(ServiceHandlerBehavior.Exclusive)]
		public IEnumerator<ITask> SetSonarModeHandler(Messages.SetSonarMode message)
		{
			ShowMessage(String.Format("SetSonarMode request. TriggerPin = {0}, EchoPin = {1}, MaxDistance = {2}", message.Body.TriggerPin, message.Body.EchoPin, message.Body.MaxDistance));
			_firmata.setSonarMode(message.Body.TriggerPin, message.Body.EchoPin, message.Body.MaxDistance);

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
			yield break;
		}

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public IEnumerator<ITask> SetPinModeHandler(Messages.SetPinMode message)
        {
            ShowMessage(String.Format("SetPinMode request. Pin = {0}, Mode = {1}", message.Body.Pin, message.Body.Mode));
            _firmata.setPinMode(message.Body.Pin, message.Body.Mode);

            message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
            yield break;
        }

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public IEnumerator<ITask> SetPortDigitalValueHandler(Messages.SetPortDigitalValue message)
        {
			_firmata.digitalWrite(message.Body.Pin, message.Body.Value);
            message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
			yield break;
        }

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public IEnumerator<ITask> SetPortAnalogValueHandler(Messages.SetPortAnalogValue message)
        {
            _firmata.analogWrite(message.Body.Pin, message.Body.Value);
            message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
			yield break;
        }

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public IEnumerator<ITask> SetPinReportingHandler(Messages.SetPinReporting message)
        {
            ShowMessage(String.Format("SetPinReporting request. Pin = {0}. ReportingEnabled = {1}", message.Body.Pin, message.Body.ReportingEnabled));
			_firmata.sendSetPinReporting(message.Body.Pin, message.Body.ReportingEnabled);

            message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
            yield break;
        }
        #endregion
    }
}
