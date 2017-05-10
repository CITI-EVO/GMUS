using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.Tools.Collections
{
    [Serializable]
    public class NameValueDataList : Dictionary<String, Object>
	{
		public NameValueDataList()
			: base(StringComparer.InvariantCultureIgnoreCase)
		{
		}

		public NameValueDataList(IDictionary<String, Object> dictionary)
			: base(dictionary, StringComparer.InvariantCultureIgnoreCase)
		{
		}

		public new Object this[String key]
		{
			get
			{
				Object value;
				if (!TryGetValue(key, out value))
					value = ReflectionUtil.GetPropertyValue(this, key);

				return value;
			}
			set
			{
				base[key] = value;
			}
		}

		public new bool ContainsKey(String key)
		{
			if (base.ContainsKey(key))
				return true;

			return ReflectionUtil.ContainsProperty(this, key);
		}
	}

}
