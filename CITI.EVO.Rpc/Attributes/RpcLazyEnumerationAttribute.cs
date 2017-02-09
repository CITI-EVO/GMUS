using System;

namespace CITI.EVO.Rpc.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = false)]
	public class RpcLazyEnumerationAttribute : Attribute
	{
		public RpcLazyEnumerationAttribute()
			: this(true)
		{
		}
		public RpcLazyEnumerationAttribute(bool fullLoad)
		{
			FullLoad = fullLoad;
		}

		public bool FullLoad { get; set; }
	}
}