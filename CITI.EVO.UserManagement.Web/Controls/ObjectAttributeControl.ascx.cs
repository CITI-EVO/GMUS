using System;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class ObjectAttributeControl : BaseUserControlExtend<ObjectAttributeModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void comboBox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyViewMode();
            OnDataChanged(e);
        }

        protected override void OnSetModel(object model)
        {
            ApplyViewMode();
        }

        protected void ApplyViewMode()
        {
            if (cbxProject.DataSource == null)
            {
                var projects = (from n in HbSession.Query<UM_Project>()
                                where n.DateDeleted == null
                                orderby n.Name
                                select n);

                cbxProject.DataSource = projects;
                cbxProject.DataBind();
            }

            var projectID = cbxProject.TryGetGuidValue();
            if (projectID != null)
            {
                var schemas = (from n in HbSession.Query<UM_AttributeSchema>()
                               where n.DateDeleted == null && n.ProjectID == projectID
                               orderby n.Name
                               select n);

                cbxSchema.DataSource = schemas;
                cbxSchema.DataBind();
            }

            var schemaID = cbxSchema.TryGetGuidValue();
            if (schemaID != null)
            {
                var nodes = (from n in HbSession.Query<UM_AttributeField>()
                             where n.DateDeleted == null && n.AttributeSchemaID == schemaID
                             orderby n.Name
                             select n);

                cbxField.DataSource = nodes;
                cbxField.DataBind();
            }

            var nodeID = cbxField.TryGetGuidValue();
            if (nodeID != null)
            {
                var parentID = DataConverter.ToNullableGuid(hdParentID.Value);
                if (parentID == null)
                    return;

                var attributeValue = (from n in HbSession.Query<UM_AttributeValue>()
                                      where n.DateDeleted == null && n.AttributeFieldID == nodeID && n.ParentID == parentID
                                      select n).FirstOrDefault();

                if (attributeValue != null)
                    tbxValue.Text = attributeValue.Value;
            }
        }
    }
}