using System;
using System.IO;
using System.Runtime.CompilerServices;
using NHibernate;
using NHibernate.Cfg;

namespace CITI.EVO.Tools.Utils
{
    public static class Hb8Factory
    {
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

        public static ISession CreateSession()
        {
            return Factory.OpenSession();
        }

        public static IStatelessSession CreateStateless()
        {
            var configuration = new Configuration();
            configuration.Configure();

            return Factory.OpenStatelessSession();
        }
    }
}