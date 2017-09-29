using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
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
using Gms.Portal.Web.Entities.Others;

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

        private IEnumerable<IDictionary<String, Object>> _monitoringData;
        protected IEnumerable<IDictionary<String, Object>> MonitoringData
        {
            get
            {
                _monitoringData = (_monitoringData ?? LoadMonitoringData());
                return _monitoringData;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            monitoringDataGridsControl.InitStructure(FormEntity);

            FillMonitoringData();
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
                    ["ID"] = Guid.NewGuid(),
                    ["OwnerID"] = FormID,
                    ["RecordID"] = RecordID,

                    ["Goal"] = null,
                    ["TaskID"] = null,
                    ["DateOfTransfer"] = false,

                    ["Remain"] = null,
                    ["Incoming"] = null,
                    ["Outgoing"] = null,

                    ["CreateUserID"] = UserUtil.GetCurrentUserID(),

                    ["Accepted"] = false,
                    ["AcceptUserID"] = null,
                    ["DateOfAccept"] = null,

                    ["Returned"] = false,
                    ["ReturnUserID"] = null,
                    ["DateOfReturn"] = null,

                    ["Comment"] = null,

                    ["DateCreated"] = DateTime.Now,
                    ["DateChanged"] = null,
                    ["DateDeleted"] = null,
                };
            }

            dict["Goal"] = model.Goal;
            dict["TaskID"] = model.TaskID;
            dict["DateOfTransfer"] = model.DateOfTransfer;

            var list = MonitoringData.Select(n => new MoneyTransferEntity(n)).ToList();

            var remain = DataConverter.ToNullableDouble(dict["Remain"]);

            if (isNew)
            {
                var last = list.LastOrDefault();
                if (last != null)
                    remain = last.Remain;
            }
            else
            {
                var index = list.FindIndex(n => n.ID == model.ID);
                if (index > -1)
                    remain = list[index].Remain;
            }

            if (model.Type == "Incoming")
            {
                dict["Incoming"] = model.Amount;
                dict["Outgoing"] = null;

                dict["Remain"] = remain.GetValueOrDefault() + model.Amount;
            }
            else if (model.Type == "Outgoing")
            {
                dict["Incoming"] = null;
                dict["Outgoing"] = model.Amount;

                dict["Remain"] = remain.GetValueOrDefault() - model.Amount;
            }

            var bsonDoc = BsonDocumentConverter.ConvertToBsonDocument(dict);
            if (isNew)
                MongoDbUtil.InsertDocument(MongoDbUtil.MonitoringCollectionName, bsonDoc);
            else
                MongoDbUtil.UpdateDocument(MongoDbUtil.MonitoringCollectionName, bsonDoc);

            mpeMonitoringItem.Hide();

            FillMonitoringData();
        }

        protected void btnMonitoringItemNew_OnClick(object sender, EventArgs e)
        {
            var model = new MonitoringItemModel();
            monitoringItemControl.Model = model;

            mpeMonitoringItem.Show();
        }

        protected void btnMonitoringItemCancel_OnClick(object sender, EventArgs e)
        {
            mpeMonitoringItem.Hide();
        }

        protected void monitoringItemControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeMonitoringItem.Show();
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
                Goal = DataConverter.ToString(dict.GetValueOrDefault("Goal")),
                TaskID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("TaskID")),
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
                Goal = DataConverter.ToString(dict.GetValueOrDefault("Goal")),
                TaskID = DataConverter.ToNullableGuid(dict.GetValueOrDefault("TaskID")),
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

        protected IEnumerable<IDictionary<String, Object>> LoadMonitoringData()
        {
            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null
            };

            var sorts = new[] { "DateCreated" };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringCollectionName, filter, sorts);
            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary);

            return dictionaries;
        }

        protected void FillMonitoringData()
        {
            var filter = new Dictionary<String, Object>
            {
                ["OwnerID"] = FormID,
                ["RecordID"] = RecordID,
                ["DateDeleted"] = null
            };

            var sorts = new[] { "DateCreated" };

            var documents = MongoDbUtil.FindDocuments(MongoDbUtil.MonitoringCollectionName, filter, sorts);
            var dictionaries = documents.Select(BsonDocumentConverter.ConvertToDictionary);

            monitoringDataGridsControl.BindData(FormData);

            monitoringBudgetDataControl.BindData(FormEntity, FormData, dictionaries);
            summaryBudgetDataControl.BindData(FormEntity, FormData, dictionaries);
            monitoringItemControl.BindData(FormEntity, FormData);

        }
    }
}