using System.Collections.Generic;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Web.Entities.GovTalk.Common
{
	[XmlType(AnonymousType = true)]
	[XmlRoot("GovTalkErrors", IsNullable = false)]
	public class GovTalkErrorsEntity
	{
		//public ErrorEntity Error { get; set; }

		[XmlElement("Error")]
		public List<ErrorEntity> Errors { get; set; }
	}
}