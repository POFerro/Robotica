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
    public class SetPortDigitalValue : Update<SetPortDigitalValueRequest, PortSet<DefaultUpdateResponseType, Fault>>
    {
        public SetPortDigitalValue()
            : base(new SetPortDigitalValueRequest())
        {

        }
    }

    [DataContract]
    public class SetPortDigitalValueRequest
    {
        public SetPortDigitalValueRequest()
        {

        }

        [DataMember]
        public Arduino.Firmata.Types.Pins Pin
        {
            get;
            set;
        }

        [DataMember]
        public Arduino.Firmata.Types.PinDigitalValue Value
        {
            get;
            set;
        }
    }
}
