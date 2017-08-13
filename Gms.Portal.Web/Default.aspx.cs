using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
using MongoDB.Bson;

public partial class _Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var command = Convert.ToString(Request["command"]);
        if (command == "setup")
        {
            var schema = new NHibernate.Tool.hbm2ddl.SchemaExport(Hb8Factory.Configuration);
            schema.Create(false, true);
        }

        //var q = (from n in CultureInfo.GetCultures(CultureTypes.AllCultures)
        //         where !String.IsNullOrWhiteSpace(n.Name)
        //         select new
        //         {
        //             ISO = n.Name,
        //             n.EnglishName,
        //             n.NativeName,
        //         });

        //gvData.DataSource = q;
        //gvData.DataBind();

        //var forms = HbSession.Query<GM_Form>().Where(n => n.DateDeleted == null).ToList();

        //foreach (var dbForm in forms)
        //{
        //    var collection = MongoDbUtil.GetCollection(dbForm.ID);
        //    var documents = collection.AsQueryable().ToList();

        //    foreach (var document in documents)
        //    {
        //        if (Change(document))
        //            MongoDbUtil.UpdateDocument(collection, document);
        //    }
        //}

        //var baseCollection = MongoDbUtil.GetCollection(Guid.Parse("6320E740-820A-4C63-8A00-69D1DD7C50F1"));
        //var subCollection = MongoDbUtil.GetCollection(Guid.Parse("30DAE979-8416-45AF-A122-8D54B403F121"));
        //var leafCollection = MongoDbUtil.GetCollection(Guid.Parse("30DAE979-8416-45AF-A122-8D54B403F121"));

        //var query = (from n in subCollection.AsQueryable()
        //             where n[FormDataConstants.DateDeletedField] == (DateTime?)null &&
        //                   n[FormDataConstants.DateCreatedField] > (DateTime?)null
        //             join m in baseCollection.AsQueryable() on n[FormDataConstants.ParentIDField] equals m[FormDataConstants.IDField]
        //             where m[FormDataConstants.DateDeletedField] == (DateTime?)null
        //             select m);

        //MongoDbUtil.ExtractQueryFields(query).ToList();
    }

    protected bool Change(BsonDocument document)
    {
        BsonValue bsonValue;
        if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
            return false;

        var bsonArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
        if (bsonArray == null)
            bsonArray = new BsonArray();

        var statusUnits = BsonDocumentConverter.ConvertToFormStatuses(bsonArray);

        var statusList = statusUnits.ToList();
        if (statusList.Count == 0)
            return false;

        foreach (var statusUnit in statusList)
        {
            statusUnit.Params = (statusUnit.Params ?? new Dictionary<String, Object>());
            statusUnit.Params[FormDataConstants.ScoringParams] = "@";
        }

        var formStatusesArray = BsonDocumentConverter.ConvertToFormStatusesArray(statusList);
        document[FormDataConstants.UserStatusesFields] = formStatusesArray;

        return true;
    }
}
