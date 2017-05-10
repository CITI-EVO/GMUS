using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Entities.ServiseStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class ServiceControl : BaseUserControlExtend<ServiceModel>
    {
        public ServiceEntity ServiceEntity
        {
            get { return ViewState["Service"] as ServiceEntity; }
            set { ViewState["Service"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillElementsTree();
        }

        public override ServiceModel GetModel()
        {
            var model = base.GetModel();
            model.Entity = ServiceEntity;

            return model;
        }

        public override void SetModel(ServiceModel model)
        {
            ServiceEntity = model.Entity;

            base.SetModel(model);

            FillElementsTree();
        }

        protected void FillElementsTree()
        {
            tlData.DataSource = GetAllTreeNodes().ToList();
            tlData.DataBind();
        }

        protected IEnumerable<TreeNodeEntity> GetAllTreeNodes()
        {
            if (ServiceEntity == null)
                yield break;

            var serviceNode = new TreeNodeEntity
            {
                ID = ServiceEntity.ID,
                Name = ServiceEntity.Name,
                Tag = "Service"
            };

            yield return serviceNode;

            var classesNode = new TreeNodeEntity
            {
                ID = "Classes".ComputeMd5Guid(),
                Name = "Classes",
                ParentID = serviceNode.ID,
                Tag = "Class"
            };

            yield return classesNode;

            foreach (var classEntity in ServiceEntity.Classes)
            {
                var classNode = new TreeNodeEntity
                {
                    ID = classEntity.ID,
                    Name = classEntity.Name,
                    ParentID = classesNode.ID,
                    Tag = "Class"
                };

                yield return classNode;

                foreach (var propertyEntity in classEntity.Properties)
                {
                    var propertyNode = new TreeNodeEntity
                    {
                        ID = propertyEntity.ID,
                        Name = propertyEntity.Name,
                        ParentID = classNode.ID,
                        Tag = "Property"
                    };

                    yield return propertyNode;
                }
            }

            var methodsNode = new TreeNodeEntity
            {
                ID = "Methods".ComputeMd5Guid(),
                Name = "Methods",
                ParentID = serviceNode.ID,
                Tag = "Method"
            };

            yield return methodsNode;

            foreach (var methodEntity in ServiceEntity.Methods)
            {
                var methodNode = new TreeNodeEntity
                {
                    ID = methodEntity.ID,
                    Name = methodEntity.Name,
                    ParentID = methodsNode.ID,
                    Tag = "Method"
                };

                yield return methodNode;

                foreach (var parameterEntity in methodEntity.Parameters)
                {
                    var parameterNode = new TreeNodeEntity
                    {
                        ID = parameterEntity.ID,
                        Name = parameterEntity.Name,
                        ParentID = methodNode.ID,
                        Tag = "Parameter"
                    };

                    yield return parameterNode;
                }
            }
        }

        protected String GetImageClass(object eval)
        {
            var type = Convert.ToString(eval);

            if (type == "Class")
                return "fa fa-drivers-license-o";

            if (type == "Property")
                return "fa fa-pencil-square-o";

            if (type == "Method")
                return "fa fa-table";

            if (type == "Parameter")
                return "fa fa-list-alt";

            return null;
        }

    }
}