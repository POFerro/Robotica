using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

namespace POFerro.Robotics.ArduinoGenericInfraredReceiver
{
    /// <summary>
    /// ArduinoGenericInfraredReceiver contract class
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
        /// DSS contract identifer for ArduinoGenericInfraredReceiver
        /// </summary>
        [DataMember]
        public const string Identifier = "http://schemas.pferro/2013/04/arduinogenericinfraredreceiver.html";
    }

    /// <summary>
    /// ArduinoGenericInfraredReceiver state
    /// </summary>
    [DataContract]
    public class ArduinoGenericInfraredReceiverState
    {
		// Summary:
		//     Hardware port identifier
		[DataMember(Order = -1)]
		[Description("Identifies the hardware port for the sensor.")]
		public int HardwareIdentifier { get; set; }

		[DataMember()]
		public int ReceivedCommand { get; set; }
	}

    /// <summary>
    /// ArduinoGenericInfraredReceiver main operations port
    /// </summary>
    [ServicePort]
	public class ArduinoGenericInfraredReceiverOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get, InfreredCommandNotify, Subscribe>
    {
    }

    /// <summary>
    /// ArduinoGenericInfraredReceiver get operation
    /// </summary>
    public class Get : Get<GetRequestType, PortSet<ArduinoGenericInfraredReceiverState, Fault>>
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
        public Get(GetRequestType body, PortSet<ArduinoGenericInfraredReceiverState, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }

	public class InfreredCommandNotify : Update<ArduinoGenericInfraredReceiverState, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public InfreredCommandNotify()
		{
		}

		public InfreredCommandNotify(ArduinoGenericInfraredReceiverState body)
			: base(body)
		{
		}

		public InfreredCommandNotify(ArduinoGenericInfraredReceiverState body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}

    /// <summary>
    /// ArduinoGenericInfraredReceiver subscribe operation
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


