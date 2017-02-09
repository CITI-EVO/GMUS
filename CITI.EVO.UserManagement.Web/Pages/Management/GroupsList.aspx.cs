using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Controls;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Svc.Contracts;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Converters.EntityToModel;
using CITI.EVO.UserManagement.Web.Converters.ModelToEntity;
using CITI.EVO.UserManagement.Web.Enums;
using CITI.EVO.UserManagement.Web.Extensions;
using CITI.EVO.UserManagement.Web.Models;
using CITI.EVO.UserManagement.Web.Models.Helpers;
using CITI.EVO.UserManagement.Web.Utils;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using NHibernate.Linq;
using Button = System.Web.UI.WebControls.Button;
using DropDownList = System.Web.UI.WebControls.DropDownList;

namespace CITI.EVO.UserManagement.Web.Pages.Management
{
    public partial class GroupsList : BasePage
    {
        #region Properties

        public List<UM_GroupUser> CurrentAdminGroupUsers
        {
            get
            {
                return LoginUtil.CurrentUser.GroupUsers.Where(u => u.AccessLevel > 0).ToList();
            }

        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyPermissions();
            FillGroupsTree();
        }


        protected void btGroupOK_Click(object sender, EventArgs e)
        {
            var model = groupControl.Model;

            if (String.IsNullOrWhiteSpace(model.Name))
            {
                lblGroupError.Text = "შეიყვანეთ სახელი";

                mpeGroup.Show();
                return;
            }

            var parent = HbSession.Query<UM_Group>().FirstOrDefault(n => n.ID == model.ParentID);
            var project = HbSession.Query<UM_Project>().FirstOrDefault(n => n.ID == model.ProjectID);

            if (project == null && parent != null)
            {
                var grandParent = parent;

                while (grandParent.Parent != null)
                    grandParent = grandParent.Parent;

                model.ProjectID = grandParent.ProjectID;
            }

            var group = HbSession.Query<UM_Group>().FirstOrDefault(n => n.ID == model.ID);
            if (group == null)
            {
                group = new UM_Group
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now
                };
            }

            var converter = new GroupModelEntityConverter(HbSession);
            converter.FillObject(group, model);

            HbSession.SubmitChanges(group);

            FillGroupsTree();
        }
        protected void btUserOK_Click(object sender, EventArgs e)
        {
            var model = selectUserControl.Model;

            var userID = model.User.UserID;
            var groupID = model.ParentID;
            var accessLevel = DataConverter.ToNullableEnum<AccessLevelEnum>(model.AccessLevel);

            if (userID == null || groupID == null || accessLevel == null)
                return;

            var group = HbSession.Query<UM_Group>().FirstOrDefault(n => n.ID == groupID);
            if (group == null)
                return;

            var user = HbSession.Query<UM_User>().FirstOrDefault(n => n.ID == userID);
            if (user == null)
                return;

            var exists = (from n in HbSession.Query<UM_GroupUser>()
                          where n.DateDeleted == null &&
                                n.GroupID == groupID &&
                                n.UserID == userID
                          select n).Any();
            if (exists)
                return;

            var groupUser = new UM_GroupUser
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                GroupID = groupID.Value,
                UserID = userID.Value,
                AccessLevel = (int)accessLevel
            };

            HbSession.SubmitChanges(groupUser);

            FillGroupsTree();
        }
        protected void btAttributeOK_Click(object sender, EventArgs e)
        {
            var model = objectAttributeControl.Model;

            var groupID = model.ParentID;
            var nodeID = model.FieldID;

            var group = HbSession.Query<UM_Group>().FirstOrDefault(n => n.ID == groupID);
            if (group == null)
                return;

            var node = HbSession.Query<UM_AttributeField>().FirstOrDefault(n => n.ID == nodeID);
            if (node == null)
                return;

            var attributeValue = (from v in HbSession.Query<UM_AttributeValue>()
                                  where v.ParentID == @group.ID &&
                                        v.AttributeFieldID == node.ID
                                  select v).FirstOrDefault();

            if (attributeValue == null)
            {
                attributeValue = new UM_AttributeValue
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now
                };
            }

            var converter = new AttributeValueModelEntityConverter(HbSession);
            converter.FillObject(attributeValue, model);

            HbSession.SubmitChanges(attributeValue);

            upnlGroupAttributes.Update();
            mpeGroupAttributes.Show();
        }
        protected void btViewAttributeOK_Click(object sender, EventArgs e)
        {
            mpeViewGroupAttributes.Hide();
        }
        protected void btAttributesCancel_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void lnkMessage_Click(object sender, EventArgs e)
        {
            //var lnkBtn = sender as Button;
            //if (lnkBtn == null)
            //{
            //    return;
            //}

            //var nodeKeyObject = NodeKeyObject.Parse(lnkBtn.CommandArgument);
            //if (nodeKeyObject == null)
            //{
            //    return;
            //}

            //ucMessage.ObjectId = nodeKeyObject.GroupID;
            //ucMessage.Show();
            //ucMessage.Update();
        }

        protected void groupsControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var group = HbSession.Query<UM_Group>().FirstOrDefault(n => n.ID == e.Value);
            if (group != null)
            {
                using (var transaction = HbSession.BeginTransaction())
                {
                    group.DateDeleted = DateTime.Now;
                    HbSession.Update(group);

                    foreach (var item in group.GroupUsers)
                    {
                        if (item.DateDeleted == null)
                        {
                            item.DateDeleted = group.DateDeleted;
                            HbSession.Update(item);
                        }
                    }

                    transaction.Commit();
                }

                return;
            }

            var groupUser = HbSession.Query<UM_GroupUser>().FirstOrDefault(n => n.ID == e.Value);
            if (groupUser != null)
            {
                groupUser.DateDeleted = DateTime.Now;
                HbSession.Update(groupUser);
            }

            FillGroupsTree();
        }

        protected void groupsControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var group = HbSession.Query<UM_Group>().FirstOrDefault(n => n.ID == e.Value);
            if (group == null)
                return;

            var converter = new GroupEntityModelConverter(HbSession);
            var model = converter.Convert(group);

            groupControl.Model = model;
            mpeGroup.Show();
        }

        protected void groupsControl_OnNew(object sender, GenericEventArgs<Guid> e)
        {
            var allGroups = UmGroupsUtil.GetAllProjectsGroupsUsers(HbSession);

            var item = allGroups.FirstOrDefault(n => n.ID == e.Value);
            if (item == null)
                return;

            var type = Convert.ToString(item.Tag);
            if (type == "Project")
            {
                var model = new GroupModel
                {
                    ProjectID = item.ID
                };

                groupControl.Model = model;
                mpeGroup.Show();
            }
            else if (type == "Group")
            {
                var model = new GroupModel
                {
                    ParentID = item.ID
                };

                groupControl.Model = model;
                mpeGroup.Show();
            }
        }

        protected void groupsControl_OnAddUser(object sender, GenericEventArgs<Guid> e)
        {
            var allGroups = UmGroupsUtil.GetAllProjectsGroupsUsers(HbSession);

            var item = allGroups.FirstOrDefault(n => n.ID == e.Value);
            if (item == null)
                return;

            var model = new SelectUserModel
            {
                ParentID = item.ID
            };

            selectUserControl.Model = model;
            mpeGroup.Show();
        }

        protected void groupsControl_OnSetAttribute(object sender, GenericEventArgs<Guid> e)
        {
            var group = HbSession.Query<UM_Group>().FirstOrDefault(n => n.ID == e.Value);
            if (group == null)
                return;

            var grandParent = group;

            while (grandParent.Parent != null)
                grandParent = grandParent.Parent;

            var model = new ObjectAttributeModel
            {
                ParentID = group.ID,
                ProjectID = grandParent.ProjectID
            };

            objectAttributeControl.Model = model;
            mpeGroupAttributes.Show();
        }

        protected void groupsControl_OnViewAttributes(object sender, GenericEventArgs<Guid> e)
        {
            var query = (from n in HbSession.Query<UM_Group>()
                         join m in HbSession.Query<UM_AttributeValue>() on n.ID equals m.ParentID
                         let node = m.AttributeField
                         let schema = node.AttributeSchema
                         where n.ID == e.Value
                         select new ObjectAttributeUnitModel
                         {
                             ID = m.ID,
                             Value = m.Value,
                             Node = node.Name,
                             Schema = schema.Name
                         });

            var model = new ObjectAttributeUnitsModel
            {
                List = query.ToList()
            };

            objectAttributesControl.Model = model;
            objectAttributesControl.DataBind();

            mpeViewGroupAttributes.Show();
        }

        protected void selectUserControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeUsers.Show();
        }

        #endregion

        #region Methods


        protected void FillGroupsTree()
        {
            var model = new GroupUnitsModel
            {
                List = UmGroupsUtil.GetAllProjectsGroupsUsers(HbSession).ToList()
            };

            groupsControl.Model = model;
            groupsControl.DataBind();
        }

        protected void ApplyPermissions()
        {
            if (!UmUtil.Instance.HasAccess("GroupList"))
                Response.Redirect("~/Pages/Management/UsersList.aspx");
        }

        #endregion
    }
}