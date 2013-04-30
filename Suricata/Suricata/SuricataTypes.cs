using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

using ir = Microsoft.Robotics.Services.AnalogSensor.Proxy;
using sonar = Microsoft.Robotics.Services.Sonar.Proxy;

namespace POFerro.Robotics.Suricata
{
	public sealed class Contract
	{
		[DataMember]
		public const string Identifier = "http://schemas.pferro/2013/04/suricata.html";
	}

	public enum SuricataLogicalState
	{
		Unknown = 0,
		FollowingShepherd,
		FollowingSound
	}
	
	[DataContract]
	public class SuricataState
	{
		protected int state;
		[DataMember]
		public SuricataLogicalState CurrentState
		{
			get
			{
				return (SuricataLogicalState)state;
			}
			set
			{
				state = (int)value;
			}
		}

		public virtual double MaxSpeed { get { return 0.6; } }
		public virtual double MaxLateralSpeed { get { return 0.6; } }
		
		[DataMember]
		public ir.AnalogSensorState LastRightSonarReading { get; set; }

		[DataMember]
		public ir.AnalogSensorState LastLeftSonarReading { get; set; }

		[DataMember]
		public ir.AnalogSensorState LastCenterIRReading { get; set; }
		[DataMember]
		public ir.AnalogSensorState LastLeftIRReading { get; set; }
		[DataMember]
		public ir.AnalogSensorState LastRightIRReading { get; set; }

		[DataMember]
		public double LeftWheelPower { get; set; }
		[DataMember]
		public double RightWheelPower { get; set; }

		public SuricataState()
		{
			this.LastCenterIRReading = new ir.AnalogSensorState();
			this.LastLeftIRReading = new ir.AnalogSensorState();
			this.LastRightIRReading = new ir.AnalogSensorState();

			this.LastLeftSonarReading = new ir.AnalogSensorState();
			this.LastRightSonarReading = new ir.AnalogSensorState();

			this.LeftWheelPower = 0;
			this.RightWheelPower = 0;

			this.CurrentState = SuricataLogicalState.Unknown;
		}

		public SkeletonFollower.Proxy.SkeletonFollowerState SkeletonFollowerState { get; set; }

		public SoundFollower.Proxy.SoundFollowerState SoundFollowerState { get; set; }
	}
	
	[ServicePort]
	public class SuricataOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get, StateChangeNotify, Subscribe>
	{
	}
	
	public class Get : Get<GetRequestType, PortSet<SuricataState, Fault>>
	{
		public Get()
		{
		}
		
		public Get(GetRequestType body)
			: base(body)
		{
		}
		
		public Get(GetRequestType body, PortSet<SuricataState, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}

	public class StateChangeNotify : Update<SuricataState, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public StateChangeNotify()
		{
		}

		public StateChangeNotify(SuricataState body)
			: base(body)
		{
		}

		public StateChangeNotify(SuricataState body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
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


