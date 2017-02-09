using System;
using System.Collections.Generic;
using CITI.EVO.Rpc;
using CITI.EVO.Rpc.Attributes;
using CITI.EVO.UserManagement.Svc.Contracts;
using CITI.EVO.UserManagement.Svc.Enums;

namespace CITI.EVO.Proxies
{
	public static class UserManagementProxy
	{
		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetCurrentUser")]
		public static UserContract GetCurrentUser(Guid token)
		{
			return RpcInvoker.InvokeMethod<UserContract>(token);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.IsTokenActual")]
		public static bool IsTokenActual(Guid token)
		{
			return RpcInvoker.InvokeMethod<bool>(token);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetAllUsers")]
		public static List<UserContract> GetAllUsers(Guid token, bool deleteds)
		{
			return RpcInvoker.InvokeMethod<List<UserContract>>(token, deleteds);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.ChangePassword")]
		public static PasswordChangeResultEnum ChangePassword(Guid token, String newPassword, String oldPassword)
		{
			return RpcInvoker.InvokeMethod<PasswordChangeResultEnum>(token, newPassword, oldPassword);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.Login")]
		public static Guid? Login(String loginName, String password, bool encryptedPassword)
		{
			return RpcInvoker.InvokeMethod<Guid?>(loginName, password, encryptedPassword);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetProjects")]
		public static List<ProjectContract> GetProjects()
		{
			return RpcInvoker.InvokeMethod<List<ProjectContract>>();
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetProjectGroups")]
		public static List<GroupContract> GetProjectGroups(Guid token, Guid projectID)
		{
			return RpcInvoker.InvokeMethod<List<GroupContract>>(token, projectID);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetGroupUsers")]
		public static List<UserContract> GetGroupUsers(Guid token, Guid groupID)
		{
			return RpcInvoker.InvokeMethod<List<UserContract>>(token, groupID);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetUserGroups")]
		public static List<GroupContract> GetUserGroups(Guid token, Guid userID, Guid projectID)
		{
			return RpcInvoker.InvokeMethod<List<GroupContract>>(token, userID, projectID);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetAttributeValues")]
		public static List<AttributeValueContract> GetAttributeValues(Guid token, Guid parentID)
		{
			return RpcInvoker.InvokeMethod<List<AttributeValueContract>>(token, parentID);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetResourcePermission")]
		public static PermissionContract GetResourcePermission(Guid token, String resourcePath)
		{
			return RpcInvoker.InvokeMethod<PermissionContract>(token, resourcePath);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetAllResourcesPermissions")]
		public static List<PermissionContract> GetAllResourcesPermissions(Guid token, Guid? projectID)
		{
			return RpcInvoker.InvokeMethod<List<PermissionContract>>(token, projectID);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetUserAttributesDictionary")]
		public static Dictionary<String, String> GetUserAttributesDictionary(Guid token, Guid userID, Guid projectID)
		{
			return RpcInvoker.InvokeMethod<Dictionary<String, String>>(token, userID, projectID);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetAllUsersGlobalAttribetes")]
		public static Dictionary<Guid, Dictionary<String, String>> GetAllUsersGlobalAttribetes(Guid token, bool deleteds)
		{
			return RpcInvoker.InvokeMethod<Dictionary<Guid, Dictionary<String, String>>>(token, deleteds);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetProjectByUserToken")]
		public static List<ProjectContract> GetProjectByUserToken(Guid token)
		{
			return RpcInvoker.InvokeMethod<List<ProjectContract>>(token);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.GetMessages")]
		public static List<MessageContract> GetMessages(Guid token, Guid? projectID)
		{
			return RpcInvoker.InvokeMethod<List<MessageContract>>(token, projectID);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.HasMessages")]
		public static bool HasMessages(Guid token, Guid? projectID)
		{
			return RpcInvoker.InvokeMethod<bool>(token, projectID);
		}

		[RpcRemoteMethod("UserManagement.CITI.EVO.UserManagement.Web.Services.CommonServiceWrapper.Logout")]
		public static void Logout(Guid token)
		{
			RpcInvoker.InvokeMethod(token);
		}
	}

}
