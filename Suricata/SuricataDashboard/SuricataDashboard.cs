using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Arduino.Firmata.Types.Proxy;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

using drive = Microsoft.Robotics.Services.Drive.Proxy;
using sonar = Microsoft.Robotics.Services.Sonar.Proxy;
using ir = Microsoft.Robotics.Services.AnalogSensor.Proxy;

using ccrwpf = Microsoft.Ccr.Adapters.Wpf;
using common = Microsoft.Robotics.Common;
using kinect = Microsoft.Robotics.Services.Sensors.Kinect;
using suricata = POFerro.Robotics.Suricata.Proxy;
using follower = POFerro.Robotics.SkeletonFollower.Proxy;
using soundfollower = POFerro.Robotics.SoundFollower.Proxy;
using kinectProxy = Microsoft.Robotics.Services.Sensors.Kinect.Proxy;
using mskinect = Microsoft.Kinect;

namespace POFerro.Robotics.SuricataDashboard
{
	[Contract(Contract.Identifier)]
	[DisplayName("SuricataDashboard")]
	[Description("SuricataDashboard service (no description provided)")]
	public class SuricataDashboardService : DsspServiceBase
	{
		[ServiceState]
		SuricataDashboardState _state = new SuricataDashboardState();
		/// <summary>
		/// Used to guage frequency of reading the state (which is much lower than that of reading frames)
		/// </summary>
		private double lastStateReadTime = 0;

		/// <summary>
		/// We don't want to flood logs with same errors
		/// </summary>
		private bool frameQueryFailed = false;

		/// <summary>
		/// We don't want to flood logs with same errors
		/// </summary>
		private bool tiltPollFailed = false;

		/// <summary>
		/// Gets or sets a value indicating whether to render depth information
		/// </summary>
		public bool IncludeDepth { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to render video information
		/// </summary>
		public bool IncludeVideo { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to render skeleton information
		/// </summary>
		public bool IncludeSkeletons { get; set; }
		
		[ServicePort("/SuricataDashboard", AllowMultipleInstances = false)]
		SuricataDashboardOperations _mainPort = new SuricataDashboardOperations();

		/// <summary>
		/// ArduinoService partner
		/// </summary>
		[Partner("ArduinoService", Contract = Arduino.Proxy.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry, Optional = true)]
		Arduino.Proxy.ArduinoOperations _arduinoServicePort = new Arduino.Proxy.ArduinoOperations();
		Arduino.Proxy.ArduinoOperations _arduinoServiceNotify = new Arduino.Proxy.ArduinoOperations();

		[Partner("SuricataService", Contract = suricata.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry, Optional = false)]
		suricata.SuricataOperations _suricataServicePort = new suricata.SuricataOperations();
		suricata.SuricataOperations _suricataServiceNotify = new suricata.SuricataOperations();

		[Partner("SkeletonFollowerService", Contract = follower.Contract.Identifier, Optional = true, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
		follower.SkeletonFollowerOperations _followerServicePort = new follower.SkeletonFollowerOperations();
		follower.SkeletonFollowerOperations _followerServiceNotify = new follower.SkeletonFollowerOperations();

		[Partner("SoundFollowerService", Contract = soundfollower.Contract.Identifier, Optional = true, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry)]
		soundfollower.SoundFollowerOperations _soundFollowerServicePort = new soundfollower.SoundFollowerOperations();
		soundfollower.SoundFollowerOperations _soundFollowerServiceNotify = new soundfollower.SoundFollowerOperations();


		/// <summary>
		/// DriveDifferentialTwoWheel partner
		/// </summary>
		[Partner("DriveDifferentialTwoWheel", Contract = drive.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		drive.DriveOperations _driveDifferentialTwoWheelPort = new drive.DriveOperations();

		/// <summary>
		/// Sonar partner
		/// </summary>
		[Partner("SonarLeft", Contract = ir.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		ir.AnalogSensorOperations _sonarLeftPort = new ir.AnalogSensorOperations();
		ir.AnalogSensorOperations _sonarLeftNotify = new ir.AnalogSensorOperations();

		/// <summary>
		/// Sonar partner
		/// </summary>
		[Partner("SonarRight", Contract = ir.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		ir.AnalogSensorOperations _sonarRightPort = new ir.AnalogSensorOperations();
		ir.AnalogSensorOperations _sonarRightNotify = new ir.AnalogSensorOperations();

		/// <summary>
		/// Right Infrared partner
		/// </summary>
		[Partner("IRCenter", Contract = ir.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		ir.AnalogSensorOperations _irCenterPort = new ir.AnalogSensorOperations();
		ir.AnalogSensorOperations _irCenterNotify = new ir.AnalogSensorOperations();

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
		/// Kinect partner service
		/// </summary>
		[Partner("Kinect", Contract = kinectProxy.Contract.Identifier, Optional = true, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		private kinectProxy.KinectOperations kinectPort = new kinectProxy.KinectOperations();
		private kinectProxy.KinectOperations kinectNotify = new kinectProxy.KinectOperations();

		/// <summary>
		/// Frame Pre Processor
		/// </summary>
		private FramePreProcessor frameProcessor;

		SuricataDashboardWPF userInterface;
		/// <summary>
		/// WPF service port
		/// </summary>
		private ccrwpf.WpfServicePort wpfServicePort;
		
		public SuricataDashboardService(DsspServiceCreationPort creationPort)
			: base(creationPort)
		{
			// by default, lets show all the good stuff
			this.IncludeDepth = true;
			this.IncludeVideo = true;
			this.IncludeSkeletons = true;
			this.PartnerEnumerationTimeout = TimeSpan.FromSeconds(5);
		}
		
		protected override void Start()
		{
			_arduinoServicePort.Subscribe(_arduinoServiceNotify);
			_suricataServicePort.Subscribe(_suricataServiceNotify, typeof(suricata.StateChangeNotify));
			if (this._followerServicePort != null)
				_followerServicePort.Subscribe(_followerServiceNotify, typeof(follower.StateChangeNotify));
			if (this._soundFollowerServicePort != null)
				this._soundFollowerServicePort.Subscribe(this._soundFollowerServiceNotify, typeof(soundfollower.StateChangeNotify));

			this._sonarLeftPort.Subscribe(_sonarLeftNotify, typeof(ir.Replace));
			this._sonarRightPort.Subscribe(_sonarRightNotify, typeof(ir.Replace));

			this._irCenterPort.Subscribe(_irCenterNotify, typeof(ir.Replace));
			this._irRightPort.Subscribe(_irRightNotify, typeof(ir.Replace));
			this._irLeftPort.Subscribe(_irLeftNotify, typeof(ir.Replace));

			base.Start();

			MainPortInterleave.CombineWith(
				new Interleave(
					new TeardownReceiverGroup(),
					new ExclusiveReceiverGroup(),
					new ConcurrentReceiverGroup(
						Arbiter.Receive<Arduino.Messages.Proxy.AnalogOutputUpdate>(true, _arduinoServiceNotify, AnalogOutputUpdateHandler),
						Arbiter.Receive<Arduino.Messages.Proxy.DigitalOutputUpdate>(true, _arduinoServiceNotify, DigitalOutputUpdateHandler),
						Arbiter.Receive<suricata.StateChangeNotify>(true, _suricataServiceNotify, SuricataStateChangeHandler),
						Arbiter.Receive<follower.StateChangeNotify>(true, _followerServiceNotify, FollowerStateChangeHandler),
						Arbiter.Receive<follower.EnabledChanged>(true, _followerServiceNotify, FollowerEnableChangeHandler),
						Arbiter.Receive<soundfollower.StateChangeNotify>(true, _soundFollowerServiceNotify, SoundFollowerStateChangeHandler),
						Arbiter.Receive<soundfollower.EnabledChanged>(true, _soundFollowerServiceNotify, SoundFollowerEnableChangeHandler),

						Arbiter.Receive<ir.Replace>(true, _sonarLeftNotify, OnSonarLeft),
						Arbiter.Receive<ir.Replace>(true, _sonarRightNotify, OnSonarRight),

						Arbiter.Receive<ir.Replace>(true, _irCenterNotify, OnInfraredCenter),
						Arbiter.Receive<ir.Replace>(true, _irLeftNotify, OnInfraredLeft),
						Arbiter.Receive<ir.Replace>(true, _irRightNotify, OnInfraredRight)
					))
			);

			SpawnIterator(this.InitializeDashboard);
		}

		private IEnumerator<ITask> InitializeDashboard()
		{
			//for (int pin = 2; pin < (int)Pins.A0; pin++)
			//{
			//	yield return _arduinoServicePort.SetPinReporting(new Arduino.Messages.Proxy.SetPinReportingRequest() { Pin = (Pins)pin, ReportingEnabled = true }).Choice();
			//}
			// create WPF adapter
			this.wpfServicePort = ccrwpf.WpfAdapter.Create(TaskQueue);

			var runWindow = this.wpfServicePort.RunWindow(() => new SuricataDashboardWPF(this));
			yield return (Choice)runWindow;

			var exception = (Exception)runWindow;
			if (exception != null)
			{
				LogError(exception);
				StartFailed();
				yield break;
			}

			// need double cast because WPF adapter doesn't know about derived window types
			this.userInterface = (Window)runWindow as SuricataDashboardWPF;

			if (this.kinectPort != null)
			{
				this.frameProcessor = new FramePreProcessor(this.kinectPort);

				yield return this.kinectPort.Get().Choice(
					kinectState =>
					{
						this.UpdateState(kinectState);
					},
					failure =>
					{
						LogError(failure);
					});

				yield return this.kinectPort.GetDepthProperties().Choice(
					GetDepthProperties =>
					{
						SuricataDashboardWPF.MaxValidDepth = GetDepthProperties.MaxDepthValue;
					},
					failure =>
					{
						LogError(failure);
					});

				SpawnIterator(this.ReadKinectLoop);
			}
		}

		/// <summary>
		/// Main read loop
		/// Read raw frame from Kinect service, then process it asynchronously, then request UI update
		/// </summary>
		/// <returns>A standard CCR iterator.</returns>
		private IEnumerator<ITask> ReadKinectLoop()
		{
			while (true)
			{
				kinectProxy.QueryRawFrameRequest frameRequest = new kinectProxy.QueryRawFrameRequest();
				frameRequest.IncludeDepth = this.IncludeDepth;
				frameRequest.IncludeVideo = this.IncludeVideo;
				frameRequest.IncludeSkeletons = this.IncludeSkeletons;

				if (!this.IncludeDepth && !this.IncludeVideo && !this.IncludeSkeletons)
				{
					// poll 5 times a sec if user for some reason deselected all image options (this would turn
					// into a busy loop then)
					yield return TimeoutPort(200).Receive();
				}

				kinect.RawKinectFrames rawFrames = null;

				// poll depth camera
				yield return this.kinectPort.QueryRawFrame(frameRequest).Choice(
					rawFrameResponse =>
					{
						rawFrames = rawFrameResponse.RawFrames;
					},
					failure =>
					{
						if (!this.frameQueryFailed)
						{
							this.frameQueryFailed = true;
							LogError(failure);
						}
					});

				this.frameProcessor.SetRawFrame(rawFrames);

				if (null != rawFrames.RawSkeletonFrameData)
				{
					yield return new IterativeTask(this.frameProcessor.ProcessSkeletons);
				}

				this.UpdateUI(this.frameProcessor);

				// poll state at low frequency to see if tilt has shifted (may happen on an actual robot due to shaking)
				if (common.Utilities.ElapsedSecondsSinceStart - this.lastStateReadTime > 1)
				{
					yield return this.kinectPort.Get().Choice(
						kinectState =>
						{
							this.UpdateState(kinectState);
						},
						failure =>
						{
							if (!this.tiltPollFailed)
							{
								this.tiltPollFailed = true;
								LogError(failure);
							}
						});

					this.lastStateReadTime = common.Utilities.ElapsedSecondsSinceStart;
				}
			}
		}

		/// <summary>
		/// Update the UI for each frame
		/// </summary>
		/// <param name="processedFrames">Processed Frames</param>
		private void UpdateUI(FramePreProcessor processedFrames)
		{
			this.wpfServicePort.Invoke(() => this.userInterface.DrawFrame(processedFrames));
		}

		/// <summary>
		/// Update the kinect state on the UI
		/// </summary>
		/// <param name="kinectState">Kinect State</param>
		private void UpdateState(kinectProxy.KinectState kinectState)
		{
			this.wpfServicePort.Invoke(() => this.userInterface.UpdateState(kinectState));
		}

		/// <summary>
		/// Update the tilt angle on the kinect
		/// </summary>
		/// <param name="degrees">Angle in degrees</param>
		internal void UpdateTilt(int degrees)
		{
			kinectProxy.UpdateTiltRequest request = new kinectProxy.UpdateTiltRequest();
			request.Tilt = degrees;

			Activate(
				Arbiter.Choice(
					this.kinectPort.UpdateTilt(request),
					success =>
					{
						// nothing to do
					},
					fault =>
					{
						// the fault handler is outside the WPF dispatcher
						// to perfom any UI related operation we need to go through the WPF adapter

						// show an error message
						this.wpfServicePort.Invoke(() => this.userInterface.ShowFault(fault));
					}));
		}

		/// <summary>
		/// Send a request to Kinect service to update smoothing parameters
		/// </summary>
		/// <param name="transformSmooth">A value indicating whether to apply transform smooth</param>
		/// <param name="smoothing">The amount of smoothing to be applied</param>
		/// <param name="correction">The amount of correction to be applied</param>
		/// <param name="prediction">The amount of prediction to be made</param>
		/// <param name="jitterRadius">The radius for jitter processing</param>
		/// <param name="maxDeviationRadius">Maximum deviation radius</param>
		internal void UpdateSkeletalSmoothing(
			bool transformSmooth,
			float smoothing,
			float correction,
			float prediction,
			float jitterRadius,
			float maxDeviationRadius)
		{
			mskinect.TransformSmoothParameters newSmoothParams = new mskinect.TransformSmoothParameters();
			newSmoothParams.Correction = correction;
			newSmoothParams.JitterRadius = jitterRadius;
			newSmoothParams.MaxDeviationRadius = maxDeviationRadius;
			newSmoothParams.Prediction = prediction;
			newSmoothParams.Smoothing = smoothing;

			kinectProxy.UpdateSkeletalSmoothingRequest request = new kinectProxy.UpdateSkeletalSmoothingRequest();
			request.TransfrormSmooth = transformSmooth;
			request.SkeletalEngineTransformSmoothParameters = newSmoothParams;

			Activate(
				Arbiter.Choice(
					this.kinectPort.UpdateSkeletalSmoothing(request),
					success =>
					{
						// nothing to do
					},
					fault =>
					{
						// the fault handler is outside the WPF dispatcher
						// to perfom any UI related operation we need to go through the WPF adapter

						// show an error message
						this.wpfServicePort.Invoke(() => this.userInterface.ShowFault(fault));
					}));
		}

		private void DigitalOutputUpdateHandler(Arduino.Messages.Proxy.DigitalOutputUpdate message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.UpdateDigitalPin(message.Body));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void AnalogOutputUpdateHandler(Arduino.Messages.Proxy.AnalogOutputUpdate message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.UpdateAnalogPin(message.Body));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void FollowerStateChangeHandler(follower.StateChangeNotify message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.UpdateState(message.Body));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void SuricataStateChangeHandler(suricata.StateChangeNotify message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.UpdateState(message.Body));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void SoundFollowerStateChangeHandler(soundfollower.StateChangeNotify message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.UpdateState(message.Body));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void FollowerEnableChangeHandler(follower.EnabledChanged message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.UpdateFollowerEnabled(message.Body.Enabled));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void SoundFollowerEnableChangeHandler(soundfollower.EnabledChanged message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.UpdateSoundFollowerEnabled(message.Body.Enabled));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void OnSonarRight(ir.Replace message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.RightSonarReadingUpdate(message.Body));

			message.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		private void OnSonarLeft(ir.Replace message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.LeftSonarReadingUpdate(message.Body));

			message.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		private void OnInfraredCenter(ir.Replace message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.CenterIRReadingUpdate(message.Body));

			message.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		private void OnInfraredLeft(ir.Replace message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.LeftIRReadingUpdate(message.Body));

			message.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}

		private void OnInfraredRight(ir.Replace message)
		{
			if (this.userInterface != null)
				this.wpfServicePort.Invoke(() => this.userInterface.RightIRReadingUpdate(message.Body));

			message.ResponsePort.Post(DefaultReplaceResponseType.Instance);
		}
	}
}


