using System;
using System.Xml.Serialization;
using CITI.EVO.CommonData.Web.Entities.GovTalk.Common;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Response
{
	[XmlType(AnonymousType = true, Namespace = "urn:g3.ge:moh:call:MOH_SSA_CheckPersonScoreRangeEx:v1")]
	[XmlRoot(Namespace = "urn:g3.ge:moh:call:MOH_SSA_CheckPersonScoreRangeEx:v1", IsNullable = false)]
	public class PersonScoreCheckByRangExtResultEntity : BaseMessageEntity
	{
		[XmlElement("ResultStatus")]
		public ResultStatusEntity ResultStatus { get; set; }

		[XmlElement("PrivateNumber")]
		public String PrivateNumber { get; set; }

		[XmlElement("LastName")]
		public String LastName { get; set; }

		[XmlElement("FirstName")]
		public String FirstName { get; set; }

		[XmlElement("BirthDate")]
		public DateTime? BirthDate { get; set; }

		[XmlElement("LegalScoreDate")]
		public DateTime? LegalScoreDate { get; set; }

		[XmlElement("IsInRange")]
		public bool? IsInRange { get; set; }
	}
}