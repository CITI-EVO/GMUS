using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class DataApproveGridFilterModel
    {
        public Guid? FormID { get; set; }

        public Guid? FieldID { get; set; }
    }
}