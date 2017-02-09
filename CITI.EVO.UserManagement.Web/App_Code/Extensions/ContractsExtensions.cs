using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Svc.Contracts;
using CITI.EVO.UserManagement.Svc.Enums;

namespace CITI.EVO.UserManagement.Web.Extensions
{
    public static class ContractsExtensions
    {
        #region ToContract
        public static AttributeValueContract ToContract(this UM_AttributeValue entity)
        {
            if (entity == null)
            {
                return null;
            }

            var contract = new AttributeValueContract();
            contract.DateChanged = entity.DateChanged;
            contract.DateCreated = entity.DateCreated;
            contract.DateDeleted = entity.DateDeleted;
            contract.ID = entity.ID;
            contract.AttributeFieldID = entity.AttributeFieldID;

            return contract;
        }

        public static GroupContract ToContract(this UM_Group entity)
        {
            if (entity == null)
            {
                return null;
            }

            var contract = new GroupContract();
            contract.DateChanged = entity.DateChanged;
            contract.DateCreated = entity.DateCreated;
            contract.DateDeleted = entity.DateDeleted;
            contract.ID = entity.ID;
            contract.ParentID = entity.ParentID;
            contract.Name = entity.Name;
            contract.ProjectID = entity.ProjectID;

            return contract;
        }

        public static ResourceContract ToContract(this UM_Resource entity)
        {
            if (entity == null)
            {
                return null;
            }

            var contract = new ResourceContract();
            contract.DateChanged = entity.DateChanged;
            contract.DateCreated = entity.DateCreated;
            contract.Description = entity.Description;
            contract.ID = entity.ID;
            contract.Name = entity.Name;
            contract.Type = (RuleTypesEnum)entity.Type;
            contract.ParentID = entity.ParentID;
            contract.Value = entity.Value;
            contract.ProjectID = entity.ProjectID;
            return contract;
        }

        public static PermissionContract ToContract(this UM_Permission entity, Guid? projectID)
        {
            if (entity == null)
            {
                return null;
            }

            var contract = new PermissionContract();
            contract.ProjectID = projectID;
            contract.ResourceID = entity.ResourceID;
            contract.ResourcePath = entity.Resource.FullPath();
            contract.RuleValue = (RulePermissionsEnum)entity.RuleValue;
            contract.PermissionParameter = entity.PermissionParameters.ToDictionary(k => k.Name, v => v.Value);

            return contract;
        }

        public static String FullPath(this UM_Resource resource)
        {
			var list = new List<String>();

            var parent = resource;
            while (parent != null)
            {
                list.Add(parent.Value);
                parent = parent.Parent;
            }

            list.Reverse();

            return String.Join("/", list);
        }

        //public static PermissionContract ToContract(this UM_Permission entity)
        //{
        //    if (entity == null)
        //    {
        //        return null;
        //    }

        //    var contract = new PermissionContract();
        //    contract.GroupID = entity.GroupID;
        //    contract.ID = entity.ID;
        //    contract.ResourceID = entity.ResourceID;
        //    contract.RuleValue = (RulePermissionsEnum)entity.RuleValue;
        //    contract.PermissionParameter = entity.PermissionParameters.ToDictionary(k => k.Name, v => v.Value);
        //    contract.DateCreated = entity.DateCreated;
        //    contract.DateChanged = entity.DateChanged;
        //    contract.DateDeleted = entity.DateDeleted;

        //    return contract;
        //}

        public static PermissionParameterContract ToContract(this UM_PermissionParameter entity)
        {
            if (entity == null)
            {
                return null;
            }

            var contract = new PermissionParameterContract();
            contract.ID = entity.ID;
            contract.PermissionID = entity.PermissionID;
            contract.Name = entity.Name;
            contract.Value = entity.Value;
            contract.DateCreated = contract.DateCreated;
            contract.DateChanged = entity.DateChanged;
            contract.DateDeleted = entity.DateDeleted;

            return contract;

        }

        public static GroupUserContract ToContract(this UM_GroupUser entity)
        {
            if (entity == null)
            {
                return null;
            }

            var contract = new GroupUserContract();
            contract.DateChanged = entity.DateChanged;
            contract.DateCreated = entity.DateCreated;
            contract.DateDeleted = entity.DateDeleted;
            contract.GroupID = entity.GroupID;
            contract.ID = entity.ID;
            contract.UserID = entity.UserID;

            return contract;
        }

        public static ProjectContract ToContract(this UM_Project entity)
        {
            if (entity == null)
            {
                return null;
            }

            var contract = new ProjectContract();
            contract.DateChanged = entity.DateChanged;
            contract.DateCreated = entity.DateCreated;
            contract.DateDeleted = entity.DateDeleted;
            contract.ID = entity.ID;
            contract.Name = entity.Name;
            contract.IsActive = entity.IsActive;

            return contract;
        }

        public static RuleContract ToContract(this UM_Rule entity)
        {
            if (entity == null)
            {
                return null;
            }


            var contract = new RuleContract();
            contract.DateChanged = entity.DateChanged;
            contract.DateCreated = entity.DateCreated;
            contract.DateDeleted = entity.DateDeleted;
            contract.ID = entity.ID;
            contract.ProjectID = entity.ProjectID;
            contract.Name = entity.Name;
            contract.AccessLevel = entity.AccessLevel;
            contract.CanAdd = entity.CanAdd;
            contract.CanDelete = entity.CanDelete;
            contract.CanEdit = entity.CanEdit;
            contract.CanView = entity.CanView;

            return contract;
        }

        public static UserContract ToContract(this UM_User entity)
        {
            if (entity == null)
            {
                return null;
            }

            var contract = new UserContract();

            contract.Address = entity.Address;
            contract.DateChanged = entity.DateChanged;
            contract.DateCreated = entity.DateCreated;
            contract.DateDeleted = entity.DateDeleted;
            contract.Email = entity.Email;
            contract.FirstName = entity.FirstName;
            contract.ID = entity.ID;
            contract.LastName = entity.LastName;
            contract.LoginName = entity.LoginName;
            contract.Password = entity.Password;
            contract.IsActive = entity.IsActive;
            contract.IsSuperAdmin = entity.IsSuperAdmin;
            contract.PasswordExpirationDate = entity.PasswordExpirationDate;
            contract.UserCategoryID = entity.UserCategoryID;

            return contract;
        }

        public static MessageContract ToContract(this UM_Message entity)
        {
            if (entity == null)
            {
                return null;
            }

            var contract = new MessageContract();

            contract.ID = entity.ID;
            contract.Text = entity.Text;
            contract.ObjectID = entity.ObjectID;
            contract.Type = entity.Type;
            contract.DateCreated = entity.DateCreated;
            contract.DateChanged = entity.DateChanged;
            contract.DateDeleted = entity.DateDeleted;

            return contract;
        }


        public static MessageViewerContract ToContract(this UM_MessageViewer entity)
        {
            if (entity == null)
            {
                return null;
            }

            var contract = new MessageViewerContract();

            contract.ID = entity.ID;
            contract.MessageID = entity.MessageID;
            contract.UserID = entity.UserID;
            contract.DateCreated = entity.DateCreated;
            contract.DateChanged = entity.DateChanged;
            contract.DateDeleted = entity.DateDeleted;

            return contract;
        }

        #endregion

        #region ToContracts

        public static List<AttributeValueContract> ToContracts(this IEnumerable<UM_AttributeValue> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
            return result;
        }

        public static List<GroupContract> ToContracts(this IEnumerable<UM_Group> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
            return result;
        }

        public static List<ResourceContract> ToContracts(this IEnumerable<UM_Resource> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
            return result;
        }


        public static List<PermissionContract> ToContracts(this IEnumerable<UM_Permission> list, Guid projectID)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract(projectID)).ToList();
            return result;
        }

        public static List<PermissionParameterContract> ToContracts(this IEnumerable<UM_PermissionParameter> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
            return result;
        }


        public static List<GroupUserContract> ToContracts(this IEnumerable<UM_GroupUser> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
            return result;
        }

        public static List<ProjectContract> ToContracts(this IEnumerable<UM_Project> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
            return result;
        }

        public static List<RuleContract> ToContracts(this IEnumerable<UM_Rule> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
            return result;
        }

        public static List<UserContract> ToContracts(this IEnumerable<UM_User> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
            return result;
        }

        public static List<MessageContract> ToContracts(this IEnumerable<UM_Message> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
            return result;
        }

        public static List<MessageViewerContract> ToContracts(this IEnumerable<UM_MessageViewer> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
            return result;
        }

        #endregion

        #region ToEntity

        public static UM_Group ToEntity(this GroupContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_Group();
            entity.DateChanged = contract.DateChanged;
            entity.DateCreated = contract.DateCreated;
            entity.DateDeleted = contract.DateDeleted;
            entity.ID = contract.ID;
            entity.ParentID = contract.ParentID;
            entity.Name = contract.Name;
            entity.ProjectID = contract.ProjectID;

            return entity;
        }

        public static UM_Permission ToEntity(this PermissionContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_Permission();
            entity.ResourceID = contract.ResourceID;
            entity.RuleValue = Convert.ToInt32(contract.RuleValue);
            return entity;
        }

        public static UM_PermissionParameter ToEntity(this PermissionParameterContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_PermissionParameter();
            entity.ID = contract.ID;
            entity.PermissionID = contract.PermissionID;
            entity.Name = contract.Name;
            entity.Value = contract.Value;
            entity.DateCreated = contract.DateCreated;
            entity.DateChanged = contract.DateChanged;
            entity.DateDeleted = contract.DateDeleted;

            return entity;
        }

        public static UM_Resource ToEntity(this ResourceContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_Resource();
            entity.DateChanged = contract.DateChanged;
            entity.DateCreated = contract.DateCreated;
            entity.DateDeleted = contract.DateDeleted;
            entity.Description = contract.Description;
            entity.ID = contract.ID;
            entity.Type = Convert.ToInt32(contract.Type);
            entity.Name = contract.Name;
            entity.ParentID = contract.ParentID;
            entity.Value = contract.Value;
            entity.ProjectID = contract.ProjectID;

            return entity;
        }

        public static UM_GroupUser ToEntity(this GroupUserContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_GroupUser();
            entity.DateChanged = contract.DateChanged;
            entity.DateCreated = contract.DateCreated;
            entity.DateDeleted = contract.DateDeleted;
            entity.GroupID = contract.GroupID;
            entity.ID = contract.ID;
            entity.UserID = contract.UserID;

            return entity;
        }

        public static UM_Project ToEntity(this ProjectContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_Project();
            entity.DateChanged = contract.DateChanged;
            entity.DateCreated = contract.DateCreated;
            entity.DateDeleted = contract.DateDeleted;
            entity.ID = contract.ID;
            entity.Name = contract.Name;

            return entity;
        }

        public static UM_Rule ToEntity(this RuleContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_Rule();
            entity.DateChanged = contract.DateChanged;
            entity.DateCreated = contract.DateCreated;
            entity.DateDeleted = contract.DateDeleted;
            entity.ID = contract.ID;
            entity.ProjectID = contract.ProjectID;
            entity.Name = contract.Name;
            entity.AccessLevel = contract.AccessLevel;
            entity.CanAdd = contract.CanAdd;
            entity.CanDelete = contract.CanDelete;
            entity.CanEdit = contract.CanEdit;
            entity.CanView = contract.CanView;


            return entity;
        }

        public static UM_AttributeValue ToEntity(this AttributeValueContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_AttributeValue();
            entity.DateChanged = contract.DateChanged;
            entity.DateCreated = contract.DateCreated;
            entity.DateDeleted = contract.DateDeleted;
            entity.ID = contract.ID;
            entity.ParentID = contract.ParentID;
            entity.AttributeFieldID = contract.AttributeFieldID;
            entity.Value = contract.Value;

            return entity;
        }

        public static UM_User ToEntity(this UserContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_User();
            entity.Address = contract.Address;
            entity.DateChanged = contract.DateChanged;
            entity.DateCreated = contract.DateCreated;
            entity.DateDeleted = contract.DateDeleted;
            entity.Email = contract.Email;
            entity.FirstName = contract.FirstName;
            entity.ID = contract.ID;
            entity.LastName = contract.LastName;
            entity.LoginName = contract.LoginName;
            entity.Password = contract.Password;
            entity.IsActive = contract.IsActive;
            entity.UserCategoryID = contract.UserCategoryID;

            return entity;
        }


        public static UM_Message ToEntity(this MessageContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_Message();
            entity.ID = contract.ID;
            entity.Text = contract.Text;
            entity.ObjectID = contract.ObjectID;
            entity.Type = contract.Type;
            entity.DateChanged = contract.DateChanged;
            entity.DateCreated = contract.DateCreated;
            entity.DateDeleted = contract.DateDeleted;

            return entity;
        }

        public static UM_MessageViewer ToEntity(this MessageViewerContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            var entity = new UM_MessageViewer();
            entity.ID = contract.ID;
            entity.MessageID = contract.MessageID;
            entity.UserID = contract.UserID;
            entity.DateChanged = contract.DateChanged;
            entity.DateCreated = contract.DateCreated;
            entity.DateDeleted = contract.DateDeleted;

            return entity;
        }

        #endregion

        #region ToEntities
        public static List<UM_AttributeValue> ToEntities(this List<AttributeValueContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }

        public static List<UM_Group> ToEntities(this List<GroupContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }

        public static List<UM_Resource> ToEntities(this List<ResourceContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }

        public static List<UM_Permission> ToEntities(this List<PermissionContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }

        public static List<UM_PermissionParameter> ToEntities(this List<PermissionParameterContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }

        public static List<UM_GroupUser> ToEntities(this List<GroupUserContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }

        public static List<UM_Project> ToEntities(this List<ProjectContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }

        public static List<UM_Rule> ToEntities(this List<RuleContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }

        public static List<UM_User> ToEntities(this List<UserContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }

        public static List<UM_Message> ToEntities(this List<MessageContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }

        public static List<UM_MessageViewer> ToEntities(this List<MessageViewerContract> list)
        {
            if (list == null)
            {
                return null;
            }

            var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
            return result;
        }
        #endregion
    }
}