using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Rpc.Common;

namespace CITI.EVO.Rpc.Collection
{
	[Serializable]
	public class LazyRpcCollection<TItem> : IEnumerable<TItem>, IEnumerator<TItem>
	{
		private readonly String _peer;
		private readonly bool _fullLoad;
		private readonly Guid _collectionID;

		private readonly IEnumerator<TItem> _enumerator;

		public LazyRpcCollection(String peer, Guid collectionID, bool fullLoad)
		{
			_peer = peer;
			_fullLoad = fullLoad;
			_collectionID = collectionID;

			if (_fullLoad)
				_enumerator = new LocalRpcEnumerator<TItem>(_peer, _collectionID);
			else
				_enumerator = new RemoteRpcEnumerator<TItem>(_peer, _collectionID);
		}

		public String Peer
		{
			get { return _peer; }
		}

		public Guid CollectionID
		{
			get { return _collectionID; }
		}

		public bool FullLoad
		{
			get { return _fullLoad; }
		}

		public TItem Current
		{
			get { return _enumerator.Current; }
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}

		public IEnumerator<TItem> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Dispose()
		{
			_enumerator.Dispose();
		}

		public bool MoveNext()
		{
			return _enumerator.MoveNext();
		}

		public void Reset()
		{
			_enumerator.Reset();
		}
	}
}
