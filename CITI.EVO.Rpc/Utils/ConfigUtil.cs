using System.Configuration;
using System.Runtime.CompilerServices;
using CITI.EVO.Rpc.Config;

namespace CITI.EVO.Rpc.Utils
{
	public static class ConfigUtil
	{
		private static RpcSection configSection;
		public static RpcSection ConfigSection
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			get
			{
				configSection = (configSection ?? (RpcSection)ConfigurationManager.GetSection("rpc"));
				return configSection;
			}
		}
	}
}
