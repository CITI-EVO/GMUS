using System;
using CITI.EVO.Tools.Web.UI.Helpers;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class FormApproveDataGridModel
    {
        public Guid? FormID { get; set; }

        public String SourceField { get; set; }

        public DictionaryDataView DataView { get; set; }
    }
}