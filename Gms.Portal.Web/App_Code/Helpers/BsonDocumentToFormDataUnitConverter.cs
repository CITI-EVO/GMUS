using System;
using System.Collections.Generic;
using Gms.Portal.Web.Entities.DataContainer;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gms.Portal.Web.Helpers
{
    public class BsonDocumentToFormDataUnitConverter
    {
        public BsonDocumentToFormDataUnitConverter()
        {
        }

        public IEnumerable<FormDataUnit> Convert(IMongoQueryable<BsonDocument> source)
        {
            var list = source.ToList();
            return Convert(list);
        }

        public IEnumerable<FormDataUnit> Convert(IEnumerable<BsonDocument> source)
        {
            foreach (var document in source)
            {
                var formDataUnit = Convert(document);
                yield return formDataUnit;
            }
        }

        public FormDataUnit Convert(BsonDocument source)
        {
            var recordID = source[FormDataUnit.IDField].AsNullableGuid;
            var formDataUnit = new FormDataUnit();

            foreach (var element in source.Elements)
            {
                var bsonValue = element.Value;

                if (bsonValue.IsBsonDocument)
                {
                    var listRefDoc = bsonValue.AsBsonDocument;

                    var subFormID = listRefDoc[FormDataUnit.FormIDField].AsNullableGuid;
                    var subOwnerID = listRefDoc[FormDataUnit.OwnerIDField].AsNullableGuid;
                    var subParentID = recordID;

                    var listRef = new FormDataListRef(subFormID, subOwnerID, subParentID);
                    formDataUnit[element.Name] = listRef;
                }
                else if (bsonValue.IsObjectId)
                {
                    formDataUnit[element.Name] = bsonValue.AsNullableObjectId;
                }
                else
                {
                    formDataUnit[element.Name] = BsonTypeMapper.MapToDotNetValue(bsonValue);
                }
            }

            return formDataUnit;
        }
    }
}