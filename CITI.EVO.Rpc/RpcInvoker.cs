using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using CITI.EVO.Rpc.Attributes;
using CITI.EVO.Rpc.Common;
using CITI.EVO.Rpc.Exceptions;
using CITI.EVO.Rpc.Utils;

namespace CITI.EVO.Rpc
{
	public static class RpcInvoker
	{
		public static void InvokeMethod()
		{
			var methodBase = GetStackMethodBase();
			var genericTypes = GetGenericTypes(methodBase);

			InvokeMethod<Object>(methodBase, genericTypes, new MethodParam[0]);
		}
		public static void InvokeMethod(params Object[] parameters)
		{
			var methodBase = GetStackMethodBase();

			var genericTypes = GetGenericTypes(methodBase);
			var methodParams = GetMethodParams(methodBase, parameters);

			InvokeMethod<Object>(methodBase, genericTypes, methodParams);
		}

		public static TResult InvokeMethod<TResult>()
		{
			var methodBase = GetStackMethodBase();
			var genericTypes = GetGenericTypes(methodBase);

			return InvokeMethod<TResult>(methodBase, genericTypes, new MethodParam[0]);
		}
		public static TResult InvokeMethod<TResult>(params Object[] parameters)
		{
			var methodBase = GetStackMethodBase();

			var genericTypes = GetGenericTypes(methodBase);
			var methodParams = GetMethodParams(methodBase, parameters);

			return InvokeMethod<TResult>(methodBase, genericTypes, methodParams);
		}
		public static TResult InvokeMethod<TResult>(params MethodParam[] parameters)
		{
			var methodBase = GetStackMethodBase();
			var genericTypes = GetGenericTypes(methodBase);

			return InvokeMethod<TResult>(methodBase, genericTypes, parameters);
		}
		private static TResult InvokeMethod<TResult>(MethodBase methodBase, IEnumerable<String> genericTypes, IEnumerable<MethodParam> methodParams)
		{
			if (!Attribute.IsDefined(methodBase, typeof(RpcRemoteMethodAttribute)))
			{
				throw new MethodAttributeNotDefinedException("HmisRemoteMethodAttribute is not defined");
			}

			var remoteMethodAttr = (RpcRemoteMethodAttribute)Attribute.GetCustomAttribute(methodBase, typeof(RpcRemoteMethodAttribute));
			if (String.IsNullOrWhiteSpace(remoteMethodAttr.FullName))
			{
				throw new MethodFullNameEmptyException("HmisRemoteMethodAttribute.MethodFullName is Empty");
			}

            return CallMethod<TResult>(remoteMethodAttr.FullName, genericTypes, methodParams);
		}

        public static void CallMethod(String fullName)
		{
			InvokeMethod<Object>(fullName, null, new MethodParam[0]);
		}
        public static void CallMethod(String fullName, params Object[] parameters)
		{
			var methodParams = GetMethodParams(parameters);
			InvokeMethod<Object>(fullName, null, methodParams);
		}

		public static TResult CallMethod<TResult>(String fullName)
		{
			return InvokeMethod<TResult>(fullName, null, new MethodParam[0]);
		}
        public static TResult CallMethod<TResult>(String fullName, params Object[] parameters)
		{
			var methodParams = GetMethodParams(parameters);

			return InvokeMethod<TResult>(fullName, null, methodParams);
		}
        public static TResult CallMethod<TResult>(String fullName, params MethodParam[] parameters)
		{
            return CallMethod<TResult>(fullName, null, parameters);
		}

        private static TResult CallMethod<TResult>(String fullName, IEnumerable<String> genericTypes, IEnumerable<MethodParam> methodParams)
		{
			if (ConfigUtil.ConfigSection == null || ConfigUtil.ConfigSection.Client == null)
			{
				throw new Exception("Invalid configuration");
			}

			var serverUrl = ConfigUtil.ConfigSection.Client.ServerUrl;

			var timeout = ConfigUtil.ConfigSection.Client.RequestTimeout;
			if (timeout == 0)
				timeout = 100000;

			var parts = fullName.Split('.');

			var peer = parts[0];
			var methodName = parts[parts.Length - 1];

			var className = String.Join(".", parts, 1, parts.Length - 2);

			var requestEntity = new RequestEntity
			{
				RequestID = Guid.NewGuid(),
				Peer = peer,
				ClassName = className,
				MethodName = methodName,
				GenericTypes = new List<String>(genericTypes),
				MethodParams = new List<MethodParam>(methodParams),
			};

			var responseEntity = SendRequest(serverUrl, timeout, requestEntity);
			if (responseEntity.ErrorCode > 0)
			{
				throw new ServerInvocationException(responseEntity.ErrorCode, responseEntity.ErrorMessage);
			}

			return (TResult)responseEntity.ResultObject;
		}

		private static ResponseEntity SendRequest(String serverUrl, int timeout, RequestEntity requestEntity)
		{
			var requestBytes = GetRequestBytes(requestEntity);

			Uri serverUri;
			if (!Uri.TryCreate(serverUrl, UriKind.RelativeOrAbsolute, out serverUri))
			{
				throw new Exception("Invalid Server URL");
			}

			var transporter = TransporterUtil.CreateServerTransporter(serverUrl, timeout);
			var responseBytes = transporter.Send(requestBytes);

			var responseEntity = GetResponseEntity(responseBytes);
			return responseEntity;
		}

		private static IEnumerable<String> GetGenericTypes(MethodBase methodBase)
		{
			var genericTypes = new List<String>();
			if (methodBase.IsGenericMethod)
			{
				var genericArgs = methodBase.GetGenericArguments();
				foreach (var genericArg in genericArgs)
				{
					genericTypes.Add(genericArg.FullName);
				}
			}

			return genericTypes;
		}

		private static IEnumerable<MethodParam> GetMethodParams(Object[] parameters)
		{
			var methodParams = new MethodParam[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				var paramType = parameters[i].GetType();
				var paramValue = parameters[i];

				var methodParam = new MethodParam
				{
					TypeName = paramType.FullName,
					ParamValue = paramValue
				};

				methodParams[i] = methodParam;
			}
			return methodParams;
		}
		private static IEnumerable<MethodParam> GetMethodParams(MethodBase methodBase, Object[] parameters)
		{
			var @params = methodBase.GetParameters();
			var methodParams = new MethodParam[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				var paramType = @params[i].ParameterType;
				var paramValue = parameters[i];

				var methodParam = new MethodParam
				{
					TypeName = paramType.FullName,
					ParamValue = paramValue
				};

				methodParams[i] = methodParam;
			}
			return methodParams;
		}

		private static byte[] GetRequestBytes(RequestEntity requestEntity)
		{
			using (var stream = new MemoryStream())
			{
				var writer = new BinaryWriter(stream);

                var peerBytes = Encoding.UTF8.GetBytes(requestEntity.Peer);
				var requestBytes = SerializationUtil.Serialize(requestEntity);

				writer.Write(peerBytes.Length);
				writer.Write(peerBytes);

				switch (ConfigUtil.ConfigSection.Client.Compression.ToLower())
				{
					case "def":
						{
							writer.Write((byte)1);
							requestBytes = CompressionUtil.CompressDef(requestBytes);
						}
						break;
					case "lz":
						{
							writer.Write((byte)2);
							requestBytes = CompressionUtil.CompressLZ(requestBytes, false);
						}
						break;
					default:
						writer.Write((byte)0);
						break;
				}

				writer.Write(requestBytes.Length);
				writer.Write(requestBytes);

				writer.Flush();

				return stream.ToArray();
			}
		}

		private static ResponseEntity GetResponseEntity(byte[] bytes)
		{
			using (var stream = new MemoryStream(bytes))
			{
				var reader = new BinaryReader(stream);
				var compression = reader.ReadByte();

				var dataLen = reader.ReadInt32();
				var dataBytes = reader.ReadBytes(dataLen);

				switch (compression)
				{
					case 1:
						dataBytes = CompressionUtil.DecompressDef(dataBytes);
						break;
					case 2:
						dataBytes = CompressionUtil.DecompressLZ(dataBytes, false);
						break;
				}

				var responseEntity = SerializationUtil.Deserialize<ResponseEntity>(dataBytes);
				return responseEntity;
			}
		}

		private static MethodBase GetStackMethodBase()
		{
			var stackTrace = new StackTrace();

			var stackFrames = stackTrace.GetFrames();
			if (stackFrames == null)
			{
				return null;
			}

			for (int i = 0; i < stackFrames.Length; i++)
			{
				var stackFrame = stackFrames[i];
				var methodBase = stackFrame.GetMethod();

				if (Attribute.IsDefined(methodBase, typeof(RpcRemoteMethodAttribute)))
				{
					return methodBase;
				}
			}

			return null;
		}
	}
}
