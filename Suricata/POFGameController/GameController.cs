//-----------------------------------------------------------------------
//  This file is part of Microsoft Robotics Developer Studio Code Samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  $File: GameController.cs $ $Revision: 7 $
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;

using Microsoft.Ccr.Core;
using Microsoft.Dss.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;

using sm = Microsoft.Dss.Services.SubscriptionManager;
using gamecontroller = Microsoft.Robotics.Services.GameController.Proxy;
using W3C.Soap;



namespace POFerro.Robotics.GameController
{

    /// <summary>
    /// Provides access to a DirectInput game controller such as a joystick or gamepad.
    /// </summary>
    [DisplayName("Fake Game Controller")]
    [Contract(Contract.Identifier)]
    [DssServiceDescription("http://msdn.microsoft.com/library/dd145253.aspx")]
	[AlternateContract(gamecontroller.Contract.Identifier)]
    public class GameControllerService : DsspServiceBase
    {
        [ServiceState]
        [InitialStatePartner(Optional = true)]
        private GameControllerState _state;

        [ServicePort("/gamecontroller", AllowMultipleInstances=true)]
        private GameControllerOperations _mainPort = new GameControllerOperations();

		[AlternateServicePort(AlternateContract=gamecontroller.Contract.Identifier, AllowMultipleInstances = true)]
		private gamecontroller.GameControllerOperations _gameControllerPort = new gamecontroller.GameControllerOperations();

        [Partner("SubMgr", Contract = sm.Contract.Identifier, CreationPolicy = PartnerCreationPolicy.CreateAlways, Optional = false)]
        sm.SubscriptionManagerPort _subMgr = new sm.SubscriptionManagerPort();

        /// <summary>
        /// Default Service Constructor
        /// </summary>
        public GameControllerService(DsspServiceCreationPort creationPort) :
                base(creationPort)
        {
        }

        /// <summary>
        /// Service Start
        /// </summary>
        protected override void Start()
        {
            if (_state == null)
            {
                _state = new GameControllerState();
            }
            base.Start();

            // post a replace message to ourself, this causes the correct initialization
			//gamecontroller.Replace replace = new gamecontroller.Replace(_state);
			//_gameControllerPort.Post(replace);

            // start the timer
            Spawn(DateTime.Now, TimerHandler);
        }

        void TimerHandler(DateTime signal)
        {
            try
            {
				gamecontroller.Poll poll = new gamecontroller.Poll(new gamecontroller.PollRequest());
                _gameControllerPort.Post(poll);
            }
            finally
            {
                Activate(
                    Arbiter.Receive(false, TimeoutPort(50), TimerHandler)
                );
            }
        }

        /// <summary>
        /// Handles the replace message by replacing the state of the service
        /// </summary>
        /// <param name="replace"></param>
        /// <returns></returns>
        [ServiceHandler(ServiceHandlerBehavior.Exclusive, PortFieldName="_gameControllerPort")]
        public virtual IEnumerator<ITask> ReplaceHandler(gamecontroller.Replace replace)
        {
			//_state.Controller.Dispose();

			//_state = replace.Body;

            //_state.Controller.FindInstance();

            replace.ResponsePort.Post(DefaultReplaceResponseType.Instance);
            yield break;
        }

        /// <summary>
        /// Handles the Poll message by updating the state of the contollers and sending
        /// appropriate notifications.
        /// </summary>
        /// <param name="poll"></param>
        /// <returns></returns>
		[ServiceHandler(ServiceHandlerBehavior.Exclusive, PortFieldName = "_gameControllerPort")]
		public virtual IEnumerator<ITask> PollHandler(gamecontroller.Poll poll)
        {
			gamecontroller.Substate updated = _state.Update(DateTime.Now);

			if ((updated & gamecontroller.Substate.Axes) != gamecontroller.Substate.None)
            {
				SendNotification<gamecontroller.UpdateAxes>(_subMgr, _state.Axes);
            }
			if ((updated & gamecontroller.Substate.Buttons) != gamecontroller.Substate.None)
            {
				SendNotification<gamecontroller.UpdateButtons>(_subMgr, _state.Buttons);
            }
			if ((updated & gamecontroller.Substate.PovHats) != gamecontroller.Substate.None)
            {
				SendNotification<gamecontroller.UpdatePovHats>(_subMgr, _state.PovHats);
            }
			if ((updated & gamecontroller.Substate.Sliders) != gamecontroller.Substate.None)
            {
				SendNotification<gamecontroller.UpdateSliders>(_subMgr, _state.Sliders);
            }

            poll.ResponsePort.Post(DefaultSubmitResponseType.Instance);
            yield break;
        }

        /// <summary>
        /// Handles a subscribe request.
        /// </summary>
        /// <param name="subscribe"></param>
        /// <returns></returns>
		[ServiceHandler(ServiceHandlerBehavior.Concurrent, PortFieldName = "_gameControllerPort")]
		public virtual IEnumerator<ITask> SubscribeHandler(gamecontroller.Subscribe subscribe)
        {
			SubscribeHelper(_subMgr, subscribe, subscribe.ResponsePort);
			yield break;
			//SubscribeRequestType request = subscribe.Body;

			//yield return Arbiter.Choice(
			//	SubscribeHelper(_subMgr, request, subscribe.ResponsePort),
			//	delegate(SuccessResult success)
			//	{
			//		SendNotificationToTarget<Replace>(request.Subscriber, _subMgr, _state);
			//	},
			//	delegate(Exception failure) { }
			//);
        }

        /// <summary>
        /// Handles a request to change the current controller.
        /// </summary>
        /// <param name="changeController"></param>
        /// <returns></returns>
		[ServiceHandler(ServiceHandlerBehavior.Exclusive, PortFieldName = "_gameControllerPort")]
		public virtual IEnumerator<ITask> ChangeControllerHandler(gamecontroller.ChangeController changeController)
        {
			gamecontroller.Controller newController = changeController.Body;
			//if (newController.FindInstance())
			//{
			//	_state.Controller.Dispose();

			//	_state.Controller = newController;

			//	changeController.ResponsePort.Post(DefaultUpdateResponseType.Instance);

				//SendNotification<ChangeController>(_subMgr, _state.Controller);
			//}
			//else
			//{
				changeController.ResponsePort.Post(Fault.FromCodeSubcodeReason(
					W3C.Soap.FaultCodes.Receiver,
					DsspFaultCodes.OperationFailed,
					Resources.ControllerNotFound
					)
				);
			//}
            yield break;
        }

        /// <summary>
        /// Handles a request to update the controller axes state.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
		[ServiceHandler(ServiceHandlerBehavior.Exclusive, PortFieldName = "_gameControllerPort")]
		public virtual IEnumerator<ITask> UpdateAxesHandler(gamecontroller.UpdateAxes update)
        {
            ActionNotSupported(update.ResponsePort);
            yield break;
        }

        /// <summary>
        /// Handles a request to update the controller button state.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
		[ServiceHandler(ServiceHandlerBehavior.Exclusive, PortFieldName = "_gameControllerPort")]
		public virtual IEnumerator<ITask> UpdateButtonsHandler(gamecontroller.UpdateButtons update)
        {
            ActionNotSupported(update.ResponsePort);
            yield break;
        }

        /// <summary>
        /// Handles a request to update the Point Of View Hats state.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
		[ServiceHandler(ServiceHandlerBehavior.Exclusive, PortFieldName = "_gameControllerPort")]
		public virtual IEnumerator<ITask> UpdatePovHatsHandler(gamecontroller.UpdatePovHats update)
        {
            ActionNotSupported(update.ResponsePort);
            yield break;
        }

        /// <summary>
        /// Handles a request to update the sliders state.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
		[ServiceHandler(ServiceHandlerBehavior.Exclusive, PortFieldName = "_gameControllerPort")]
		public virtual IEnumerator<ITask> UpdateSlidersHandler(gamecontroller.UpdateSliders update)
        {
            ActionNotSupported(update.ResponsePort);
            yield break;
        }

        private void ActionNotSupported(PortSet<DefaultUpdateResponseType, W3C.Soap.Fault> responsePort)
        {
            responsePort.Post(Fault.FromCodeSubcodeReason(
                W3C.Soap.FaultCodes.Receiver,
                DsspFaultCodes.ActionNotSupported,
                Resources.NotModifiable
                )
            );
        }

        /// <summary>
        /// Handles a request to return the current controllers.
        /// </summary>
        /// <param name="getControllers"></param>
        /// <returns></returns>
		[ServiceHandler(ServiceHandlerBehavior.Concurrent, PortFieldName = "_gameControllerPort")]
        public virtual IEnumerator<ITask> GetControllersHandler(gamecontroller.GetControllers getControllers)
        {
			gamecontroller.GetControllersResponse response = new gamecontroller.GetControllersResponse();
            //response.Controllers.AddRange(Controller.Attached);

			foreach (gamecontroller.Controller controller in response.Controllers)
            {
                controller.Current = (controller.Instance == _state.Controller.Instance);
            }

            getControllers.ResponsePort.Post(response);
            yield break;
        }
    }
}
