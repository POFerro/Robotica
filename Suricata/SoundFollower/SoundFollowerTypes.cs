using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

namespace POFerro.Robotics.SoundFollower
{
    /// <summary>
    /// SoundFollower contract class
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
        /// DSS contract identifer for SoundFollower
        /// </summary>
        [DataMember]
        public const string Identifier = "http://schemas.pferro/2013/04/soundfollower.html";
    }

	public enum SoundFollowerLogicalState
	{
		Unknown,
		WaitingForSound,
		FollowingSound,
		FacingSound
	}

	/// <summary>
    /// SoundFollower state
    /// </summary>
    [DataContract]
    public class SoundFollowerState
    {
		[DataMember]
		public double MinConfidenceLevel { get; set; }
		[DataMember]
		public double MaxLateralSpeed { get; set; }
		[DataMember]
		public SoundFollowerLogicalState CurrentState { get; set; }

		[DataMember]
		public double CurrentConfidenceLevel { get; set; }
		[DataMember]
		public double CurrentSoundAngle { get; set; }

		[DataMember]
		public bool Enabled { get; set; }

		public SoundFollowerState()
		{
			this.Enabled = true;
		}
	}

    /// <summary>
    /// SoundFollower main operations port
    /// </summary>
    [ServicePort]
	public class SoundFollowerOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get, EnableBehavior, EnabledChanged, StateChangeNotify, Subscribe>
    {
    }

    /// <summary>
    /// SoundFollower get operation
    /// </summary>
    public class Get : Get<GetRequestType, PortSet<SoundFollowerState, Fault>>
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
        public Get(GetRequestType body, PortSet<SoundFollowerState, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }

	public class StateChangeNotify : Update<SoundFollowerState, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public StateChangeNotify()
		{
		}

		public StateChangeNotify(SoundFollowerState body)
			: base(body)
		{
		}

		public StateChangeNotify(SoundFollowerState body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}

	public class EnableBehavior : Update<EnableBehaviorState, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public EnableBehavior()
		{
		}

		public EnableBehavior(EnableBehaviorState enable)
			: base(enable)
		{
		}

		public EnableBehavior(EnableBehaviorState enable, PortSet<DefaultUpdateResponseType, Fault> responsePort)
			: base(enable, responsePort)
		{
		}
	}

	[DataContract]
	public class EnableBehaviorState
	{
		[DataMember]
		[DataMemberConstructor]
		public bool Enabled { get; set; }

		public EnableBehaviorState()
		{
		}
		public EnableBehaviorState(bool enable)
		{
			this.Enabled = enable;
		}
	}

	public class EnabledChanged : Update<EnableBehaviorChangeNotify, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public EnabledChanged()
		{
		}

		public EnabledChanged(EnableBehaviorChangeNotify enable)
			: base(enable)
		{
		}

		public EnabledChanged(EnableBehaviorChangeNotify enable, PortSet<DefaultUpdateResponseType, Fault> responsePort)
			: base(enable, responsePort)
		{
		}
	}

	[DataContract]
	public class EnableBehaviorChangeNotify
	{
		[DataMember]
		[DataMemberConstructor]
		public bool Enabled { get; set; }

		public EnableBehaviorChangeNotify()
		{
		}
		public EnableBehaviorChangeNotify(bool enable)
		{
			this.Enabled = enable;
		}
	}

	/// <summary>
    /// SoundFollower subscribe operation
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


