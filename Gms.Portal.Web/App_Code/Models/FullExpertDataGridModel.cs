using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class FullExpertDataGridModel
    {
        public DictionaryDataView DataView { get; set; }

        public IDictionary<Guid?, RatingEntity> Entities { get; set; }
    }
}