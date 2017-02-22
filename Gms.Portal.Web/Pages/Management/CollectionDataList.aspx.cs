using System;
using System.Collections;
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
using MongoDB.Driver;

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

        protected void collectionDatasControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditCollectionData.aspx");
            urlHelper["CollectionID"] = RequestUrl["collectionID"];
            urlHelper["RecordID"] = e.Value;

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void collectionDatasControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {

        }

        protected void collectionDatasControl_OnView(object sender, GenericEventArgs<Guid> e)
        {

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
            fields.Add(FormDataUnit.IDField, FormDataUnit.IDField);

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