using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using CommonUtil = Gms.Portal.Web.Utils.CommonUtil;

namespace Gms.Portal.Web.Pages.User
{
    public partial class FormDataGrid : BasePage
    {
        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FormID"]); }
        }

        public Guid? OwnerID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["OwnerID"]); }
        }

        public Guid? ParentID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["ParentID"]); }
        }

        public Guid? UserID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["UserID"]); }
        }


        private GM_Form _dbForm;
        protected GM_Form DbForm
        {
            get
            {
                if (FormID != null)
                {
                    if (_dbForm == null)
                        _dbForm = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == FormID);
                }

                return _dbForm;
            }
        }

        private FormEntity _formEntity;
        protected FormEntity FormEntity
        {
            get
            {
                if (_formEntity == null)
                {
                    if (DbForm == null)
                        return null;

                    var converter = new FormEntityModelConverter(HbSession);
                    var model = converter.Convert(DbForm);

                    _formEntity = model.Entity;
                }

                return _formEntity;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UmUtil.Instance.IsLogged)
                return;

            CheckFormUserMode();
            FillGridView();
            SetControlAccesibilities();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx");
            urlHelper["Mode"] = "Edit";
            urlHelper["FormID"] = FormID;
            urlHelper["OwnerID"] = (OwnerID ?? FormID);
            urlHelper["RecordID"] = null;
            urlHelper["ParentID"] = ParentID;
            urlHelper["ReturnUrl"] = CommonUtil.ConvertToBase64(returnUrl);

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {

        }

        protected void formDataGridControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx");
            urlHelper["Mode"] = "Edit";
            urlHelper["FormID"] = FormID;
            urlHelper["OwnerID"] = (OwnerID ?? FormID);
            urlHelper["RecordID"] = e.Value;
            urlHelper["ParentID"] = ParentID;
            urlHelper["ReturnUrl"] = CommonUtil.ConvertToBase64(returnUrl);

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void formDataGridControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx");
            urlHelper["Mode"] = "View";
            urlHelper["FormID"] = FormID;
            urlHelper["OwnerID"] = (OwnerID ?? FormID);
            urlHelper["RecordID"] = e.Value;
            urlHelper["ParentID"] = ParentID;
            urlHelper["ReturnUrl"] = CommonUtil.ConvertToBase64(returnUrl);

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void formDataGridControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var collection = MongoDbUtil.GetCollection(OwnerID);

            var filter = Builders<BsonDocument>.Filter.Eq("ID", e.Value);
            var update = Builders<BsonDocument>.Update.Set("DateDeleted", DateTime.Now);

            collection.UpdateMany(filter, update);

            FillGridView();
        }

        protected void formDataGridControl_OnStatus(object sender, GenericEventArgs<Guid> e)
        {
            var ownerID = (OwnerID ?? FormID);

            var document = MongoDbUtil.GetDocument(ownerID, e.Value);

            var formDataUnit = BsonDocumentConverter.ConvertToFormDataUnit(document);
            if (formDataUnit == null)
                return;

            var model = new RecordStatusModel
            {
                RecordID = formDataUnit.ID,
                StatusID = formDataUnit.StatusID
            };

            recordStatusControl.Model = model;
            mpeRecordStatus.Show();
        }

        protected void recordStatusControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeRecordStatus.Show();
        }

        protected void btnRecordStatusOK_Click(object sender, EventArgs e)
        {
            var model = recordStatusControl.Model;

            var ownerID = (OwnerID ?? FormID);

            var document = MongoDbUtil.GetDocument(ownerID, model.RecordID);

            var formDataUnit = BsonDocumentConverter.ConvertToFormDataUnit(document);
            if (formDataUnit == null)
                return;

            FormDataDbUtil.ChangeStatus(ownerID.GetValueOrDefault(), model);

            FillGridView();
        }

        protected void FillGridView()
        {
            if (FormID == null)
                return;

            if (FormEntity == null)
                return;

            var controls = FormStructureUtil.InOrderFirstLevelTraversal(FormEntity);

            var list = (from n in controls.OfType<FieldEntity>()
                        where n.Visible && n.DisplayOnGrid.GetValueOrDefault()
                        select n).ToList();

            var formFields = list.Select(n => Convert.ToString(n.ID)).Union(FormDataBase.DefaultFields);
            var fieldSet = formFields.ToHashSet();

            var userID = UserID;
            if (!UmUtil.Instance.HasAccess("Admin"))
                userID = UserUtil.GetCurrentUserID();

            var collectionID = (OwnerID ?? FormID);

            var collection = MongoDbUtil.GetCollection(collectionID);

            var baseQuery = from doc in collection.AsQueryable()
                            where doc[FormDataConstants.DateDeletedField] == (DateTime?)null
                            select doc;

            if (ParentID != null)
            {
                baseQuery = (from doc in baseQuery
                             where doc[FormDataConstants.ParentIDField] == ParentID
                             select doc);
            }

            if (userID != null)
            {
                baseQuery = (from doc in baseQuery
                             where doc[FormDataConstants.UserIDField] == userID
                             select doc);
            }

            var filterModel = formGridFilterControl.Model;
            if (filterModel.StatusID != null)
            {
                baseQuery = (from doc in baseQuery
                             where doc[FormDataConstants.StatusIDField] == filterModel.StatusID
                             select doc);
            }

            if (filterModel.StartDate != null)
            {
                baseQuery = (from doc in baseQuery
                             where doc[FormDataConstants.DateCreatedField] >= filterModel.StartDate
                             select doc);
            }

            if (filterModel.EndDate != null)
            {
                baseQuery = (from doc in baseQuery
                             where doc[FormDataConstants.DateCreatedField] <= filterModel.EndDate
                             select doc);
            }

            baseQuery = (from doc in baseQuery
                         orderby doc[FormDataConstants.DateCreatedField] descending
                         select doc);

            var formDataUnits = BsonDocumentConverter.ConvertToFormDataUnit(baseQuery);

            var formDataView = new DictionaryDataView(formDataUnits, fieldSet);

            var dataGridModel = new FormDataGridModel
            {
                Fields = list,
                DataView = formDataView
            };

            formDataGridControl.Model = dataGridModel;
            formDataGridControl.DataBind();
        }

        protected void CheckFormUserMode()
        {
            if (OwnerID != null && FormID != OwnerID)
                return;

            if (!UmUtil.Instance.HasAccess("Admin"))
            {
                if (DbForm == null)
                    return;

                if (String.IsNullOrWhiteSpace(DbForm.UserMode))
                    return;

                if (DbForm.UserMode == "SingleRecord")
                {
                    var returnUrl = RequestUrl.ToEncodedUrl();

                    var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx");
                    urlHelper["Mode"] = "Edit";
                    urlHelper["FormID"] = FormID;
                    urlHelper["OwnerID"] = (OwnerID ?? FormID);
                    urlHelper["RecordID"] = null;
                    urlHelper["ParentID"] = ParentID;
                    urlHelper["ReturnUrl"] = CommonUtil.ConvertToBase64(returnUrl); 

                    var userID = UserUtil.GetCurrentUserID();
                    var collection = MongoDbUtil.GetCollection(DbForm.ID);

                    var document = (from n in collection.AsQueryable()
                                    where n[FormDataConstants.UserIDField] == userID &&
                                          n[FormDataConstants.DateDeletedField] == (DateTime?)null
                                    select n).FirstOrDefault();

                    var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
                    if (formData != null)
                    {
                        if (formData.ID == null)
                            throw new Exception();

                        urlHelper["RecordID"] = formData.ID;
                    }

                    Response.Redirect(urlHelper.ToEncodedUrl());
                }
            }
        }

        protected void SetControlAccesibilities()
        {
            dvNew.Visible = !UmUtil.Instance.HasAccess("Admin");
        }
    }
}