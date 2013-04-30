using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

using ccrwpf = Microsoft.Ccr.Adapters.Wpf;
using wanderer = POFerro.Robotics.Wanderer.Proxy;
using sonarturret = POFerro.Robotics.ArduinoSonarTurret.Proxy;
using Arduino.Firmata.Types.Proxy;

namespace POFerro.Robotics.WandererDashboard
{
	[Contract(Contract.Identifier)]
	[DisplayName("WandererDashboard")]
	[Description("WandererDashboard service (no description provided)")]
	class WandererDashboardService : DsspServiceBase
	{
		[ServiceState]
		WandererDashboardState _state = new WandererDashboardState();
		
		[ServicePort("/WandererDashboard", AllowMultipleInstances = false)]
		WandererDashboardOperations _mainPort = new WandererDashboardOperations();

		/// <summary>
		/// ArduinoService partner
		/// </summary>
		[Partner("ArduinoService", Contract = Arduino.Proxy.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UsePartnerListEntry, Optional = true)]
		Arduino.Proxy.ArduinoOperations _arduinoServicePort = new Arduino.Proxy.ArduinoOperations();
		Arduino.Proxy.ArduinoOperations _arduinoServiceNotify = new Arduino.Proxy.ArduinoOperations();

		/// <summary>
		/// ArduinoSonar partner
		/// </summary>
		[Partner("Wanderer", Contract = wanderer.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		wanderer.WandererOperations _wandererPort = new wanderer.WandererOperations();
		wanderer.WandererOperations _wandererServiceNotify = new wanderer.WandererOperations();

		/// <summary>
		/// Sonar partner
		/// </summary>
		[Partner("Sonar", Contract = sonarturret.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.UseExisting)]
		sonarturret.ArduinoSonarTurretOperations _sonarPort = new sonarturret.ArduinoSonarTurretOperations();
		sonarturret.ArduinoSonarTurretOperations _sonarNotify = new sonarturret.ArduinoSonarTurretOperations();

		WandererDashboardWPF _form;
		/// <summary>
		/// WPF service port
		/// </summary>
		private ccrwpf.WpfServicePort wpfServicePort;

		public WandererDashboardService(DsspServiceCreationPort creationPort)
			: base(creationPort)
		{
		}
		
		protected override void Start()
		{
			_arduinoServicePort.Subscribe(_arduinoServiceNotify);
			_wandererPort.Subscribe(_wandererServiceNotify, typeof(wanderer.StateChangeNotify));
			_sonarPort.Subscribe(_sonarNotify, typeof(sonarturret.RangePositionReadNotify), typeof(sonarturret.RangeSweepCompleteNotify));

			base.Start();

			MainPortInterleave.CombineWith(
				new Interleave(
					new TeardownReceiverGroup(),
					new ExclusiveReceiverGroup(),
					new ConcurrentReceiverGroup(
						Arbiter.Receive<Arduino.Messages.Proxy.AnalogOutputUpdate>(true, _arduinoServiceNotify, AnalogOutputUpdateHandler),
						Arbiter.Receive<Arduino.Messages.Proxy.DigitalOutputUpdate>(true, _arduinoServiceNotify, DigitalOutputUpdateHandler),
						Arbiter.Receive<wanderer.StateChangeNotify>(true, _wandererServiceNotify, WandererStateChangeHandler),
						Arbiter.Receive<sonarturret.RangePositionReadNotify>(true, _sonarNotify, RangePositionReadNotifyHandler),
						Arbiter.Receive<sonarturret.RangeSweepCompleteNotify>(true, _sonarNotify, RangeSweepCompleteNotifyHandler)
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

			var runWindow = this.wpfServicePort.RunWindow(() => new WandererDashboardWPF());
			yield return (Choice)runWindow;

			var exception = (Exception)runWindow;
			if (exception != null)
			{
				LogError(exception);
				StartFailed();
				yield break;
			}

			// need double cast because WPF adapter doesn't know about derived window types
			this._form = (Window)runWindow as WandererDashboardWPF;
		}

		private void DigitalOutputUpdateHandler(Arduino.Messages.Proxy.DigitalOutputUpdate message)
		{
			if (this._form != null)
				this.wpfServicePort.Invoke(() => this._form.UpdateDigitalPin(message.Body));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void AnalogOutputUpdateHandler(Arduino.Messages.Proxy.AnalogOutputUpdate message)
		{
			if (this._form != null)
				this.wpfServicePort.Invoke(() => this._form.UpdateAnalogPin(message.Body));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void RangePositionReadNotifyHandler(sonarturret.RangePositionReadNotify message)
		{
			if (this._form != null)
				this.wpfServicePort.Invoke(() => this._form.UpdateRadar(message.Body));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void RangeSweepCompleteNotifyHandler(sonarturret.RangeSweepCompleteNotify message)
		{
			if (this._form != null)
				this.wpfServicePort.Invoke(() => this._form.RangeSweepCompleted(message.Body));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}

		private void WandererStateChangeHandler(wanderer.StateChangeNotify message)
		{
			if (this._form != null)
				this.wpfServicePort.Invoke(() => this._form.UpdateState(message.Body));

			message.ResponsePort.Post(DefaultUpdateResponseType.Instance);
		}
	}
}


