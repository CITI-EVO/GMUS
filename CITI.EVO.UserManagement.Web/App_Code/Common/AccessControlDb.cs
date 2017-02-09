using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using CITI.EVO.Tools.Utils;

using CITI.EVO.Tools.Extensions;
using CITI.EVO.UserManagement.DAL.Domain;
using NHibernate;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Common
{
    public class AccessControlDb : IAccessController
    {
        private readonly IDictionary<Guid, Object> lockersDict = new Dictionary<Guid, Object>();

        private TimeSpan? expireTime;
        public TimeSpan ExpireTime
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                if (expireTime == null)
                {
                    var loginExpireTime = DataConverter.ToNullableInt(ConfigurationManager.AppSettings["LoginExpireTime"]);
                    if (loginExpireTime != null)
                        expireTime = TimeSpan.FromMinutes(loginExpireTime.Value);
                    else
                        expireTime = TimeSpan.FromMinutes(30);
                }

                return expireTime.Value;
            }
        }

        public Guid CreateUserToken(Guid userID)
        {
            lock (lockersDict)
            {
                using (var session = Hb8Factory.CreateSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var entity = (from n in session.Query<UM_LoginToken>()
                                      where n.UserID == userID &&
                                            n.DateDeleted == null
                                      orderby n.DateCreated descending
                                      select n).FirstOrDefault();

                        if (entity == null || CheckExpiration(entity, session))
                        {
                            entity = new UM_LoginToken
                            {
                                ID = Guid.NewGuid(),
                                DateCreated = DateTime.Now,
                                UserID = userID,
                                LoginToken = Guid.NewGuid(),
                            };

                            session.Save(entity);

                            SetExpiration(entity, session);
                            GetLocker(entity.LoginToken);
                        }

                        transaction.Commit();

                        return entity.LoginToken;
                    }
                }
            }
        }

        public void ReleaseUserToken(Guid token)
        {
            var locker = GetLocker(token);
            lock (locker)
            {
                using (var session = Hb8Factory.CreateSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var entity = (from n in session.Query<UM_LoginToken>()
                                      where n.DateDeleted == null &&
                                            n.LoginToken == token
                                      select n).FirstOrDefault();

                        if (entity != null)
                        {
                            entity.DateDeleted = DateTime.Now;
                            entity.DeleteReason = 1;

                            session.Update(entity);

                            RemoveLocker(token);
                        }

                        transaction.Commit();
                    }
                }
            }
        }

        public Guid? GetTokenOwnerID(Guid token)
        {
            var locker = GetLocker(token);
            lock (locker)
            {
                using (var session = Hb8Factory.CreateSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var entity = (from n in session.Query<UM_LoginToken>()
                                      where n.DateDeleted == null &&
                                            n.LoginToken == token
                                      select n).FirstOrDefault();

                        var user = (Guid?)null;

                        if (!CheckExpiration(entity, session))
                        {
                            if (entity != null)
                                user = entity.UserID;
                        }

                        transaction.Commit();

                        return user;
                    }
                }
            }
        }

        public IDictionary<Guid, Guid?> GetTokensOwners()
        {
            lock (lockersDict)
            {
                using (var session = Hb8Factory.CreateSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var entities = (from n in session.Query<UM_LoginToken>()
                                        where n.DateDeleted == null
                                        select n).ToList();

                        var dict = new Dictionary<Guid, Guid?>(entities.Count);

                        foreach (var entity in entities)
                        {
                            if (!CheckExpiration(entity, session))
                                dict.Add(entity.ID, entity.UserID);
                        }

                        transaction.Commit();

                        return dict;
                    }
                }
            }
        }

        public bool ValidateToken(Guid token)
        {
            var locker = GetLocker(token);
            lock (locker)
            {
                using (var session = Hb8Factory.CreateSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var entity = (from n in session.Query<UM_LoginToken>()
                                      where n.DateDeleted == null &&
                                            n.LoginToken == token
                                      select n).FirstOrDefault();

                        var result = !CheckExpiration(entity, session);
                        if (result)
                            result = (entity != null);

                        transaction.Commit();

                        return result;
                    }
                }
            }
        }

        private bool CheckExpiration(UM_LoginToken entity, ISession session)
        {
            if (entity == null)
                return false;

            if (entity.ExpireDate != null)
            {
                if (entity.ExpireDate < DateTime.Now)
                {
                    entity.DateDeleted = DateTime.Now;
                    entity.DeleteReason = 2;

                    RemoveLocker(entity.LoginToken);

                    session.Update(entity);
                    return true;
                }
            }

            SetExpiration(entity, session);

            return false;
        }

        private void SetExpiration(UM_LoginToken entity, ISession session)
        {
            if (entity == null)
                return;

            var currentDate = DateTime.Now;
            entity.LastAccessDate = currentDate;
            entity.ExpireDate = currentDate.Add(ExpireTime);

            session.Update(entity);
        }

        private Object GetLocker(Guid token)
        {
            lock (lockersDict)
            {
                var locker = lockersDict.GetValueOrDefault(token);
                if (locker == null)
                {
                    locker = new Object();
                    lockersDict.Add(token, locker);
                }

                return locker;
            }
        }

        private void RemoveLocker(Guid token)
        {
            lock (lockersDict)
                lockersDict.Remove(token);
        }
    }
}