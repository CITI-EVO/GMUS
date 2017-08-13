using System;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class FormFieldInfoEntity
    {
        public Guid? FormID { get; set; }

        public String FormName { get; set; }

        public Guid? OwnerID { get; set; }

        public String OwnerName { get; set; }

        public Guid FieldID { get; set; }

        public String FieldName { get; set; }

        public String DataSource { get; set; }

    }
}