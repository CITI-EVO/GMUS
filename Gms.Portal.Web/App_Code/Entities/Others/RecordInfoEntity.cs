using System;
using MongoDB.Bson;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class RecordInfoEntity
    {
        public BsonValue ID { get; set; }

        public BsonValue FormID { get; set; }

        public BsonValue UserID { get; set; }

        public BsonValue OwnerID { get; set; }

        public BsonValue IDNumber { get; set; }

        public BsonValue DateCreated { get; set; }

        public BsonValue DateOfSubmit { get; set; }

        public BsonValue UserStatuses { get; set; }

        public BsonValue FieldValue { get; set; }
    }
}