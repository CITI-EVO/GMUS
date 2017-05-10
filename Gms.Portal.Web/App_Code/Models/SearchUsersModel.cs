using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class SearchUsersModel
    {
        public ISet<Guid> Users { get; set; }
    }
}