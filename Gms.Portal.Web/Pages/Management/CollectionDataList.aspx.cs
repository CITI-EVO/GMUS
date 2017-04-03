using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class CollectionDataList : BasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditCollectionData.aspx");
            urlHelper["CollectionID"] = RequestUrl["collectionID"];

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            var collectionID = DataConverter.ToNullableGuid(RequestUrl["collectionID"]);
            MongoDbUtil.ClearCollection(collectionID);

            BindData();
        }

        protected void btnImport_OnClick(object sender, EventArgs e)
        {
            mpeImport.Show();
        }

        protected void btnImportOK_Click(object sender, EventArgs e)
        {
            var collectionID = DataConverter.ToNullableGuid(RequestUrl["collectionID"]);

            var fileBytes = fuFile.FileBytes;
            if (fileBytes == null || fileBytes.Length == 0)
                return;

            var dbEntity = HbSession.Get<GM_Collection>(collectionID);
            if (dbEntity == null)
                return;

            var converter = new CollectionEntityModelConverter(HbSession);
            var model = converter.Convert(dbEntity);

            var entity = model.Entity;
            if (entity == null || entity.Fields == null)
                return;

            var workbook = ExcelUtil.ReadExcel(fileBytes);
            var dataSet = ExcelUtil.ConvertToDataSet(workbook);

            var collection = MongoDbUtil.GetCollection(collectionID);

            var list = new List<BsonDocument>();

            var tableName = String.Format("#{0}", entity.Name);
            var dataTable = dataSet.Tables[tableName];

            foreach (var dataRow in dataTable.AsEnumerable())
            {
                var bsonDoc = new BsonDocument();
                bsonDoc[FormDataConstants.IDField] = Guid.NewGuid();

                foreach (var field in entity.Fields)
                {
                    var key = Convert.ToString(field.ID);
                    var name = String.Format("#{0}", field.Name);

                    var val = (Convert.ToString(dataRow[name]) ?? String.Empty);
                    val = val.Replace("\n", ", \n");

                    bsonDoc[key] = val;
                }

                list.Add(bsonDoc);
            }

            collection.InsertMany(list);

            BindData();
        }

        protected void collectionDatasControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditCollectionData.aspx");
            urlHelper["CollectionID"] = RequestUrl["collectionID"];
            urlHelper["RecordID"] = e.Value;
            urlHelper["Mode"] = "Edit";

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void collectionDatasControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var collectionID = DataConverter.ToNullableGuid(RequestUrl["collectionID"]);

            var dbEntity = HbSession.Query<GM_Collection>().FirstOrDefault(n => n.ID == collectionID);
            if (dbEntity == null)
                return;

            var collection = MongoDbUtil.GetCollection(collectionID);
            var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, e.Value);

            collection.DeleteOne(filter);

            BindData();
        }

        protected void collectionDatasControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditCollectionData.aspx");
            urlHelper["CollectionID"] = RequestUrl["collectionID"];
            urlHelper["RecordID"] = e.Value;
            urlHelper["Mode"] = "View";

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void BindData()
        {
            var collectionID = DataConverter.ToNullableGuid(RequestUrl["collectionID"]);

            var dbEntity = HbSession.Get<GM_Collection>(collectionID);
            if (dbEntity == null)
                return;

            var converter = new CollectionEntityModelConverter(HbSession);
            var model = converter.Convert(dbEntity);

            var entity = model.Entity;
            if (entity == null || entity.Fields == null)
                return;

            var fields = entity.Fields.ToDictionary(n => (Object)n.ID, n => n.Name);
            fields.Add(FormDataConstants.IDField, FormDataConstants.IDField);

            var viewFields = fields.Keys.Select(Convert.ToString).ToHashSet();

            var collection = MongoDbUtil.GetCollection(collectionID);

            var query = collection.AsQueryable();

            var dictionaries = BsonDocumentConverter.ConvertToDictionary(query);
            var dataView = new DictionaryDataView(dictionaries, viewFields);

            var datasModel = new CollectionDatasModel
            {
                Fields = fields,
                DataView = dataView
            };

            collectionDatasControl.Model = datasModel;
            collectionDatasControl.DataBind();
        }
    }
}