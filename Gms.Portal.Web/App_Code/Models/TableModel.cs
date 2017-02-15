using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class TableModel
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public String Status { get; set; }

        public List<ColumnModel> Columns { get; set; }
    }
}