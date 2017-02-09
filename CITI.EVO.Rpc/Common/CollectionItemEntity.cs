using System;

namespace CITI.EVO.Rpc.Common
{
	[Serializable]
	public class CollectionItemEntity
	{
		public bool Success { get; set; }

		public Object Item { get; set; }
	}
}
