using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CITI.EVO.Rpc.Attributes;
using CITI.EVO.Rpc.Collection;
using CITI.EVO.Rpc.Common;
using CITI.EVO.Rpc.Exceptions;
using CITI.EVO.Rpc.Utils;

namespace CITI.EVO.Rpc.Processing
{
	public class ClientRequestProcessor : RequestProcessorBase
	{
		protected override byte[] ProcessRequest(String peer, byte[] requestBytes)
		{
			var requestEntity = GetRequestEntity(requestBytes);

			var className = requestEntity.ClassName;
			var methodName = requestEntity.MethodName;

			var genericTypesArray = GetGenericTypesArray(requestEntity);
			var methodParamsArray = requestEntity.MethodParams.ToArray();

			var responseEntity = new ResponseEntity
			{
				RequestID = requestEntity.RequestID
			};

			try
			{
				responseEntity.ResultObject = InvokeMethod(peer, className, methodName, genericTypesArray, methodParamsArray);
			}
			catch (Exception ex)
			{
				responseEntity.ErrorCode = 1;
				responseEntity.ErrorMessage = ex.ToString();
			}

			var responseBytes = GetResponseBytes(responseEntity);
			return responseBytes;
		}

		private Object InvokeMethod(String peer, String className, String methodName, Type[] genericTypes, MethodParam[] parameters)
		{
			var paramsValues = new Object[parameters.Length];
			var paramsTypes = new Type[parameters.Length];

			if (parameters.Length > 0)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					var methodParam = parameters[i];

					paramsTypes[i] = ReflectionUtil.FindType(methodParam.TypeName);
					paramsValues[i] = methodParam.ParamValue;
				}
			}

			return InvokeMethod(peer, className, methodName, genericTypes, paramsTypes, paramsValues);
		}

		private Object InvokeMethod(String peer, String className, String methodName, Type[] genericTypes, Type[] paramsTypes, Object[] paramsValues)
		{
			var classType = ReflectionUtil.FindType(className);
			if (classType == null)
			{
				throw new ClassNotFoundException($"Unable to find class '{className}'");
			}

			var methodInfo = ReflectionUtil.FindMethod(className, methodName, genericTypes, paramsTypes);
			if (methodInfo == null)
			{
				var paramsCount = (paramsTypes == null ? 0 : paramsTypes.Length);
				var errorMessage = $"Unable to find method '{methodName}({paramsCount})' in class '{classType.FullName}'";
				throw new MethodNotFoundException(errorMessage);
			}

			if (!Attribute.IsDefined(methodInfo, typeof(RpcAllowRemoteCallAttribute)))
			{
				var paramsCount = (paramsTypes == null ? 0 : paramsTypes.Length);
				var errorMessage = $"Remote call is not allowed for method {methodName}({paramsCount})";
				throw new RemoteCallNotAllowedException(errorMessage);
			}

			var obj = (methodInfo.IsStatic ? null : Activator.CreateInstance(classType));
			if (methodInfo.IsGenericMethod && genericTypes != null)
			{
				methodInfo = methodInfo.MakeGenericMethod(genericTypes);
			}

			var disposable = obj as IDisposable;
			if (disposable != null)
			{
				using (disposable)
				{
					return methodInfo.Invoke(obj, paramsValues);
				}
			}

			var result = methodInfo.Invoke(obj, paramsValues);

			if (ConfigUtil.ConfigSection.Client.LazyCollection)
			{
				result = ReplaceCollections(methodInfo, peer, result);
			}

			return result;
		}

		private Type[] GetGenericTypesArray(RequestEntity requestEntity)
		{
			var genericTypesArr = new Type[0];

			if (requestEntity.GenericTypes != null)
			{
				var genericTypesList = requestEntity.GenericTypes;

				genericTypesArr = new Type[genericTypesList.Count];

				for (var i = 0; i < genericTypesList.Count; i++)
				{
					var genericTypeName = genericTypesList[i];

					var genericType = ReflectionUtil.FindType(genericTypeName);
					if (genericType == null)
					{
						throw new GenericTypeNotFoundException($"Unable find generic type '{genericTypeName}'");
					}

					genericTypesArr[i] = genericType;
				}
			}

			return genericTypesArr;
		}

		private Object ReplaceCollections(MemberInfo memberInfo, String peer, Object obj)
		{
			if (obj == null)
			{
				return obj;
			}

			var type = obj.GetType();
			if (ReflectionUtil.IsBaseType(type))
			{
				return obj;
			}

			var lazyAttr = Attribute.GetCustomAttribute(memberInfo, typeof(RpcLazyEnumerationAttribute)) as RpcLazyEnumerationAttribute;
			if (lazyAttr == null)
			{
				return obj;
			}

			if (type == typeof(IEnumerable) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
			{
				var enumerable = (IEnumerable)obj;
				obj = enumerable.GetEnumerator();

				var lazyCollType = typeof(LazyRpcCollection<>);
				if (type.IsGenericType)
				{
					var genericType = type.GetGenericArguments()[0];
					lazyCollType = lazyCollType.MakeGenericType(genericType);
				}

				var collectionID = CollectionsCache.InsertCollection((IEnumerator)obj);

				var lazyCollection = Activator.CreateInstance(lazyCollType, peer, collectionID, lazyAttr.FullLoad);
				return lazyCollection;
			}

			var properties = type.GetProperties();
			foreach (var propertyInfo in properties)
			{
				if (Attribute.IsDefined(propertyInfo, typeof(RpcLazyEnumerationAttribute)))
				{
					var propertyValue = propertyInfo.GetValue(obj, null);
					propertyValue = ReplaceCollections(propertyInfo, peer, propertyValue);

					propertyInfo.SetValue(obj, propertyValue, null);
				}
			}

			return obj;
		}
	}
}
