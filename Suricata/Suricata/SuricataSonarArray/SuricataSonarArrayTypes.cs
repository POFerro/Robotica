//------------------------------------------------------------------------------
//  <copyright file="ReferencePlatform2011SonarArrayTypes.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation.  All rights reserved.
//  </copyright>
//------------------------------------------------------------------------------
using Microsoft.Dss.Core.Attributes;

using sonar = Microsoft.Robotics.Services.Sonar.Proxy;

namespace POFerro.Robotics.Suricata.SuricataSonarArray
{
    /// <summary>
    /// The contract
    /// </summary>
    public sealed class Contract
    {
        /// <summary>
        /// The contract
        /// </summary>
        [DataMember]
        public const string Identifier = "http://schemas.pferro/robotics/2013/04/suricatasonararray.html";
    }

	/// <summary>
	/// SuricataIRArray state
	/// </summary>
	[DataContract]
	public class SuricataIRArrayState
	{
		[DataMember()]
		public sonar.SonarState SonarLeftState { get; set; }
		[DataMember()]
		public sonar.SonarState SonarRightState { get; set; }
	}
}