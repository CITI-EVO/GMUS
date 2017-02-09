using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITI.EVO.UserManagement.DAL.Interfaces
{
    public interface IDbEntity
    {
        Guid ID { get; set; }

        DateTime DateCreated { get; set; }
        DateTime? DateChanged { get; set; }
        DateTime? DateDeleted { get; set; }
    }
}
