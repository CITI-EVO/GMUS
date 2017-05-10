using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using NHibernate.Linq;

namespace Gms.Portal.Web.Utils
{
    public static class GmsCommonUtil
    {
        private static readonly IComparer<String> _comparer = StringLogicalComparer.DetectFloatNumber;

        public static int Compare(Object x, Object y)
        {
            var sx = Convert.ToString(x);
            sx = (sx ?? String.Empty);

            var sy = Convert.ToString(y);
            sy = (sy ?? String.Empty);

            return _comparer.Compare(sx, sy);
        }

        public static Guid? ExtractCollectionID(String dataSourceID)
        {
            return DataConverter.ToNullableGuid(dataSourceID);
        }

        public static IDictionary<Guid, int?> GetFormDeadlines()
        {
            var formDeadlines = CommonObjectCache.InitObject("FormDeadlines", CommonCacheStore.Request, LoadFormDeadlines);
            return formDeadlines;
        }

        private static IDictionary<Guid, int?> LoadFormDeadlines()
        {
            var session = Hb8Factory.InitSession();

            var query = (from n in session.Query<GM_Form>()
                         where n.DateDeleted == null
                         select new
                         {
                             n.ID,
                             n.ApprovalDeadline
                         });

            return query.ToDictionary(n => n.ID, n => n.ApprovalDeadline);
        }

        public static String ConvertToBase64(String text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        public static String ConvertFromBase64(String text)
        {
            var bytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(bytes);
        }

        public static DateTime? Merge(DateTime? date, DateTime? time)
        {
            var dateVal = date.GetValueOrDefault();
            var timeVal = time.GetValueOrDefault();

            var result = new DateTime(dateVal.Year, dateVal.Month, dateVal.Day, timeVal.Hour, timeVal.Minute, timeVal.Second);
            return result;
        }
    }
}