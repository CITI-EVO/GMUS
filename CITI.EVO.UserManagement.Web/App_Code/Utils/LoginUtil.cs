using System;
using System.Linq;
using System.Web;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.DAL.Domain;
using NHibernate.Linq;


namespace CITI.EVO.UserManagement.Web.Utils
{
    public class LoginUtil
    {
        private const String sessionKey = "$current_user_usermanagement$";

        public static UM_User CurrentUser
        {
            get
            {
                return HttpContext.Current.Session[sessionKey] as UM_User;
            }
            private set
            {
                HttpContext.Current.Session[sessionKey] = value;
            }
        }

        public static bool Login(String loginName, String password)
        {
            if (String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(loginName))
            {
                return false;
            }

            loginName = loginName.Trim().ToLower();


            using (var session = Hb8Factory.CreateSession())
            {
                CurrentUser = session.Query<UM_User>().FirstOrDefault(n => n.LoginName == loginName &&
                                                                          n.Password == password &&
                                                                          n.IsActive &&
                                                                          n.IsSuperAdmin &&
                                                                          n.DateDeleted == null);

                return (CurrentUser != null);
            }
        }



        public static bool HasAccessLevel()
        {
            if (CurrentUser == null)
            {
                return false;
            }

            return CurrentUser.GroupUsers.Any(n => n.AccessLevel > 0);

        }

        public static bool IsLogged()
        {
            return CurrentUser != null;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}