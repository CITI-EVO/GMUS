using System;
using System.Xml.Serialization;
using CITI.EVO.CommonData.Web.Entities.GovTalk.Common;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Request
{
	[XmlType(AnonymousType = true, Namespace = "urn:g3.ge:moh:call:MOH_SSA_CheckPersonScoreRangeEx:v1")]
	[XmlRoot(Namespace = "urn:g3.ge:moh:call:MOH_SSA_CheckPersonScoreRangeEx:v1", IsNullable = false)]
	public class PersonScoreRangeMessageEntity : BaseMessageEntity
	{
		[XmlElement("PrivateNumber")]
		public String PrivateNumber { get; set; }

		[XmlElement("BirthDate")]
		public String BirthDate { get; set; }

		[XmlElement("UserName")]
		public String UserName { get; set; }

		[XmlElement("Password")]
		public String Password { get; set; }
	}
}