using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Controls;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Svc.Enums;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Entities;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Pages.Management
{
    public partial class PermissionList : BasePage
    {
        #region Properties

        protected String PermissionInfo { get; private set; }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyPermissions();
            FillProjects();

            if (!IsPostBack)
                cmbProject_SelectedIndexChanged(cmbProject, EventArgs.Empty);

            FillGroups();
            FillResources();
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            var permissionKey = GetPermissionKey();

            if (permissionKey.GroupID == Guid.Empty ||
                permissionKey.ResourseID == Guid.Empty ||
                permissionKey.ProjectID == Guid.Empty)
            {
                return;
            }

            var permission = GetRelatedPermission(true);
            if (permission == null)
            {
                permission = new UM_Permission
                {
                    ID = Guid.NewGuid(),

                    DateCreated = DateTime.Now
                };
            }

            permission.GroupID = permissionKey.GroupID;
            permission.ResourceID = permissionKey.ResourseID;
            permission.RuleValue = GetDecValue();

            HbSession.SubmitChanges(permission);

            DisplayRelatedRule();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            var permissionKey = GetPermissionKey();
            if (permissionKey.GroupID == Guid.Empty ||
                permissionKey.ResourseID == Guid.Empty ||
                permissionKey.ProjectID == Guid.Empty)
            {
                return;
            }

            var permission = GetRelatedPermission(true);
            if (permission == null)
                return;

            permission.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(permission);

            DisplayRelatedRule();
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            var lnkBtn = sender as ImageLinkButton;
            if (lnkBtn == null || String.IsNullOrWhiteSpace(lnkBtn.CommandArgument))
                return;

            var entityID = DataConverter.ToNullableGuid(lnkBtn.CommandArgument);

            var permissionParameter = (from n in HbSession.Query<UM_PermissionParameter>()
                                       where n.ID == entityID &&
                                             n.DateDeleted == null
                                       select n).FirstOrDefault();

            if (permissionParameter == null)
                return;

            permissionParameter.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(permissionParameter);

            btPermissionParameter_Click(this, EventArgs.Empty);

        }

        protected void btProjectCancel_OnClick(object sender, EventArgs e)
        {
            mpePermissionParametersForm.Hide();
        }

        protected void tlGroups_FocusedNodeChanged(object sender, EventArgs e)
        {
            DisplayRelatedRule();
        }

        protected void tlResources_FocusedNodeChanged(object sender, EventArgs e)
        {
            DisplayRelatedRule();
        }

        protected void btPermissionParameter_Click(object sender, EventArgs e)
        {
            var permission = GetRelatedPermission(false);
            if (permission == null)
            {
                return;
            }

            var permissionParameter = (from n in HbSession.Query<UM_PermissionParameter>()
                                       where n.PermissionID == permission.ID &&
                                             n.DateDeleted == null
                                       select n).ToList();

            gvPermissionParameters.DataSource = permissionParameter;
            gvPermissionParameters.DataBind();
            hdPermissionID.Value = permission.ID.ToString();

            mpePermissionParametersForm.Show();
        }

        protected void btPermissionParameterOK_Click(object sender, EventArgs e)
        {
            var permissionID = DataConverter.ToNullableGuid(hdPermissionID.Value);

            var perParameterName = tbName.Text;
            var perParameterValue = tbValue.Text;

            var permissionParameter = new UM_PermissionParameter
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                PermissionID = permissionID.Value,
                Name = perParameterName,
                Value = perParameterValue
            };

            HbSession.SubmitChanges(permissionParameter);

            btPermissionParameter_Click(this, EventArgs.Empty);
        }

        protected void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayRelatedRule();
        }

        #endregion

        #region Methods

        protected int GetDecValue()
        {
            var sb = new StringBuilder();

            foreach (ListItem listItem in chklRules.Items)
            {
                var @char = (listItem.Selected ? '1' : '0');
                sb.Append(@char);
            }

            var retValue = Convert.ToInt32(sb.ToString(), 2);

            if (Enum.IsDefined(typeof(RulePermissionsEnum), retValue))
            {
                return retValue;
            }

            return retValue;
        }

        protected void FillGroups()
        {
            var projectID = DataConverter.ToNullableGuid(cmbProject.SelectedItem.Value);
            if (projectID == null)
                return;

            var query = from n in HbSession.Query<UM_Group>()
                        where n.DateDeleted == null &&
                              n.ProjectID == projectID
                        select n;

            var groups = from n in FullHierarchyTraversal(query)
                         orderby n.Name
                         select n;

            tlGroups.DataSource = groups.ToHashSet();
            tlGroups.DataBind();
        }

        protected void FillResources()
        {
            var projectID = DataConverter.ToNullableGuid(cmbProject.SelectedItem.Value);
            if (projectID == null)
                return;

            var resources = (from n in HbSession.Query<UM_Resource>()
                             where n.DateDeleted == null && n.ProjectID == projectID
                             select n).ToList();

            var keyword = (tbxKeyword.Text ?? String.Empty).Trim();
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                var list = (from n in resources
                            where n.Name.Contains(keyword) ||
                                  n.Value.Contains(keyword)
                            select n).ToList();


                var @set = FullHierarchyTraversal(list, resources).ToHashSet();
                resources = @set.ToList();
            }

            tlResources.DataSource = resources;
            tlResources.DataBind();
        }

        protected void FillProjects()
        {
            var projects = (from n in HbSession.Query<UM_Project>()
                            where n.DateDeleted == null
                            orderby n.Name
                            select n).ToList();

            cmbProject.BindData(projects);

            var selValue = cmbProject.TryGetGuidValue();
            if (selValue == null)
            {
                var first = projects.FirstOrDefault();
                if (first != null)
                    cmbProject.TrySetSelectedValue(first.ID);
            }
        }

        protected void ClearChklRules()
        {
            foreach (ListItem listItem in chklRules.Items)
            {
                listItem.Selected = false;
            }
        }

        protected void ApplyPermissions()
        {
            if (!UmUtil.Instance.HasAccess("PermissionList"))
                Response.Redirect("~/Pages/Management/UsersList.aspx");
        }

        protected void DisplayRelatedRule()
        {
            var permission = GetRelatedPermission(false);
            lbPermissionInfo.Text = PermissionInfo;

            if (permission != null)
            {
                int ruleValue = permission.RuleValue.GetValueOrDefault();
                if (Enum.IsDefined(typeof(RulePermissionsEnum), ruleValue))
                {
                    string stRuleValue = Convert.ToString(ruleValue, 2).PadLeft(4, '0');

                    for (int i = 0; i < chklRules.Items.Count; i++)
                    {
                        chklRules.Items[i].Selected = stRuleValue[i] == '1';
                    }

                }

                btPermissionParameter.Visible = true;

            }

            else
            {
                ClearChklRules();
                btPermissionParameter.Visible = false;

            }
        }

        protected bool CheckSelectedCheckboxes()
        {
            int count = 0;

            foreach (ListItem listItem in chklRules.Items)
            {
                if (listItem.Selected)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                return true;
            }

            return false;
        }

        protected PermissionKeyContainer GetPermissionKey()
        {
            var permissionKey = new PermissionKeyContainer();

            var projectID = cmbProject.TryGetGuidValue();
            permissionKey.ProjectID = projectID.Value;

            if (tlGroups.FocusedNode != null)
            {
                var groupID = DataConverter.ToNullableGuid(tlGroups.FocusedNode.Key);
                if (groupID != null)
                    permissionKey.GroupID = groupID.Value;
            }

            if (tlResources.FocusedNode != null)
            {
                var resourceID = DataConverter.ToNullableGuid(tlResources.FocusedNode.Key);
                if (resourceID != null)
                    permissionKey.ResourseID = resourceID.Value;
            }

            return permissionKey;
        }

        protected UM_Permission GetRelatedPermission(bool operation)
        {
            var permissionKey = GetPermissionKey();
            if (permissionKey.GroupID == Guid.Empty ||
                permissionKey.ResourseID == Guid.Empty)
            {
                return null;
            }

            var projectExists = HbSession.Query<UM_Project>().Count(n => n.ID == permissionKey.ProjectID) > 0;
            if (!projectExists)
            {
                return null;
            }

            var groupExists = HbSession.Query<UM_Group>().Count(n => n.ID == permissionKey.GroupID) > 0;
            if (!groupExists)
            {
                return null;
            }

            var resourceExists = HbSession.Query<UM_Resource>().Count(n => n.ID == permissionKey.ResourseID) > 0;
            if (!resourceExists)
            {
                return null;
            }

            var permission = (from n in HbSession.Query<UM_Permission>()
                              where n.GroupID == permissionKey.GroupID &&
                                    n.ResourceID == permissionKey.ResourseID &&
                                    n.DateDeleted == null
                              select n).FirstOrDefault();

            if (operation)
            {
                return permission;
            }


            if (permission == null)
            {
                return GetGroupResourcePermission(permissionKey.GroupID, permissionKey.ResourseID);
            }

            PermissionInfo = "უფლება გაწერილია მიმდინარე რესურსზე";
            return permission;


        }

        protected UM_Permission GetGroupResourcePermission(Guid groupID, Guid? resourceID)
        {
            var resource = (from n in HbSession.Query<UM_Resource>()
                            where n.ID == resourceID &&
                                  n.DateDeleted == null
                            select n).FirstOrDefault();

            var permission = (from n in HbSession.Query<UM_Permission>()
                              where n.GroupID == groupID &&
                                    n.ResourceID == resource.ParentID &&
                                    n.DateDeleted == null
                              select n).FirstOrDefault();

            if (permission != null)
            {
                PermissionInfo = "უფლება გაწერილია მშობელ რესურსზე";
                return permission;
            }

            if (resource.ParentID == null)
            {
                PermissionInfo = "უფლება არ არის გაწერილი";
                return null;
            }

            return GetGroupResourcePermission(groupID, resource.ParentID);
        }

        protected IEnumerable<UM_Group> FullHierarchyTraversal(IQueryable<UM_Group> query)
        {
            foreach (var umGroup in query)
                yield return umGroup;

            var childQuery = (from n in query
                              from m in n.Children
                              where m.DateDeleted == null
                              select m);

            if (childQuery.Count() > 0)
            {
                var children = FullHierarchyTraversal(childQuery);
                foreach (var umGroup in children)
                    yield return umGroup;
            }
        }

        protected IEnumerable<UM_Resource> FullHierarchyTraversal(IList<UM_Resource> resources, IList<UM_Resource> allResources)
        {
            var resourcesDict = allResources.ToDictionary(n => n.ID);
            var resourcesLp = allResources.ToLookup(n => n.ParentID.GetValueOrDefault());

            foreach (var item in resources)
            {
                yield return item;

                foreach (var parent in GetAllParents(item, resourcesDict))
                    yield return parent;

                foreach (var child in GetAllChildren(item, resourcesLp))
                    yield return child;
            }
        }

        protected IEnumerable<UM_Resource> GetAllParents(UM_Resource resource, IDictionary<Guid, UM_Resource> allResourceses)
        {
            while (resource != null)
            {
                resource = allResourceses.GetValueOrDefault(resource.ParentID.GetValueOrDefault());
                if (resource != null)
                    yield return resource;
            }
        }

        protected IEnumerable<UM_Resource> GetAllChildren(UM_Resource resource, ILookup<Guid, UM_Resource> allResourceses)
        {
            var children = allResourceses[resource.ID];

            var stack = new Stack<UM_Resource>(children);
            while (stack.Count > 0)
            {
                var item = stack.Pop();
                yield return item;
            }
        }

        #endregion

    }
}