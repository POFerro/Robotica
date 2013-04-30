using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

using submgr = Microsoft.Dss.Services.SubscriptionManager;
using kinect = Microsoft.Robotics.Services.Sensors.Kinect.Proxy;
//using kinectsoundtracker = POFerro.Robotics.KinectSoundTracker.Proxy;
using drive = Microsoft.Robotics.Services.Drive.Proxy;

namespace POFerro.Robotics.SoundFollower
{
    [Contract(Contract.Identifier)]
    [DisplayName("SoundFollower")]
    [Description("SoundFollower service (no description provided)")]
    class SoundFollowerService : DsspServiceBase
    {
        /// <summary>
        /// Service state
        /// </summary>
        [ServiceState]
		[InitialStatePartner(Optional = true, ServiceUri = "SoundFollowerService.Config.xml")]
		SoundFollowerState _state = new SoundFollowerState();

        /// <summary>
        /// Main service port
        /// </summary>
        [ServicePort("/SoundFollower", AllowMultipleInstances = true)]
        SoundFollowerOperations _mainPort = new SoundFollowerOperations();

        [SubscriptionManagerPartner]
        submgr.SubscriptionManagerPort _submgrPort = new submgr.SubscriptionManagerPort();

		///// <summary>
		///// KinectSoundTrackerService partner
		///// </summary>
		//[Partner("KinectSoundTrackerService", Contract = kinectsoundtracker.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExistingOrCreate)]
		//kinectsoundtracker.KinectSoundTrackerOperations _kinectSoundTrackerServicePort = new kinectsoundtracker.KinectSoundTrackerOperations();
		//kinectsoundtracker.KinectSoundTrackerOperations _kinectSoundTrackerServiceNotify = new kinectsoundtracker.KinectSoundTrackerOperations();

		/// <summary>
		/// KinectSoundTrackerService partner
		/// </summary>
		[Partner("Kinect", Contract = kinect.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExistingOrCreate)]
		kinect.KinectOperations _kinectSoundTrackerServicePort = new kinect.KinectOperations();
		kinect.KinectOperations _kinectSoundTrackerServiceNotify = new kinect.KinectOperations();

        /// <summary>
        /// DriveDifferentialTwoWheel partner
        /// </summary>
        [Partner("DriveDifferentialTwoWheel", Contract = drive.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
        drive.DriveOperations _driveDifferentialTwoWheelPort = new drive.DriveOperations();

        /// <summary>
        /// Service constructor
        /// </summary>
        public SoundFollowerService(DsspServiceCreationPort creationPort)
            : base(creationPort)
        {
        }

        /// <summary>
        /// Service start
        /// </summary>
        protected override void Start()
        {
			if (_state == null)
			{
				_state = new SoundFollowerState();
				_state.MinConfidenceLevel = 0.8;
				_state.MaxLateralSpeed = 0.7;
				this.SaveState(_state);
			}
            _kinectSoundTrackerServicePort.Subscribe(_kinectSoundTrackerServiceNotify);
			this._state.Enabled = true;

            base.Start();

			this.MainPortInterleave.CombineWith(
				new Interleave(
					new TeardownReceiverGroup(),
					new ExclusiveReceiverGroup(
					),
					new ConcurrentReceiverGroup(
						//Arbiter.ReceiveWithIterator<kinectsoundtracker.SoundSourceAngleChanged>(true, _kinectSoundTrackerServiceNotify, OnNewSoundTrackerAngle)
						Arbiter.ReceiveWithIterator<kinect.SoundSourceAngleChanged>(true, _kinectSoundTrackerServiceNotify, OnNewSoundAngle)//,
//						Arbiter.Receive<Enable>(true, _mainPort, EnableHandler)
					)));
		}

		//private IEnumerator<ITask> OnNewSoundTrackerAngle(kinectsoundtracker.SoundSourceAngleChanged message)
		//{
		//	if (message.Body.CurrentConfidenceLevel > _state.MinConfidenceLevel)
		//	{
		//		this._state.CurrentConfidenceLevel = message.Body.CurrentConfidenceLevel;
		//		this._state.CurrentSoundAngle = message.Body.CurrentAngle;
		//		if (Math.Abs(this._state.CurrentSoundAngle) < 10)
		//			this._state.CurrentState = SoundFollowerLogicalState.FacingSound;
		//		else
		//		{
		//			this._state.CurrentState = SoundFollowerLogicalState.FollowingSound;
		//			yield return _driveDifferentialTwoWheelPort.RotateDegrees(this._state.CurrentSoundAngle, _state.MaxLateralSpeed).Choice();
		//		}
		//		base.SendNotification(_submgrPort, new StateChangeNotify(_state));
		//	}
		//	message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		//}

		private IEnumerator<ITask> OnNewSoundAngle(kinect.SoundSourceAngleChanged message)
		{
			if (message.Body.CurrentConfidenceLevel > _state.MinConfidenceLevel)
			{
				this._state.CurrentConfidenceLevel = message.Body.CurrentConfidenceLevel;
				this._state.CurrentSoundAngle = message.Body.CurrentAngle;
				
				if (Math.Abs(this._state.CurrentSoundAngle) < 10)
					this._state.CurrentState = SoundFollowerLogicalState.FacingSound;
				else
				{
					this._state.CurrentState = SoundFollowerLogicalState.FollowingSound;
					if (_state.Enabled)
						yield return _driveDifferentialTwoWheelPort.RotateDegrees(Math.Sign(this._state.CurrentSoundAngle)*5, _state.MaxLateralSpeed).Choice();
				}
			}
			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);

			if (message.Body.CurrentConfidenceLevel > _state.MinConfidenceLevel)
				base.SendNotification(_submgrPort, new StateChangeNotify(_state));
	
			yield break;
		}

		public static double DegreeToRadian(double degree)
		{
			return degree * (Math.PI / 180.0);
		}

		public static double RadianToDegree(double radian)
		{
			return radian * (180.0 / Math.PI);
		}

		[ServiceHandler(ServiceHandlerBehavior.Concurrent)]
		public void GetHandler(Get get)
		{
			get.ResponsePort.Post(this._state);
		}

		[ServiceHandler(ServiceHandlerBehavior.Concurrent)]
		public void EnableHandler(EnableBehavior enable)
		{
			_state.Enabled = enable.Body.Enabled;

			enable.ResponsePort.Post(DefaultUpdateResponseType.Instance);

			this.SendNotification(_submgrPort, new EnabledChanged(new EnableBehaviorChangeNotify(_state.Enabled)));
		}
		
		/// <summary>
        /// Handles Subscribe messages
        /// </summary>
        /// <param name="subscribe">the subscribe request</param>
        [ServiceHandler]
        public void SubscribeHandler(Subscribe subscribe)
        {
            SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
        }
    }
}


