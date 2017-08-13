using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
using DevExpress.XtraPrinting.Native;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

namespace Gms.Portal.Web.Pages.User
{
    public partial class RecordMonitoring : BasePage
    {
        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FormID"]); }
        }

        public Guid? RecordID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["RecordID"]); }
        }

        private GM_Form _dbForm;
        protected GM_Form DbForm
        {
            get
            {
                if (_dbForm == null)
                    _dbForm = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == FormID);

                return _dbForm;
            }
        }

        private FormModel _formModel;
        protected FormModel FormModel
        {
            get
            {
                if (_formModel == null)
                    _formModel = ModelConverter.Convert(DbForm);

                return _formModel;
            }
        }

        protected FormEntity FormEntity
        {
            get { return FormModel.Entity; }
        }

        private FormDataUnit _formData;
        protected FormDataUnit FormData
        {
            get
            {
                if (RecordID == null)
                    return null;

                if (_formData == null)
                    _formData = LoadFormData(FormID, RecordID);

                return _formData;
            }
        }

        private FormEntityModelConverter _modelConverter;
        protected FormEntityModelConverter ModelConverter
        {
            get
            {
                _modelConverter = (_modelConverter ?? new FormEntityModelConverter(HbSession));
                return _modelConverter;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            monitoringDataGridsControl.InitStructure(FormEntity);

            FillMonitoringData();
        }

        protected void monitoringItemControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeMonitoringItem.Show();
        }

        protected void btnMonitoringItemOK_OnClick(object sender, EventArgs e)
        {
            var model = monitoringItemControl.Model;

            var dict = (IDictionary<String, Object>)null;

            if (model.ID != null)
            {
                var filter = new Dictionary<String, Object>
                {
                    ["ID"] = RecordID,
                };

                var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringCollectionName, filter);
                var document = documents.SingleOrDefault();

                if (document != null)
                    dict = BsonDocumentConverter.ConvertToDictionary(document);
            }

            var isNew = (dict == null);
            if (isNew)
            {
                dict = new Dictionary<String, Object>
                {
                    ["FormID"] = FormID,
                    ["RecordID"] = RecordID,
                    ["Accepted"] = false,
                    ["Returned"] = false,
                    ["AcceptUserID"] = null,
                    ["ReturnUserID"] = null,
                    ["DateCreated"] = DateTime.Now,
                    ["CreateUserID"] = UserUtil.GetCurrentUserID(),
                };
            }

            dict["GoalID"] = model.GoalID;
            dict["BudgetID"] = model.BudgetID;
            dict["DateOfTransfer"] = model.DateOfTransfer;

            if (model.Type == "Incoming")
            {
                dict["Incoming"] = model.Amount;
                dict["Outgoing"] = 0D;
            }
            else if (model.Type == "Outgoing")
            {
                dict["Incoming"] = 0D;
                dict["Outgoing"] = model.Amount;
            }

            var bsonDoc = BsonDocumentConverter.ConvertToBsonDocument(dict);
            if (isNew)
                MongoDbUtil.InsertDocument(MongoDbUtil.MonitoringCollectionName, bsonDoc);
            else
                MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringCollectionName, bsonDoc);

            mpeMonitoringItem.Hide();
        }

        protected void btnMonitoringItemCancel_OnClick(object sender, EventArgs e)
        {
            mpeMonitoringItem.Hide();
        }

        protected void monitoringBudgetDataControl_OnAccept(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringCollectionName, filter);
            var document = documents.SingleOrDefault();

            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);

            var accepted = DataConverter.ToNullableBool(dict.GetValueOrDefault("Accepted"));

            dict["Accepted"] = !accepted.GetValueOrDefault();
            dict["AcceptUserID"] = UserUtil.GetCurrentUserID();

            document = BsonDocumentConverter.ConvertToBsonDocument(dict);
            MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringCollectionName, document);

            FillMonitoringData();
        }

        protected void monitoringBudgetDataControl_OnReturn(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringCollectionName, filter);
            var document = documents.SingleOrDefault();

            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);

            var returned = DataConverter.ToNullableBool(dict.GetValueOrDefault("Returned"));

            dict["Returned"] = !returned.GetValueOrDefault();
            dict["ReturnUserID"] = UserUtil.GetCurrentUserID();

            document = BsonDocumentConverter.ConvertToBsonDocument(dict);
            MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringCollectionName, document);

            FillMonitoringData();
        }

        protected void monitoringBudgetDataControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringCollectionName, filter);
            var document = documents.SingleOrDefault();

            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);

            var incoming = DataConverter.ToNullableDouble(dict.GetValueOrDefault("Incoming"));
            var outgoing = DataConverter.ToNullableDouble(dict.GetValueOrDefault("Outgoing"));

            var type = String.Empty;
            var amount = (double?)null;

            if (incoming == null && outgoing != null)
            {
                type = "Outgoing";
                amount = outgoing;
            }
            else if (incoming != null && outgoing == null)
            {
                type = "Incoming";
                amount = incoming;
            }

            var model = new MonitoringItemModel
            {
                ID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("ID")),
                Type = type,
                Amount = amount,
                GoalID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("GoalID")),
                BudgetID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("BudgetID")),
                DateOfTransfer = DataConverter.ToNullableDateTime(dict.GetValueOrDefault("DateOfTransfer")),
            };

            monitoringItemControl.Model = model;
            mpeMonitoringItem.Show();
        }

        protected void monitoringBudgetDataControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringCollectionName, filter);
            var document = documents.SingleOrDefault();

            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);

            var incoming = DataConverter.ToNullableDouble(dict.GetValueOrDefault("Incoming"));
            var outgoing = DataConverter.ToNullableDouble(dict.GetValueOrDefault("Outgoing"));

            var type = String.Empty;
            var amount = (double?)null;

            if (incoming == null && outgoing != null)
            {
                type = "Outgoing";
                amount = outgoing;
            }
            else if (incoming != null && outgoing == null)
            {
                type = "Incoming";
                amount = incoming;
            }

            var model = new MonitoringItemModel
            {
                ID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("ID")),
                Type = type,
                Amount = amount,
                GoalID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("GoalID")),
                BudgetID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("BudgetID")),
                DateOfTransfer = DataConverter.ToNullableDateTime(dict.GetValueOrDefault("DateOfTransfer")),
            };

            monitoringItemControl.Model = model;
            mpeMonitoringItem.Show();
        }

        protected void monitoringBudgetDataControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var filter = new Dictionary<String, Object>
            {
                ["ID"] = e.Value,
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringCollectionName, filter);
            var document = documents.SingleOrDefault();

            if (document == null)
                return;

            var dict = BsonDocumentConverter.ConvertToDictionary(document);

            dict["DateDeleted"] = DateTime.Now;

            document = BsonDocumentConverter.ConvertToBsonDocument(dict);
            MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringCollectionName, document);

            FillMonitoringData();
        }

        protected FormDataUnit LoadFormData(Guid? ownerID, Guid? recordID)
        {
            if (ownerID == null || recordID == null)
                return null;

            var filter = new Dictionary<String, Object>
            {
                {FormDataConstants.IDField, recordID }
            };

            var documents = MongoDbUtil.FindDocuments(ownerID, filter).ToList();

            var document = documents.SingleOrDefault();
            if (document == null)
                return null;

            var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
            return formData;
        }

        protected void FillMonitoringData()
        {
            var filter = new Dictionary<String, Object>
            {
                ["RecordID"] = RecordID,
                ["FormID"] = FormID,
                ["DateDeleted"] = null
            };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringCollectionName, filter);
            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary);

            monitoringDataGridsControl.BindData(FormData);

            monitoringBudgetDataControl.BindData(FormEntity, FormData, dictionaries);
            monitoringItemControl.BindData(FormEntity, FormData);
        }

    }
}