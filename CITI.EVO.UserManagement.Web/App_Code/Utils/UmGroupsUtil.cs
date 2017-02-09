using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Entities;
using NHibernate;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Utils
{
    /// <summary>
    /// Summary description for UmGroupsUtil
    /// </summary>
    public static class UmGroupsUtil
    {
        public static IEnumerable<ParentChildEntity> GetAllProjectsGroups(ISession session)
        {
            var projectsQuery = (from n in session.Query<UM_Project>()
                                 where n.DateDeleted == null
                                 select n);

            var groupsQuery = (from n in projectsQuery
                               from m in n.Groups
                               where m.DateDeleted == null
                               select m);

            var projectsGroupsLp = groupsQuery.ToLookup(n => n.ProjectID);
            var byParentGroupsLp = groupsQuery.ToLookup(n => n.ParentID);

            foreach (var project in projectsQuery)
            {
                var projectEntity = new ParentChildEntity
                {
                    ID = project.ID,
                    Name = project.Name,
                    Tag = "Project"
                };

                yield return projectEntity;

                foreach (var @group in projectsGroupsLp[project.ID])
                {
                    var allGroups = CollectionUtil.Traversal(@group, byParentGroupsLp, n => n.ID);
                    foreach (var childGroup in allGroups)
                    {
                        var groupEntity = new ParentChildEntity
                        {
                            ID = childGroup.ID,
                            Name = childGroup.Name,
                            ParentID = (childGroup.ParentID ?? childGroup.ProjectID),
                            Tag = "Group"
                        };

                        yield return groupEntity;
                    }
                }
            }
        }

        public static IEnumerable<ParentChildEntity> GetAllProjectsGroupsUsers(ISession session)
        {
            var projectsQuery = (from n in session.Query<UM_Project>()
                                 where n.DateDeleted == null
                                 select n);

            var groupsQuery = (from n in projectsQuery
                               from m in n.Groups
                               where m.DateDeleted == null
                               select m);

            var usersQuery = (from n in groupsQuery
                              from m in n.GroupUsers
                              where m.DateDeleted == null
                              select new
                              {
                                  m.ID,
                                  m.GroupID,
                                  m.User
                              });

            var projectsGroupsLp = groupsQuery.ToLookup(n => n.ProjectID);
            var groupsByParentLp = groupsQuery.ToLookup(n => n.ParentID);
            var usersByGroupLp = usersQuery.ToLookup(n => n.GroupID);

            foreach (var project in projectsQuery)
            {
                var projectEntity = new ParentChildEntity
                {
                    ID = project.ID,
                    Name = project.Name,
                    Tag = "Project"
                };

                yield return projectEntity;

                foreach (var @group in projectsGroupsLp[project.ID])
                {
                    var allGroups = CollectionUtil.Traversal(@group, groupsByParentLp, n => n.ID);
                    foreach (var childGroup in allGroups)
                    {
                        var groupEntity = new ParentChildEntity
                        {
                            ID = childGroup.ID,
                            Name = childGroup.Name,
                            ParentID = (childGroup.ParentID ?? childGroup.ProjectID),
                            Tag = "Group"
                        };

                        yield return groupEntity;

                        foreach (var item in usersByGroupLp[childGroup.ID])
                        {
                            var user = item.User;

                            var userEntity = new ParentChildEntity
                            {
                                ID = item.ID,
                                Name = String.Format("{0} - {1} {2}", user.LoginName, user.FirstName, user.LastName),
                                ParentID = childGroup.ID,
                                Tag = "User"
                            };

                            yield return userEntity;
                        }
                    }
                }
            }
        }
    }
}