//using System.Collections.Generic;
//using System.Linq;
//
//using CITI.EVO.UserManagement.Web.Forms;

//namespace CITI.EVO.UserManagement.Web.Extensions
//{
//    public static class FormsExtensions
//    {
//        public static GroupForm ToGroupForm(this UM_Group group)
//        {
//            if (group == null)
//            {
//                return null;
//            }

//            var groupForm = new GroupForm();
//            groupForm.ID = group.ID;
//            groupForm.Name = group.Name;
//            groupForm.ProjectID = group.ProjectID;
//            groupForm.ProjectName = (group.Project != null ? group.Project.Name : null);
//            groupForm.DateCreated = group.DateCreated;
//            groupForm.DateChanged = group.DateChanged;
//            groupForm.DateDeleted = group.DateDeleted;

//            return groupForm;
//        }

//        public static IEnumerable<GroupForm> ToGroupForms(this IEnumerable<UM_Group> groups)
//        {
//            if (groups == null)
//            {
//                return null;
//            }

//            var groupsForms = (from n in groups
//                               let g = n.ToGroupForm()
//                               where g != null
//                               select g);

//            return groupsForms;
//        }
//    }
//}