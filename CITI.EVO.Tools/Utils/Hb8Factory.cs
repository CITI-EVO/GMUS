using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

namespace CITI.EVO.Tools.Utils
{
    public static class Hb8Factory
    {
        private const String SessionKey = "@{Hb8_Session}";

        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;

        public static Configuration Configuration
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                if (_configuration == null)
                {
                    var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    var path = Path.Combine(baseDir, "Hibernate.cfg.xml");

                    _configuration = new Configuration();
                    _configuration.Configure(path);
                }

                return _configuration;
            }
        }

        public static ISessionFactory Factory
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                if (_sessionFactory == null)
                    _sessionFactory = Configuration.BuildSessionFactory();

                return _sessionFactory;
            }
        }

        public static ISession InitSession()
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                var session = context.Items[SessionKey] as ISession;
                if (session == null)
                {
                    session = CreateSession();
                    context.Items[SessionKey] = session;
                }

                return session;
            }
            else
            {
                if (!CurrentSessionContext.HasBind(Factory))
                {
                    var newSession = CreateSession();
                    CurrentSessionContext.Bind(newSession);
                }

                var session = GetCurrentSession();
                return session;
            }

        }

        public static ISession CreateSession()
        {
            return Factory.OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            var context = HttpContext.Current;
            if (context != null)
                return context.Items[SessionKey] as ISession;

            return Factory.GetCurrentSession();
        }

        public static IStatelessSession OpenStateless()
        {
            return Factory.OpenStatelessSession();
        }

        public static void ReleaseSession()
        {
            ReleaseSession(true);
        }
        public static void ReleaseSession(bool commit)
        {
            using (var session = GetCurrentSession())
            {
                if (session == null)
                    return;

                if (!commit)
                    return;

                var transaction = session.Transaction;
                if (transaction == null)
                    return;

                if(!transaction.IsActive)
                    return;

                try
                {
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}