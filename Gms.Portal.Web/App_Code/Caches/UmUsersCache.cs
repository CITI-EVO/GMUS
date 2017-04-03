using System;
using System.Collections.Generic;
using CITI.EVO.Proxies;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.UserManagement.Svc.Contracts;
using Gms.Portal.DAL.Domain;

namespace Gms.Portal.Web.Caches
{
    public static class UmUsersCache
    {
        private static readonly Object _syncLock;

        private static IDictionary<Guid, UserContract> _usersByIdDict;

        static UmUsersCache()
        {
            _syncLock = new Object();
        }

        public static UserContract GetUser(Guid? userID)
        {
            if (userID == null)
                return null;

            Initialize();

            if (_usersByIdDict == null)
                return null;

            UserContract contract;
            if (!_usersByIdDict.TryGetValue(userID.Value, out contract))
                return null;

            return contract;
        }

        private static void Initialize()
        {
            lock (_syncLock)
            {
                var token = UmUtil.Instance.CurrentToken;
                if (token == null)
                    return;

                if (_usersByIdDict == null)
                {
                    _usersByIdDict = ConcurrencyHelper.CreateDictionary<Guid, UserContract>();

                    var users = UserManagementProxy.GetAllUsers(token.Value, true);
                    foreach (var userContract in users)
                        _usersByIdDict.Add(userContract.ID, userContract);
                }
            }
        }
    }
}