using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using Microsoft.Dss.Core;

using submgr = Microsoft.Dss.Services.SubscriptionManager;
using drive = Microsoft.Robotics.Services.Drive.Proxy;
using arduino = Arduino.Firmata.Types.Proxy;
using Arduino.Firmata.Types.Proxy;

namespace POFerro.Robotics.ArduinoGenericDrive
{
	[Contract(Contract.Identifier)]
	[DisplayName("Arduino Generic Drive")]
	[Description("Arduino Generic Drive service (no description provided)")]
	[AlternateContract(drive.Contract.Identifier)]
	class ArduinoGenericDriveService : DsspServiceBase
	{
		[ServiceState]
		[InitialStatePartner(Optional = true, ServiceUri = "ArduinoGenericDriveService.xml")]
		ArduinoGenericDriveState _state = new ArduinoGenericDriveState();
		
		[ServicePort("/ArduinoGenericDrive", AllowMultipleInstances = true)]
		ArduinoGenericDriveOperations _mainPort = new ArduinoGenericDriveOperations();
		
		[SubscriptionManagerPartner]
		submgr.SubscriptionManagerPort _submgrPort = new submgr.SubscriptionManagerPort();
		
		[AlternateServicePort(AlternateContract = drive.Contract.Identifier)]
		drive.DriveOperations _driveDifferentialTwoWheelPort = new drive.DriveOperations();

		/// <summary>
		/// ArduinoService partner
		/// </summary>
		[Partner("ArduinoService", Contract = Arduino.Proxy.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry, Optional = true)]
		Arduino.Proxy.ArduinoOperations _arduinoServicePort = new Arduino.Proxy.ArduinoOperations();
		Arduino.Proxy.ArduinoOperations _arduinoServiceNotify = new Arduino.Proxy.ArduinoOperations();
		
		public ArduinoGenericDriveService(DsspServiceCreationPort creationPort)
			: base(creationPort)
		{
		}
		
		protected override void Start()
		{
			
			// 
			// Add service specific initialization here
			// 
			
			if (_state == null)
			{
				_state = new ArduinoGenericDriveState();

				//Right Motor (A)
				_state.RightForwardPin = arduino.Pins.D7;
				_state.RightBackwardPin = arduino.Pins.D5;
				_state.RightEnginePWMPin = arduino.Pins.D6;

				//Left Motor (B)
				_state.LeftForwardPin = arduino.Pins.D4;
				_state.LeftBackwardPin = arduino.Pins.D2;
				_state.LeftEnginePWMPin = arduino.Pins.D3;

				_state.MotorShieldType = MotorShieldTypeEnum.Keyes;

				this.SaveState(_state);
				Console.WriteLine("State not found in: " + ServicePaths.Store + "/ArduinoGenericDriveService.xml");
			}
			if (_state.LeftWheel == null)
				_state.LeftWheel = new Microsoft.Robotics.Services.Motor.Proxy.WheeledMotorState();
			if (_state.RightWheel == null)
				_state.RightWheel = new Microsoft.Robotics.Services.Motor.Proxy.WheeledMotorState();
			if (_state.LeftWheel.EncoderState == null)
				_state.LeftWheel.EncoderState = new Microsoft.Robotics.Services.Encoder.Proxy.EncoderState();
			if (_state.RightWheel.EncoderState == null)
				_state.RightWheel.EncoderState = new Microsoft.Robotics.Services.Encoder.Proxy.EncoderState();
			if (_state.LeftWheel.MotorState == null)
			{
				_state.LeftWheel.MotorState = new Microsoft.Robotics.Services.Motor.Proxy.MotorState();
				_state.LeftWheel.MotorState.PowerScalingFactor = 255;
			}
			if (_state.RightWheel.MotorState == null)
			{
				_state.RightWheel.MotorState = new Microsoft.Robotics.Services.Motor.Proxy.MotorState();
				_state.RightWheel.MotorState.PowerScalingFactor = 255;
			}

			_arduinoServicePort.Subscribe(_arduinoServiceNotify, typeof(Arduino.Messages.Proxy.AnalogOutputUpdate));

			base.Start();

			MainPortInterleave.CombineWith(
				new Interleave(
					new TeardownReceiverGroup(),
					new ExclusiveReceiverGroup(),
					new ConcurrentReceiverGroup(
						Arbiter.Receive<Arduino.Messages.Proxy.AnalogOutputUpdate>(true, _arduinoServiceNotify, AnalogOutputUpdateHandler)
					)));

			Activate(
				_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.RightEnginePWMPin, Mode = arduino.PinMode.PWM }).Choice(),
				_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.LeftEnginePWMPin, Mode = arduino.PinMode.PWM }).Choice()
				);
			if (_state.LeftEngineCurrentSensor != Pins.None)
				Activate(
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.LeftEngineCurrentSensor, Mode = arduino.PinMode.Analog }).Choice()
					);
			if (_state.RightEngineCurrentSensor != Pins.None)
				Activate(
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.RightEngineCurrentSensor, Mode = arduino.PinMode.Analog }).Choice()
					);

			if (_state.MotorShieldType == MotorShieldTypeEnum.Keyes)
			{
				Activate(
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.RightForwardPin, Mode = arduino.PinMode.Output }).Choice(),
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.RightBackwardPin, Mode = arduino.PinMode.Output }).Choice(),
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.LeftForwardPin, Mode = arduino.PinMode.Output }).Choice(),
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.LeftBackwardPin, Mode = arduino.PinMode.Output }).Choice()
					);
			}
			else
			{
				Activate(
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.LeftEngineDirPin, Mode = arduino.PinMode.Output }).Choice(),
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.LeftEngineBreakPin, Mode = arduino.PinMode.Output }).Choice(),
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.RightEngineDirPin, Mode = arduino.PinMode.Output }).Choice(),
					_arduinoServicePort.SetPinMode(new Arduino.Messages.Proxy.SetPinModeRequest() { Pin = _state.RightEngineBreakPin, Mode = arduino.PinMode.Output }).Choice()
					);
			}
		}

		private void AnalogOutputUpdateHandler(Arduino.Messages.Proxy.AnalogOutputUpdate message)
		{
			if (message.Body.CurrentPin == _state.LeftEngineCurrentSensor)
				_state.LeftWheel.MotorState.CurrentPower = (double)message.Body.Value / _state.LeftWheel.MotorState.PowerScalingFactor;
			else if (message.Body.CurrentPin == _state.RightEngineCurrentSensor)
				_state.RightWheel.MotorState.CurrentPower = (double)message.Body.Value / _state.LeftWheel.MotorState.PowerScalingFactor;

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}
		
		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public IEnumerator<ITask> DriveDifferentialTwoWheelEnableDriveHandler(drive.EnableDrive enabledrive)
		{
			if (_state.MotorShieldType == MotorShieldTypeEnum.ArduinoShield)
			{
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest() { Pin = _state.LeftEngineBreakPin, Value = enabledrive.Body.Enable ? PinDigitalValue.Low : PinDigitalValue.High }).Choice();
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest() { Pin = _state.RightEngineBreakPin, Value = enabledrive.Body.Enable ? PinDigitalValue.Low : PinDigitalValue.High }).Choice();
			}
			else if (!enabledrive.Body.Enable)
			{
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest() { Pin = _state.LeftBackwardPin, Value = PinDigitalValue.Low }).Choice();
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest() { Pin = _state.LeftForwardPin, Value = PinDigitalValue.Low }).Choice();
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest() { Pin = _state.RightBackwardPin, Value = PinDigitalValue.Low }).Choice();
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest() { Pin = _state.RightForwardPin, Value = PinDigitalValue.Low }).Choice();
			}

			_state.IsEnabled = enabledrive.Body.Enable;

			this.SendNotification(this._submgrPort, enabledrive);
			enabledrive.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}
		
		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public IEnumerator<ITask> DriveDifferentialTwoWheelSetDrivePowerHandler(drive.SetDrivePower setdrivepower)
		{
			return SetDrivePower(setdrivepower.Body.LeftWheelPower, setdrivepower.Body.RightWheelPower, drive.DriveState.DrivePower, setdrivepower.ResponsePort);
		}

		public IEnumerator<ITask> SetDrivePower(double leftWheelPower, double rightWheelPower, drive.DriveState driveState, PortSet<DefaultUpdateResponseType, Fault> responsePort)
		{
			if (_state.MotorShieldType == MotorShieldTypeEnum.Keyes)
			{
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest()
				{
					Pin = _state.LeftForwardPin,
					Value = leftWheelPower < 0 ? PinDigitalValue.Low : PinDigitalValue.High
				}).Choice();
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest() { 
					Pin = _state.LeftBackwardPin,
					Value = leftWheelPower > 0 ? PinDigitalValue.Low : PinDigitalValue.High
				}).Choice();
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest()
				{
					Pin = _state.RightForwardPin,
					Value = rightWheelPower < 0 ? PinDigitalValue.Low : PinDigitalValue.High
				}).Choice();
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest()
				{
					Pin = _state.RightBackwardPin,
					Value = rightWheelPower > 0 ? PinDigitalValue.Low : PinDigitalValue.High
				}).Choice();
			}
			else
			{
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest() { Pin = _state.LeftEngineBreakPin, Value = arduino.PinDigitalValue.Low }).Choice();
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest() { Pin = _state.RightEngineBreakPin, Value = arduino.PinDigitalValue.Low }).Choice();

				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest()
				{
					Pin = _state.LeftEngineDirPin,
					Value = leftWheelPower < 0 ? PinDigitalValue.Low : PinDigitalValue.High
				}).Choice();
				yield return _arduinoServicePort.SetPortDigitalValue(new Arduino.Messages.Proxy.SetPortDigitalValueRequest()
				{
					Pin = _state.RightEngineDirPin,
					Value = rightWheelPower < 0 ? PinDigitalValue.High : PinDigitalValue.Low
				}).Choice();

			}

			yield return _arduinoServicePort.SetPortAnalogValue(new Arduino.Messages.Proxy.SetPortAnalogValueRequest()
			{
				Pin = _state.LeftEnginePWMPin,
				Value = (int)(Math.Abs(leftWheelPower) * _state.LeftWheel.MotorState.PowerScalingFactor)
			}).Choice();
			yield return _arduinoServicePort.SetPortAnalogValue(new Arduino.Messages.Proxy.SetPortAnalogValueRequest()
			{
				Pin = _state.RightEnginePWMPin,
				Value = (int)(Math.Abs(rightWheelPower) * _state.RightWheel.MotorState.PowerScalingFactor)
			}).Choice();

			_state.LeftWheel.MotorState.CurrentPower = leftWheelPower;
			_state.RightWheel.MotorState.CurrentPower = rightWheelPower;

			Console.WriteLine("Set drive power: " + leftWheelPower + "," + rightWheelPower);
			if (driveState != drive.DriveState.RotateDegrees && _state.DriveState == drive.DriveState.RotateDegrees)
				_state.RotateDegreesStage = drive.DriveStage.Canceled;
			_state.DriveState = driveState;

			if (responsePort != null)
				responsePort.Post(DefaultUpdateResponseType.Instance);
		}
		
		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public IEnumerator<ITask> DriveDifferentialTwoWheelSetDriveSpeedHandler(drive.SetDriveSpeed setdrivespeed)
		{
			setdrivespeed.ResponsePort.Post(Fault.FromException(new NotSupportedException()));
			yield break;
		}
		
		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public IEnumerator<ITask> DriveDifferentialTwoWheelRotateDegreesHandler(drive.RotateDegrees rotatedegrees)
		{
			_state.RotateDegreesStage = drive.DriveStage.InitialRequest;

			Console.WriteLine("Rotate degrees: " + rotatedegrees.Body.Degrees + "<->" + rotatedegrees.Body.Power);
			var tasks = SetDrivePower(
				-Math.Sign(rotatedegrees.Body.Degrees) * rotatedegrees.Body.Power,
				Math.Sign(rotatedegrees.Body.Degrees) * rotatedegrees.Body.Power, 
				drive.DriveState.RotateDegrees, 
				null);
			while (tasks.MoveNext())
				yield return tasks.Current;

			_state.RotateDegreesStage = drive.DriveStage.Started;
			this.SendNotification(this._submgrPort, new drive.Update(this._state));

			yield return this.Timeout((int)(Math.Abs(rotatedegrees.Body.Degrees) * _state.MillisecondsPerAngle));
			//Activate(Arbiter.ReceiveWithIterator(false, this.TimeoutPort((int)(Math.Abs(rotatedegrees.Body.Degrees) * _state.MillisecondsPerAngle)), OnFinishRotateOperation));

			yield return _arduinoServicePort.SetPortAnalogValue(new Arduino.Messages.Proxy.SetPortAnalogValueRequest() { Pin = _state.LeftEnginePWMPin, Value = 0 }).Choice();
			yield return _arduinoServicePort.SetPortAnalogValue(new Arduino.Messages.Proxy.SetPortAnalogValueRequest() { Pin = _state.RightEnginePWMPin, Value = 0 }).Choice();

			Console.WriteLine("Complete rotate degrees: " + rotatedegrees.Body.Degrees + "<->" + rotatedegrees.Body.Power);

			_state.RotateDegreesStage = drive.DriveStage.Completed;
			_state.DriveState = drive.DriveState.Stopped;
			this.SendNotification(this._submgrPort, new drive.Update(this._state));

			rotatedegrees.ResponsePort.Post(DefaultUpdateResponseType.Instance);
			yield break;
		}

		//private IEnumerator<ITask> OnFinishRotateOperation(DateTime message)
		//{
		//	if (_state.DriveState == drive.DriveState.RotateDegrees)
		//	{
		//		yield return _arduinoServicePort.SetPortAnalogValue(new Arduino.Messages.Proxy.SetPortAnalogValueRequest() { Pin = _state.LeftEnginePWMPin, Value = 0 }).Choice();
		//		yield return _arduinoServicePort.SetPortAnalogValue(new Arduino.Messages.Proxy.SetPortAnalogValueRequest() { Pin = _state.RightEnginePWMPin, Value = 0 }).Choice();

		//		_state.RotateDegreesStage = drive.DriveStage.Completed;
		//		_state.DriveState = drive.DriveState.Stopped;
		//		this.SendNotification(this._submgrPort, new drive.Update(this._state));
		//	}
		//}
		
		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public IEnumerator<ITask> DriveDifferentialTwoWheelDriveDistanceHandler(drive.DriveDistance drivedistance)
		{
			drivedistance.ResponsePort.Post(Fault.FromException(new NotSupportedException()));
			yield break;
		}
		
		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public IEnumerator<ITask> DriveDifferentialTwoWheelAllStopHandler(drive.AllStop allstop)
		{
			yield return _arduinoServicePort.SetPortAnalogValue(new Arduino.Messages.Proxy.SetPortAnalogValueRequest() { Pin = _state.LeftEnginePWMPin, Value = 0 }).Choice();
			yield return _arduinoServicePort.SetPortAnalogValue(new Arduino.Messages.Proxy.SetPortAnalogValueRequest() { Pin = _state.RightEnginePWMPin, Value = 0 }).Choice();

			_state.DriveState = drive.DriveState.Stopped;

			allstop.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}
		
		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public IEnumerator<ITask> DriveDifferentialTwoWheelResetEncodersHandler(drive.ResetEncoders resetencoders)
		{
			resetencoders.ResponsePort.Post(Fault.FromException(new NotSupportedException()));
			yield break;
		}

		#region DSS Handlers
		[ServiceHandler]
		public void SubscribeHandler(Subscribe subscribe)
		{
			SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
		}

		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public void DriveDifferentialTwoWheelGetHandler(drive.Get get)
		{
			get.ResponsePort.Post(_state);
		}

		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public void DriveDifferentialTwoWheelHttpGetHandler(Microsoft.Dss.Core.DsspHttp.HttpGet httpget)
		{
			httpget.ResponsePort.Post(new Microsoft.Dss.Core.DsspHttp.HttpResponseType(_state));
		}

		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public void DriveDifferentialTwoWheelHttpPostHandler(Microsoft.Dss.Core.DsspHttp.HttpPost httppost)
		{
			httppost.ResponsePort.Post(new Microsoft.Dss.Core.DsspHttp.HttpResponseType(_state));
		}

		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public void DriveDifferentialTwoWheelReliableSubscribeHandler(drive.ReliableSubscribe reliablesubscribe)
		{
			SubscribeHelper(_submgrPort, reliablesubscribe.Body, reliablesubscribe.ResponsePort);
		}

		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public void DriveDifferentialTwoWheelSubscribeHandler(drive.Subscribe subscribe)
		{
			SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
		}

		[ServiceHandler(PortFieldName = "_driveDifferentialTwoWheelPort")]
		public void DriveDifferentialTwoWheelUpdateHandler(drive.Update update)
		{
			_state.DistanceBetweenWheels = update.Body.DistanceBetweenWheels;

			update.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}
		#endregion
	}
}


