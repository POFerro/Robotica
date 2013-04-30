using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

using arduino = Arduino.Firmata.Types.Proxy;
using drive = Microsoft.Robotics.Services.Drive.Proxy;

namespace POFerro.Robotics.ArduinoGenericDrive
{
	public sealed class Contract
	{
		[DataMember]
		public const string Identifier = "http://schemas.pferro/2013/03/arduinogenericdrive.html";
	}

	public enum MotorShieldTypeEnum
	{
		ArduinoShield,
		Keyes
	}

	[DataContract]
	public class ArduinoGenericDriveState : drive.DriveDifferentialTwoWheelState
	{
		[DataMember]
		public MotorShieldTypeEnum MotorShieldType { get; set; }

		//Right Motor (A)
		[DataMember]
		public arduino.Pins RightForwardPin { get; set; }
		[DataMember]
		public arduino.Pins RightBackwardPin { get; set; }

		//Left Motor (B)
		[DataMember]
		public arduino.Pins LeftForwardPin { get; set; }
		[DataMember]
		public arduino.Pins LeftBackwardPin { get; set; }

		//Right Motor (A)
		[DataMember]
		public arduino.Pins LeftEngineDirPin { get; set; }
		[DataMember]
		public arduino.Pins LeftEngineBreakPin { get; set; }
		[DataMember]
		public arduino.Pins LeftEnginePWMPin { get; set; }

		[DataMember]
		public arduino.Pins LeftEngineCurrentSensor { get; set; }

		//Left Motor (B)
		[DataMember]
		public arduino.Pins RightEngineDirPin { get; set; }
		[DataMember]
		public arduino.Pins RightEngineBreakPin { get; set; }
		[DataMember]
		public arduino.Pins RightEnginePWMPin { get; set; }

		[DataMember]
		public arduino.Pins RightEngineCurrentSensor { get; set; }

		[DataMember]
		[Description("Rotation time ms/degree  (16 on carpet and 8 on wooden floor)")]
		public int MillisecondsPerAngle { get; set; }

		public ArduinoGenericDriveState()
		{
			this.MotorShieldType = MotorShieldTypeEnum.Keyes;
			this.MillisecondsPerAngle = 8;

			LeftWheel = new Microsoft.Robotics.Services.Motor.Proxy.WheeledMotorState();
			RightWheel = new Microsoft.Robotics.Services.Motor.Proxy.WheeledMotorState();
			LeftWheel.EncoderState = new Microsoft.Robotics.Services.Encoder.Proxy.EncoderState();
			RightWheel.EncoderState = new Microsoft.Robotics.Services.Encoder.Proxy.EncoderState();
			LeftWheel.MotorState = new Microsoft.Robotics.Services.Motor.Proxy.MotorState();
			LeftWheel.MotorState.PowerScalingFactor = 255;
			RightWheel.MotorState = new Microsoft.Robotics.Services.Motor.Proxy.MotorState();
			RightWheel.MotorState.PowerScalingFactor = 255;
		}
	}
	
	[ServicePort]
	public class ArduinoGenericDriveOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get, Subscribe>
	{
	}
	
	public class Get : Get<GetRequestType, PortSet<ArduinoGenericDriveState, Fault>>
	{
		public Get()
		{
		}
		
		public Get(GetRequestType body)
			: base(body)
		{
		}
		
		public Get(GetRequestType body, PortSet<ArduinoGenericDriveState, Fault> responsePort)
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


