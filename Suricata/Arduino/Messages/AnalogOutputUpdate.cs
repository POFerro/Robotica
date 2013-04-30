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
    public class AnalogOutputUpdate : Update<AnalogOutputUpdateRequest, PortSet<DefaultUpdateResponseType, Fault>>
    {
        public AnalogOutputUpdate()
            : base(new AnalogOutputUpdateRequest())
        {

        }
    }

    [DataContract]
    public class AnalogOutputUpdateRequest
    {
        public AnalogOutputUpdateRequest()
        {

        }

        [DataMember]
        public Arduino.Firmata.Types.Pins CurrentPin
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
