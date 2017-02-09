using System;
using System.Configuration;

namespace CITI.EVO.Tools.Security.Configs
{
	public class PermissionElementCollection : ConfigurationElementCollection
	{

		public PermissionElement this[int index]
		{
			get
			{
				return (PermissionElement)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}

				BaseAdd(index, value);
			}
		}

		public void Add(PermissionElement element)
		{
			BaseAdd(element);
		}

		public void Clear()
		{
			BaseClear();
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new PermissionElement();
		}

		protected override Object GetElementKey(ConfigurationElement element)
		{
			return ((PermissionElement)element).PermissionKey;
		}

		public void Remove(PermissionElement element)
		{
			BaseRemove(element.PermissionKey);
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public void Remove(string name)
		{
			BaseRemove(name);
		}
	}
}