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
    public class SetPinReporting : Update<SetPinReportingRequest, PortSet<DefaultUpdateResponseType, Fault>>
    {
        public SetPinReporting()
            : base(new SetPinReportingRequest())
        {

        }
    }

    [DataContract]
    public class SetPinReportingRequest
    {
        public SetPinReportingRequest()
        {

        }

        [DataMember]
        public Arduino.Firmata.Types.Pins Pin
        {
            get;
            set;
        }

        [DataMember]
        public bool ReportingEnabled
        {
            get;
            set;
        }

    }
}
