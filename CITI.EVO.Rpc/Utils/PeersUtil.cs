using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.CompilerServices;
using CITI.EVO.Rpc.Config;

namespace CITI.EVO.Rpc.Utils
{
	public static class PeersUtil
	{
		private static volatile IDictionary<String, RpcPeerElement> peers;

		public static IDictionary<String, RpcPeerElement> Peers
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			get
			{
				peers = (peers ?? GetPeers());
				return peers;
			}
		}

		private static IDictionary<String, RpcPeerElement> GetPeers()
		{
			var configSection = (RpcSection)ConfigurationManager.GetSection("rpc");

			var dict = new ConcurrentDictionary<String, RpcPeerElement>();
			foreach (RpcPeerElement element in configSection.Peers)
			{
				if (!dict.TryAdd(element.Name, element))
					throw new Exception();
			}

			return dict;
		}

		public static RpcPeerElement GetPeer(String name)
		{
			RpcPeerElement element;
			if (Peers.TryGetValue(name, out element))
				return element;

			return null;
		}
	}
}
