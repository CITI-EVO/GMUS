using System;
using System.Runtime.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts
{
	[DataContract]
	[Serializable]
	public class TranslationContract
	{
		[DataMember]
		public String TrnKey { get; set; }

		[DataMember]
		public String DefaultText { get; set; }

		[DataMember]
		public String TranslatedText { get; set; }
	}
}
