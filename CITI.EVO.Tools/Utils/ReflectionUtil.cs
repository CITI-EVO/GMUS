using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CITI.EVO.Tools.Utils
{
	public static class ReflectionUtil
	{
		private static readonly StringComparer defComparer = StringComparer.OrdinalIgnoreCase;

		public static Object GetDefault(Type type)
		{
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}

			return null;
		}


		public static Type FindType(String fullName)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (var assembly in assemblies)
			{
				var type = assembly.GetType(fullName);
				if (type != null)
				{
					return type;
				}
			}

			return null;
		}


		public static void CopyTo<T>(this T sourceObject, ref T destObject, params String[] exeptProperties) where T : class
		{
			var hashSet = new HashSet<String>();
			if (exeptProperties != null)
			{
				hashSet.UnionWith(exeptProperties);
			}

			sourceObject.CopyTo(ref destObject, hashSet);
		}

		public static void CopyTo<T>(this T sourceObject, ref T destObject, ISet<String> exeptProperties) where T : class
		{
			if (sourceObject == null)
			{
				throw new ArgumentNullException("destObject");
			}

			var type = typeof(T);
			if (destObject == null)
			{
				destObject = Activator.CreateInstance<T>();
			}

			var properties = type.GetProperties();

			foreach (var propertyInfo in properties)
			{
				if ((exeptProperties != null && exeptProperties.Contains(propertyInfo.Name)) || !propertyInfo.CanRead || !propertyInfo.CanWrite)
				{
					continue;
				}

				var value = propertyInfo.GetValue(sourceObject, null);
				propertyInfo.SetValue(destObject, value, null);
			}
		}

		public static void CopyFrom<T>(this T destObject, T sourceObject, params String[] exeptProperties) where T : class
		{
			var hashSet = new HashSet<String>();
			if (exeptProperties != null)
			{
				hashSet.UnionWith(exeptProperties);
			}

			destObject.CopyFrom(sourceObject, hashSet);
		}

		public static void CopyFrom<T>(this T destObject, T sourceObject, ISet<String> exeptProperties) where T : class
		{
			if (destObject == null)
			{
				throw new ArgumentNullException("destObject");
			}

			if (sourceObject == null)
			{
				throw new ArgumentNullException("sourceObject");
			}

			var type = typeof(T);
			var properties = type.GetProperties();

			foreach (var propertyInfo in properties)
			{
				if ((exeptProperties != null && exeptProperties.Contains(propertyInfo.Name)) || !propertyInfo.CanRead || !propertyInfo.CanWrite)
				{
					continue;
				}

				var value = propertyInfo.GetValue(sourceObject, null);
				propertyInfo.SetValue(destObject, value, null);
			}
		}


		public static Object GetValue(Object obj, String propertyName, params Object[] args)
		{
			if (obj == null)
			{
				return null;
			}

			var type = obj.GetType();

			var propInfo = type.GetProperty(propertyName);
			if (propInfo == null)
			{
				return null;
			}

			return propInfo.GetValue(obj, args);
		}

		public static void SetValue(Object obj, String propertyName, Object value, params Object[] args)
		{
			if (obj == null)
			{
				return;
			}

			var type = obj.GetType();

			var propInfo = type.GetProperty(propertyName);
			if (propInfo == null)
			{
				return;
			}

			propInfo.SetValue(obj, value, args);
		}

		private static MemberInfo MemberOfCommon<TItem>(Expression<TItem> expression)
		{
			if (expression == null)
			{
				return null;
			}

			var memberExp = expression.Body as MemberExpression;
			if (memberExp != null)
			{
				var member = memberExp.Member;
				return member;
			}

			var methodCallExp = expression.Body as MethodCallExpression;
			if (methodCallExp != null)
			{
				return methodCallExp.Method;
			}

			var unaryExp = expression.Body as UnaryExpression;
			if (unaryExp != null)
			{
				memberExp = unaryExp.Operand as MemberExpression;
				if (memberExp != null)
				{
					var member = memberExp.Member;
					return member;
				}

				methodCallExp = unaryExp.Operand as MethodCallExpression;
				if (methodCallExp != null)
				{
					return methodCallExp.Method;
				}
			}

			return null;
		}

		public static MemberInfo MemberOf<TItem>(Expression<Func<TItem, Object>> expression)
		{
			return MemberOfCommon(expression);
		}


		public static MemberInfo MemberOf(Expression<Func<Object>> expression)
		{
			return MemberOfCommon(expression);
		}

		public static String NameOf<TItem>(Expression<Func<TItem, Object>> expression)
		{
			var memberInfo = MemberOf(expression);
			if (memberInfo != null)
			{
				return memberInfo.Name;
			}

			throw new Exception();
		}

		public static String NameOf(Expression<Func<Object>> expression)
		{
			var memberInfo = MemberOf(expression);
			if (memberInfo != null)
			{
				return memberInfo.Name;
			}

			throw new Exception();
		}

		public static String DescriptionOf<TItem>(Expression<Func<TItem, Object>> expression)
		{
			var memberInfo = MemberOf(expression);
			if (memberInfo != null)
			{
				var result = Attribute.GetCustomAttributes(memberInfo, typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
				if (result != null)
				{
					return result.Description;
				}
			}

			throw new Exception();
		}

		public static String DescriptionOf(Expression<Func<Object>> expression)
		{
			var memberInfo = MemberOf(expression);
			if (memberInfo != null)
			{
				var result = Attribute.GetCustomAttributes(memberInfo, typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
				if (result != null)
				{
					return result.Description;
				}
			}

			throw new Exception();
		}

		public static Object Call(Object obj, String methodName, params Object[] args)
		{
			if (obj == null)
			{
				return null;
			}

			var type = obj.GetType();

			var methodInfo = type.GetMethod(methodName);
			if (methodInfo == null)
			{
				return null;
			}

			return methodInfo.Invoke(obj, args);
		}


		public static Type MakeTypeNullable(Type type)
		{
			if (type.IsValueType)
			{
				return typeof(Nullable<>).MakeGenericType(type);
			}

			return type;
		}

		public static String GetCorrectMemberName(String name)
		{
			return Regex.Replace(name, @"\W", "_").Trim();
		}


		public static bool ContainsProperty(Object obj, String propertyPath)
		{
			var properties = propertyPath.Split('.');

			foreach (var propertyName in properties)
			{
				Object result;
				if (!TryFindPropertyValue(obj, propertyName, out result))
				{
					return false;
				}

				obj = result;
			}

			return true;
		}

		public static ISet<PropertyInfo> GetAllProperties(Type type)
		{
			var allProperties = new HashSet<PropertyInfo>();

			if (type.IsInterface)
			{
				var stack = new Stack<Type>();
				stack.Push(type);

				while (stack.Count > 0)
				{
					var current = stack.Pop();

					var members = current.GetProperties();
					allProperties.UnionWith(members);

					var parents = current.GetInterfaces();
					foreach (var parent in parents)
					{
						stack.Push(parent);
					}
				}
			}
			else
			{
				var members = type.GetProperties();
				allProperties.UnionWith(members);
			}

			return allProperties;
		}


		public static Object GetPropertyValue(Object obj, String propertyPath)
		{
			Object result;
			if (!TryGetPropertyValue(obj, propertyPath, out result))
			{
				throw new Exception("Unable to get object");
			}

			return result;
		}

		public static Object FindPropertyValue(Object obj, String propertyPath)
		{
			Object result;
			TryGetPropertyValue(obj, propertyPath, out result);

			return result;
		}

		public static bool TryGetPropertyValue(Object obj, String propertyPath, out Object result)
		{
			result = null;

			var properties = propertyPath.Split('.');

			foreach (var propertyName in properties)
			{
				Object value;
				if (!TryFindPropertyValue(obj, propertyName, out value))
				{
					return false;
				}

				obj = value;
			}

			result = obj;
			return true;
		}

		private static bool FindPropertyValue(Object obj, String propertyName, out Object result)
		{
			result = default(Object);

			if (obj == null || String.IsNullOrWhiteSpace(propertyName))
			{
				return false;
			}

			var objType = obj.GetType();

			if (obj is IDictionary)
			{
				var dict = (IDictionary)obj;
				if (!dict.Contains(propertyName))
				{
					return false;
				}

				result = dict[propertyName];
			}
			else if (IsDictionary(obj))
			{
				var prop = GetProperty(objType, "Item");
				if (prop == null)
				{
					return false;
				}

				result = prop.GetValue(obj, new Object[] { propertyName });
			}
			else if (obj is IEnumerable)
			{
				var list = TryCastToList(obj);

				var numeralIndex = -1;
				if (defComparer.Equals(propertyName, "@first"))
				{
					numeralIndex = 0;
				}
				else if (defComparer.Equals(propertyName, "@last"))
				{
					numeralIndex = list.Count - 1;
				}
				else if (propertyName.StartsWith("@"))
				{
					var strIndex = propertyName.Substring(1);

					var index = DataConverter.ToNullableInt(strIndex);
					if (index == null || index < 0 || index >= list.Count)
					{
						return false;
					}

					numeralIndex = index.GetValueOrDefault();
				}

				result = list[numeralIndex];
			}
			else
			{
				var prop = GetProperty(objType, propertyName);
				if (prop == null)
				{
					return false;
				}

				result = prop.GetValue(obj, null);
			}

			return true;
		}

		public static bool IsNullable(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static bool IsList(Object obj)
		{
			if (obj == null)
				return false;

			return IsSubclassOf(obj, typeof(IList<>));
		}

		public static bool IsGenericType(Object obj)
		{
			if (obj == null)
			{
				return false;
			}

			var type = obj.GetType();
			return (type.IsGenericType && !type.IsGenericTypeDefinition);
		}

		public static bool IsDictionary(Object obj)
		{
			if (obj == null)
				return false;

			return IsSubclassOf(obj, typeof(IDictionary<,>));
		}

		public static bool IsEnumerableType(Object obj)
		{
			return (obj is IEnumerable);
		}

		public static bool IsSubclassOf(Object obj, Type type)
		{
			var objType = obj.GetType();
			return IsSubclassOf(objType, type);
		}

		public static bool IsSubclassOf(Type objType, Type baseType)
		{
			var simpleBaseType = GetSimpleType(baseType);
			if (objType == baseType || objType == simpleBaseType)
			{
				return true;
			}

			var interfacesTypes = objType.GetInterfaces();
			foreach (var interfaceType in interfacesTypes)
			{
				var simpleInterfaceType = GetSimpleType(interfaceType);
				if (simpleBaseType.IsAssignableFrom(simpleInterfaceType))
				{
					return true;
				}
			}

			var objBaseSimpleType = GetSimpleType(objType.BaseType);
			if (objType.BaseType != typeof(Object) && (baseType.IsAssignableFrom(objType.BaseType) || baseType.IsAssignableFrom(objBaseSimpleType)))
			{
				return true;
			}

			return false;
		}


		public static IList TryCastToList(Object obj)
		{
			var list = obj as IList;
			if (list != null)
			{
				return list;
			}

			if (obj is IEnumerable)
			{
				var collection = obj as IEnumerable;

				var array = collection.Cast<Object>().ToArray();
				return array;
			}

			return null;
		}

		public static Object GetGenericOrElementType(Object obj)
		{
			if (obj == null)
			{
				return null;
			}

			var type = obj.GetType();
			if (IsGenericType(obj))
			{
				return type.GetGenericArguments()[0];
			}

			if (IsEnumerableType(obj))
			{
				var collection = (IEnumerable)obj;
				var types = GetAllTypes(collection);
				if (types.Count > 1)
				{
					throw new Exception();
				}

				return types.First();
			}

			return obj;
		}

		private static ISet<Type> GetAllTypes(IEnumerable collection)
		{
			var typesSet = new SortedSet<Type>();
			foreach (var item in collection)
			{
				if (item != null)
				{
					typesSet.Add(item.GetType());
				}
			}

			if (typesSet.Count == 0)
			{
				typesSet.Add(typeof(Object));
			}

			return typesSet;
		}

		private static PropertyInfo GetProperty(Type type, String name)
		{
			var properties = (from n in type.GetProperties()
							  where n.Name == name
							  select n).ToList();

			if (properties.Count > 1)
			{
				properties = (from n in properties
							  where n.DeclaringType == type
							  select n).ToList();
			}

			return properties.FirstOrDefault();
		}

		public static IDictionary<String, Object> ObjectToDictionary(Object obj, String rootName)
		{
			var output = new Dictionary<String, Object> { { rootName, obj } };

			//form output
			if (obj != null && !obj.GetType().IsValueType && !(obj is IEnumerable)) //TODO: solve IEnumerable
			{
				foreach (var property in obj.GetType().GetProperties())
				{
					ComposeComplexOutput(output, obj, rootName, property);
				}
			}

			return output;
		}


		private static Type GetSimpleType(Type type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition();
			}

			return type;
		}

		private static bool TryFindPropertyValue(Object obj, String propertyName, out Object result)
		{
			result = default(Object);

			if (obj == null || String.IsNullOrWhiteSpace(propertyName))
			{
				return false;
			}

			var objType = obj.GetType();

			if (obj is IDictionary)
			{
				var dict = (IDictionary)obj;
				if (!dict.Contains(propertyName))
				{
					return false;
				}

				result = dict[propertyName];
			}
			else if (IsDictionary(obj))
			{
				var prop = GetProperty(objType, "Item");
				if (prop == null)
				{
					return false;
				}

				result = prop.GetValue(obj, new Object[] { propertyName });
			}
			else if (obj is IEnumerable)
			{
				var list = TryCastToList(obj);

				var numeralIndex = -1;
				if (defComparer.Equals(propertyName, "@first"))
				{
					numeralIndex = 0;
				}
				else if (defComparer.Equals(propertyName, "@last"))
				{
					numeralIndex = list.Count - 1;
				}
				else if (propertyName.StartsWith("@"))
				{
					var strIndex = propertyName.Substring(1);

					var index = DataConverter.ToNullableInt(strIndex);
					if (index == null || index < 0 || index >= list.Count)
					{
						return false;
					}

					numeralIndex = index.GetValueOrDefault();
				}

				if (list != null && list.Count > 0)
				{
					result = list[numeralIndex];
				}
			}
			else
			{
				var prop = GetProperty(objType, propertyName);
				if (prop == null)
				{
					return false;
				}

				result = prop.GetValue(obj, null);
			}

			return true;
		}

		private static void ComposeComplexOutput(IDictionary<String, Object> output, Object parent, String @namespace, PropertyInfo property)
		{
			var name = String.Format("{0}.{1}", @namespace, property.Name);
			var obj = property.GetValue(parent, null);
			output.Add(String.Format("{0}", name), obj);

			if (obj != null && !obj.GetType().IsValueType && !(obj is IEnumerable)) //TODO: solve IEnumerable
			{
				foreach (var prop in obj.GetType().GetProperties())
				{
					ComposeComplexOutput(output, obj, name, prop);
				}
			}
		}

	}

}
