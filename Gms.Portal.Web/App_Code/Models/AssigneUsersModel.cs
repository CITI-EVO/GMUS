using System;
using System.Collections.Generic;
using System.Linq;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class AssigneUsersModel
    {
        public Guid? RecordID { get; set; }

        public ISet<Guid?> Users { get; set; }
    }
}