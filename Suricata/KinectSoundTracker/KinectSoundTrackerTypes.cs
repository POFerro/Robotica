using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

namespace POFerro.Robotics.KinectSoundTracker
{
    /// <summary>
    /// KinectSoundTracker contract class
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
        /// DSS contract identifer for KinectSoundTracker
        /// </summary>
        [DataMember]
        public const string Identifier = "http://schemas.pferro/2013/04/kinectsoundtracker.html";
    }

    /// <summary>
    /// KinectSoundTracker state
    /// </summary>
    [DataContract]
    public class KinectSoundTrackerState
    {
		[DataMember]
		public double CurrentAngle { get; set; }
		[DataMember]
		public double CurrentConfidenceLevel { get; set; }
	}

    /// <summary>
    /// KinectSoundTracker main operations port
    /// </summary>
    [ServicePort]
	public class KinectSoundTrackerOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get, SoundSourceAngleChanged, Subscribe>
    {
    }

    /// <summary>
    /// KinectSoundTracker get operation
    /// </summary>
    public class Get : Get<GetRequestType, PortSet<KinectSoundTrackerState, Fault>>
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
        public Get(GetRequestType body, PortSet<KinectSoundTrackerState, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }

	/// <summary>
	/// KinectSoundTracker get operation
	/// </summary>
	public class SoundSourceAngleChanged : Update<KinectSoundTrackerState, PortSet<DefaultUpdateResponseType, Fault>>
	{
		/// <summary>
		/// Creates a new instance of Get
		/// </summary>
		public SoundSourceAngleChanged()
		{
		}

		/// <summary>
		/// Creates a new instance of Get
		/// </summary>
		/// <param name="body">the request message body</param>
		public SoundSourceAngleChanged(KinectSoundTrackerState body)
			: base(body)
		{
		}

		/// <summary>
		/// Creates a new instance of Get
		/// </summary>
		/// <param name="body">the request message body</param>
		/// <param name="responsePort">the response port for the request</param>
		public SoundSourceAngleChanged(KinectSoundTrackerState body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}

    /// <summary>
    /// KinectSoundTracker subscribe operation
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


