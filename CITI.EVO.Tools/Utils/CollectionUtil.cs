using System;
using System.Collections.Generic;
using System.Linq;

namespace CITI.EVO.Tools.Utils
{
	public static class CollectionUtil
	{
		public static IEnumerable<TItem> Traversal<TItem, TKey>(IEnumerable<TItem> source, ILookup<TKey, TItem> lookup, Func<TItem, TKey> keySelector)
		{
			var stack = new Stack<TItem>(source);
			while (stack.Count > 0)
			{
				var element = stack.Pop();
				yield return element;

				var key = keySelector(element);
				var children = lookup[key];

				foreach (var child in children)
					stack.Push(child);
			}
		}

		public static IEnumerable<TItem> Traversal<TItem, TKey>(TItem item, ILookup<TKey, TItem> lookup, Func<TItem, TKey> keySelector)
		{
			var stack = new Stack<TItem>();
			stack.Push(item);

			while (stack.Count > 0)
			{
				var element = stack.Pop();
				yield return element;

				var key = keySelector(element);
				var children = lookup[key];

				foreach (var child in children)
					stack.Push(child);
			}
		}
	}
}
