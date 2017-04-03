using System;

namespace CITI.EVO.Tools.Utils
{
    public static class Hb8Util
    {
        public static TEntity GetObjectID<TEntity>(Object obj)
        {
            if (obj == null)
                return default(TEntity);

            var currentSession = Hb8Factory.GetCurrentSession();
            if (currentSession != null)
                return (TEntity)currentSession.GetIdentifier(obj);

            using (var newSession = Hb8Factory.CreateSession())
                return (TEntity)newSession.GetIdentifier(obj);
        }

        public static TEntity GetObject<TEntity>(Object ID)
        {
            if (ID == null)
                return default(TEntity);

            var currentSession = Hb8Factory.GetCurrentSession();
            if (currentSession != null)
                return currentSession.Get<TEntity>(ID);

            using (var newSession = Hb8Factory.CreateSession())
                return newSession.Get<TEntity>(ID);
        }
    }
}