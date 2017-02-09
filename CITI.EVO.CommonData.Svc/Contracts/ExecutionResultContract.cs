using System;
using System.Runtime.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts
{
	[Serializable]
	[DataContract]
	public class ExecutionResultContract
	{
		[DataMember]
		public int ErrorCode { get; set; }

		[DataMember]
		public string ErrorMessage { get; set; }
	}
}
