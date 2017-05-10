using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace CITI.EVO.Tools.Collections
{
    [Serializable]
    public class NameObjectCollection : NameObjectCollectionBase
	{
		public Object this[String name]
		{
			get { return BaseGet(name); }
			set { BaseSet(name, value); }
		}

		public void Add(String name, Object value)
		{
			this[name] = value;
		}

		public void Remove(String name, Object value)
		{
			BaseRemove(name);
		}

		public bool Contains(String name)
		{
			return BaseGetAllKeys().Contains(name);
		}

		public void Clear()
		{
			BaseClear();
		}
	}
}
