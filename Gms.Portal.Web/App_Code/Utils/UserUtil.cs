using System;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Svc.Contracts;
using Gms.Portal.Web.Caches;

namespace Gms.Portal.Web.Utils
{
    public static class UserUtil
    {
        public static String GetUserStatus(Guid? userID)
        {
            var umUser = UmUsersCache.GetUser(userID);
            return GetUserStatus(umUser);
        }
        public static String GetUserStatus(UserContract userContract)
        {
            if (userContract == null)
                return "Not Found";

            if (!userContract.IsActive)
                return "Passive";

            if (userContract.DateDeleted != null)
                return "Deleted";

            return "Active";
        }

        public static Guid? GetMandatoryFormID()
        {
            var instance = UmUtil.Instance;
            if (!instance.IsLogged || instance.CurrentUser == null)
                return null;

            var attributes = instance.CurrentUserAttributes;
            if (attributes == null)
                return null;

            var mandatoryFormID = DataConverter.ToNullableGuid(attributes.GetValueOrDefault("MandatoryFormID"));
            return mandatoryFormID;
        }
        public static Guid? GetMandatoryFormStatus()
        {
            var instance = UmUtil.Instance;
            if (!instance.IsLogged || instance.CurrentUser == null)
                return null;

            var attributes = instance.CurrentUserAttributes;
            if (attributes == null)
                return null;

            var mandatoryFormID = DataConverter.ToNullableGuid(attributes.GetValueOrDefault("MandatoryFormStatus"));
            return mandatoryFormID;
        }
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