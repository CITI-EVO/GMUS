<%@ WebHandler Language="C#" Class="DataChanger" %>

using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using NHibernate.Linq;
using IMongoCollectionExtensions = MongoDB.Driver.IMongoCollectionExtensions;

public class DataChanger : IHttpHandler, IRequiresSessionState
{
    public bool IsReusable
    {
        get { return false; }
    }

    public void ProcessRequest(HttpContext context)
    {
        var request = context.Request;
        var response = context.Response;

        var requestUrl = new UrlHelper(request.Url);

        var loginToken = DataConverter.ToNullableGuid(requestUrl["LoginToken"]);
        if (loginToken == null)
        {
            UmUtil.Instance.GoToLogin();
            return;
        }

        if (!UmUtil.Instance.Login(loginToken.Value))
        {
            UmUtil.Instance.GoToLogin();
            return;
        }

        var formID = DataConverter.ToNullableGuid(requestUrl["FormID"]);
        var fieldKey = DataConverter.ToString(requestUrl["FieldKey"]);

        var fieldVal = requestUrl["FieldVal"];

        var session = Hb8Factory.InitSession();

        var dbForm = session.Query<GM_Form>().FirstOrDefault(n => n.ID == formID);
        if (dbForm == null)
            throw new Exception("Unable to load form");

        if (dbForm.UserMode != "SingleRecord")
            throw new Exception("Unsupported form");

        var userID = UmUtil.Instance.CurrentUser.ID;

        var collection = MongoDbUtil.GetCollection(dbForm.ID);

        var document = (from n in collection.AsQueryable()
                        where n[FormDataConstants.UserIDField] == userID &&
                              n[FormDataConstants.DateDeletedField] == (DateTime?)null
                        select n).FirstOrDefault();

        var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
        if (formData == null)
            throw new Exception("Unable to find record");

        var recordID = formData.ID;
        if (recordID == null)
            throw new Exception("Unable to find record");

        if (FormDataBase.DefaultFields.Contains(fieldKey))
        {
            var update = Builders<BsonDocument>.Update.Set(fieldKey, fieldVal);
            var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, recordID);

            collection.UpdateOne(filter, update);

            var urlHelper = new UrlHelper("~/Pages/User/FormDataGrid.aspx")
            {
                ["FormID"] = dbForm.ID
            };

            response.Redirect(urlHelper.ToEncodedUrl());
        }
        else
        {
            var fieldID = DataConverter.ToNullableGuid(fieldKey);
            if (fieldID == null)
                throw new Exception("Invalid field ID");

            var converter = new FormEntityModelConverter(session);
            var formModel = converter.Convert(dbForm);

            var allControls = FormStructureUtil.PreOrderFirstLevelTraversal(formModel.Entity);

            var fieldEntity = allControls.FirstOrDefault(n => n.ID == fieldID);
            if (fieldEntity == null)
                throw new Exception("Unable to find field");

            var fieldIdKey = Convert.ToString(fieldEntity.ID);

            var update = Builders<BsonDocument>.Update.Set(fieldIdKey, fieldVal);
            var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, recordID);

            collection.UpdateOne(filter, update);

            var urlHelper = new UrlHelper("~/Pages/User/FormDataGrid.aspx")
            {
                ["FormID"] = dbForm.ID
            };

            response.Redirect(urlHelper.ToEncodedUrl());
        }
    }
}