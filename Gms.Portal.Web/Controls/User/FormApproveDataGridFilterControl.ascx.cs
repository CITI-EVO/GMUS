using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormApproveDataGridFilterControl : BaseUserControlExtend<FormApproveDataGridFilterModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyViewMode();
        }

        public override FormApproveDataGridFilterModel GetModel()
        {
            var model = base.GetModel();

            return model;
        }

        protected void ApplyViewMode()
        {
            var dbForms = (from n in HbSession.Query<GM_Form>()
                           where n.DateDeleted == null &&
                                 n.RequiresApprove == true
                           orderby n.OrderIndex, n.Name
                           select n).ToList();

            var converter = new FormEntityModelConverter(HbSession);
            var models = dbForms.Select(n => converter.Convert(n)).ToList();

            cbxForm.BindData(models);

            var formID = DataConverter.ToNullableGuid(RequestUrl["FormID"]);
            if (formID != null && !IsPostBack)
                cbxForm.TrySetSelectedValue(formID);

            formID = cbxForm.TryGetGuidValue();

            if (formID != null)
            {
                var model = models.FirstOrDefault(n => n.ID == formID);
                var fields = GetFormFields(model);

                var formFields = (from n in fields
                                  where n.FormID == n.OwnerID
                                  select new
                                  {
                                      ID = $"{n.OwnerID}/{n.FieldID}",
                                      Name = n.FieldName,
                                  });

                var subGridFields = (from n in fields
                                     where n.FormID != n.OwnerID
                                     select new
                                     {
                                         ID = $"{n.OwnerID}/{n.FieldID}",
                                         Name = $"{n.OwnerName}/{n.FieldName}",
                                     });

                var dataSources = formFields.Union(subGridFields);

                cbxField.BindData(dataSources);

                var sourceField = RequestUrl["SourceField"];
                if (sourceField != null)
                {
                    if (!IsPostBack)
                        cbxField.TrySetSelectedValue(sourceField);
                }
            }
        }

        protected IEnumerable<FormFieldInfoEntity> GetFormFields(FormModel formModel)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;

            if (formModel.Entity == null)
                yield break;

            var formEntity = formModel.Entity;
            formEntity.ID = formModel.ID.GetValueOrDefault();

            var treeNodes = FormStructureUtil.CreateTree(formEntity);
            var treeDict = treeNodes.ToDictionary(n => n.ID);

            var controls = FormStructureUtil.PreOrderTraversal(formModel.Entity);

            foreach (var control in controls)
            {
                var field = control as FieldEntity;
                if (field == null)
                    continue;

                if (!field.RequiresApproval.GetValueOrDefault())
                    continue;

                if (String.IsNullOrWhiteSpace(field.DataSourceID))
                    continue;

                if (!comparer.Equals(field.ValueExpression, FormDataConstants.IDField))
                    continue;

                if (!comparer.Equals(field.Type, "Lookup") && !comparer.Equals(field.Type, "ComboBox"))
                    continue;

                var owner = GetControlOwner(treeDict, field.ID);

                var entity = new FormFieldInfoEntity
                {
                    FormID = formModel.ID,
                    FormName = formModel.Name,
                    OwnerID = owner.ID,
                    OwnerName = owner.Name,
                    FieldID = field.ID,
                    FieldName = field.Name,
                    DataSource = field.DataSourceID,
                };

                yield return entity;
            }
        }

        protected ElementTreeNodeEntity GetControlOwner(IDictionary<Guid?, ElementTreeNodeEntity> treeDict, Guid? controlID)
        {
            var parents = FormStructureUtil.ParentsTraversal(treeDict, controlID);

            foreach (var node in parents)
            {
                if (node.ControlType == "Form" || node.ControlType == "Grid" || node.ControlType == "Tree")
                    return node;
            }

            return null;
        }

    }
}