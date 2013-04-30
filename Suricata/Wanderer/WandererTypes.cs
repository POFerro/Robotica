using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

using sonarturret = POFerro.Robotics.ArduinoSonarTurret.Proxy;
using ir = Microsoft.Robotics.Services.AnalogSensor.Proxy;

namespace POFerro.Robotics.Wanderer
{
	public sealed class Contract
	{
		[DataMember]
		public const string Identifier = "http://schemas.pferro/2013/02/wanderer.html";
	}

	public enum WandererLogicalState
	{
		Unknown = 0,
		Ranging,
		TurnLeft,
		TurnRight,
		AdjustLeft,
		AdjustRight,
		MoveForward,
		ReverseLeft,
		ReverseRight
	}
	
	[DataContract]
	public class WandererState
	{
		[DataMember]
		public double IRSafeDistance { get; set; }
		[DataMember]
		public double IRDistanceDiferenceToAdjust { get; set; }

		public virtual double MaxSpeed { get { return 0.6; } }
		public virtual double MaxLateralSpeed { get { return 0.7; } }

		protected int state;
		[DataMember]
		public WandererLogicalState CurrentState
		{
			get
			{
				return (WandererLogicalState)state;
			}
			set
			{
				state = (int)value;
			}
		}

		[DataMember]
		public ir.AnalogSensorState LastLeftIRReading { get; set; }
		[DataMember]
		public ir.AnalogSensorState LastRightIRReading { get; set; }
		[DataMember]
		public ir.AnalogSensorState LastSonarReading { get; set; }
		[DataMember]
		public sonarturret.ArduinoSonarTurretState LastTurretReading { get; set; }

		[DataMember]
		public int BestAngle { get; set; }

		[DataMember]
		public double LeftWheelPower { get; set; }
		[DataMember]
		public double RightWheelPower { get; set; }

		public WandererState()
		{
			this.LastSonarReading = new ir.AnalogSensorState();
			this.LastTurretReading = new sonarturret.ArduinoSonarTurretState();
			this.LastLeftIRReading = new ir.AnalogSensorState();
			this.LastRightIRReading = new ir.AnalogSensorState();
			this.BestAngle = -1;

			this.LeftWheelPower = 0;
			this.RightWheelPower = 0;

			this.IRSafeDistance = 0.30;
			this.IRDistanceDiferenceToAdjust = 0.20;

			this.CurrentState = WandererLogicalState.Unknown;
		}

		public static double DegreeToRadian(double degree)
		{
			return degree * (Math.PI / 180.0);
		}

		public static double RadianToDegree(double radian)
		{
			return radian * (180.0 / Math.PI);
		}
	}
	
	[ServicePort]
	public class WandererOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get, StateChangeNotify, Subscribe>
	{
	}
	
	public class Get : Get<GetRequestType, PortSet<WandererState, Fault>>
	{
		public Get()
		{
		}
		
		public Get(GetRequestType body)
			: base(body)
		{
		}
		
		public Get(GetRequestType body, PortSet<WandererState, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}

	public class StateChangeNotify : Update<WandererState, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public StateChangeNotify()
		{
		}

		public StateChangeNotify(WandererState body)
			: base(body)
		{
		}

		public StateChangeNotify(WandererState body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}

	//[DataContract]
	//public class NotifyWandererStateUpdate
	//{
	//	public NotifyWandererStateUpdate()
	//	{
	//	}
	//	public NotifyWandererStateUpdate(WandererState _state)
	//	{
	//		this.CurrentState = _state;
	//	}
	//	[DataMember]
	//	public WandererState CurrentState { get; set; }
	//}


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


