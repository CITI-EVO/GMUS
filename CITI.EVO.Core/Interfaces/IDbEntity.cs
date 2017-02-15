using System;

namespace CITI.EVO.Core.Interfaces
{
    public interface IDbEntity
    {
        Guid ID { get; set; }

        DateTime DateCreated { get; set; }
        DateTime? DateChanged { get; set; }
        DateTime? DateDeleted { get; set; }
    }
}
