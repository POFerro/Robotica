using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using System.Linq;

using submgr = Microsoft.Dss.Services.SubscriptionManager;
using drive = Microsoft.Robotics.Services.Drive.Proxy;
using kinectproxy = Microsoft.Robotics.Services.Sensors.Kinect.Proxy;
using kinect = Microsoft.Robotics.Services.Sensors.Kinect;
using mskinect = Microsoft.Kinect;

using ccrwpf = Microsoft.Ccr.Adapters.Wpf;
using System.Windows;

namespace POFerro.Robotics.SkeletonFollower
{
    [Contract(Contract.Identifier)]
    [DisplayName("SkeletonFollower")]
    [Description("SkeletonFollower service (no description provided)")]
    class SkeletonFollowerService : DsspServiceBase
    {
		/// <summary>
		/// Minimum valid depth reading in millimeters
		/// </summary>
		private const int MinValidDepth = 800;

		/// <summary>
		/// Maximum valid depth reading in millimeters
		/// </summary>
		private const int MaxValidDepth = 4000;
		
		/// <summary>
        /// Service state
        /// </summary>
        [ServiceState]
        SkeletonFollowerState _state = new SkeletonFollowerState();

        /// <summary>
        /// Main service port
        /// </summary>
        [ServicePort("/SkeletonFollower", AllowMultipleInstances = true)]
        SkeletonFollowerOperations _mainPort = new SkeletonFollowerOperations();

        [SubscriptionManagerPartner]
        submgr.SubscriptionManagerPort _submgrPort = new submgr.SubscriptionManagerPort();

        /// <summary>
        /// KinectService partner
        /// </summary>
		[Partner("KinectService", Contract = kinectproxy.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		kinectproxy.KinectOperations _kinectServicePort = new kinectproxy.KinectOperations();

        /// <summary>
        /// DriveDifferentialTwoWheel partner
        /// </summary>
        [Partner("DriveDifferentialTwoWheel", Contract = drive.Contract.Identifier, Optional=true, CreationPolicy = PartnerCreationPolicy.UseExisting)]
        drive.DriveOperations _driveDifferentialTwoWheelPort = new drive.DriveOperations();

		SkeletonFollowerDashboardWPF userInterface;
		/// <summary>
		/// WPF service port
		/// </summary>
		private ccrwpf.WpfServicePort wpfServicePort;

        /// <summary>
        /// Service constructor
        /// </summary>
        public SkeletonFollowerService(DsspServiceCreationPort creationPort)
            : base(creationPort)
        {
        }

        /// <summary>
        /// Service start
        /// </summary>
        protected override void Start()
        {

            // 
            // Add service specific initialization here
            // 

            base.Start();

//			SpawnIterator(this.InitializeDashboard);

			Activate(_kinectServicePort.UpdateTilt(new kinectproxy.UpdateTiltRequest() { Tilt = 5 }).Choice());
			Activate(Arbiter.ReceiveWithIterator(false, TimeoutPort(1000), this.BehaviourLoop));
		}

		private IEnumerator<ITask> InitializeDashboard()
		{
			// create WPF adapter
			this.wpfServicePort = ccrwpf.WpfAdapter.Create(TaskQueue);

			var runWindow = this.wpfServicePort.RunWindow(() => new SkeletonFollowerDashboardWPF());
			yield return (Choice)runWindow;

			var exception = (Exception)runWindow;
			if (exception != null)
			{
				LogError(exception);
				StartFailed();
				yield break;
			}

			// need double cast because WPF adapter doesn't know about derived window types
			this.userInterface = (Window)runWindow as SkeletonFollowerDashboardWPF;
		}

		int s = 5;            // rotation time ms/degree  (16 on carpet and 8 on wooden floor)
		private IEnumerator<ITask> BehaviourLoop(DateTime message)
		{
			int timeoutValue = 30;

			var lastLogicalState = _state.CurrentState;
			double lastLeftSpeed = _state.LeftWheelPower;
			double lastRightSpeed = _state.RightWheelPower;

			kinectproxy.QueryRawFrameRequest frameRequest = new kinectproxy.QueryRawFrameRequest();
			frameRequest.IncludeDepth = false;
			frameRequest.IncludeVideo = false;
			frameRequest.IncludeSkeletons = true;

			kinect.RawKinectFrames rawFrames = null;

			// poll skeleton camera
			yield return this._kinectServicePort.QueryRawFrame(frameRequest).Choice(
				rawFrameResponse =>
				{
					rawFrames = rawFrameResponse.RawFrames;
				},
				failure =>
				{
				});

			_state.Timestamp = rawFrames.RawSkeletonFrameData.Timestamp;

			var skeleton = rawFrames.RawSkeletonFrameData.SkeletonData.FirstOrDefault(s => s.TrackingState != mskinect.SkeletonTrackingState.NotTracked && s.TrackingId == _state.CurrentFollowedPlayer);
			if (skeleton == null)
				skeleton = rawFrames.RawSkeletonFrameData.SkeletonData.FirstOrDefault(s => s.TrackingState != mskinect.SkeletonTrackingState.NotTracked);
			if (skeleton != null)
			{
				_state.CurrentFollowedPlayer = skeleton.TrackingId;
				_state.SkeletonPosition = skeleton.Position;

				kinectproxy.SkeletonToDepthImageRequest request = new kinectproxy.SkeletonToDepthImageRequest();
				request.SkeletonVector = skeleton.Position;

				yield return this._kinectServicePort.SkeletonToDepthImage(request).Choice(
						SkeletonToDepthImageResponse =>
						{
							_state.FollowedSkeletonDepthPosition = SkeletonToDepthImageResponse.DepthPoint;
						},
						failure =>
						{
							// high freq call - no logging
						});
				if (_state.FollowedSkeletonDepthPosition.Depth <= 1500)
				{
					_state.CurrentState = SkeletonFollowerLogicalState.NearSkeleton;
					_state.CurrentAction = FollowingAction.Stopped;
						
					_state.LeftWheelPower = 0;
					_state.RightWheelPower = 0;
				}
				else if (_state.FollowedSkeletonDepthPosition.Depth > 1500)
				{
					_state.CurrentState = SkeletonFollowerLogicalState.ApproachingSkeleton;
					_state.CurrentAction = FollowingAction.MoveForward;

					_state.LeftWheelPower = _state.MaxSpeed;
					_state.RightWheelPower = _state.MaxSpeed;
				}

				_state.FollowedSkeletonMaxXPosition = (int)(Math.Cos(kinect.KinectCameraConstants.HorizontalFieldOfViewRadians) * ((double)_state.FollowedSkeletonDepthPosition.Depth));
				int goodTunnelWidth = (int)_state.FollowedSkeletonMaxXPosition / 4;
				_state.FollowedSkeletonLeftLimit = (_state.FollowedSkeletonMaxXPosition / 2 - goodTunnelWidth);
				_state.FollowedSkeletonRightLimit = (_state.FollowedSkeletonMaxXPosition / 2 + goodTunnelWidth);
				if (_state.SkeletonPosition.X < -0.4)
				{
					_state.CurrentState = SkeletonFollowerLogicalState.ApproachingSkeleton;

					if (_state.LeftWheelPower == 0 && _state.RightWheelPower == 0)
					{
						_state.LeftWheelPower = _state.MaxLateralSpeed;
						_state.RightWheelPower = -_state.MaxLateralSpeed;
						_state.CurrentAction = FollowingAction.TurnRight;
						timeoutValue = s * 2; // Cinco graus
					}
					else
					{
						_state.CurrentAction = FollowingAction.AdjustRight;
						_state.RightWheelPower /= 1.5;
						timeoutValue = s * 2; // Cinco graus
					}
				}
				else if (_state.SkeletonPosition.X > 0.4)
				{
					_state.CurrentState = SkeletonFollowerLogicalState.ApproachingSkeleton;

					if (_state.LeftWheelPower == 0 && _state.RightWheelPower == 0)
					{
						_state.LeftWheelPower = -_state.MaxLateralSpeed;
						_state.RightWheelPower = _state.MaxLateralSpeed;
						_state.CurrentAction = FollowingAction.TurnLeft;
						timeoutValue = s * 2; // Cinco graus
					}
					else
					{
						_state.CurrentAction = FollowingAction.AdjustLeft;
						_state.LeftWheelPower /= 1.5;
						timeoutValue = s * 2; // Cinco graus
					}
				}
			}
			else
			{
				_state.LeftWheelPower = 0;
				_state.RightWheelPower = 0;
				_state.CurrentState = SkeletonFollowerLogicalState.SearchingSkeleton;
				_state.CurrentAction = FollowingAction.Stopped;
			}

			if (this.userInterface != null)
				this.userInterface.UpdateState(_state);

			if (!(_state.LeftWheelPower == 0 && _state.RightWheelPower == 0 && lastLeftSpeed == 0 && lastRightSpeed == 0))
				yield return _driveDifferentialTwoWheelPort.SetDrivePower(_state.LeftWheelPower, _state.RightWheelPower).Choice();

			Activate(Arbiter.ReceiveWithIterator(false, TimeoutPort(timeoutValue), this.BehaviourLoop));

			if (lastLogicalState != SkeletonFollowerLogicalState.SearchingSkeleton || 
				_state.CurrentState != SkeletonFollowerLogicalState.SearchingSkeleton)
				base.SendNotification(_submgrPort, new StateChangeNotify(_state));
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


