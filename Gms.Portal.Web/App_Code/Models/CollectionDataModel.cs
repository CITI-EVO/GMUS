using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class CollectionDataModel
    {
        public Guid? ID { get; set; }

        public Dictionary<String, Object> Data { get; set; }
    }
}