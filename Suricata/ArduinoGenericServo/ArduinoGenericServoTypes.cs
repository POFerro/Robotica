using System;
using System.Collections.Generic;
using System.ComponentModel;
using Arduino.Firmata.Types.Proxy;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

namespace POFerro.Robotics.ArduinoGenericServo
{
	public sealed class Contract
	{
		[DataMember]
		public const string Identifier = "http://schemas.pferro/2013/03/arduinogenericservo.html";
	}
	
	[DataContract]
	public class ArduinoGenericServoState
	{
		// Summary:
		//     Hardware port identifier
		[DataMember(Order = -1)]
		[Description("Identifies the hardware port for the sensor.")]
		public int HardwareIdentifier { get; set; }

		[DataMember(Order = -1)]
		public int MinPulse { get; set; }

		[DataMember(Order = -1)]
		public int MaxPulse { get; set; }

		[DataMember(Order = -1)]
		public int StartAngle { get; set; }

		// Summary:
		//     Hardware port identifier
		[DataMember(Order = -1)]
		[Description("Identifies the current position of the servo.")]
		public int CurrentAngle { get; set; }
	}
	
	[ServicePort]
	public class ArduinoGenericServoOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, MoveServo, Get, Subscribe>
	{
	}

	public class MoveServo : Update<int, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public MoveServo()
		{
		}

		public MoveServo(int valor)
			: base(valor)
		{
		}

		public MoveServo(int valor, PortSet<DefaultUpdateResponseType, Fault> responsePort)
			: base(valor, responsePort)
		{
		}
	}

	public class Get : Get<GetRequestType, PortSet<ArduinoGenericServoState, Fault>>
	{
		public Get()
		{
		}
		
		public Get(GetRequestType body)
			: base(body)
		{
		}
		
		public Get(GetRequestType body, PortSet<ArduinoGenericServoState, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}
	
	public class Subscribe : Subscribe<SubscribeRequestType, PortSet<SubscribeResponseType, Fault>>
	{
		public Subscribe()
		{
		}
		
		public Subscribe(SubscribeRequestType body)
			: base(body)
		{
		}
		
		public Subscribe(SubscribeRequestType body, PortSet<SubscribeResponseType, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}
}


