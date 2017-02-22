using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class FormDataGridModel
    {
        public IList<FieldEntity> Fields { get; set; }

        public DictionaryDataView DataView { get; set; }
    }
}