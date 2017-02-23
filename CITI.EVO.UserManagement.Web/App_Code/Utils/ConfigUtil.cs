using System;
using System.Configuration;
using System.Linq;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.UserManagement.Web.Utils
{
    public static class ConfigUtil
    {
        public static Guid[] UserRegisterGroupID
        {
            get
            {
                var value = ConfigurationManager.AppSettings["UserRegisterGroupID"];

                var array = (from n in value.Split(',')
                             let groupID = DataConverter.ToNullableGuid(n)
                             where groupID != null
                             select groupID.Value).ToArray();

                return array;
            }
        }

        public static bool UserRegisterEnabled
        {
            get
            {
                var value = ConfigurationManager.AppSettings["UserRegisterEnabled"];
                return DataConverter.ToNullableBoolean(value).GetValueOrDefault();
            }
        }

        public static bool UserActivationEnabled
        {
            get
            {
                var value = ConfigurationManager.AppSettings["UserActivationEnabled"];
                return DataConverter.ToNullableBoolean(value).GetValueOrDefault();
            }
        }

        public static bool UserRecoveryEnabled
        {
            get
            {
                var value = ConfigurationManager.AppSettings["UserRecoveryEnabled"];
                return DataConverter.ToNullableBoolean(value).GetValueOrDefault();
            }
        }
    }
}