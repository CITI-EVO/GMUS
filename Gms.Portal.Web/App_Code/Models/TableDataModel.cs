using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class TableDataModel
    {
        public FormModel Form { get; set; }

        public LogicModel Logic { get; set; }
    }
}