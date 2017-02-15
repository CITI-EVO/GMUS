using System.Collections.Generic;
using System.Linq;
using Gms.Portal.Web.Entities.DataContainer;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gms.Portal.Web.Helpers
{
    public class FormDataUnitToBsonDocumentConverter
    {
        public FormDataUnitToBsonDocumentConverter()
        {
        }

        public IEnumerable<BsonDocument> Convert(IQueryable<FormDataUnit> source)
        {
            var list = source.ToList();
            return Convert(list);
        }

        public IEnumerable<BsonDocument> Convert(IEnumerable<FormDataUnit> source)
        {
            foreach (var document in source)
            {
                var formDataUnit = Convert(document);
                yield return formDataUnit;
            }
        }

        public BsonDocument Convert(FormDataUnit source)
        {
            var document = new BsonDocument();

            foreach (var pair in source)
            {
                if (pair.Value is FormDataListRef)
                {
                    var listRef = (FormDataListRef)pair.Value;

                    var listRefDoc = new BsonDocument
                    {
                        {"FormID", listRef.FormID },
                        {"OwnerID", listRef.OwnerID },
                        {"ParentID", source.ID }
                    };

                    document[pair.Key] = listRefDoc;
                }
                else
                {
                    document[pair.Key] = BsonValue.Create(pair.Value);
                }
            }

            return document;
        }
    }
}