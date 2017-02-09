using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Converters.EntityToModel;
using CITI.EVO.UserManagement.Web.Converters.ModelToEntity;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Pages.Management
{
    public partial class ResourceList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyPermissions();

            FillResources();
        }

        protected void btSearch_OnClick(object sender, EventArgs e)
        {

        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var filterModel = resourcesFilterControl.Model;
            var projectID = filterModel.ProjectID;

            var newModel = new ResourceModel
            {
                ProjectID = projectID
            };

            resourceControl.Model = newModel;

            mpeResource.Show();
        }

        protected void resourcesControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<UM_Resource>().FirstOrDefault(n => n.ID == e.Value);
            if (entity == null)
                return;

            var converter = new ResourceEntityModelConverter(HbSession);
            var model = converter.Convert(entity);

            resourceControl.Model = model;

            mpeResource.Show();
        }

        protected void resourcesControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<UM_Resource>().FirstOrDefault(n => n.ID == e.Value);
            if (entity == null)
                return;

            entity.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(entity);

            FillResources();
        }

        protected void resourcesControl_OnNew(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<UM_Resource>().FirstOrDefault(n => n.ID == e.Value);
            if (entity == null)
                return;

            var newModel = new ResourceModel
            {
                ParentID = entity.ID,
                ProjectID = entity.ProjectID,
            };

            resourceControl.Model = newModel;

            mpeResource.Show();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            var model = resourceControl.Model;

            if (String.IsNullOrWhiteSpace(model.Name))
            {
                lblErrorMessage.Text = "შეიყვანეთ სახელი";

                mpeResource.Show();
                return;
            }

            var converter = new ResourceModelEntityConverter(HbSession);

            var entity = HbSession.Query<UM_Resource>().FirstOrDefault(n => n.ID == model.ID);
            if (entity == null)
            {
                entity = new UM_Resource
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now
                };
            }

            converter.FillObject(entity, model);

            HbSession.SubmitChanges(entity);

            FillResources();
        }

        #region methods

        protected void FillResources()
        {
            var filterModel = resourcesFilterControl.Model;

            var query = from n in HbSession.Query<UM_Resource>()
                        where n.DateDeleted == null
                        select n;

            if (filterModel.ProjectID == null)
            {
                query = from n in query
                        where n.ProjectID == null
                        select n;
            }
            else
            {
                query = from n in query
                        where n.ProjectID == filterModel.ProjectID
                        select n;
            }

            var resources = query.ToList();

            var keyword = (filterModel.Keyword ?? String.Empty).Trim();
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                var list = (from n in resources
                            where n.Name.Contains(keyword) ||
                                  n.Value.Contains(keyword)
                            select n).ToList();


                var @set = FullHierarchyTraversal(list, resources).ToHashSet();
                resources = @set.ToList();
            }

            var converter = new ResourceEntityModelConverter(HbSession);

            var model = new ResourcesModel
            {
                List = resources.Select(n => converter.Convert(n)).ToList()
            };

            resourcesControl.Model = model;
            resourcesControl.DataBind();
        }

        protected void ApplyPermissions()
        {
            if (!UmUtil.Instance.HasAccess("ResourceList"))
            {
                Response.Redirect("~/Pages/Management/UsersList.aspx");
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