using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using Microsoft.Dss.Services.SubscriptionManager;

namespace Arduino.Messages
{
	public class DigitalOutputUpdate : Update<DigitalOutputUpdateRequest, PortSet<DefaultUpdateResponseType, Fault>>
    {
		public DigitalOutputUpdate()
			: base(new DigitalOutputUpdateRequest())
        {
        }
    }

    [DataContract]
    public class DigitalOutputUpdateRequest
    {
		public DigitalOutputUpdateRequest()
        {

        }

        [DataMember]
        public Arduino.Firmata.Types.Pins CurrentPin
        {
            get;
            set;
        }

		[DataMember]
		public Arduino.Firmata.Types.PinMode CurrentPinMode
		{
			get;
			set;
		}

		[DataMember]
        public int Value
        {
            get;
            set;
        }
    }
}
