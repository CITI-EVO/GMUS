using System;
using System.Linq;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class AssigneUsersModel
    {
        public Guid? RecordID { get; set; }

        public ILookup<int?, Guid?> Users { get; set; }
    }
}