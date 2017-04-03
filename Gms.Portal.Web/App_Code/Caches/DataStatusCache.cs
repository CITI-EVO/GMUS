using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using NHibernate.Linq;

namespace Gms.Portal.Web.Caches
{
    public static class DataStatusCache
    {
        private static readonly Object _syncLock;

        private static DateTime? _lastUpdate;

        private static IList<GM_DataStatus> _statusesList;
        private static IDictionary<Guid, GM_DataStatus> _statusesByIdDict;
        private static IDictionary<String, GM_DataStatus> _statusesByNameDict;
        private static IDictionary<String, GM_DataStatus> _statusesByCodeDict;

        static DataStatusCache()
        {
            _syncLock = new Object();
        }

        private static GM_DataStatus _none;
        public static GM_DataStatus None
        {
            get
            {
                lock (_syncLock)
                {
                    if (_none == null)
                        _none = GetStatus("None");

                    return _none;
                }
            }
        }

        private static GM_DataStatus _submit;
        public static GM_DataStatus Submit
        {
            get
            {
                lock (_syncLock)
                {
                    if (_submit == null)
                        _submit = GetStatus("Submit");

                    return _submit;
                }
            }
        }

        private static GM_DataStatus _accepted;
        public static GM_DataStatus Accepted
        {
            get
            {
                lock (_syncLock)
                {
                    if (_accepted == null)
                        _accepted = GetStatus("Accepted");

                    return _accepted;
                }
            }
        }

        private static GM_DataStatus _rejected;
        public static GM_DataStatus Rejected
        {
            get
            {
                lock (_syncLock)
                {
                    if (_rejected == null)
                        _rejected = GetStatus("Rejected");

                    return _rejected;
                }
            }
        }

        public static IList<GM_DataStatus> Statuses
        {
            get
            {
                Initialize();
                return _statusesList.ToList();
            }
        }

        public static GM_DataStatus GetStatus(Object nameOrCodeOrID)
        {
            Initialize();

            GM_DataStatus item;

            var statusID = DataConverter.ToNullableGuid(nameOrCodeOrID);
            if (statusID != null)
            {
                if (_statusesByIdDict.TryGetValue(statusID.GetValueOrDefault(), out item))
                    return item;
            }

            var key = Convert.ToString(nameOrCodeOrID);

            if (!_statusesByCodeDict.TryGetValue(key, out item))
            {
                if (!_statusesByNameDict.TryGetValue(key, out item))
                    return null;
            }

            return item;
        }

        private static void Initialize()
        {
            lock (_syncLock)
            {
                if (_lastUpdate != null)
                {
                    var elapsed = DateTime.Now - _lastUpdate.Value;
                    if (elapsed.TotalMinutes < 5D)
                        return;
                }

                var comparer = StringComparer.OrdinalIgnoreCase;

                _statusesList = (_statusesList ?? new List<GM_DataStatus>());
                _statusesByIdDict = (_statusesByIdDict ?? new ConcurrentDictionary<Guid, GM_DataStatus>());
                _statusesByNameDict = (_statusesByNameDict ?? new ConcurrentDictionary<String, GM_DataStatus>(comparer));
                _statusesByCodeDict = (_statusesByCodeDict ?? new ConcurrentDictionary<String, GM_DataStatus>(comparer));

                using (var session = Hb8Factory.CreateSession())
                {
                    _statusesList.Clear();
                    _statusesByIdDict.Clear();
                    _statusesByNameDict.Clear();
                    _statusesByCodeDict.Clear();

                    var query = session.Query<GM_DataStatus>().Where(n => n.DateDeleted == null);
                    foreach (var item in query)
                    {
                        _statusesList.Add(item);
                        _statusesByIdDict.Add(item.ID, item);
                        _statusesByNameDict.Add(item.Name, item);
                        _statusesByCodeDict.Add(item.Code, item);
                    }
                }

                _lastUpdate = DateTime.Now;
            }
        }
    }
}