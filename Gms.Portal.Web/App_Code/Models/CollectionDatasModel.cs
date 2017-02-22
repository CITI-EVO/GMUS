using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Web.UI.Helpers;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class CollectionDatasModel
    {
        public Dictionary<Object, String> Fields { get; set; }
        public DictionaryDataView DataView { get; set; }
    }
}