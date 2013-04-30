using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using Microsoft.Robotics.PhysicalModel.Proxy;
using W3C.Soap;
using sonar = Microsoft.Robotics.Services.Sonar.Proxy;

namespace POFerro.Robotics.ArduinoSonarTurret
{
	public sealed class Contract
	{
		[DataMember]
		public const string Identifier = "http://schemas.pferro/2013/03/arduinosonarturret.html";
	}
	
	[DataContract]
	public class ArduinoSonarTurretState
	{
		// Summary:
		//     Angular range of the measurement.
		[DataMember(Order = -1)]
		[Description("Specifies the sonar's scanning angle.")]
		public double AngularRange { get; set; }
		//
		// Summary:
		//     Resolution of the raycasting
		[DataMember(Order = -1)]
		[Description("Specifies the size of smallest detectable feature (in radians).")]
		public double AngularResolution { get; set; }
		//
		// Summary:
		//     The distance reading in meters.
		[Browsable(false)]
		[DataMember(Order = -1)]
		[Description("Identifies the distance reading of the sonar in meters.")]
		public double DistanceMeasurement { get; set; }
		//
		// Summary:
		//     Array of distance readings.  NOTE: This is just a discretization of the sensor's
		//     cone for raycasting
		//
		// Remarks:
		//     NOTE: This is just a discretization of the sensor's cone for raycasting
		[Browsable(false)]
		[DataMember(Order = -1)]
		[Description("Identifies the set of distance readings.")]
		public double[] DistanceMeasurements { get; set; }

		[Description("Identifies the hardware port of the sonar sensor.")]
		public int HardwareIdentifier { get; set; }
		//
		// Summary:
		//     Max distance sensor can read in meters
		[DataMember(Order = -1)]
		[Description("Specifies the maximum distance the sensor can read in meters.")]
		public double MaxDistance { get; set; }
		//
		// Summary:
		//     Position and orientation
		[Description("The position and orientation of the sonar sensor.")]
		public Pose Pose { get; set; }
		//
		// Summary:
		//     Timestamp of this sample
		[Browsable(false)]
		[DataMember(Order = -1, XmlOmitDefaultValue = true)]
		[DefaultValue(typeof(DateTime), "0001-01-01T00:00:00")]
		[Description("Identifies the timestamp for the reading of the sonar sensor.")]
		public DateTime TimeStamp { get; set; }

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
	public class ArduinoSonarTurretOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, RangeSweep, Get, Subscribe, RangePositionReadNotify, RangeSweepCompleteNotify>
	{
	}

	public class RangePositionReadNotify : Update<RangePositionRead, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public RangePositionReadNotify()
		{
		}

		public RangePositionReadNotify(RangePositionRead body)
			: base(body)
		{
		}

		public RangePositionReadNotify(RangePositionRead body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}

	[DataContract]
	public class RangePositionRead
	{
		// Summary:
		//     Angular range of the measurement.
		[DataMember(Order = -1)]
		[Description("Specifies the sonar's scanning angle.")]
		public int SweepAngularStep { get; set; }

		// Summary:
		//     Angular range of the measurement.
		[DataMember(Order = -1)]
		[Description("Specifies the sonar's scanning angle.")]
		public double CurrentAngle { get; set; }
		//
		// Summary:
		//     The distance reading in meters.
		[Browsable(false)]
		[DataMember(Order = -1)]
		[Description("Identifies the distance reading of the sonar in meters.")]
		public double DistanceMeasurement { get; set; }
		//
		// Summary:
		//     Hardware port identifier
		[DataMember(Order = -1)]
		[Description("Identifies the hardware port of the sonar sensor.")]
		public int HardwareIdentifier { get; set; }
		//
		// Summary:
		//     Max distance sensor can read in meters
		[DataMember(Order = -1)]
		[Description("Specifies the maximum distance the sensor can read in meters.")]
		public double MaxDistance { get; set; }
		//
		// Summary:
		//     Position and orientation
		[DataMember(Order = -1)]
		[Description("The position and orientation of the sonar sensor.")]
		public Pose Pose { get; set; }
		//
		// Summary:
		//     Timestamp of this sample
		[Browsable(false)]
		[DataMember(Order = -1, XmlOmitDefaultValue = true)]
		[DefaultValue(typeof(DateTime), "0001-01-01T00:00:00")]
		[Description("Identifies the timestamp for the reading of the sonar sensor.")]
		public DateTime TimeStamp { get; set; }
	}

	public class RangeSweep : Update<RangeSweepRequest, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public RangeSweep()
		{
		}

		public RangeSweep(RangeSweepRequest body)
			: base(body)
		{
		}

		public RangeSweep(RangeSweepRequest body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}

	public class RangeSweepCompleteNotify : Update<ArduinoSonarTurretState, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public RangeSweepCompleteNotify()
		{
		}

		public RangeSweepCompleteNotify(ArduinoSonarTurretState body)
			: base(body)
		{
		}

		public RangeSweepCompleteNotify(ArduinoSonarTurretState body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}

	[DataContract]
	public class RangeSweepRequest
	{
	}

	public class Get : Get<GetRequestType, PortSet<ArduinoSonarTurretState, Fault>>
	{
		public Get()
		{
		}

		public Get(GetRequestType body)
			: base(body)
		{
		}

		public Get(GetRequestType body, PortSet<ArduinoSonarTurretState, Fault> responsePort)
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


