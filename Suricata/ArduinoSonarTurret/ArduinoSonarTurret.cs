using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using submgr = Microsoft.Dss.Services.SubscriptionManager;

using sonar = Microsoft.Robotics.Services.Sonar.Proxy;
using arduinoservo = POFerro.Robotics.ArduinoGenericServo;
using System.Threading;
using Microsoft.Dss.Core;

namespace POFerro.Robotics.ArduinoSonarTurret
{
	[Contract(Contract.Identifier)]
	[DisplayName("ArduinoSonarTurret")]
	[Description("ArduinoSonarTurret service (no description provided)")]
	[AlternateContract(sonar.Contract.Identifier)]
	class ArduinoSonarTurretService : DsspServiceBase
	{
		[ServiceState]
		[InitialStatePartner(Optional = true, ServiceUri = "ArduinoSonarTurretService.xml")]
		ArduinoSonarTurretState _state = new ArduinoSonarTurretState();
		
		[ServicePort("/ArduinoSonarTurret", AllowMultipleInstances = true)]
		ArduinoSonarTurretOperations _mainPort = new ArduinoSonarTurretOperations();
		
		[SubscriptionManagerPartner]
		submgr.SubscriptionManagerPort _submgrPort = new submgr.SubscriptionManagerPort();
		
		[AlternateServicePort(AlternateContract = sonar.Contract.Identifier)]
		sonar.SonarOperations _sonarPort = new sonar.SonarOperations();

		/// <summary>
		/// ArduinoSonar partner
		/// </summary>
		[Partner("GenericSonar", Contract = sonar.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		sonar.SonarOperations _arduinoSonarPort = new sonar.SonarOperations();

		/// <summary>
		/// ArduinoSonar partner
		/// </summary>
		[Partner("ArduinoGenericServo", Contract = arduinoservo.Proxy.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		arduinoservo.Proxy.ArduinoGenericServoOperations _arduinoServoPort = new arduinoservo.Proxy.ArduinoGenericServoOperations();
		
		public ArduinoSonarTurretService(DsspServiceCreationPort creationPort)
			: base(creationPort)
		{
		}
		
		protected override void Start()
		{
			base.Start();

			if (_state == null)
			{
				_state = new ArduinoSonarTurretState();

				_state.AngularResolution = /*ArduinoSonarTurretState.DegreeToRadian(*/1/*)*/;
				_state.AngularRange = /*ArduinoSonarTurretState.DegreeToRadian(*/180/*)*/;

				this.SaveState(_state);
				Console.WriteLine("State not found in: " + ServicePaths.Store + "/ArduinoSonarTurretService.xml");
			}

			//MainPortInterleave.CombineWith(
			//	new Interleave(
			//		new TeardownReceiverGroup(),
			//		new ExclusiveReceiverGroup(),
			//		new ConcurrentReceiverGroup(
			//		)));
		}

		public const int MID_DEGREE = 90;
		public const int SERVO_PER_ANGLE_DELAY = 1;

		[ServiceHandler]
		public void RangeSweepHandler(RangeSweep message)
		{
			int lateral_range = (int)/*ArduinoSonarTurretState.RadianToDegree(*/_state.AngularRange/*)*/ / 2;
			SpawnIterator(MID_DEGREE - lateral_range, MID_DEGREE + lateral_range, (int)/*ArduinoSonarTurretState.RadianToDegree(*/_state.AngularResolution/*)*/, this.RangeSweepHandler);
			
			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		protected IEnumerator<ITask> RangeSweepHandler(int start, int end, int step)
		{
			_state.DistanceMeasurements = new double[1 + (end - start) / step];
			double[] reading1 = new double[1 + (end - start) / step];
			for (int pos = start; pos < end; pos += step)
			{
				yield return _arduinoServoPort.MoveServo(pos).Choice();
				yield return Timeout(step * SERVO_PER_ANGLE_DELAY);
				var readings = ReadSonarForRangePosition(reading1, (int)(end - (pos - start)) / step, step, end - pos);
				while (readings.MoveNext())
				{
					yield return readings.Current;
				}
			}
			double[] reading2 = new double[1 + (end - start) / step];
			for (int pos = end; pos > start; pos -= step)
			{
				yield return _arduinoServoPort.MoveServo(pos).Choice();
				yield return Timeout(step * SERVO_PER_ANGLE_DELAY);
				var readings = ReadSonarForRangePosition(reading1, (int)(end - (pos - start)) / step, step, end - pos);
				while (readings.MoveNext())
				{
					yield return readings.Current;
				}
			}

			yield return _arduinoServoPort.MoveServo(MID_DEGREE).Choice();

			this.SendNotification(_submgrPort, new RangeSweepCompleteNotify(_state));
		}

		protected IEnumerator<ITask> ReadSonarForRangePosition(double[] reading, int index, int step, int currentAngle)
		{
			DateTime dtIni = DateTime.Now;
			DateTime dtCurrent = dtIni;
			while (dtCurrent <= dtIni)
			{
				yield return Arbiter.Choice(_arduinoSonarPort.Get(),
					s =>
					{
						_state.MaxDistance = s.MaxDistance;
						reading[index] = s.DistanceMeasurement;
						if (_state.DistanceMeasurements[index] > 0 && s.DistanceMeasurement > 0)
							_state.DistanceMeasurements[index] = Math.Min(_state.DistanceMeasurements[index], s.DistanceMeasurement);
						else
							_state.DistanceMeasurements[index] = Math.Max(_state.DistanceMeasurements[index], s.DistanceMeasurement);

						dtCurrent = s.TimeStamp;
						if (dtCurrent > dtIni)
						{
							this.SendNotification(_submgrPort, new RangePositionReadNotify(new RangePositionRead()
							{
								SweepAngularStep = step,
								DistanceMeasurement = s.DistanceMeasurement,
								MaxDistance = s.MaxDistance,
								CurrentAngle = currentAngle,
								HardwareIdentifier = s.HardwareIdentifier,
								TimeStamp = s.TimeStamp,
								Pose = s.Pose,
							}));
						}
					},
					fault => { });
			}
		}

		[ServiceHandler()]
		public IEnumerator<ITask> SonarGetHandler(Get get)
		{
			yield return Arbiter.Choice(
				_arduinoSonarPort.Get(),
				s =>
				{
					_state.DistanceMeasurement = s.DistanceMeasurement;
					_state.HardwareIdentifier = s.HardwareIdentifier;
					_state.Pose = s.Pose;
					_state.TimeStamp = s.TimeStamp;
					_state.MaxDistance = s.MaxDistance;
					get.ResponsePort.Post(_state);
				},
				fault =>
				{
					_state.DistanceMeasurement = 0;
					get.ResponsePort.Post(fault);
				});
		}
		
		[ServiceHandler(PortFieldName = "_sonarPort")]
		public IEnumerator<ITask> SonarGetHandler(sonar.Get get)
		{
			yield return Arbiter.Choice(
				_arduinoSonarPort.Get(),
				s =>
				{
					_state.DistanceMeasurement = s.DistanceMeasurement;
					_state.HardwareIdentifier = s.HardwareIdentifier;
					_state.Pose = s.Pose;
					_state.TimeStamp = s.TimeStamp;
					_state.MaxDistance = s.MaxDistance;
					get.ResponsePort.Post(new sonar.SonarState()
					{
						DistanceMeasurement = _state.DistanceMeasurement, 
						DistanceMeasurements = _state.DistanceMeasurements,
						MaxDistance = _state.MaxDistance,
						TimeStamp = _state.TimeStamp,
						AngularRange = ArduinoSonarTurretState.DegreeToRadian(_state.AngularRange),
						AngularResolution = ArduinoSonarTurretState.DegreeToRadian(_state.AngularResolution),
						HardwareIdentifier = _state.HardwareIdentifier,
						Pose = _state.Pose,
					});
				},
				fault => { 
					_state.DistanceMeasurement = 0;
					get.ResponsePort.Post(fault);
				});
		}
		
		[ServiceHandler(PortFieldName = "_sonarPort")]
		public IEnumerator<ITask> SonarHttpGetHandler(Microsoft.Dss.Core.DsspHttp.HttpGet httpget)
		{
			yield return Arbiter.Choice(
				_arduinoSonarPort.Get(),
				s =>
				{
					_state.DistanceMeasurement = s.DistanceMeasurement;
					_state.HardwareIdentifier = s.HardwareIdentifier;
					_state.Pose = s.Pose;
					_state.TimeStamp = s.TimeStamp;
					_state.MaxDistance = s.MaxDistance;
				},
				fault => _state.DistanceMeasurement = -1);
			httpget.ResponsePort.Post(new Microsoft.Dss.Core.DsspHttp.HttpResponseType(_state));
		}

		[ServiceHandler(PortFieldName = "_sonarPort")]
		public void SonarReplaceHandler(sonar.Replace replace)
		{
			_state.AngularResolution = replace.Body.AngularResolution;
			_state.AngularRange = replace.Body.AngularRange;

			replace.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		[ServiceHandler(PortFieldName = "_sonarPort")]
		public void SonarReliableSubscribeHandler(sonar.ReliableSubscribe reliablesubscribe)
		{
			SubscribeHelper(_submgrPort, reliablesubscribe.Body, reliablesubscribe.ResponsePort);
		}

		[ServiceHandler(PortFieldName = "_sonarPort")]
		public void SonarSubscribeHandler(sonar.Subscribe subscribe)
		{
			SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
		}

		[ServiceHandler()]
		public void SonarTurretSubscribeHandler(Subscribe subscribe)
		{
			SubscribeHelper(_submgrPort, subscribe.Body, subscribe.ResponsePort);
		}
	}
}
