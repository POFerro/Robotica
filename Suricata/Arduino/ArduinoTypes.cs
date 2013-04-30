using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

namespace Arduino
{
    /// <summary>
    /// Arduino contract class
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
        /// DSS contract identifer for Arduino
        /// </summary>
        [DataMember]
        public const string Identifier = "http://mrdsarduino.codeplex.com/2012/01/arduino.html";
    }

    /// <summary>
    /// Arduino state
    /// </summary>
    [DataContract]
    public class ArduinoState
    {
        #region Property
        #region Connection
        public enum ConnectionTypes
        {
            None,
            SerialPort,
            TCPIP
        }

        [DataMember]
        public ConnectionTypes ConnectionType
        {
            get;
            set;
        }

        #region COM
        [DataMember]
        public int ComPort
        {
            get;
            set;
        }

        [DataMember]
        public int ComPortRate
        {
            get;
            set;
        }

        #endregion
        #endregion

        #region Pins configuration
        [DataMember]
        public PinConfigurationCollection PinsConfiguration
        {
            get;
            set;
        }

		[DataMember]
		public List<Firmata.Types.Pins> PinsReporting
		{
			get;
			set;
		}

        #endregion

        public DateTime LastTick
        {
            get;
            set;
        }

		[DataMember()]
		public int SamplingInterval { get; set; }
        #endregion

		public ArduinoState()
		{
			this.SamplingInterval = 19;
		}
    }

    /// <summary>
    /// Arduino main operations port
    /// </summary>
    [ServicePort]
    public class ArduinoOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get, Subscribe,
				Messages.DigitalOutputUpdate,
				Messages.AnalogOutputUpdate,
				Messages.SetPinMode,
				Messages.SetServoMode, 
				Messages.SetSonarMode,
                Messages.SetPortDigitalValue, 
                Messages.SetPortAnalogValue,
                Messages.SetPinReporting
                >

    {
    }

    /// <summary>
    /// Arduino get operation
    /// </summary>
    public class Get : Get<GetRequestType, PortSet<ArduinoState, Fault>>
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
        public Get(GetRequestType body, PortSet<ArduinoState, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }

    /// <summary>
    /// Arduino subscribe operation
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


