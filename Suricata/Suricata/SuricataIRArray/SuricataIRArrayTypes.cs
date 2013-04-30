//------------------------------------------------------------------------------
//  <copyright file="ReferencePlatform2011IRArrayTypes.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation.  All rights reserved.
//  </copyright>
//------------------------------------------------------------------------------

using Microsoft.Dss.Core.Attributes;

using analogsensor = Microsoft.Robotics.Services.AnalogSensor.Proxy;

namespace POFerro.Robotics.Suricata.SuricataIRArray
{
    /// <summary>
    /// Contract for SuricataIRArray
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
		/// Contract for SuricataIRArray
        /// </summary>
        [DataMember]
        public const string Identifier = "http://schemas.pferro/robotics/2013/04/suricatairarray.html";
    }

	/// <summary>
    /// SuricataIRArray state
    /// </summary>
	[DataContract]
	public class SuricataIRArrayState
	{
		[DataMember()]
		public analogsensor.AnalogSensorState FrontLeftIRState { get; set; }
		[DataMember()]
		public analogsensor.AnalogSensorState FrontMiddleIRState { get; set; }
		[DataMember()]
		public analogsensor.AnalogSensorState FrontRightIRState { get; set; }
	}
}
