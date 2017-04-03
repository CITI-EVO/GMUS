using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.CollectionStructure;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class AddEditCollectionData : BasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var entity = GetEntity();

            var fields = entity.Fields.ToDictionary(n => (Object)n.ID, n => n.Name);
            fields.Add(FormDataConstants.IDField, FormDataConstants.IDField);

            collectionDataControl.InitStructure(fields);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var recordID = DataConverter.ToNullableGuid(RequestUrl["recordID"]);
            if (recordID == null)
                return;

            var collectionID = DataConverter.ToNullableGuid(RequestUrl["collectionID"]);
            if (collectionID == null)
                return;

            var entity = GetEntity();
            if (entity == null || entity.Fields == null)
                return;

            if (!IsPostBack)
            {
                var document = MongoDbUtil.GetDocument(collectionID, recordID);
                if (document == null)
                    return;

                var dictionary = BsonDocumentConverter.ConvertToDictionary(document);

                var dataModel = new CollectionDataModel
                {
                    ID = recordID,
                    Data = (Dictionary<String, Object>)dictionary
                };

                collectionDataControl.Model = dataModel;
            }
        }

        protected void btnSaveCollectionData_OnClick(object sender, EventArgs e)
        {
            var recordID = DataConverter.ToNullableGuid(RequestUrl["recordID"]);
            var collectionID = DataConverter.ToNullableGuid(RequestUrl["collectionID"]);

            var dbEntity = HbSession.Get<GM_Collection>(collectionID);
            if (dbEntity == null)
                return;

            var converter = new CollectionEntityModelConverter(HbSession);
            var model = converter.Convert(dbEntity);

            var entity = model.Entity;
            if (entity == null || entity.Fields == null)
                return;


            var oldDocument = MongoDbUtil.GetDocument(collectionID, recordID);
            var isNew = (oldDocument == null);

            var dataModel = collectionDataControl.Model;
            var dictionary = dataModel.Data;

            if (dictionary.ContainsKey(FormDataConstants.IDField))
            {
                var value = dictionary[FormDataConstants.IDField];
                var guid = DataConverter.ToNullableGuid(value);

                dictionary[FormDataConstants.IDField] = guid;
            }

            var newDocument = BsonDocumentConverter.ConvertToBsonDocument(dictionary);

            oldDocument = (oldDocument ?? new BsonDocument());
            MongoDbUtil.MergeDocuments(oldDocument, newDocument);

            if (isNew)
            {
                recordID = Guid.NewGuid();
                oldDocument[FormDataConstants.IDField] = BsonValue.Create(recordID);
            }

            if (isNew)
                MongoDbUtil.InsertDocument(collectionID, oldDocument);
            else
                MongoDbUtil.UpdateDocument(collectionID, oldDocument);

            var urlHelper = new UrlHelper("~/Pages/Management/CollectionDataList.aspx");
            urlHelper["collectionID"] = RequestUrl["collectionID"];

            Response.Redirect(urlHelper.ToEncodedUrl());

            //var urlHelper = new UrlHelper("~/Pages/Management/AddEditCollectionData.aspx");
            //urlHelper["collectionID"] = collectionID;
            //urlHelper["recordID"] = recordID;

            //Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void btnCancelCollectionData_OnClick(object sender, EventArgs e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/CollectionDataList.aspx");
            urlHelper["collectionID"] = RequestUrl["collectionID"];

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected CollectionEntity GetEntity()
        {
            var collectionID = DataConverter.ToNullableGuid(RequestUrl["collectionID"]);

            var dbEntity = HbSession.Get<GM_Collection>(collectionID);
            if (dbEntity == null)
                return null;

            var converter = new CollectionEntityModelConverter(HbSession);
            var model = converter.Convert(dbEntity);

            var entity = model.Entity;
            if (entity == null || entity.Fields == null)
                return null;

            return entity;
        }
    }
}