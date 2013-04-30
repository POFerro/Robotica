using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using submgr = Microsoft.Dss.Services.SubscriptionManager;
using arduino = Arduino.Proxy;
using Microsoft.Dss.Core;
using Arduino.Firmata.Types.Proxy;

namespace POFerro.Robotics.ArduinoGenericInfraredReceiver
{
    [Contract(Contract.Identifier)]
    [DisplayName("ArduinoGenericInfraredReceiver")]
    [Description("ArduinoGenericInfraredReceiver service (no description provided)")]
    class ArduinoGenericInfraredReceiverService : DsspServiceBase
    {
        /// <summary>
        /// Service state
        /// </summary>
        [ServiceState]
		[InitialStatePartner(Optional = true, ServiceUri = "ArduinoGenericInfraredReceiverService.xml")]
		ArduinoGenericInfraredReceiverState _state = new ArduinoGenericInfraredReceiverState();

        /// <summary>
        /// Main service port
        /// </summary>
        [ServicePort("/ArduinoGenericInfraredReceiver", AllowMultipleInstances = true)]
        ArduinoGenericInfraredReceiverOperations _mainPort = new ArduinoGenericInfraredReceiverOperations();

        [SubscriptionManagerPartner]
        submgr.SubscriptionManagerPort _submgrPort = new submgr.SubscriptionManagerPort();

        /// <summary>
        /// ArduinoService partner
        /// </summary>
        [Partner("ArduinoService", Contract = arduino.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
        arduino.ArduinoOperations _arduinoServicePort = new arduino.ArduinoOperations();
        arduino.ArduinoOperations _arduinoServiceNotify = new arduino.ArduinoOperations();

        /// <summary>
        /// Service constructor
        /// </summary>
        public ArduinoGenericInfraredReceiverService(DsspServiceCreationPort creationPort)
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
				_state = new ArduinoGenericInfraredReceiverState();
				_state.HardwareIdentifier = (int)Pins.D11;
			}

            base.Start();
		
			_arduinoServicePort.Subscribe(_arduinoServiceNotify, typeof(Arduino.Messages.Proxy.DigitalOutputUpdate));
			MainPortInterleave.CombineWith(
				new Interleave(
					new TeardownReceiverGroup(),
					new ExclusiveReceiverGroup(),
					new ConcurrentReceiverGroup(
						Arbiter.Receive<Arduino.Messages.Proxy.DigitalOutputUpdate>(true, _arduinoServiceNotify, DigitalOutputUpdateHandler)
					)));
			
			Activate(
				_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = (Pins)_state.HardwareIdentifier, Mode = Arduino.Firmata.Types.Proxy.PinMode.IrReceiver }).Choice()
			);
		}

		private void DigitalOutputUpdateHandler(Arduino.Messages.Proxy.DigitalOutputUpdate message)
		{
			if ((int)message.Body.CurrentPin == _state.HardwareIdentifier)
			{
				_state.ReceivedCommand = message.Body.Value;
				base.SendNotification(_submgrPort, new InfreredCommandNotify(_state));
			}

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

        /// <summary>
        /// Handles Subscribe messages
        /// </summary>
        /// <param name="subscribe">the subscribe request</param>
        [ServiceHandler]
        public void SubscribeHandler(Subscribe subscribe)
        {
            SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
        }
    }
}


