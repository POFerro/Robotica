using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;

namespace POFerro.Robotics.SuricataDashboard
{
	public sealed class Contract
	{
		[DataMember]
		public const string Identifier = "http://schemas.pferro/2013/04/suricatadashboard.html";
	}
	
	[DataContract]
	public class SuricataDashboardState
	{
	}
	
	[ServicePort]
	public class SuricataDashboardOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get>
	{
	}
	
	public class Get : Get<GetRequestType, PortSet<SuricataDashboardState, Fault>>
	{
		public Get()
		{
		}
		
		public Get(GetRequestType body)
			: base(body)
		{
		}
		
		public Get(GetRequestType body, PortSet<SuricataDashboardState, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}
}


