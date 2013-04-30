//------------------------------------------------------------------------------
//  <copyright file="ObstacleAvoidanceDriveTypes.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation.  All rights reserved.
//  </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Robotics.Services.ObstacleAvoidanceDrive
{
    using System;
    using Microsoft.Ccr.Core;
    using Microsoft.Dss.Core.Attributes;
    using Microsoft.Dss.Core.DsspHttp;
    using Microsoft.Dss.ServiceModel.Dssp;
    using Microsoft.Robotics.PhysicalModel;

    /// <summary>
    /// Contract class
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
        /// Contract identifier
        /// </summary>
        [DataMember]
        public const string Identifier = "http://schemas.microsoft.com/2011/07/obstacleavoidancedrive.user.html";
    }

    /// <summary>
    /// Operations port
    /// </summary>
    public class ObstacleAvoidanceDriveOperationsPort : PortSet<Get, HttpQuery, HttpGet, DsspDefaultGet, DsspDefaultLookup, DsspDefaultDrop>
    {
    }

    /// <summary>
    /// Simple PID controller state and behavior
    /// </summary>     
    [DataContract]
    public class PIDController
    {
        /// <summary>
        /// Default proportional constant
        /// </summary>
        public const double ProportionalGainDefault = 0.25;

        /// <summary>
        /// Default derivative constant
        /// </summary>
        public const double DerivativeGainDefault = 0.05;

        /// <summary>
        /// Default integral constant
        /// </summary>
        public const double IntegralGainDefault = 0.02;

        /// <summary>
        /// Integral constant
        /// </summary>
        [DataMember]
        public double Ki = IntegralGainDefault;

        /// <summary>
        ///  Proportional constant
        /// </summary>
        [DataMember]
        public double Kp = ProportionalGainDefault;

        /// <summary>
        ///  Derivative constant
        /// </summary>
        [DataMember]
        public double Kd = DerivativeGainDefault;

        /// <summary>
        /// Previous error
        /// </summary>
        public double PreviousError;

        /// <summary>
        /// Most recent error
        /// </summary>
        public double CurrentError;

        /// <summary>
        /// Derivative error
        /// </summary>
        public double DerivativeError;

        /// <summary>
        /// Accumulated error
        /// </summary>
        public double IntegralError;

        /// <summary>
        /// Maximum integral error
        /// </summary>
        private const double MaxIntegralError = 2;

        /// <summary>
        /// Update the controller state
        /// </summary>
        /// <param name="newError">The new error value</param>
        /// <param name="updateInterval">Time since last update</param>
        public void Update(double newError, double updateInterval)
        {
            this.PreviousError = this.CurrentError;
            this.CurrentError = newError;
            if (updateInterval > 1)
            {
                // it has taken too long between updates, reset
                this.IntegralError = 0;
            }

            if (updateInterval > 0)
            {
                this.DerivativeError = (this.CurrentError - this.PreviousError) / updateInterval;
                this.IntegralError += this.DerivativeError;
                if (this.IntegralError >= MaxIntegralError ||
                    this.IntegralError <= -MaxIntegralError)
                {
                    this.IntegralError = 0;
                }
            }
        }

        /// <summary>
        /// Calculate control. It does not produce a linear speed
        /// </summary>
        /// <param name="angularSpeed">Calculated angular speed</param>
        /// <param name="speed">Calculated linear speed (not used)</param>
        public void CalculateControl(out double angularSpeed, out double speed)
        {
            angularSpeed = 0;
            angularSpeed =
                (this.CurrentError * this.Kp) +
                (this.IntegralError * this.Ki) +
                (this.DerivativeError * this.Kd);

            speed = 0;

            if (Math.Abs(angularSpeed) > 1)
            {
                angularSpeed = 1 * Math.Sign(angularSpeed);
            }
        }

        /// <summary>
        /// Reset state
        /// </summary>
        public void Reset()
        {
            this.PreviousError = this.CurrentError = this.IntegralError = 0;
        }
    }

    /// <summary>
    /// Service state
    /// </summary>
    [DataContract]
    public class ObstacleAvoidanceDriveState
    {
        /// <summary>
        /// Gets or sets robot width in meters
        /// </summary>
        [DataMember]
        public double RobotWidth { get; set; }

        /// <summary>
        /// Gets or sets max power allowed per wheel
        /// </summary>
        [DataMember]
        public double MaxPowerPerWheel { get; set; }

		/// <summary>
		/// When turning, we want to eliminate drastic differences between wheel power settings.
		/// not doing so may result in unpractically fast rotation that essentially makes robot 
		/// uncontrollable
		/// </summary>
		[DataMember]
		public double MaxPowerDifferenceBetweenWheels { get; set; }

        /// <summary>
        /// Gets or sets the minimum rotation speed
        /// </summary>
        [DataMember]
        public double MinRotationSpeed { get; set; }

        /// <summary>
        /// Gets or sets the depth camera position relative to the floor plane 
        /// and the projection of the center of mass of the robot to the floor plane
        /// </summary>
        [DataMember]
        public Vector3 DepthCameraPosition { get; set; }

        /// <summary>
        /// Gets or sets the obstacle avoidance controller state
        /// </summary>
        [DataMember]
        public PIDController Controller { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed change in Power from one call to SetPower to the next
        /// Smaller numbers will cause smoother accelrations, but can also increase chance of collision with 
        /// obstacles
        /// </summary>
        [DataMember]
        public double MaxDeltaPower { get; set; }
    }

    /// <summary>
    /// Get operation
    /// </summary>
    public class Get : Get<GetRequestType, DsspResponsePort<ObstacleAvoidanceDriveState>>
    {
    }

    /// <summary>
    /// Partner names
    /// </summary>
    [DataContract]
    public class Partners
    {
        /// <summary>
        /// Drive service
        /// </summary>
        [DataMember]
        public const string Drive = "Drive";

        /// <summary>
        /// Depth cam service
        /// </summary>
        [DataMember]
        public const string DepthCamSensor = "DepthCamera";

        /// <summary>
        /// IR sensor array
        /// </summary>
        [DataMember]
        public const string InfraredSensorArray = "InfraredSensorArray";

        /// <summary>
        /// Sonar analog sensors
        /// </summary>
        [DataMember]
        public const string SonarSensorArray = "SonarSensorArray";

        /// <summary>
        /// Time we are willing to wait for each partner to start
        /// </summary>
        [DataMember]
        public const int PartnerEnumerationTimeoutInSeconds = 120;
    }
}
