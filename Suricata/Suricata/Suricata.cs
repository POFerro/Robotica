using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

using submgr = Microsoft.Dss.Services.SubscriptionManager;
using follower = POFerro.Robotics.SkeletonFollower.Proxy;
using soundfollower = POFerro.Robotics.SoundFollower.Proxy;
using servo = POFerro.Robotics.ArduinoGenericServo.Proxy;

namespace POFerro.Robotics.Suricata
{
	[Contract(Contract.Identifier)]
	[DisplayName("Suricata")]
	[Description("Suricata service (no description provided)")]
	class SuricataService : DsspServiceBase
	{
		[ServiceState]
		SuricataState _state = new SuricataState();
		
		[ServicePort("/Suricata", AllowMultipleInstances = false)]
		SuricataOperations _mainPort = new SuricataOperations();

		[Partner("SkeletonFollowerService", Contract = follower.Contract.Identifier, Optional = true, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
		follower.SkeletonFollowerOperations _followerServicePort = new follower.SkeletonFollowerOperations();
		follower.SkeletonFollowerOperations _followerServiceNotify = new follower.SkeletonFollowerOperations();

		[Partner("SoundFollowerService", Contract = soundfollower.Contract.Identifier, Optional = true, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
		soundfollower.SoundFollowerOperations _soundFollowerServicePort = new soundfollower.SoundFollowerOperations();
		soundfollower.SoundFollowerOperations _soundFollowerServiceNotify = new soundfollower.SoundFollowerOperations();

		/// <summary>
		/// DriveDifferentialTwoWheel partner
		/// </summary>
		[Partner("KinectServo", Contract = servo.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		servo.ArduinoGenericServoOperations _kinectServoPort = new servo.ArduinoGenericServoOperations();

		/// <summary>
		/// Subscription manager port
		/// </summary>
		[SubscriptionManagerPartner]
		private submgr.SubscriptionManagerPort submgr = new submgr.SubscriptionManagerPort();
		
		public SuricataService(DsspServiceCreationPort creationPort)
			: base(creationPort)
		{
			this.PartnerEnumerationTimeout = TimeSpan.FromSeconds(5);
		}
		
		protected override void Start()
		{
			base.Start();

			if (this._followerServicePort != null)
				_followerServicePort.Subscribe(_followerServiceNotify, typeof(follower.StateChangeNotify));
			if (this._soundFollowerServicePort != null)
				this._soundFollowerServicePort.Subscribe(this._soundFollowerServiceNotify, typeof(soundfollower.StateChangeNotify));

			this.MainPortInterleave.CombineWith(
				new Interleave(
					new TeardownReceiverGroup(),
					new ExclusiveReceiverGroup(
					),
					new ConcurrentReceiverGroup(
						Arbiter.ReceiveWithIterator<follower.StateChangeNotify>(true, _followerServiceNotify, FollowerStateChangeHandler),
						Arbiter.ReceiveWithIterator<soundfollower.StateChangeNotify>(true, _soundFollowerServiceNotify, SoundFollowerStateChangeHandler)
					)));

			Activate(Arbiter.ReceiveWithIterator(false, TimeoutPort(5000), BehaviourLoop));
		}

		private IEnumerator<ITask> SoundFollowerStateChangeHandler(soundfollower.StateChangeNotify message)
		{
			_state.SoundFollowerState = message.Body;
			if (_state.SoundFollowerState.CurrentState == soundfollower.SoundFollowerLogicalState.FollowingSound)
			{
				if (_state.SkeletonFollowerState.CurrentState != follower.SkeletonFollowerLogicalState.ApproachingSkeleton)
				{
					yield return _soundFollowerServicePort.EnableBehavior(true).Choice();
					yield return _followerServicePort.EnableBehavior(false).Choice();
				}
				else
					yield return _soundFollowerServicePort.EnableBehavior(false).Choice();
				_state.CurrentState = SuricataLogicalState.FollowingSound;
			}
			else
			{
				yield return _followerServicePort.EnableBehavior(true).Choice();
				_state.CurrentState = SuricataLogicalState.Unknown;
			}

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);

			base.SendNotification(submgr, new StateChangeNotify(_state));
			yield break;
		}

		private IEnumerator<ITask> FollowerStateChangeHandler(follower.StateChangeNotify message)
		{
			_state.SkeletonFollowerState = message.Body;
			if (_state.SkeletonFollowerState.CurrentState == follower.SkeletonFollowerLogicalState.ApproachingSkeleton)
			{
				_state.CurrentState = SuricataLogicalState.FollowingShepherd;
				yield return _soundFollowerServicePort.EnableBehavior(false).Choice();
				yield return _followerServicePort.EnableBehavior(true).Choice();
			}
			else
			{
				_state.CurrentState = SuricataLogicalState.Unknown;
				yield return _soundFollowerServicePort.EnableBehavior(true).Choice();
			}

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);

			base.SendNotification(submgr, new StateChangeNotify(_state));
			yield break;
		}

		private IEnumerator<ITask> BehaviourLoop(DateTime message)
		{
			for (int i = 15; i < 180; i += 2)
			{
				yield return _kinectServoPort.MoveServo(i).Choice();
				yield return Timeout(50);
			}
			for (int i = 180; i > 110; i -= 2)
			{
				yield return _kinectServoPort.MoveServo(i).Choice();
				yield return Timeout(50);
			}
		}

//		int s = 7;            // rotation time ms/degree  (16 on carpet and 8 on wooden floor)
//		private IEnumerator<ITask> BehaviourLoop(DateTime timer)
//		{
//			int timeoutValue = 30;
//			double lastLeftSpeed = _state.LeftWheelPower;
//			double lastRightSpeed = _state.RightWheelPower;

//			if (Math.Min(Math.Min(_state.LastCenterIRReading.NormalizedMeasurement, 
//						_state.LastLeftSonarReading.NormalizedMeasurement), 
//						_state.LastRightSonarReading.NormalizedMeasurement) < _state.IRSafeDistance ||
//				Math.Min(_state.LastLeftIRReading.NormalizedMeasurement, 
//						 _state.LastRightIRReading.NormalizedMeasurement) < _state.IRLateralSafeDistance)
//			{
//				if (_state.LastLeftIRReading.NormalizedMeasurement > _state.IRLateralSafeDistance + _state.IRDistanceDiferenceToAdjust &&
//					_state.LastLeftSonarReading.NormalizedMeasurement > _state.IRSafeDistance + _state.IRDistanceDiferenceToAdjust)
//				{
//					// Turn left
//					_state.CurrentState = SuricataLogicalState.TurnLeft;
//					_state.LeftWheelPower = -_state.MaxLateralSpeed;
//					_state.RightWheelPower = _state.MaxLateralSpeed;
////					timeoutValue = s * 5; // Cinco graus
//				}
//				else if (_state.LastRightIRReading.NormalizedMeasurement > _state.IRLateralSafeDistance + _state.IRDistanceDiferenceToAdjust &&
//					_state.LastRightSonarReading.NormalizedMeasurement > _state.IRSafeDistance + _state.IRDistanceDiferenceToAdjust)
//				{
//					// Turn right
//					_state.CurrentState = SuricataLogicalState.TurnRight;
//					_state.LeftWheelPower = _state.MaxLateralSpeed;
//					_state.RightWheelPower = -_state.MaxLateralSpeed;
////					timeoutValue = s * 5; // Cinco graus
//				}
//				else
//				{
//					if (_state.LastLeftIRReading.NormalizedMeasurement > _state.LastRightIRReading.NormalizedMeasurement)
//					{
//						_state.CurrentState = SuricataLogicalState.ReverseLeft;
//						_state.LeftWheelPower = -_state.MaxLateralSpeed;
//						_state.RightWheelPower = _state.MaxLateralSpeed;
//						timeoutValue = s * 90; // Reverse
//					}
//					else
//					{
//						_state.CurrentState = SuricataLogicalState.ReverseRight;
//						_state.LeftWheelPower = _state.MaxLateralSpeed;
//						_state.RightWheelPower = -_state.MaxLateralSpeed;
//						timeoutValue = s * 90; // Reverse
//					}
//				}
//			}
//			else
//			{
//				if (Math.Abs(_state.LastLeftIRReading.NormalizedMeasurement - _state.LastRightIRReading.NormalizedMeasurement) > _state.IRDistanceDiferenceToAdjust)
//					if (_state.LastLeftIRReading.NormalizedMeasurement > _state.LastRightIRReading.NormalizedMeasurement)
//					{
//						_state.CurrentState = SuricataLogicalState.AdjustLeft;
//						_state.LeftWheelPower = _state.MaxSpeed / 2;
//						_state.RightWheelPower = _state.MaxSpeed;
////						timeoutValue = s * 5; // 2 graus
//					}
//					else
//					{
//						_state.LeftWheelPower = _state.MaxSpeed;
//						_state.RightWheelPower = _state.MaxSpeed / 2;
//						_state.CurrentState = SuricataLogicalState.AdjustRight;
////						timeoutValue = s * 5; // 2 graus
//					}
//				else if (Math.Abs(_state.LastLeftSonarReading.NormalizedMeasurement - _state.LastRightSonarReading.NormalizedMeasurement) > _state.SonarDistanceDiferenceToAdjust)
//					if (_state.LastLeftSonarReading.NormalizedMeasurement > _state.LastRightSonarReading.NormalizedMeasurement)
//					{
//						_state.CurrentState = SuricataLogicalState.AdjustLeft;
//						_state.LeftWheelPower = _state.MaxSpeed / 2;
//						_state.RightWheelPower = _state.MaxSpeed;
////						timeoutValue = s * 5; // 2 graus
//					}
//					else
//					{
//						_state.LeftWheelPower = _state.MaxSpeed;
//						_state.RightWheelPower = _state.MaxSpeed / 2;
//						_state.CurrentState = SuricataLogicalState.AdjustRight;
////						timeoutValue = s * 5; // 2 graus
//					}

//				else
//				{
//					_state.LeftWheelPower = _state.MaxSpeed;
//					_state.RightWheelPower = _state.MaxSpeed;
//					_state.CurrentState = SuricataLogicalState.MoveForward;
//				}
//			}

//			if (lastLeftSpeed != _state.LeftWheelPower || lastRightSpeed != _state.RightWheelPower)
//				yield return _driveDifferentialTwoWheelPort.SetDrivePower(_state.LeftWheelPower, _state.RightWheelPower).Choice();

//			Activate(Arbiter.ReceiveWithIterator(false, TimeoutPort(timeoutValue), BehaviourLoop));

//			base.SendNotification(submgr, new StateChangeNotify(_state));
//			yield break;
//		}

		[ServiceHandler]
		public void SubscribeHandler(Subscribe subscribe)
		{
			SubscribeHelper(submgr, subscribe.Body, subscribe.ResponsePort);
		}
	}
}


