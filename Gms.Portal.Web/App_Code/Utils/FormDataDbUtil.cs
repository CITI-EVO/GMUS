using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using CITI.EVO.Tools.Extensions;

namespace Gms.Portal.Web.Utils
{
    public static class FormDataDbUtil
    {
        public static bool ChangeStatus(Guid? ownerID, RecordStatusModel model)
        {
            return ChangeStatus(ownerID, model.RecordID.GetValueOrDefault(), model.StatusID.GetValueOrDefault(), model.Description);
        }
        public static bool ChangeStatus(Guid? ownerID, Guid recordID, Guid statusID, String description)
        {
            var collection = MongoDbUtil.GetCollection(ownerID);

            var update = Builders<BsonDocument>.Update.Set(FormDataConstants.StatusIDField, statusID);
            update = update.Set(FormDataConstants.DescriptionField, description);
            update = update.Set(FormDataConstants.StatusChangeDateField, DateTime.Now);

            if (statusID == DataStatusCache.Submit.ID)
                update = update.Set(FormDataConstants.DateOfSubmitField, DateTime.Now);

            if (statusID == DataStatusCache.Accepted.ID)
                update = update.Set(FormDataConstants.DateOfAcceptField, DateTime.Now);

            var filter = Builders<BsonDocument>.Filter.Eq(FormDataConstants.IDField, recordID);

            var result = collection.UpdateMany(filter, update);
            return (result.ModifiedCount > 0);
        }

        public static IEnumerable<FormDataUnit> FilterByUserStatus(IEnumerable<FormDataUnit> source, Guid? userID)
        {
            var query = (from n in source
                         where IsUserInUserStatus(n, userID)
                         select n);

            return query;
        }

        public static bool IsUserInUserStatus(FormDataUnit formData, Guid? userID)
        {
            if (userID == null || formData == null || formData.UserStatuses == null)
                return false;

            if (formData.ID == null)
            {
                var query = from m in formData.UserStatuses
                            where m.UserID == userID
                            select m;

                return query.Any();
            }

            var cache = CommonObjectCache.InitObject("@UserStatuses", CommonCacheStore.Request, ConcurrencyHelper.CreateDictionary<Guid?, ILookup<Guid?, FormStatusUnit>>);

            var statusesLp = cache.GetValueOrDefault(formData.ID);
            if (statusesLp == null)
            {
                statusesLp = formData.UserStatuses.ToLookup(n => n.UserID);
                cache.Add(formData.ID, statusesLp);
            }

            return statusesLp.Contains(userID);
        }
    }
}