using System;
using System.Collections.Generic;
using CITI.EVO.Rpc.Attributes;
using CITI.EVO.UserManagement.Svc.Contracts;
using CITI.EVO.UserManagement.Svc.Enums;
using CITI.EVO.UserManagement.Web.Services.Managers;

namespace CITI.EVO.UserManagement.Web.Services
{
	public static class CommonServiceWrapper
	{
		[RpcAllowRemoteCall]
		public static UserContract GetCurrentUser(Guid token)
		{
			return UserManagementManager.GetCurrentUser(token);
		}

		[RpcAllowRemoteCall]
		public static bool IsTokenActual(Guid token)
		{
			return UserManagementManager.IsTokenActual(token);
		}

	    [RpcAllowRemoteCall]
	    public static UserContract GetUser(Guid token, Guid userID)
	    {
	        return UserManagementManager.GetUser(token, userID);
	    }

        [RpcAllowRemoteCall]
		public static List<UserContract> GetAllUsers(Guid token, bool deleteds)
		{
			return UserManagementManager.GetAllUsers(token, deleteds);
		}

		[RpcAllowRemoteCall]
		public static PasswordChangeResultEnum ChangePassword(Guid token, String newPassword, String oldPassword)
		{
			return UserManagementManager.ChangePassword(token, newPassword, oldPassword);
		}

		[RpcAllowRemoteCall]
		public static Guid? Login(String loginName, String password, bool encryptedPassword)
		{
			return UserManagementManager.Login(loginName, password, encryptedPassword);
		}

		[RpcAllowRemoteCall]
		public static List<ProjectContract> GetProjects()
		{
			return UserManagementManager.GetProjects();
		}

		[RpcAllowRemoteCall]
		public static List<GroupContract> GetProjectGroups(Guid token, Guid projectID)
		{
			return UserManagementManager.GetProjectGroups(token, projectID);
		}

		[RpcAllowRemoteCall]
		public static List<UserContract> GetGroupUsers(Guid token, Guid groupID)
		{
			return UserManagementManager.GetGroupUsers(token, groupID);
		}

		[RpcAllowRemoteCall]
		public static List<GroupContract> GetUserGroups(Guid token, Guid userID, Guid projectID)
		{
			return UserManagementManager.GetUserGroups(token, userID, projectID);
		}

		[RpcAllowRemoteCall]
		public static List<AttributeValueContract> GetAttributeValues(Guid token, Guid parentID)
		{
			return UserManagementManager.GetAttributeValues(token, parentID);
		}

		[RpcAllowRemoteCall]
		public static List<AttributeValueContract> GetAttributeValues(Guid token, Guid parentID, Guid projectID)
		{
			return UserManagementManager.GetAttributeValues(token, parentID, projectID);
		}

		[RpcAllowRemoteCall]
		public static PermissionContract GetResourcePermission(Guid token, String resourcePath)
		{
			return UserManagementManager.GetResourcePermission(token, resourcePath);
		}

		[RpcAllowRemoteCall]
		public static List<PermissionContract> GetAllResourcesPermissions(Guid token, Guid? projectID)
		{
			return UserManagementManager.GetAllResourcesPermissions(token, projectID);
		}

		[RpcAllowRemoteCall]
        public static Dictionary<String, String> GetUserAttributesDictionary(Guid token, Guid userID, Guid projectID)
		{
			return UserManagementManager.GetUserAttributesDictionary(token, userID, projectID);
		}

		[RpcAllowRemoteCall]
		public static Dictionary<Guid, Dictionary<String, String>> GetAllUsersGlobalAttribetes(Guid token, bool deleteds)
		{
			return UserManagementManager.GetAllUsersGlobalAttribetes(token, deleteds);
		}

		[RpcAllowRemoteCall]
		public static List<MessageContract> GetMessages(Guid token, Guid? projectID)
		{
			return UserManagementManager.GetMessages(token, projectID);
		}

		[RpcAllowRemoteCall]
		public static List<ProjectContract> GetProjectByUserToken(Guid token)
		{
			return UserManagementManager.GetProjectByUserToken(token);
		}

		[RpcAllowRemoteCall]
		public static bool HasMessages(Guid token, Guid? projectID)
		{
			return UserManagementManager.HasMessages(token, projectID);
		}

		[RpcAllowRemoteCall]
		public static void Logout(Guid token)
		{
			UserManagementManager.Logout(token);
		}
	}
}