using System;
using System.Collections;
using CITI.EVO.Rpc.Attributes;
using CITI.EVO.Rpc.Common;
using System.Collections.Generic;

namespace CITI.EVO.Rpc.Managers
{
	public class RpcCollectionManager
	{
		[RpcAllowRemoteCall]
		public CollectionItemEntity Next(Guid collectionID)
		{
			var enumerator = CollectionsCache.GetCollection(collectionID);

			var entity = new CollectionItemEntity
			{
				Success = enumerator.MoveNext(),
				Item = enumerator.Current,
			};

			return entity;
		}

		[RpcAllowRemoteCall]
		public void Reset(Guid collectionID)
		{
			var enumerator = CollectionsCache.GetCollection(collectionID);
			enumerator.Reset();
		}

		[RpcAllowRemoteCall]
		public List<Object> LoadWhole(Guid collectionID)
		{
			var list = new List<Object>();
			
			var enumerator = CollectionsCache.GetCollection(collectionID);
			while (enumerator.MoveNext())
				list.Add(enumerator.Current);

			CollectionsCache.DeleteCollection(collectionID);

			return list;
		}

		[RpcAllowRemoteCall]
		public void Dispose(Guid collectionID)
		{
			var enumerator = CollectionsCache.GetCollection(collectionID) as IDisposable;
			if (enumerator != null)
				enumerator.Dispose();

			CollectionsCache.DeleteCollection(collectionID);
		}
	}
}
