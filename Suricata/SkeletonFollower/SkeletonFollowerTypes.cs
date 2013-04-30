using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

using mskinect = Microsoft.Kinect;


namespace POFerro.Robotics.SkeletonFollower
{
    /// <summary>
    /// SkeletonFollower contract class
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
        /// DSS contract identifer for SkeletonFollower
        /// </summary>
        [DataMember]
        public const string Identifier = "http://schemas.pferro/2013/04/skeletonfollower.html";
    }

	public enum SkeletonFollowerLogicalState
	{
		Unknown,
		SearchingSkeleton,
		ApproachingSkeleton,
		NearSkeleton
	}

	public enum FollowingAction
	{
		Unknown,
		MoveForward,
		AdjustLeft,
		AdjustRight,
		TurnLeft,
		TurnRight,
		Stopped
	}

	/// <summary>
    /// SkeletonFollower state
    /// </summary>
    [DataContract]
    public class SkeletonFollowerState
    {
		[DataMember]
		public int CurrentFollowedPlayer { get; set; }
		[DataMember]
		public SkeletonFollowerLogicalState CurrentState { get; set; }
		[DataMember]
		public FollowingAction CurrentAction { get; set; }
		[DataMember]
		public mskinect.DepthImagePoint FollowedSkeletonDepthPosition { get; set; }
		[DataMember]
		public mskinect.SkeletonPoint SkeletonPosition { get; set; }
		[DataMember]
		public int FollowedSkeletonMaxXPosition { get; set; }

		[DataMember]
		public int FollowedSkeletonLeftLimit { get; set; }

		[DataMember]
		public int FollowedSkeletonRightLimit { get; set; }

		public virtual double MaxSpeed { get { return 1.0; } }
		public virtual double MaxLateralSpeed { get { return 0.8; } }

		[DataMember]
		public double LeftWheelPower { get; set; }
		[DataMember]
		public double RightWheelPower { get; set; }

		[DataMember]
		public long Timestamp { get; set; }

		[DataMember]
		public bool Enabled { get; set; }

		public SkeletonFollowerState()
		{
			this.CurrentFollowedPlayer = -1;
			this.Enabled = true;
		}
	}

    /// <summary>
    /// SkeletonFollower main operations port
    /// </summary>
    [ServicePort]
	public class SkeletonFollowerOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get, EnableBehavior, EnabledChanged, StateChangeNotify, Subscribe>
    {
    }

    /// <summary>
    /// SkeletonFollower get operation
    /// </summary>
    public class Get : Get<GetRequestType, PortSet<SkeletonFollowerState, Fault>>
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
        public Get(GetRequestType body, PortSet<SkeletonFollowerState, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }

	public class StateChangeNotify : Update<SkeletonFollowerState, PortSet<DefaultUpdateResponseType, Fault>>
	{
		public StateChangeNotify()
		{
		}

		public StateChangeNotify(SkeletonFollowerState body)
			: base(body)
		{
		}

		public StateChangeNotify(SkeletonFollowerState body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
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
    /// SkeletonFollower subscribe operation
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


