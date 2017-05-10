using System;
using System.Collections.Generic;
using CITI.EVO.Proxies;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.UserManagement.Svc.Contracts;
using Gms.Portal.DAL.Domain;

namespace Gms.Portal.Web.Caches
{
    public static class UmUsersCache
    {
        public static IList<UserContract> Users
        {
            get
            {
                var list = CommonObjectCache.InitObject("@UsersList", LoadUsersList);
                return list;
            }
        }

        public static UserContract GetUser(Guid? userID)
        {
            if (userID == null)
                return null;

            var dictionary = CommonObjectCache.InitObject("@UsersDict", LoadUsersDictionary);

            UserContract contract;
            if (!dictionary.TryGetValue(userID.Value, out contract))
                return null;

            return contract;
        }

        private static IDictionary<Guid, UserContract> LoadUsersDictionary()
        {
            var list = CommonObjectCache.InitObject("@UsersList", LoadUsersList);
            var dict = ConcurrencyHelper.CreateDictionary<Guid, UserContract>();

            foreach (var item in list)
                dict.Add(item.ID, item);

            return dict;
        }

        private static IList<UserContract> LoadUsersList()
        {
            var token = UmUtil.Instance.CurrentToken;
            if (token == null)
                return null;

            return UserManagementProxy.GetAllUsers(token.Value, true);
        }
    }
}