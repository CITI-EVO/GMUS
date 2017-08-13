using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class FormApproveDataGridFilterModel
    {
        public Guid? FormID { get; set; }

        public String SourceField { get; set; }
    }
}