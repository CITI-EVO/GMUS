using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using NHibernate.Linq;

namespace Gms.Portal.Web.Caches
{
    public static class DataStatusCache
    {
        private static GM_DataStatus _none;
        public static GM_DataStatus None
        {
            get
            {
                if (_none == null)
                    _none = GetStatus("None");

                return _none;
            }
        }

        private static GM_DataStatus _submit;
        public static GM_DataStatus Submit
        {
            get
            {
                if (_submit == null)
                    _submit = GetStatus("Submit");

                return _submit;
            }
        }

        private static GM_DataStatus _inProgress;
        public static GM_DataStatus InProgress
        {
            get
            {
                if (_inProgress == null)
                    _inProgress = GetStatus("InProgress");

                return _inProgress;
            }
        }

        private static GM_DataStatus _accepted;
        public static GM_DataStatus Accepted
        {
            get
            {
                if (_accepted == null)
                    _accepted = GetStatus("Accepted");

                return _accepted;
            }
        }

        private static GM_DataStatus _rejected;
        public static GM_DataStatus Rejected
        {
            get
            {
                if (_rejected == null)
                    _rejected = GetStatus("Rejected");

                return _rejected;
            }
        }

        private static GM_DataStatus _winner;
        public static GM_DataStatus Winner
        {
            get
            {
                if (_winner == null)
                    _winner = GetStatus("Winner");

                return _winner;
            }
        }

        public static IList<GM_DataStatus> Statuses
        {
            get
            {
                var list = CommonObjectCache.InitObject("@DataStatusList", LoadStatusList);
                return list;
            }
        }

        public static GM_DataStatus GetStatus(Object nameOrCodeOrID)
        {
            GM_DataStatus item;

            var statusID = DataConverter.ToNullableGuid(nameOrCodeOrID);
            if (statusID != null)
            {
                var statusByIdDict = CommonObjectCache.InitObject("@DataStatusByIdDict", LoadStatusByIdDict);
                if (statusByIdDict.TryGetValue(statusID.GetValueOrDefault(), out item))
                    return item;
            }

            var key = Convert.ToString(nameOrCodeOrID);

            var statusByCodeDict = CommonObjectCache.InitObject("@DataStatusByCodeDict", LoadStatusByCodeDict);
            if (!statusByCodeDict.TryGetValue(key, out item))
            {
                var statusByNameDict = CommonObjectCache.InitObject("@DataStatusByNameDict", LoadStatusByNameDict);
                if (!statusByNameDict.TryGetValue(key, out item))
                    return null;
            }

            return item;
        }

        private static IList<GM_DataStatus> LoadStatusList()
        {
            var session = Hb8Factory.InitSession();
            var query = session.Query<GM_DataStatus>().Where(n => n.DateDeleted == null);

            return query.ToList();
        }
        private static IDictionary<Guid, GM_DataStatus> LoadStatusByIdDict()
        {
            var list = CommonObjectCache.InitObject("@DataStatusList", LoadStatusList);
            var dict = ConcurrencyHelper.CreateDictionary<Guid, GM_DataStatus>();

            foreach (var item in list)
                dict.Add(item.ID, item);

            return dict;
        }
        private static IDictionary<String, GM_DataStatus> LoadStatusByNameDict()
        {
            var list = CommonObjectCache.InitObject("@DataStatusList", LoadStatusList);
            var dict = ConcurrencyHelper.CreateDictionary<String, GM_DataStatus>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in list)
                dict.Add(item.Name, item);

            return dict;
        }
        private static IDictionary<String, GM_DataStatus> LoadStatusByCodeDict()
        {
            var list = CommonObjectCache.InitObject("@DataStatusList", LoadStatusList);
            var dict = ConcurrencyHelper.CreateDictionary<String, GM_DataStatus>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in list)
                dict.Add(item.Code, item);

            return dict;
        }
    }
}