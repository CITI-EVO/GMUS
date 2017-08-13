using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormStatusUnit
    {
        public Guid? UserID { get; set; }

        public Guid? StatusID { get; set; }

        public DateTime? DateOfAssigne { get; set; }

        public DateTime? DateOfStatus { get; set; }

        public IDictionary<String, Object> Params { get; set; }
    }
}