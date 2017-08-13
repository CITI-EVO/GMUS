using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Pages.User
{
    public partial class RecordHistory : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var recordID = DataConverter.ToNullableGuid(RequestUrl["recordID"]);

            var filter = new Dictionary<String, Object>
            {
                ["ParentID"] = recordID,
                ["DateDeleted"] = null
            };

            var historyCollection = MongoDbUtil.GetCollection(MongoDbUtil.HistoryCollectionName);
            var historyDocs = MongoDbUtil.FindDocuments(historyCollection, filter);

            var fields = new HashSet<String>
            {
                "ID",
                "UserID",
                "ParentID",
                "RawData",
                "DateCreated",
                "DateDeleted",
            };

            var query = (from n in historyDocs
                         let d = BsonDocumentConverter.ConvertToDictionary(n)
                         orderby d["DateCreated"]
                         select d);

            var dataView = new DictionaryDataView(query, fields);
            var model = new RecordHistoryModel
            {
                DataView = dataView
            };

            recordHistoryControl.Model = model;
            recordHistoryControl.DataBind();
        }
    }
}