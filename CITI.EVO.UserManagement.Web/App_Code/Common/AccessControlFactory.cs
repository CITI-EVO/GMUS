using System;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace CITI.EVO.UserManagement.Web.Common
{
    public static class AccessControlFactory
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IAccessController GetAccessController()
        {
            var accessControllerName = ConfigurationManager.AppSettings["AccessController"];
            if (String.IsNullOrWhiteSpace(accessControllerName))
            {
                accessControllerName = typeof(AccessControlDb).FullName;
            }

            var accessControllerType = Type.GetType(accessControllerName);
            if (accessControllerType == null)
            {
                throw new Exception("Invalid Access Controler Name");
            }

            var accessController = Activator.CreateInstance(accessControllerType) as IAccessController;
            if (accessController == null)
            {
                throw new Exception("Invalid Access Controler Type");
            }

            return accessController;
        }
    }
}