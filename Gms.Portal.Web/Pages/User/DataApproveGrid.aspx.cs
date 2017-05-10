using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using NHibernate.Linq;
using System.Linq;
using CITI.EVO.Tools.Collections;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Enums;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using CITI.EVO.Tools.Web.UI.Helpers;

namespace Gms.Portal.Web.Pages.User
{
    public partial class DataApproveGrid : BasePage
    {
        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FormID"]); }
        }

        public Guid? FieldID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FieldID"]); }
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

        protected void Page_Init(object sender, EventArgs e)
        {
            InitDataGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UmUtil.Instance.IsLogged)
                return;

            FillDataGrid();
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            var model = dataApproveGridFilterControl.Model;

            var url = new UrlHelper(RequestUrl)
            {
                ["FormID"] = model.FormID,
                ["FieldID"] = model.FieldID
            };

            Response.Redirect(url.ToEncodedUrl());
        }

        protected void dataApproveGridControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = Convert.ToString(FormMode.View),
                ["FormID"] = FormID,
                ["OwnerID"] = FormID,
                ["RecordID"] = e.Value,
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void dataApproveGridControl_OnStatus(object sender, GenericEventArgs<Guid> e)
        {
            var document = MongoDbUtil.GetDocument(FormID, e.Value);

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

        protected void dataApproveGridControl_OnPrint(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Handlers/PrintFormData.ashx")
            {
                ["FormID"] = FormID,
                ["RecordID"] = e.Value,
                ["LoginToken"] = UmUtil.Instance.CurrentToken,
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void recordStatusControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeRecordStatus.Show();
        }

        protected void btnRecordStatusOK_Click(object sender, EventArgs e)
        {
            var model = recordStatusControl.Model;
            if (model.StatusID == DataStatusCache.Accepted.ID)
            {
                mpeConfirmAccept.Show();
                return;
            }

            ChangeStatus();
            FillDataGrid();

            mpeRecordStatus.Hide();
        }

        protected void btnConfirmAcceptOK_Click(object sender, EventArgs e)
        {
            ChangeStatus();

            mpeConfirmAccept.Hide();
            mpeRecordStatus.Hide();
        }

        protected void btnRecordStatusCancel_OnClick(object sender, EventArgs e)
        {
            mpeRecordStatus.Hide();
        }

        protected void btnConfirmAcceptCancel_OnClick(object sender, EventArgs e)
        {
            mpeConfirmAccept.Hide();
        }

        protected void ChangeStatus()
        {
            var filterModel = dataApproveGridFilterControl.Model;
            if (filterModel.FormID == null)
                return;

            var model = recordStatusControl.Model;
            var ownerID = filterModel.FormID;

            var document = MongoDbUtil.GetDocument(ownerID, model.RecordID);

            if (UserUtil.IsSuperAdmin())
            {
                var formDataUnit = BsonDocumentConverter.ConvertToFormDataUnit(document);
                if (formDataUnit == null)
                    return;

                FormDataDbUtil.ChangeStatus(ownerID.GetValueOrDefault(), model);
            }
            else
            {
                BsonValue bsonValue;
                if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                    return;

                var oldArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
                if (oldArray == null)
                    return;

                var formStatusUnits = BsonDocumentConverter.ConvertToFormStatuses(oldArray).ToList();
                var dictionary = formStatusUnits.ToDictionary(n => n.UserID);

                var userID = UserUtil.GetCurrentUserID();
                if (userID == null)
                    throw new Exception();

                var formStatus = dictionary[userID];
                formStatus.StatusID = model.StatusID;
                formStatus.DateOfStatus = DateTime.Now;

                var formStatusDocs = BsonDocumentConverter.ConvertToFormStatuses(formStatusUnits);
                var newArray = new BsonArray(formStatusDocs);

                document[FormDataConstants.UserStatusesFields] = newArray;

                MongoDbUtil.UpdateDocument(ownerID, document);
            }

            FillDataGrid();
        }

        protected void InitDataGrid()
        {
            if (DbForm == null)
                return;

            dataApproveGridControl.InitStructure(DbForm);
        }

        protected void FillDataGrid()
        {
            if (FormID == null || FieldID == null || FormEntity == null)
                return;

            var fields = FormStructureUtil.PreOrderFirstLevelTraversal(FormEntity).OfType<FieldEntity>();

            var field = fields.FirstOrDefault(n => n.ID == FieldID);
            if (field == null || String.IsNullOrWhiteSpace(field.DataSourceID))
                return;

            if (field.ValueExpression != FormDataConstants.IDField)
                return;

            var childID = (Guid?)null;
            var parentID = DataConverter.ToNullableGuid(field.DataSourceID);

            if (RegexUtil.DataSourceParserRx.IsMatch(field.DataSourceID))
            {
                var match = RegexUtil.DataSourceParserRx.Match(field.DataSourceID);

                parentID = DataConverter.ToNullableGuid(match.Groups["parentID"].Value);
                childID = DataConverter.ToNullableGuid(match.Groups["childID"].Value);
            }

            var userID = UserUtil.GetCurrentUserID();
            var collectionID = (childID ?? parentID);

            var myDataFilter = new Dictionary<String, Object>
            {
                [FormDataConstants.UserIDField] = userID,
                [FormDataConstants.DateDeletedField] = null,
            };

            var myDocuments = MongoDbUtil.FindDocuments(collectionID, myDataFilter);

            var myFormDatas = BsonDocumentConverter.ConvertToFormDataUnit(myDocuments).ToList();
            if (myFormDatas.Count == 0)
                return;

            var myFormDatasID = myFormDatas.Select(n => Convert.ToString(n.ID)).ToArray();

            var fieldKey = Convert.ToString(FieldID);

            var userDataFilter = new Dictionary<String, Object>
            {
                [fieldKey] = myFormDatasID,
                [FormDataConstants.DateDeletedField] = null,
            };

            var userDocuments = MongoDbUtil.FindDocuments(FormID, userDataFilter);
            var userFormDatas = BsonDocumentConverter.ConvertToFormDataUnit(userDocuments).ToList();

            var list = (from n in fields
                        where n.Visible &&
                              n.DisplayOnGrid == "Always"
                        select n).ToList();

            var formFields = list.Select(n => Convert.ToString(n.ID)).Union(FormDataBase.DefaultFields);
            var fieldSet = formFields.ToHashSet();

            var dataView = new DictionaryDataView(userFormDatas, fieldSet);

            dataApproveGridControl.BindData(dataView);
        }
    }
}