using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using CITI.EVO.Tools.Utils;

using CITI.EVO.UserManagement.Svc.Contracts;
using CITI.EVO.UserManagement.Svc.Enums;
using CITI.EVO.UserManagement.Web.Common;
using CITI.EVO.UserManagement.Web.Enums;
using CITI.EVO.UserManagement.Web.Extensions;
using CITI.EVO.UserManagement.Web.Manages;
using log4net;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.UserManagement.DAL.Domain;
using DevExpress.Data.WcfLinq.Helpers;
using NHibernate;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Services.Managers
{
    public static class UserManagementManager
    {
        private static ILog logger;
        public static ILog Logger
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                logger = (logger ?? LogUtil.GetLogger("ServiceLoginLogger"));
                return logger;
            }
        }

        private static IAccessController accessController;
        public static IAccessController AccessController
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                accessController = (accessController ?? AccessControlFactory.GetAccessController());
                return accessController;
            }
        }

        public static Guid? Login(String loginName, String password, bool encryptedPassword)
        {
            if (String.IsNullOrWhiteSpace(loginName) || String.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = UsersManager.GetUser(loginName);
            if (user == null)
            {
                return null;
            }

            if (!encryptedPassword)
            {
                if (user.Password != password)
                    return null;
            }
            else
            {
                var userPasswordHash = CryptographyUtil.ComputeMD5(user.Password);
                if (userPasswordHash != password)
                    return null;
            }

            var token = AccessController.CreateUserToken(user.ID);

            if (Logger != null)
            {
                var clientIP = String.Empty;

                if (HttpContext.Current != null)
                {
                    var request = HttpContext.Current.Request;
                    clientIP = String.Format("{0}, {1}", request.UserHostAddress, request.UserHostName);
                }

                Logger.Info(String.Format("Login - LoginName: {0}, Password: {1}, ClientIP: {2}, Token: {3}", loginName, password, clientIP, token));
            }

            return token;
        }

        public static void Logout(Guid token)
        {
            if (Logger != null)
            {
                var clientIP = String.Empty;

                if (HttpContext.Current != null)
                {
                    var request = HttpContext.Current.Request;

                    clientIP = String.Format("{0}, {1}", request.UserHostAddress, request.UserHostName);
                }

                var loginName = String.Empty;
                var password = String.Empty;

                var user = GetCurrentUser(token);
                if (user != null)
                {
                    loginName = user.LoginName;
                    password = user.Password;
                }

                Logger.Info(String.Format("Logout - LoginName: {0}, Password: {1}, ClientIP: {2}, Token: {3}", loginName, password, clientIP, token));
            }

            AccessController.ReleaseUserToken(token);
        }

        public static bool IsTokenActual(Guid token)
        {
            return AccessController.ValidateToken(token);
        }

        public static UserContract GetCurrentUser(Guid token)
        {
            if (!AccessController.ValidateToken(token))
                return null;

            var userID = AccessController.GetTokenOwnerID(token);
            if (userID == null)
                return null;

            var user = UsersManager.GetUser(userID.Value);
            return user.ToContract();
        }

        public static PasswordChangeResultEnum ChangePassword(Guid token, String newPassword, String oldPassword)
        {
            if (!AccessController.ValidateToken(token))
                return PasswordChangeResultEnum.TokenNotFound;

            var userID = AccessController.GetTokenOwnerID(token);
            if (userID == null)
                return PasswordChangeResultEnum.UserNotFound;

            var user = UsersManager.GetUser(userID.Value);
            lock (user)
            {
                if (user.Password != oldPassword)
                    return PasswordChangeResultEnum.PasswordMismatch;

                if (user.Password == newPassword)
                    return PasswordChangeResultEnum.NewAndOldPasswordMatch;

                if (String.IsNullOrWhiteSpace(newPassword))
                    return PasswordChangeResultEnum.InvalidPattern;

                if (newPassword == user.LoginName)
                    return PasswordChangeResultEnum.InvalidPattern;

                var monthNumber = DataConverter.ToInt32(ConfigurationManager.AppSettings["MonthNumber"]);

                user.Password = newPassword;
                user.PasswordExpirationDate = DateTime.Now.AddMonths(monthNumber);

                user.DateChanged = DateTime.Now;

                UsersManager.UpdateUser(user);
            }

            return PasswordChangeResultEnum.Success;
        }

        public static List<PermissionContract> GetAllResourcesPermissions(Guid token, Guid? projectID)
        {
            var tokenUser = GetCurrentUser(token);
            if (tokenUser == null)
                return null;

            var session = Hb8Factory.InitSession();

            var query = from user in session.Query<UM_User>()
                        where user.ID == tokenUser.ID
                        from userGroup in user.GroupUsers
                        where userGroup.DateDeleted == null
                        let @group = userGroup.Group
                        where @group.DateDeleted == null
                        from perm in @group.Permissions
                        let res = perm.Resource
                        where perm.DateDeleted == null &&
                              res.DateDeleted == null
                        select new
                        {
                            res.ProjectID,
                            Permission = perm
                        };

            if (projectID != null)
            {
                query = from item in query
                        where item.ProjectID == projectID || item.ProjectID == null
                        select item;
            }

            var entities = query.ToList();
            var contracts = entities.Select(n => n.Permission.ToContract(n.ProjectID)).ToList();

            return contracts;
        }

        public static List<ProjectContract> GetProjects()
        {
            var session = Hb8Factory.InitSession();

            var items = session.Query<UM_Project>().Where(n => n.DateDeleted == null).ToList();
            return items.ToContracts();
        }

        public static List<GroupContract> GetProjectGroups(Guid token, Guid projectID)
        {
            if (!AccessController.ValidateToken(token))
                return null;

            var session = Hb8Factory.InitSession();

            var items = (from n in session.Query<UM_Group>()
                         where n.ProjectID == projectID &&
                               n.DateDeleted == null
                         select n).ToList();

            return items.ToContracts();
        }


        public static List<UserContract> GetAllUsers(Guid token, bool deleteds)
        {
            if (!AccessController.ValidateToken(token))
                return null;

            var currentUser = GetCurrentUser(token);
            if (currentUser == null)
                return null;

            var session = Hb8Factory.InitSession();

            var query = from n in session.Query<UM_User>()
                        select n;

            if (!deleteds)
            {
                query = from n in query
                        where n.DateDeleted == null
                        select n;
            }

            var items = query.ToList();

            if (!currentUser.IsSuperAdmin)
                items.ForEach(p => p.Password = null);

            return items.ToContracts();
        }

        public static List<UserContract> GetGroupUsers(Guid token, Guid groupID)
        {
            if (!AccessController.ValidateToken(token))
                return null;

            var session = Hb8Factory.InitSession();

            var items = (from gu in session.Query<UM_GroupUser>()
                         where gu.GroupID == groupID && gu.DateDeleted == null
                         let user = gu.User
                         where user != null &&
                               user.ID == gu.UserID &&
                               user.DateDeleted == null
                         select user).ToList();

            return items.ToContracts();
        }

        public static List<GroupContract> GetUserGroups(Guid token, Guid userID, Guid? projectID)
        {
            if (!AccessController.ValidateToken(token))
                return null;

            var session = Hb8Factory.InitSession();

            var items = (from gu in session.Query<UM_GroupUser>()
                         where gu.UserID == userID &&
                                gu.DateDeleted == null
                         let @group = gu.Group
                         where @group != null &&
                                @group.DateDeleted == null &&
                                @group.ProjectID == projectID
                         select @group).ToList();

            return items.ToContracts();
        }

        public static List<AttributeValueContract> GetAttributeValues(Guid token, Guid parentID)
        {
            if (!AccessController.ValidateToken(token))
                return null;

            var session = Hb8Factory.InitSession();

            var items = (from groupAttr in session.Query<UM_AttributeValue>()
                         where groupAttr.ParentID == parentID &&
                               groupAttr.DateDeleted == null
                         let field = groupAttr.AttributeField
                         where field != null &&
                               field.DateDeleted == null
                         let schema = field.AttributeSchema
                         where schema != null &&
                               schema.DateDeleted == null
                         select groupAttr).ToList();

            return items.ToContracts();
        }

        public static List<AttributeValueContract> GetAttributeValues(Guid token, Guid parentID, Guid projectID)
        {
            if (!AccessController.ValidateToken(token))
            {
                return null;
            }

            var session = Hb8Factory.InitSession();

            var globalItems = (from userAttr in session.Query<UM_AttributeValue>()
                               where userAttr.DateDeleted == null &&
                                     userAttr.ParentID == parentID
                               let field = userAttr.AttributeField
                               where field != null &&
                                     field.DateDeleted == null
                               let schema = field.AttributeSchema
                               where schema != null &&
                                     schema.DateDeleted == null
                               where schema.ProjectID == null
                               select userAttr).ToList();

            var projectItems = (from userAttr in session.Query<UM_AttributeValue>()
                                where userAttr.DateDeleted == null &&
                                      userAttr.ParentID == parentID
                                let field = userAttr.AttributeField
                                where field != null &&
                                      field.DateDeleted == null
                                let schema = field.AttributeSchema
                                where schema != null &&
                                      schema.DateDeleted == null
                                let project = schema.Project
                                where project != null &&
                                      project.DateDeleted == null &&
                                      project.ID == projectID
                                select userAttr).ToList();

            var allItems = globalItems.Union(projectItems);
            return allItems.ToContracts();
        }

        public static Dictionary<String, String> GetUserAttributesDictionary(Guid token, Guid parentID, Guid projectID)
        {
            if (!AccessController.ValidateToken(token))
                return null;

            var session = Hb8Factory.InitSession();

            var globalItems = (from userAttr in session.Query<UM_AttributeValue>()
                               where userAttr.DateDeleted == null &&
                                     userAttr.ParentID == parentID
                               let field = userAttr.AttributeField
                               where field != null &&
                                     field.DateDeleted == null
                               let schema = field.AttributeSchema
                               where schema != null &&
                                     schema.DateDeleted == null
                               where schema.ProjectID == null
                               select new
                               {
                                   field.Name,
                                   userAttr.Value
                               }).ToList();

            var projectItems = (from userAttr in session.Query<UM_AttributeValue>()
                                where userAttr.DateDeleted == null &&
                                      userAttr.ParentID == parentID
                                let field = userAttr.AttributeField
                                where field != null &&
                                      field.DateDeleted == null
                                let schema = field.AttributeSchema
                                where schema != null &&
                                      schema.DateDeleted == null
                                let project = schema.Project
                                where project != null &&
                                      project.DateDeleted == null &&
                                      project.ID == projectID
                                select new
                                {
                                    field.Name,
                                    userAttr.Value
                                }).ToList();

            var allItems = globalItems.Union(projectItems);

            var comparer = StringComparer.OrdinalIgnoreCase;

            var allItemsLp = allItems.ToLookup(n => n.Name, comparer);

            var dict = new Dictionary<String, String>(comparer);
            foreach (var itemsGrp in allItemsLp)
            {
                var valuesQuery = (from n in itemsGrp
                                   where !String.IsNullOrWhiteSpace(n.Value)
                                   select n.Value);

                var @set = valuesQuery.ToHashSet(comparer);

                var values = String.Join(",", @set);
                dict[itemsGrp.Key] = values;
            }

            return dict;
        }

        public static Dictionary<Guid, Dictionary<String, String>> GetAllUsersGlobalAttribetes(Guid token, bool deleteds)
        {
            if (!AccessController.ValidateToken(token))
                return null;

            var session = Hb8Factory.InitSession();

            var usersQuery = session.Query<UM_User>();

            if (!deleteds)
            {
                usersQuery = from n in usersQuery
                             where n.DateDeleted == null
                             select n;
            }

            var globalAttrQuery = from user in usersQuery
                                  join attr in session.Query<UM_AttributeValue>() on user.ID equals attr.ParentID
                                  where attr.DateDeleted == null
                                  let field = attr.AttributeField
                                  where field != null &&
                                        field.DateDeleted == null
                                  let schema = field.AttributeSchema
                                  where schema != null &&
                                        schema.DateDeleted == null
                                  where schema.ProjectID == null
                                  select new
                                  {
                                      attr.ParentID,
                                      field.Name,
                                      attr.Value
                                  };

            var globaAttrLp = globalAttrQuery.ToLookup(n => n.ParentID);

            var globalAttrDict = new Dictionary<Guid, Dictionary<String, String>>();
            foreach (var globalAttrGrp in globaAttrLp)
            {
                var dict = new Dictionary<String, String>();

                var attrLp = globalAttrGrp.ToLookup(n => n.Name);
                foreach (var attrGrp in attrLp)
                {
                    var valuesQuery = attrGrp.Select(n => n.Value).ToHashSet(StringComparer.OrdinalIgnoreCase);
                    var values = String.Join(";", valuesQuery);

                    dict.Add(attrGrp.Key, values);
                }

                globalAttrDict.Add(globalAttrGrp.Key, dict);
            }

            return globalAttrDict;
        }

        public static PermissionContract GetResourcePermission(Guid token, String resourcePath)
        {
            var session = Hb8Factory.InitSession();

            var comparer = StringComparer.OrdinalIgnoreCase;

            var pathArray = resourcePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (pathArray.Length == 0)
                return null;

            var projectID = DataConverter.ToNullableGuid(pathArray[0]);
            if (projectID == null)
                return null;

            var exists = session.Query<UM_Project>().Any(n => n.ID == projectID && n.IsActive);
            if (!exists)
                return null;

            var user = GetCurrentUser(token);
            if (user == null)
                return null;

            var requestGlobal = comparer.Equals(pathArray[1], "Global");

            var actualProject = (requestGlobal ? null : projectID);
            var index = (requestGlobal ? 2 : 1);

            if (pathArray.Length == index)
                return new PermissionContract();

            var groupsID = (from n in session.Query<UM_GroupUser>()
                            where n.UserID == user.ID &&
                                   n.DateDeleted == null
                            let m = n.Group
                            where m.DateDeleted == null &&
                                  m.ProjectID == projectID
                            select m.ID).ToList();

            if (groupsID.Count == 0)
                return null;

            var resQuery = from n in session.Query<UM_Resource>()
                           where n.DateDeleted == null
                           select n;

            if (requestGlobal)
            {
                resQuery = from n in resQuery
                           where n.ProjectID == null
                           select n;
            }
            else
            {
                resQuery = from n in resQuery
                           where n.ProjectID == projectID
                           select n;
            }

            using (var transaction = session.BeginTransaction())
            {
                UM_Resource parentRes = null;
                UM_Resource currentRes = null;

                for (int i = index; i < pathArray.Length; i++)
                {
                    var resourceName = pathArray[i];

                    var finalQuery = from n in resQuery
                                     where n.Value == resourceName
                                     select n;

                    if (parentRes == null)
                    {
                        finalQuery = from n in finalQuery
                                     where n.ParentID == null
                                     select n;
                    }
                    else
                    {
                        finalQuery = from n in finalQuery
                                     where n.ParentID == parentRes.ID
                                     select n;
                    }

                    currentRes = finalQuery.FirstOrDefault();

                    if (currentRes == null)
                    {
                        currentRes = new UM_Resource
                        {
                            ID = Guid.NewGuid(),
                            DateCreated = DateTime.Now,
                            Name = resourceName,
                            ProjectID = actualProject,
                            Description = "Automatic Record",
                            Value = resourceName,
                            Type = 0
                        };

                        if (parentRes == null)
                            session.Save(currentRes);
                        else
                            parentRes.Children.Add(currentRes);
                    }

                    parentRes = currentRes;
                }

                transaction.Commit();

                return GetPermissionContract(currentRes, groupsID, projectID.Value);
            }
        }

        private static PermissionContract GetPermissionContract(UM_Resource resource, IList<Guid> groupsID, Guid projectID)
        {
            var result = new PermissionContract
            {
                ProjectID = projectID,
                ResourceID = resource.ID,
                ResourcePath = resource.FullPath()
            };

            foreach (var groupID in groupsID)
            {
                var groupPerm = GetPermissionContract(resource, groupID, projectID);
                result.RuleValue |= groupPerm.RuleValue;

                if (groupPerm.PermissionParameter != null)
                {
                    foreach (var pair in groupPerm.PermissionParameter)
                    {
                        if (result.PermissionParameter.ContainsKey(pair.Key))
                        {
                            var key = String.Format("{0}_{1}", pair.Key, groupID);
                            result.PermissionParameter[key] = pair.Value;
                        }
                        else
                        {
                            result.PermissionParameter[pair.Key] = pair.Value;
                        }
                    }
                }
            }

            return result;
        }

        private static PermissionContract GetPermissionContract(UM_Resource resource, Guid groupID, Guid projectID)
        {
            var session = Hb8Factory.InitSession();

            var permission = (from n in session.Query<UM_Permission>()
                              where n.DateDeleted == null &&
                                    n.GroupID == groupID &&
                                    n.ResourceID == resource.ID
                              orderby n.RuleValue descending
                              select n).FirstOrDefault();

            if (permission != null)
            {
                return permission.ToContract(projectID);
            }

            if (resource.Parent == null)
            {
                var newPemission = new UM_Permission
                {
                    ID = Guid.NewGuid(),
                    GroupID = groupID,
                    ResourceID = resource.ID,
                    RuleValue = (int)RulePermissionsEnum.View,
                    DateCreated = DateTime.Now
                };

                session.SubmitInsert(newPemission);

                return newPemission.ToContract(projectID);
            }

            return GetPermissionContract(resource.Parent, groupID, projectID);
        }

        public static List<MessageContract> GetMessages(Guid token, Guid? projectID)
        {
            var user = GetCurrentUser(token);
            var userGroups = GetUserGroups(token, user.ID, projectID);

            var objectsSet = userGroups.Select(p => (Guid?)p.ID).ToHashSet();

            objectsSet.Add(projectID);
            objectsSet.Add(user.ID);

            var session = Hb8Factory.InitSession();

            var commonMessagesQuery = from n in session.Query<UM_Message>()
                                      where n.DateDeleted == null &&
                                            n.Type == (int)MessageTypeEnum.All &&
                                            Enumerable.Contains(objectsSet, n.ObjectID)
                                      select n;

            var commonMessages = commonMessagesQuery.ToList();

            var userMessagesQuery = from n in session.Query<UM_Message>()
                                    from m in n.MessageViewers
                                    where n.DateDeleted == null &&
                                          n.Type == (int)MessageTypeEnum.Standard &&
                                          Enumerable.Contains(objectsSet, n.ObjectID)
                                    select n;

            var userMessages = userMessagesQuery.ToList();

            var messagesViewersQuery = from n in userMessagesQuery
                                       from m in n.MessageViewers
                                       where m.DateDeleted == null &&
                                             m.UserID == user.ID
                                       select m;

            var messageViewersLp = messagesViewersQuery.ToLookup(n => n.MessageID);

            var unreadMessages = (from n in userMessages
                                  where !messageViewersLp[n.ID].Any()
                                  select n).ToList();

            var allMessages = commonMessages.Union(unreadMessages).ToList();

            using (var transaction = session.BeginTransaction())
            {
                foreach (var message in allMessages)
                {
                    if (message.Type == (int)MessageTypeEnum.All)
                    {
                        var count = messageViewersLp[message.ID].Count();
                        if (count == 0)
                        {
                            var messageViewer = new UM_MessageViewer
                            {
                                ID = Guid.NewGuid(),
                                UserID = user.ID,
                                MessageID = message.ID
                            };

                            session.Save(messageViewer);
                        }
                    }
                    else
                    {
                        var messageViewer = new UM_MessageViewer
                        {
                            ID = Guid.NewGuid(),
                            UserID = user.ID,
                            MessageID = message.ID
                        };

                        session.Save(messageViewer);
                    }
                }

                transaction.Commit();
            }

            return allMessages.ToContracts();
        }

        public static List<ProjectContract> GetProjectByUserToken(Guid token)
        {
            if (!AccessController.ValidateToken(token))
                return null;

            var currentUser = GetCurrentUser(token);
            if (currentUser == null)
                return null;

            var session = Hb8Factory.InitSession();

            var projects = (from gu in session.Query<UM_GroupUser>()
                            where gu.UserID == currentUser.ID &&
                                  gu.DateDeleted == null
                            let @group = gu.Group
                            where @group != null && @group.DateDeleted == null
                            let project = @group.Project
                            where project != null && project.DateDeleted == null
                            select project).Distinct().ToList();

            return projects.ToContracts();
        }

        public static bool HasMessages(Guid token, Guid? projectID)
        {
            var user = GetCurrentUser(token);
            if (user == null)
                throw new Exception("Unable to get user by token");

            var userGroups = GetUserGroups(token, user.ID, projectID);
            if (userGroups == null)
                return false;

            var objectsSet = userGroups.Select(p => (Guid?)p.ID).ToHashSet();

            objectsSet.Add(projectID);
            objectsSet.Add(user.ID);

            var session = Hb8Factory.InitSession();

            var commonMessagesQuery = from n in session.Query<UM_Message>()
                                      where n.DateDeleted == null &&
                                            n.Type == (int)MessageTypeEnum.All &&
                                            Enumerable.Contains(objectsSet, n.ObjectID)
                                      select n;

            var commonMessages = commonMessagesQuery.ToList();

            var userMessagesQuery = from n in session.Query<UM_Message>()
                                    where n.DateDeleted == null &&
                                          n.Type == (int)MessageTypeEnum.Standard &&
                                          Enumerable.Contains(objectsSet, n.ObjectID)
                                    select n;

            var userMessages = userMessagesQuery.ToList();

            var messagesViewersQuery = from n in userMessagesQuery
                                       from m in n.MessageViewers
                                       where m.DateDeleted == null &&
                                             m.UserID == user.ID
                                       select m;

            var messageViewersLp = messagesViewersQuery.ToLookup(n => n.MessageID);

            var unreadMessages = (from n in userMessages
                                  where !messageViewersLp[n.ID].Any()
                                  select n).ToList();

            var totalCount = commonMessages.Count + unreadMessages.Count;
            return (totalCount > 0);
        }
    }
}