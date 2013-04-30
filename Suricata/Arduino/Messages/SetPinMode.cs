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
    public class SetPinMode : Update<SetPinModeRequest, PortSet<DefaultUpdateResponseType, Fault>>
    {
        public SetPinMode()
            : base(new SetPinModeRequest())
        {

        }
    }

    [DataContract]
    public class SetPinModeRequest
    {
        public SetPinModeRequest()
        {

        }

        [DataMember]
        public Arduino.Firmata.Types.Pins Pin
        {
            get;
            set;
        }

        [DataMember]
        public Arduino.Firmata.Types.PinMode Mode
        {
            get;
            set;
        }

    }

	public class SetServoMode : Update<SetServoModeRequest, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public SetServoMode()
			: base(new SetServoModeRequest())
		{

		}
	}

	[DataContract]
	public class SetServoModeRequest
	{
		public SetServoModeRequest()
		{

		}

		[DataMember]
		public Arduino.Firmata.Types.Pins Pin
		{
			get;
			set;
		}

		[DataMember]
		public int MinPulse { get; set; }

		[DataMember]
		public int MaxPulse { get; set; }

		[DataMember]
		public int StartAngle { get; set; }
	}

	public class SetSonarMode : Update<SetSonarModeRequest, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public SetSonarMode()
			: base(new SetSonarModeRequest())
		{

		}
	}

	[DataContract]
	public class SetSonarModeRequest
	{
		public SetSonarModeRequest()
		{
		}

		[DataMember]
		public Arduino.Firmata.Types.Pins TriggerPin
		{
			get;
			set;
		}

		[DataMember]
		public Arduino.Firmata.Types.Pins EchoPin
		{
			get;
			set;
		}

		[DataMember]
		public int MaxDistance { get; set; }
	}
}
