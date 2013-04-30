using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

using submgr = Microsoft.Dss.Services.SubscriptionManager;
using sonarturret = POFerro.Robotics.ArduinoSonarTurret.Proxy;
using drive = Microsoft.Robotics.Services.Drive.Proxy;
using ir = Microsoft.Robotics.Services.AnalogSensor.Proxy;
using irrecv = POFerro.Robotics.ArduinoGenericInfraredReceiver.Proxy;

namespace POFerro.Robotics.Wanderer
{
	[Contract(Contract.Identifier)]
	[DisplayName("Wanderer")]
	[Description("Wanderer service (no description provided)")]
	public class WandererService : DsspServiceBase
	{
		[ServiceState]
		WandererState _state = new WandererState();
		
		[ServicePort("/Wanderer", AllowMultipleInstances = false)]
		WandererOperations _mainPort = new WandererOperations();

		/// <summary>
		/// DriveDifferentialTwoWheel partner
		/// </summary>
		[Partner("DriveDifferentialTwoWheel", Contract = drive.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		drive.DriveOperations _driveDifferentialTwoWheelPort = new drive.DriveOperations();

		/// <summary>
		/// Sonar partner
		/// </summary>
		[Partner("SonarTurret", Contract = sonarturret.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		sonarturret.ArduinoSonarTurretOperations _sonarTurretPort = new sonarturret.ArduinoSonarTurretOperations();
		sonarturret.ArduinoSonarTurretOperations _sonarTurretNotify = new sonarturret.ArduinoSonarTurretOperations();

		/// <summary>
		/// Right Infrared partner
		/// </summary>
		[Partner("Sonar", Contract = ir.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		ir.AnalogSensorOperations _sonarPort = new ir.AnalogSensorOperations();
		ir.AnalogSensorOperations _sonarNotify = new ir.AnalogSensorOperations();

		/// <summary>
		/// Right Infrared partner
		/// </summary>
		[Partner("IRRight", Contract = ir.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		ir.AnalogSensorOperations _irRightPort = new ir.AnalogSensorOperations();
		ir.AnalogSensorOperations _irRightNotify = new ir.AnalogSensorOperations();

		/// <summary>
		/// Left Infrared partner
		/// </summary>
		[Partner("IRLeft", Contract = ir.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		ir.AnalogSensorOperations _irLeftPort = new ir.AnalogSensorOperations();
		ir.AnalogSensorOperations _irLeftNotify = new ir.AnalogSensorOperations();

		/// <summary>
		/// Right Infrared partner
		/// </summary>
		[Partner("IRReceiver", Contract = irrecv.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		irrecv.ArduinoGenericInfraredReceiverOperations _irRecvPort = new irrecv.ArduinoGenericInfraredReceiverOperations();
		irrecv.ArduinoGenericInfraredReceiverOperations _irRecvNotify = new irrecv.ArduinoGenericInfraredReceiverOperations();

		/// <summary>
		/// Subscription manager port
		/// </summary>
		[SubscriptionManagerPartner]
		private submgr.SubscriptionManagerPort submgr = new submgr.SubscriptionManagerPort();

		public WandererService(DsspServiceCreationPort creationPort)
			: base(creationPort)
		{
		}

		protected override void Start()
		{
			base.Start();

			this._sonarPort.Subscribe(_sonarNotify, typeof(ir.Replace));
			this._sonarTurretPort.Subscribe(_sonarTurretNotify, typeof(sonarturret.RangeSweepCompleteNotify));
			this._irRightPort.Subscribe(_irRightNotify, typeof(ir.Replace));
			this._irLeftPort.Subscribe(_irLeftNotify, typeof(ir.Replace));
			this._irRecvPort.Subscribe(_irRecvNotify, typeof(irrecv.InfreredCommandNotify));

			this.MainPortInterleave.CombineWith(
				new Interleave(
					new TeardownReceiverGroup(),
					new ExclusiveReceiverGroup(
					),
					new ConcurrentReceiverGroup(
						Arbiter.Receive<ir.Replace>(true, _sonarNotify, OnSonar),
						Arbiter.Receive<ir.Replace>(true, _irLeftNotify, OnInfraredLeft),
						Arbiter.Receive<ir.Replace>(true, _irRightNotify, OnInfraredRight),
						Arbiter.ReceiveWithIterator<sonarturret.RangeSweepCompleteNotify>(true, _sonarTurretNotify, OnRangeSweepComplete),
						Arbiter.Receive<irrecv.InfreredCommandNotify>(true, _irRecvNotify, OnInfraredCommandReceived)
					)));

			Activate(Arbiter.ReceiveWithIterator(false, TimeoutPort(1000), BehaviourLoop));
		}

		private void OnInfraredCommandReceived(irrecv.InfreredCommandNotify message)
		{
			if (message.Body.ReceivedCommand != 0)
				Console.WriteLine("Got command ir: " + message.Body.ReceivedCommand);
			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void OnSonar(ir.Replace message)
		{
			_state.LastSonarReading = message.Body;

			base.SendNotification(submgr, new StateChangeNotify(_state));

			message.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		private void OnInfraredLeft(ir.Replace message)
		{
			_state.LastLeftIRReading = message.Body;

			base.SendNotification(submgr, new StateChangeNotify(_state));

			message.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		private void OnInfraredRight(ir.Replace message)
		{
			_state.LastRightIRReading = message.Body;

			base.SendNotification(submgr, new StateChangeNotify(_state));

			message.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		int s = 8;            // rotation time ms/degree  (16 on carpet and 8 on wooden floor)
		private IEnumerator<ITask> BehaviourLoop(DateTime timer)
		{
			if (_state.CurrentState == WandererLogicalState.Ranging)
				yield break;

			int timeoutValue = 30;
			double lastLeftSpeed = _state.LeftWheelPower;
			double lastRightSpeed = _state.RightWheelPower;

			if (_state.LastSonarReading.NormalizedMeasurement < _state.IRSafeDistance ||
				Math.Min(_state.LastLeftIRReading.NormalizedMeasurement, _state.LastRightIRReading.NormalizedMeasurement) < _state.IRSafeDistance)
			{
				if (_state.LastLeftIRReading.NormalizedMeasurement > _state.IRSafeDistance + _state.IRDistanceDiferenceToAdjust)
				{
					// Turn left
					_state.CurrentState = WandererLogicalState.TurnLeft;
					_state.LeftWheelPower = -_state.MaxLateralSpeed;
					_state.RightWheelPower = _state.MaxLateralSpeed;
					timeoutValue = s * 5; // Cinco graus
				}
				else if (_state.LastRightIRReading.NormalizedMeasurement > _state.IRSafeDistance + _state.IRDistanceDiferenceToAdjust)
				{
					// Turn right
					_state.CurrentState = WandererLogicalState.TurnRight;
					_state.LeftWheelPower = _state.MaxLateralSpeed;
					_state.RightWheelPower = -_state.MaxLateralSpeed;
					timeoutValue = s * 5; // Cinco graus
				}
				else
				{
					_state.LeftWheelPower = 0;
					_state.RightWheelPower = 0;
					_state.CurrentState = WandererLogicalState.Ranging;
				}
			}
			else
			{
				if (Math.Abs(_state.LastLeftIRReading.NormalizedMeasurement - _state.LastRightIRReading.NormalizedMeasurement) > _state.IRDistanceDiferenceToAdjust)
					if (_state.LastLeftIRReading.NormalizedMeasurement > _state.LastRightIRReading.NormalizedMeasurement)
					{
						_state.CurrentState = WandererLogicalState.AdjustLeft;
						_state.LeftWheelPower = _state.MaxSpeed / 3;
						_state.RightWheelPower = _state.MaxSpeed;
						timeoutValue = s * 5; // 2 graus
					}
					else
					{
						_state.LeftWheelPower = _state.MaxSpeed;
						_state.RightWheelPower = _state.MaxSpeed / 3;
						_state.CurrentState = WandererLogicalState.AdjustRight;
						timeoutValue = s * 5; // 2 graus
					}
				else
				{
					_state.LeftWheelPower = _state.MaxSpeed;
					_state.RightWheelPower = _state.MaxSpeed;
					_state.CurrentState = WandererLogicalState.MoveForward;
				}
			}

			if (!(_state.LeftWheelPower == 0 && _state.RightWheelPower == 0 && lastLeftSpeed == 0 && lastRightSpeed == 0))
				yield return _driveDifferentialTwoWheelPort.SetDrivePower(_state.LeftWheelPower, _state.RightWheelPower).Choice();

			if (_state.CurrentState == WandererLogicalState.Ranging)
				yield return _sonarTurretPort.RangeSweep().Choice();
			else
				Activate(Arbiter.ReceiveWithIterator(false, TimeoutPort(timeoutValue), BehaviourLoop));

			base.SendNotification(submgr, new StateChangeNotify(_state));
		}

		private IEnumerator<ITask> OnRangeSweepComplete(sonarturret.RangeSweepCompleteNotify message)
		{
			//double lastLeftSpeed = _state.LeftWheelPower;
			//double lastRightSpeed = _state.RightWheelPower;

			int timeoutValue = 40;
			sonarturret.ArduinoSonarTurretState state = message.Body;
			if (state != null)
			{
				_state.LastTurretReading = state;
				_state.BestAngle = getAngle(state.DistanceMeasurements, (int)/*WandererState.RadianToDegree(*/state.AngularResolution/*)*/);
				// if there is a clear path to right or left
				// then move towards it, othrwise, reverse direction
				if (_state.BestAngle > (90 + 10))
				{
					// Turn right
					_state.CurrentState = WandererLogicalState.TurnRight;
					yield return _driveDifferentialTwoWheelPort.RotateDegrees(-(_state.BestAngle - 90), _state.MaxLateralSpeed).Choice();
					//_state.LeftWheelPower = _state.MaxLateralSpeed;
					//_state.RightWheelPower = -_state.MaxLateralSpeed;
					//timeoutValue = s * (_state.BestAngle - 90); // turn
				}
				else if (_state.BestAngle < (90 - 10))
				{
					// Turn left
					_state.CurrentState = WandererLogicalState.TurnLeft;
					yield return _driveDifferentialTwoWheelPort.RotateDegrees((90 - _state.BestAngle), _state.MaxLateralSpeed).Choice();
					//_state.LeftWheelPower = -_state.MaxLateralSpeed;
					//_state.RightWheelPower = _state.MaxLateralSpeed;
					//timeoutValue = s * (90 - _state.BestAngle); // turn
				}
				else
				{
					if (_state.LastLeftIRReading.NormalizedMeasurement > _state.LastRightIRReading.NormalizedMeasurement)
					{
						_state.CurrentState = WandererLogicalState.ReverseLeft;
						yield return _driveDifferentialTwoWheelPort.RotateDegrees(-90, _state.MaxLateralSpeed).Choice();
						//_state.LeftWheelPower = -_state.MaxLateralSpeed;
						//_state.RightWheelPower = _state.MaxLateralSpeed;
						//timeoutValue = s * 90; // Reverse
					}
					else
					{
						_state.CurrentState = WandererLogicalState.ReverseRight;
						yield return _driveDifferentialTwoWheelPort.RotateDegrees(90, _state.MaxLateralSpeed).Choice();
						//_state.LeftWheelPower = _state.MaxLateralSpeed;
						//_state.RightWheelPower = -_state.MaxLateralSpeed;
						//timeoutValue = s * 90; // Reverse
					}
				}
			}
			//if (lastLeftSpeed != _state.LeftWheelPower || lastRightSpeed != _state.RightWheelPower)
			//	yield return _driveDifferentialTwoWheelPort.SetDrivePower(_state.LeftWheelPower, _state.RightWheelPower).Choice();

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);

			Activate(Arbiter.ReceiveWithIterator(false, TimeoutPort(timeoutValue), BehaviourLoop));

			base.SendNotification(submgr, new StateChangeNotify(_state));
		}


		/**
		 * Get the angle at which the distance is maximum
		 */
		int getAngle(double[] dist, int step)
		{
			double maxDist = 0;
			int numSamples = /* Sonar lateral resolution */ 20 / step;
			int angle = 90;
			double[] normalizedDepth = new double[dist.Length];
			for (int i = 0; i < dist.Length; i++)
			{
				normalizedDepth[i] = 0;
				for (int numAvg = Math.Max(0, i - (numSamples / 2)); numAvg < Math.Min(i + (numSamples / 2), dist.Length); numAvg++)
				{
					normalizedDepth[i] += dist[numAvg];
				}
				normalizedDepth[i] /= numSamples;
			}
			for (int i = 0; i < normalizedDepth.Length; i++)
			{
				if (maxDist < normalizedDepth[i])
				{
					maxDist = normalizedDepth[i];
					angle = i * step;
				}
			}
			return angle;
		}

		[ServiceHandler]
		public void SubscribeHandler(Subscribe subscribe)
		{
			SubscribeHelper(submgr, subscribe.Body, subscribe.ResponsePort);
		}
	}
}


