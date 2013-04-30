using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using System.Linq;

using Microsoft.Kinect;

using submgr = Microsoft.Dss.Services.SubscriptionManager;
using System.IO;
using System.Threading;


namespace POFerro.Robotics.KinectSoundTracker
{
    [Contract(Contract.Identifier)]
    [DisplayName("KinectSoundTracker")]
    [Description("KinectSoundTracker service (no description provided)")]
    class KinectSoundTrackerService : DsspServiceBase
    {
        /// <summary>
        /// Service state
        /// </summary>
        [ServiceState]
        KinectSoundTrackerState _state = new KinectSoundTrackerState();
		/// <summary>
		/// Number of milliseconds between each read of audio data from the stream.
		/// Faster polling (few tens of ms) ensures a smoother audio stream visualization.
		/// </summary>
		private const int AudioPollingInterval = 50;

		/// <summary>
		/// Number of samples captured from Kinect audio stream each millisecond.
		/// </summary>
		private const int SamplesPerMillisecond = 16;

		/// <summary>
		/// Number of bytes in each Kinect audio stream sample.
		/// </summary>
		private const int BytesPerSample = 2;

		private KinectSensor kinect;

		/// <summary>
		/// Stream of audio being captured by Kinect sensor.
		/// </summary>
		private Stream audioStream;

        /// <summary>
        /// Main service port
        /// </summary>
        [ServicePort("/KinectSoundTracker", AllowMultipleInstances = false)]
        KinectSoundTrackerOperations _mainPort = new KinectSoundTrackerOperations();

        [SubscriptionManagerPartner]
        submgr.SubscriptionManagerPort _submgrPort = new submgr.SubscriptionManagerPort();

        /// <summary>
        /// Service constructor
        /// </summary>
        public KinectSoundTrackerService(DsspServiceCreationPort creationPort)
            : base(creationPort)
        {
        }

        /// <summary>
        /// Service start
        /// </summary>
        protected override void Start()
        {
			this.kinect = KinectSensor.KinectSensors.FirstOrDefault(k => k.Status == KinectStatus.Connected);
			if (null != this.kinect)
			{
				try
				{
					// Start the sensor!
					this.kinect.Start();
				}
				catch (Exception e)
				{
					LogError("Kinect failed to start", e);
					this.StartFailed();
					return;
				}
			}
			else
			{
				LogError("Kinect is not ready");
				this.StartFailed();
			}
			this.kinect.AudioSource.AutomaticGainControlEnabled = false;
			this.kinect.AudioSource.SoundSourceAngleChanged += this.AudioSourceSoundSourceAngleChanged;
			this.audioStream = this.kinect.AudioSource.Start();


            // 
            // Add service specific initialization here
            // 

            base.Start();


			//Activate(Arbiter.ReceiveWithIterator(false, TimeoutPort(1000), this.BehaviourLoop));
        }


		/// <summary>
		/// Buffer used to hold audio data read from audio stream.
		/// </summary>
		private readonly byte[] audioBuffer = new byte[AudioPollingInterval * SamplesPerMillisecond * BytesPerSample];
		private IEnumerator<ITask> BehaviourLoop(DateTime message)
		{
			int readCount = audioStream.Read(audioBuffer, 0, audioBuffer.Length);

			Activate(Arbiter.ReceiveWithIterator(false, TimeoutPort(AudioPollingInterval), this.BehaviourLoop));

			yield break;
		}

		private void AudioSourceSoundSourceAngleChanged(object sender, SoundSourceAngleChangedEventArgs e)
		{
			_state.CurrentConfidenceLevel = e.ConfidenceLevel;
			_state.CurrentAngle = e.Angle;

			this.SendNotification(_submgrPort, new SoundSourceAngleChanged(_state));
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


