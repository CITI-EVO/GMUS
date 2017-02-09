using System;
using System.Collections;
using System.Collections.Generic;
using CITI.EVO.Rpc;
using CITI.EVO.Rpc.Common;

namespace CITI.EVO.Rpc.Collection
{
	[Serializable]
	public class RemoteRpcEnumerator<TItem> : IEnumerator<TItem>
	{
		private readonly String _peer;
		private readonly Guid _collectionID;

		private readonly String _nextMethodName;
		private readonly String _resetMethodName;
		private readonly String _disposeMethodName;

		public RemoteRpcEnumerator(String peer, Guid collectionID)
		{
			_peer = peer;
			_collectionID = collectionID;

			_nextMethodName = String.Format("{0}.CITI.EVO.Rpc.Managers.RpcCollectionManager.Next", _peer);
			_resetMethodName = String.Format("{0}.CITI.EVO.Rpc.Managers.RpcCollectionManager.Reset", _peer);
			_disposeMethodName = String.Format("{0}.CITI.EVO.Rpc.Managers.RpcCollectionManager.Dispose", _peer);
		}

		public String Peer
		{
			get { return _peer; }
		}

		public Guid CollectionID
		{
			get { return _collectionID; }
		}

		public TItem Current { get; private set; }

		object IEnumerator.Current
		{
			get { return Current; }
		}

		public void Dispose()
		{
			RpcInvoker.CallMethod(_disposeMethodName, _collectionID);
		}

		public bool MoveNext()
		{
			var entity = RpcInvoker.CallMethod<CollectionItemEntity>(_nextMethodName, _collectionID);
			if (entity == null || !entity.Success)
				return false;

			Current = (TItem)entity.Item;
			return true;
		}

		public void Reset()
		{
			RpcInvoker.CallMethod(_resetMethodName, _collectionID);
		}
	}
}
