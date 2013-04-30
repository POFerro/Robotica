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
    public class SetPortAnalogValue : Update<SetPortAnalogValueRequest, PortSet<DefaultUpdateResponseType, Fault>>
    {
        public SetPortAnalogValue()
            : base(new SetPortAnalogValueRequest())
        {

        }
    }

    [DataContract]
    public class SetPortAnalogValueRequest
    {
        public SetPortAnalogValueRequest()
        {

        }

        [DataMember]
        public Arduino.Firmata.Types.Pins Pin
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
