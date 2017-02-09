using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CITI.EVO.Rpc.Utils
{
	/// <summary>
	/// Object reflection util
	/// </summary>
	public static class ReflectionUtil
	{
		private readonly static IDictionary<String, MethodInfo> _methods = new Dictionary<String, MethodInfo>();
		private readonly static IDictionary<String, Type> _types = new Dictionary<String, Type>();

		private readonly static IList<Assembly> _assemblies = new List<Assembly>();

		public static bool IsBaseType(Type type)
		{
			if (type == typeof(sbyte) ||
				type == typeof(byte) ||
				type == typeof(short) ||
				type == typeof(ushort) ||
				type == typeof(int) ||
				type == typeof(uint) ||
				type == typeof(long) ||
				type == typeof(ulong) ||
				type == typeof(float) ||
				type == typeof(double) ||
				type == typeof(DateTime) ||
				type == typeof(TimeSpan) ||
				type == typeof(String) ||
				type == typeof(Guid) ||
				type == typeof(Enum) ||
				type.IsEnum)
			{
				return true;
			}

			return false;
		}

		public static Type FindType(String fullName)
		{
			Type type;
			if (!_types.TryGetValue(fullName, out type))
			{
				var assemblies = GetAssemblies();

				foreach (var assembly in assemblies)
				{
					type = assembly.GetType(fullName);
					if (type != null && !_types.ContainsKey(fullName))
					{
						_types.Add(fullName, type);

						break;
					}
				}
			}

			return type;
		}

		public static MethodInfo FindMethod(String className, String methodName, Type[] genericTypes, Type[] paramsTypes)
		{
			var methodKey = GetMethodKey(className, methodName, genericTypes, paramsTypes);

			MethodInfo method;
			if (!_methods.TryGetValue(methodKey, out method))
			{
				var classType = FindType(className);

				method = classType.GetMethod(methodName, paramsTypes);
				if (method != null)
				{
					_methods.Add(methodKey, method);
				}
			}

			return method;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private static IList<Assembly> GetAssemblies()
		{
			if (_assemblies.Count == 0)
			{

				var assemblies = AppDomain.CurrentDomain.GetAssemblies();

				foreach (var assembly in assemblies)
				{
					_assemblies.Add(assembly);
				}
			}

			return _assemblies;
		}
		private static String GetMethodKey(String className, String methodName, IEnumerable<Type> genericTypes, IEnumerable<Type> paramsTypes)
		{
			var list = new List<String>();
			list.Add(className);
			list.Add(methodName);

			if (genericTypes != null)
			{
				foreach (var genericType in genericTypes)
				{
					var hashCode = Convert.ToString(genericType.FullName.GetHashCode());
					list.Add(hashCode);
				}
			}


			if (paramsTypes != null)
			{
				foreach (var paramType in paramsTypes)
				{
					var hashCode = Convert.ToString(paramType.FullName.GetHashCode());
					list.Add(hashCode);
				}
			}

			return String.Join("_", list);
		}
	}
}