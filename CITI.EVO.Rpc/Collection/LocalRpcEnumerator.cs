using System;
using System.Collections;
using System.Collections.Generic;
using CITI.EVO.Rpc;

namespace CITI.EVO.Rpc.Collection
{
	[Serializable]
	public class LocalRpcEnumerator<TItem> : IEnumerator<TItem>
	{
		private readonly String _peer;
		private readonly Guid _collectionID;

		private readonly String _resetMethodName;
		private readonly String _disposeMethodName;
		private readonly String _loadWholeMethodName;

		private readonly Lazy<IEnumerator<Object>> _enumeratorLazy;

		public LocalRpcEnumerator(String peer, Guid collectionID)
		{
			_peer = peer;
			_collectionID = collectionID;

			_enumeratorLazy = new Lazy<IEnumerator<Object>>(LoadWhole);

			_resetMethodName = String.Format("{0}.CITI.EVO.Rpc.Managers.RpcCollectionManager.Reset", _peer);
			_disposeMethodName = String.Format("{0}.CITI.EVO.Rpc.Managers.RpcCollectionManager.Dispose", _peer);
			_loadWholeMethodName = String.Format("{0}.CITI.EVO.Rpc.Managers.RpcCollectionManager.LoadWhole", _peer);
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
			if (_enumeratorLazy.IsValueCreated)
				_enumeratorLazy.Value.Dispose();
			else
				RpcInvoker.CallMethod(_disposeMethodName, _collectionID);
		}

		public bool MoveNext()
		{
			var flag = _enumeratorLazy.Value.MoveNext();
			Current = (TItem)_enumeratorLazy.Value.Current;

			return flag;
		}

		public void Reset()
		{
			if (_enumeratorLazy.IsValueCreated)
				_enumeratorLazy.Value.Reset();
			else
				RpcInvoker.CallMethod(_resetMethodName, _collectionID);
		}

		private IEnumerator<Object> LoadWhole()
		{
			var list = RpcInvoker.CallMethod<List<Object>>(_loadWholeMethodName, _collectionID);
			return list.GetEnumerator();
		}
	}
}
