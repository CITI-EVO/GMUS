using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Utils;

using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Enums;
using CITI.EVO.UserManagement.Web.Models;
using CITI.EVO.UserManagement.Web.Utils;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class SearchGroupsControl : BaseUserControlExtend<SearchGroupsModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillGroupsTree();
        }

        protected override void OnGetModel(object model)
        {
            var searchGroupsModel = model as SearchGroupsModel;
            if (searchGroupsModel == null)
                return;

            var @set = new HashSet<Guid?>();
            var nodes = tlGroups.GetSelectedNodes();

            foreach (var treeListNode in nodes)
            {
                var groupID = DataConverter.ToNullableGuid(treeListNode.Key);
                if (groupID != null)
                    @set.Add(groupID);
            }

            searchGroupsModel.GroupsID = @set;
        }

        protected void FillGroupsTree()
        {
            var allGroups = UmGroupsUtil.GetAllProjectsGroups(HbSession);

            tlGroups.DataSource = allGroups;
            tlGroups.DataBind();
        }
    }
}