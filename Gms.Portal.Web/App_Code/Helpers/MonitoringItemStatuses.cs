using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Helpers
{
    public static class MonitoringItemStatuses
    {
        public const String Accepted = "დადასტურებული";
        public const String Rejected = "ხარვეზიანი";

        private static ISet<String> _statuses;

        static MonitoringItemStatuses()
        {
            _statuses = new HashSet<String>
            {
                Accepted,
                Rejected
            };
        }

        public static ISet<String> Statuses
        {
            get { return _statuses; }
        }
    }
}