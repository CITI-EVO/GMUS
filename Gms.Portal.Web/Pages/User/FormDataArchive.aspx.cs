using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.User
{
    public partial class FormDataArchive : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        protected void FillDataGrid()
        {
            var fields = new HashSet<String>
            {
                FormDataConstants.IDField,
                FormDataConstants.FormIDField,
                FormDataConstants.UserIDField,
                FormDataConstants.OwnerIDField,
                FormDataConstants.IDNumberField,
                FormDataConstants.DateCreatedField,
                FormDataConstants.DateOfSubmitField,
            };

            var formDatas = GetFormDatas();
            var dataView = new DictionaryDataView(formDatas, fields);

            var model = new FormDataArchiveGridModel
            {
                DataView = dataView
            };

            formDataArchiveGridControl.Model = model;
            formDataArchiveGridControl.DataBind();
        }

        protected IEnumerable<FormDataBase> GetFormDatas()
        {
            var owners = GetOwners();
            var recordsSet = new HashSet<Guid?>();

            foreach (var ownerID in owners)
            {
                var formDatas = GetFormData(ownerID);

                foreach (var formRecord in formDatas)
                {
                    if (!recordsSet.Add(formRecord.ID))
                        continue;

                    var formData = new FormDataBase
                    {
                        [FormDataConstants.IDField] = formRecord.ID,
                        [FormDataConstants.FormIDField] = formRecord.FormID,
                        [FormDataConstants.UserIDField] = formRecord.UserID,
                        [FormDataConstants.OwnerIDField] = formRecord.OwnerID,
                        [FormDataConstants.IDNumberField] = formRecord.IDNumber,
                        [FormDataConstants.DateCreatedField] = formRecord.DateCreated,
                        [FormDataConstants.DateOfSubmitField] = formRecord.DateOfSubmit,
                    };

                    yield return formData;
                }
            }
        }

        protected IEnumerable<FormDataUnit> GetFormData(Guid ownerID)
        {
            var userID = UserUtil.GetCurrentUserID();

            var filter = new Dictionary<String, Object>
            {
                {FormDataConstants.UserIDField, userID},
                {FormDataConstants.DateDeletedField, null},
            };

            var documents = MongoDbUtil.FindDocuments(ownerID, filter);

            foreach (var document in documents)
            {
                var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
                yield return formData;
            }
        }

        protected IEnumerable<Guid> GetOwners()
        {
            var dbForms = (from n in HbSession.Query<GM_Form>()
                           where n.DateDeleted == null
                           orderby n.OrderIndex, n.Name
                           select n).ToList();

            var converter = new FormEntityModelConverter(HbSession);
            var models = dbForms.Select(n => converter.Convert(n));

            foreach (var formModel in models)
            {
                if (formModel.Entity == null)
                    continue;

                yield return formModel.ID.GetValueOrDefault();
            }
        }
    }
}