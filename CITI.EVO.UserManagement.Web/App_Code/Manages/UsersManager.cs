using System;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.DAL.Domain;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Manages
{
    public static class UsersManager
    {
        public static UM_User GetUser(Guid userID)
        {
            using (var session = Hb8Factory.CreateSession())
            {
                return session.Query<UM_User>().FirstOrDefault(n => n.ID == userID);
            }
        }

        public static UM_User GetUser(String loginName)
        {
            loginName = (loginName ?? String.Empty);

            using (var session = Hb8Factory.CreateSession())
            {
                var user = (from n in session.Query<UM_User>()
                            where n.DateDeleted == null &&
                                  n.LoginName.Trim().ToLower() == loginName.Trim().ToLower()
                            select n).FirstOrDefault();

                return user;
            }
        }

        public static void UpdateUser(UM_User user)
        {
            if (user == null)
                return;

            using (var session = Hb8Factory.CreateSession())
            {
                var exUser = session.Query<UM_User>().FirstOrDefault(n => n.ID == user.ID);
                if (exUser == null)
                    return;

                exUser.ID = user.ID;
                exUser.LoginName = user.LoginName;
                exUser.Password = user.Password;
                exUser.PasswordExpirationDate = user.PasswordExpirationDate;
                exUser.FirstName = user.FirstName;
                exUser.LastName = user.LastName;
                exUser.IsActive = user.IsActive;
                exUser.IsSuperAdmin = user.IsSuperAdmin;
                exUser.Email = user.Email;
                exUser.Address = user.Address;
                exUser.DateChanged = user.DateChanged;
                exUser.DateCreated = user.DateCreated;
                exUser.DateDeleted = user.DateDeleted;

                session.SubmitUpdate(exUser);
            }
        }
    }
}