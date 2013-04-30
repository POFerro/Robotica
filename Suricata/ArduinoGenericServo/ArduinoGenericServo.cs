using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Arduino.Firmata.Types.Proxy;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using submgr = Microsoft.Dss.Services.SubscriptionManager;

namespace POFerro.Robotics.ArduinoGenericServo
{
	[Contract(Contract.Identifier)]
	[DisplayName("ArduinoGenericServo")]
	[Description("ArduinoGenericServo service (no description provided)")]
	class ArduinoGenericServoService : DsspServiceBase
	{
		[ServiceState]
		[InitialStatePartner(Optional = true, ServiceUri = "ArduinoGenericServoService.xml")]
		ArduinoGenericServoState _state = new ArduinoGenericServoState();
		
		[ServicePort("/ArduinoGenericServo", AllowMultipleInstances = true)]
		ArduinoGenericServoOperations _mainPort = new ArduinoGenericServoOperations();
		
		[SubscriptionManagerPartner]
		submgr.SubscriptionManagerPort _submgrPort = new submgr.SubscriptionManagerPort();

		/// <summary>
		/// ArduinoService partner
		/// </summary>
		[Partner("ArduinoService", Contract = Arduino.Proxy.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry, Optional = true)]
		Arduino.Proxy.ArduinoOperations _arduinoServicePort = new Arduino.Proxy.ArduinoOperations();
		
		public ArduinoGenericServoService(DsspServiceCreationPort creationPort)
			: base(creationPort)
		{
		}
		
		protected override void Start()
		{
			if (_state == null)
			{
				_state = new ArduinoGenericServoState();
				_state.HardwareIdentifier = (int)Pins.D9;
				_state.MinPulse = 635;
				_state.MaxPulse = 2400;
				_state.StartAngle = 90;

				this.SaveState(_state);
				Console.WriteLine("State not found in: " + ServicePaths.Store + "/ArduinoGenericServoService.xml");
			}

			base.Start();

			Activate(_arduinoServicePort.SetServoMode(
				new Arduino.Messages.Proxy.SetServoModeRequest() { Pin = (Pins)_state.HardwareIdentifier, MinPulse = _state.MinPulse, MaxPulse = _state.MaxPulse, StartAngle = _state.StartAngle })
				.Choice());
		}

		[ServiceHandler]
		public IEnumerator<ITask> MoveServo(MoveServo valor)
		{
			yield return _arduinoServicePort.SetPortAnalogValue(new Arduino.Messages.Proxy.SetPortAnalogValueRequest() { Pin = (Pins)_state.HardwareIdentifier, Value = valor.Body }).Choice();
			_state.CurrentAngle = valor.Body;

			valor.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		[ServiceHandler]
		public void GetHandler(Get message)
		{
			message.ResponsePort.Post(_state);
		}
		
		[ServiceHandler]
		public void SubscribeHandler(Subscribe subscribe)
		{
			SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
		}
	}
}


