using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class ChooseTemplateModel
    {
        public Guid? RecordID { get; set; }

        public Guid? TemplateID { get; set; }
    }
}