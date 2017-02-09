//using System;
//using System.Text.RegularExpressions;
//using CITI.EVO.Tools.Utils;
//using CITI.EVO.UserManagement.Svc.Contracts;

//namespace CITI.EVO.UserManagement.Web.Utils
//{
//    /// <summary>
//    /// Summary description for NodeKeyObjectUtil
//    /// </summary>
//    [Serializable]
//    public class NodeKeyObject : IComparable<NodeKeyObject>, IEquatable<NodeKeyObject>
//    {
//        protected const String ProjectType = "project";
//        protected const String GroupType = "group";
//        protected const String UserType = "user";
//        protected const String ChildType = "child";

//        public NodeKeyObject(String type, Guid projectID, Guid groupID, Guid userID, Guid parentID)
//        {
//            Type = type;
//            ProjectID = projectID;
//            GroupID = groupID;
//            UserID = userID;
//            ParentID = parentID;
//        }

//        public NodeKeyObject(String type, ProjectContract project, GroupContract group, UserContract user, Guid parentID)
//        {
//            Type = type;

//            if (project != null)
//            {
//                ProjectID = project.ID;
//                Project = project;
//            }

//            if (group != null)
//            {
//                GroupID = group.ID;
//                Group = group;
//            }

//            if (user != null)
//            {
//                UserID = user.ID;
//                User = user;
//            }

//            ParentID = parentID;

//        }

//        public String Type { get; private set; }

//        public Guid ProjectID { get; private set; }
//        public ProjectContract Project { get; private set; }

//        public Guid GroupID { get; private set; }
//        public GroupContract Group { get; private set; }

//        public Guid UserID { get; private set; }
//        public UserContract User { get; private set; }

//        public Guid ParentID { get; private set; }

//        public bool IsProjectType()
//        {
//            return IsTypeOf(ProjectType);
//        }
//        public bool IsGroupType()
//        {
//            return IsTypeOf(GroupType);
//        }
//        public bool IsUserType()
//        {
//            return IsTypeOf(UserType);
//        }

//        public bool IsChildType()
//        {
//            return IsTypeOf(ChildType);
//        }

//        public bool IsTypeOf(String type)
//        {
//            return Type == type;
//        }

//        public static NodeKeyObject Parse(Object nodeObjectKey)
//        {
//            var nodeKey = Convert.ToString(nodeObjectKey);

//            var rx = new Regex(@"(?<Type>\S+)[\\](?<ProjectID>\S+)[\\](?<GroupID>\S+)[\\](?<UserID>\S+)[\\](?<ParentID>\S+)");
//            if (!rx.IsMatch(nodeKey))
//            {
//                return null;
//            }

//            var match = rx.Match(nodeKey);

//            var type = match.Groups["Type"].Value;

//            var projectID = DataConverter.ToNullableGuid(match.Groups["ProjectID"].Value);
//            var groupID = DataConverter.ToNullableGuid(match.Groups["GroupID"].Value);
//            var userID = DataConverter.ToNullableGuid(match.Groups["UserID"].Value);
//            var parentID = DataConverter.ToNullableGuid(match.Groups["ParentID"].Value);

//            if (projectID == null || groupID == null || userID == null || parentID == null)
//            {
//                return null;
//            }

//            var nodeKeyObject = new NodeKeyObject(type, projectID.Value, groupID.Value, userID.Value, parentID.Value);
//            return nodeKeyObject;
//        }

//        public static NodeKeyObject CreateForProject(Guid projectID)
//        {
//            return new NodeKeyObject(ProjectType, projectID, Guid.Empty, Guid.Empty, Guid.Empty);
//        }
//        public static NodeKeyObject CreateForProject(ProjectContract project)
//        {
//            return new NodeKeyObject(ProjectType, project, null, null, Guid.Empty);
//        }

//        public static NodeKeyObject CreateForGroup(Guid groupID)
//        {
//            return new NodeKeyObject(GroupType, Guid.Empty, groupID, Guid.Empty, Guid.Empty);
//        }
//        public static NodeKeyObject CreateForGroup(Guid projectID, Guid groupID, Guid parentID)
//        {
//            return new NodeKeyObject(ChildType, projectID, groupID, Guid.Empty, parentID);
//        }

//        public static NodeKeyObject CreateForGroup(ProjectContract project, GroupContract group)
//        {
//            return new NodeKeyObject(GroupType, project, group, null, Guid.Empty);
//        }
//        public static NodeKeyObject CreateForGroup(ProjectContract project, GroupContract group, Guid parentID)
//        {
//            return new NodeKeyObject(ChildType, project, group, null, parentID);
//        }

//        public static NodeKeyObject CreateForGroup(Guid projectID, Guid groupID)
//        {
//            return new NodeKeyObject(GroupType, projectID, groupID, Guid.Empty, Guid.Empty);
//        }

//        public static NodeKeyObject CreateForUser(Guid userID)
//        {
//            return new NodeKeyObject(UserType, Guid.Empty, Guid.Empty, userID, Guid.Empty);
//        }
//        public static NodeKeyObject CreateForUser(Guid groupID, Guid userID)
//        {
//            return new NodeKeyObject(UserType, Guid.Empty, groupID, userID, Guid.Empty);
//        }
//        public static NodeKeyObject CreateForUser(Guid projectID, Guid groupID, Guid userID)
//        {
//            return new NodeKeyObject(UserType, projectID, groupID, userID, Guid.Empty);
//        }

//        public static NodeKeyObject CreateForUser(ProjectContract project, GroupContract group, UserContract user)
//        {
//            return new NodeKeyObject(UserType, project, group, user, Guid.Empty);
//        }

//        public int CompareTo(NodeKeyObject other)
//        {
//            var thisKey = ToString();
//            var otherKey = other.ToString();

//            return StringComparer.OrdinalIgnoreCase.Compare(thisKey, otherKey);
//        }

//        public bool Equals(NodeKeyObject other)
//        {
//            return CompareTo(other) == 0;
//        }

//        public override String ToString()
//        {
//            return String.Format(@"{0}\{1}\{2}\{3}\{4}", Type, ProjectID, GroupID, UserID, ParentID);
//        }
//    }
//}