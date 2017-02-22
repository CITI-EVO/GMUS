using System;
using CITI.EVO.Tools.Security;

namespace Gms.Portal.Web.Utils
{
    public static class UserUtil
    {
        public static Guid? GetCurrentUserID()
        {
            var instance = UmUtil.Instance;
            if (!instance.IsLogged || instance.CurrentUser == null)
                return null;

            return instance.CurrentUser.ID;
        }

        public static bool IsSuperAdmin()
        {
            var instance = UmUtil.Instance;
            if (!instance.IsLogged || instance.CurrentUser == null)
                return false;

            return instance.CurrentUser.IsSuperAdmin;
        }
    }
}