using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using Microsoft.Robotics.PhysicalModel.Proxy;
using W3C.Soap;

namespace ArduinoGenericDistanceSensor
{
    /// <summary>
    /// ArduinoGenericDistanceSensor contract class
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
        /// DSS contract identifer for ArduinoGenericDistanceSensor
        /// </summary>
        [DataMember]
        public const string Identifier = "http://mrdsarduino.codeplex.com/2012/01/arduinogenericdistancesensor.html";
    }

    /// <summary>
    /// ArduinoGenericDistanceSensor state
    /// </summary>
    [DataContract]
    public class ArduinoGenericDistanceSensorState
    {
        [DataMember]
        public Sensors.SensorsModels SensorModel
        {
            get;
            set;
        }

		// Summary:
		//     Hardware port identifier
		[DataMember(Order = -1)]
		[Description("Identifies the hardware port for the sensor.")]
		public int HardwareIdentifier { get; set; }

		//
		// Summary:
		//     Position and orientation
		[DataMember(Order = -1)]
		[Description("Specifies the position and orientation of the sensor.")]
		public Pose Pose { get; set; }

		[DataMember]
		public Arduino.Firmata.Types.Proxy.Pins TriggerPin { get; set; }
	}

    /// <summary>
    /// ArduinoGenericDistanceSensor main operations port
    /// </summary>
    [ServicePort]
    public class ArduinoGenericDistanceSensorOperations : PortSet<
                        DsspDefaultLookup, DsspDefaultDrop, Get, Subscribe>
    {

    }

    /// <summary>
    /// ArduinoGenericDistanceSensor get operation
    /// </summary>
    public class Get : Get<GetRequestType, PortSet<ArduinoGenericDistanceSensorState, Fault>>
    {
        /// <summary>
        /// Creates a new instance of Get
        /// </summary>
        public Get()
        {
        }

        /// <summary>
        /// Creates a new instance of Get
        /// </summary>
        /// <param name="body">the request message body</param>
        public Get(GetRequestType body)
            : base(body)
        {
        }

        /// <summary>
        /// Creates a new instance of Get
        /// </summary>
        /// <param name="body">the request message body</param>
        /// <param name="responsePort">the response port for the request</param>
        public Get(GetRequestType body, PortSet<ArduinoGenericDistanceSensorState, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }

    /// <summary>
    /// ArduinoGenericDistanceSensor subscribe operation
    /// </summary>
    public class Subscribe : Subscribe<SubscribeRequestType, PortSet<SubscribeResponseType, Fault>>
    {
        /// <summary>
        /// Creates a new instance of Subscribe
        /// </summary>
        public Subscribe()
        {
        }

        /// <summary>
        /// Creates a new instance of Subscribe
        /// </summary>
        /// <param name="body">the request message body</param>
        public Subscribe(SubscribeRequestType body)
            : base(body)
        {
        }

        /// <summary>
        /// Creates a new instance of Subscribe
        /// </summary>
        /// <param name="body">the request message body</param>
        /// <param name="responsePort">the response port for the request</param>
        public Subscribe(SubscribeRequestType body, PortSet<SubscribeResponseType, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }
}
