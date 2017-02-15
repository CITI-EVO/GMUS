using System;
using System.Configuration;

namespace Gms.Portal.Web.Utils
{
    public static class ConfigUtil
    {
        public static String MongoConnectionString
        {
            get
            {
                var connectinString = ConfigurationManager.ConnectionStrings["MongoConnectionString"];
                return connectinString.ConnectionString;
            }
        }

        public static String MongoDatabaseName
        {
            get
            {
                var databaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];
                return databaseName;
            }
        }
    }
}