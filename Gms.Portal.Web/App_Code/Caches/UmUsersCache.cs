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
            {
                //lock (dictionary)
                //{
                //    contract = LoadUser(userID.Value);
                //    dictionary[userID.Value] = contract;
                //}

                lock (dictionary)
                {
                    var users = LoadUsersList();

                    foreach (var user in users)
                    {
                        if (user == null)
                            continue;

                        if (user.ID == userID)
                            contract = user;
                        else
                            dictionary[user.ID] = user;
                    }

                    dictionary[userID.Value] = contract;
                }
            }

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

        private static UserContract LoadUser(Guid userID)
        {
            var token = UmUtil.Instance.CurrentToken;
            if (token == null)
                return null;

            return UserManagementProxy.GetUser(token.Value, userID);
        }
    }
}