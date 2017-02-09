using System;
using System.Collections.Generic;

namespace CITI.EVO.UserManagement.Web.Common
{
    public interface IAccessController
    {
        TimeSpan ExpireTime { get; }

        Guid CreateUserToken(Guid userID);

        void ReleaseUserToken(Guid token);

        bool ValidateToken(Guid token);

        Guid? GetTokenOwnerID(Guid token);

        IDictionary<Guid, Guid?> GetTokensOwners();
    }
}