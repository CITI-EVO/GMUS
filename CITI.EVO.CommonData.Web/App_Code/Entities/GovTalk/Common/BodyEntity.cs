using System.Xml.Serialization;
using CITI.EVO.CommonData.Web.Entities.GovTalk.Request;
using CITI.EVO.CommonData.Web.Entities.GovTalk.Response;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("Body", IsNullable = false)]
	public class BodyEntity
	{
		[XmlElement("syncfault", typeof(SyncFaultEntity), Namespace = "urn:government-gateway.cz:sync:error")]
		[XmlElement("Message", typeof(PersonScoreRangeMessageEntity), Namespace = "urn:g3.ge:moh:call:MOH_SSA_CheckPersonScoreRangeEx:v1")]
		[XmlElement("PersonScoreCheckByRangExtResult", typeof(PersonScoreCheckByRangExtResultEntity), Namespace = "urn:g3.ge:moh:call:MOH_SSA_CheckPersonScoreRangeEx:v1")]
		public BaseMessageEntity Entity { get; set; }
	}
}