using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.ModelToEntity;
using MongoDB.Bson;

namespace Gms.Portal.Web.Pages.User
{
    public partial class FullExpertDataGrid : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillFormList();
            FillDataGrid();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {

        }

        protected void btnUserMessageOK_Click(object sender, EventArgs e)
        {
            var model = userMessageControl.Model;
            var converter = new UserMessageModelEntityConverter(HbSession);

            var entity = converter.Convert(model);

            HbSession.SubmitChanges(entity);

            mpeUserMessage.Hide();
        }

        protected void btnUserMessageCancel_OnClick(object sender, EventArgs e)
        {
            mpeUserMessage.Hide();
        }

        protected void userMessage_OnDataChanged(object sender, EventArgs e)
        {
            mpeUserMessage.Show();
        }

        protected void fullExpertDataGridControl_OnAccept(object sender, GenericEventArgs<String> e)
        {
            if (String.IsNullOrWhiteSpace(e.Value))
                return;

            if (!RegexUtil.DataSourceParserRx.IsMatch(e.Value))
                return;

            var match = RegexUtil.DataSourceParserRx.Match(e.Value);

            var parentID = match.Groups["parentID"].Value;
            var childID = match.Groups["childID"].Value;

            var recordID = DataConverter.ToNullableGuid(parentID);
            var formID = DataConverter.ToNullableGuid(childID);

            if (recordID == null || formID == null)
                return;

            var document = MongoDbUtil.GetDocument(formID, recordID);
            if (document == null)
                return;

            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                bsonValue = new BsonArray();

            var bsonArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (bsonArray == null)
                bsonArray = new BsonArray();

            var userID = UserUtil.GetCurrentUserID();

            var statusUnits = BsonDocumentConverter.ConvertToFormStatuses(bsonArray).ToHashSet();

            var formStatus = statusUnits.FirstOrDefault(n => n.UserID == userID);
            if (formStatus == null)
                return;

            formStatus.StatusID = DataStatusCache.Accepted.ID;

            var formStatusesArray = BsonDocumentConverter.ConvertToFormStatusesArray(statusUnits);
            document[FormDataConstants.UserStatusesFields] = formStatusesArray;

            MongoDbUtil.UpdateDocument(formID, document);
        }

        protected void fullExpertDataGridControl_OnReject(object sender, GenericEventArgs<String> e)
        {
            if (String.IsNullOrWhiteSpace(e.Value))
                return;

            if (!RegexUtil.DataSourceParserRx.IsMatch(e.Value))
                return;

            var match = RegexUtil.DataSourceParserRx.Match(e.Value);

            var parentID = match.Groups["parentID"].Value;
            var childID = match.Groups["childID"].Value;

            var recordID = DataConverter.ToNullableGuid(parentID);
            var formID = DataConverter.ToNullableGuid(childID);

            if (recordID == null || formID == null)
                return;

            var document = MongoDbUtil.GetDocument(formID, recordID);
            if (document == null)
                return;

            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                bsonValue = new BsonArray();

            var bsonArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (bsonArray == null)
                bsonArray = new BsonArray();

            var userID = UserUtil.GetCurrentUserID();
            var statusUnits = BsonDocumentConverter.ConvertToFormStatuses(bsonArray).ToHashSet();

            var formStatus = statusUnits.FirstOrDefault(n => n.UserID == userID);
            if (formStatus == null)
                return;

            formStatus.StatusID = DataStatusCache.Rejected.ID;

            var formStatusesArray = BsonDocumentConverter.ConvertToFormStatusesArray(statusUnits);
            document[FormDataConstants.UserStatusesFields] = formStatusesArray;

            MongoDbUtil.UpdateDocument(formID, document);
        }

        protected void fullExpertDataGridControl_OnMessage(object sender, GenericEventArgs<String> e)
        {
            if (!RegexUtil.RecordFormParserRx.IsMatch(e.Value))
                return;

            var match = RegexUtil.RecordFormParserRx.Match(e.Value);

            var recordID = DataConverter.ToNullableGuid(match.Groups["recordID"].Value);
            var formID = DataConverter.ToNullableGuid(match.Groups["formID"].Value);
            var userID = UserUtil.GetCurrentUserID();

            var docuemnt = MongoDbUtil.GetDocument(formID, recordID);
            var formData = BsonDocumentConverter.ConvertToFormDataUnit(docuemnt);

            if (formData == null || formData.UserID == null)
                return;

            var model = new UserMessageExModel
            {
                FromUserID = userID,
                ToUserID = formData.UserID,
                RecordID = recordID,
                FormID = formID,
            };

            userMessageControl.CreateMode = true;
            userMessageControl.Model = model;

            mpeUserMessage.Show();
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
                FormDataConstants.UserStatusesFields,
            };

            var owners = GetOwners();
            var dict = owners.ToDictionary(n => n.ID, n => n.Entity.Rating);

            var formDatas = GetFormDatas(dict.Keys);
            var dataView = new DictionaryDataView(formDatas, fields);

            var model = new FullExpertDataGridModel
            {
                Entities = dict,
                DataView = dataView
            };

            fullExpertDataGridControl.Model = model;
            fullExpertDataGridControl.DataBind();
        }

        protected void FillFormList()
        {
            var dbForms = (from n in HbSession.Query<GM_Form>()
                           where n.DateDeleted == null &&
                                 n.RequiresApprove == true
                           orderby n.OrderIndex, n.Name
                           select n).ToList();

            var converter = new FormEntityModelConverter(HbSession);
            var models = dbForms.Select(n => converter.Convert(n)).ToList();

            cbxForms.BindData(models);
        }

        protected IEnumerable<FormDataBase> GetFormDatas(IEnumerable<Guid?> owners)
        {
            var recordsSet = new HashSet<Guid?>();

            foreach (var ownerID in owners)
            {
                var formDatas = GetFormData(ownerID);

                foreach (var formRecord in formDatas)
                {
                    var userStatuses = formRecord.UserStatuses;
                    if (userStatuses == null)
                        continue;

                    var query = (from n in userStatuses
                                 where n.Params != null &&
                                       n.Params.ContainsKey(FormDataConstants.ScoringParams)
                                 select n);

                    if (!UmUtil.Instance.HasAccess("Admin"))
                    {
                        query = (from n in query
                                 where n.UserID == UserUtil.GetCurrentUserID()
                                 select n);
                    }

                    if (!query.Any())
                        continue;

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
                        [FormDataConstants.UserStatusesFields] = formRecord.UserStatuses,
                    };

                    yield return formData;
                }
            }
        }

        protected IEnumerable<FormDataUnit> GetFormData(Guid? ownerID)
        {
            var filter = new Dictionary<String, Object>
            {
                {FormDataConstants.DateDeletedField, null}
            };

            if (!UserUtil.IsSuperAdmin())
            {
                filter[FormDataConstants.UserStatusUserIDField] = UserUtil.GetCurrentUserID();
            }

            var documents = MongoDbUtil.FindDocuments(ownerID, filter);

            foreach (var document in documents)
            {
                var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
                yield return formData;
            }
        }

        protected IEnumerable<FormModel> GetOwners()
        {
            var selFormID = cbxForms.TryGetGuidValue();

            var formsQuery = from n in HbSession.Query<GM_Form>()
                             where n.Visible == true &&
                                   n.DateDeleted == null &&
                                   n.RequiresApprove == true
                             select n;

            if (selFormID != null)
            {
                formsQuery = (from n in formsQuery
                              where n.ID == selFormID
                              select n);
            }

            formsQuery = (from n in formsQuery
                          orderby n.OrderIndex, n.Name
                          select n);


            var dbForms = formsQuery.ToList();

            var converter = new FormEntityModelConverter(HbSession);
            var models = dbForms.Select(n => converter.Convert(n));

            var expGlobals = new ExpressionGlobalsUtil();

            foreach (var formModel in models)
            {
                if (formModel.Entity == null)
                    continue;

                if (!formModel.Visible.GetValueOrDefault())
                    continue;

                if (!UmUtil.Instance.HasAccess("Admin") && !String.IsNullOrWhiteSpace(formModel.VisibleExpression))
                {
                    var expNode = ExpressionParser.GetOrParse(formModel.VisibleExpression);

                    Object eval;
                    if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out eval))
                        continue;

                    var result = DataConverter.ToNullableBoolean(eval);
                    if (!result.GetValueOrDefault())
                        continue;
                }

                yield return formModel;
            }
        }
    }
}