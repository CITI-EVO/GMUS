using System;
using NHibernate;

namespace CITI.EVO.Tools.Extensions
{
    public static class Hb8Extensions
    {
        public static void SubmitChanges(this ISession session, Object item)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Merge(item);
                transaction.Commit();
            }
        }

        public static void SubmitInsert(this ISession session, Object item)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(item);
                transaction.Commit();
            }
        }

        public static void SubmitUpdate(this ISession session, Object item)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(item);
                transaction.Commit();
            }
        }
    }
}