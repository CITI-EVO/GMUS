using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Utils
{
    public static class FormDataUtil
    {
        private static readonly IComparer<byte[]> _bytesComparer = new ByteArrayComparer();
        private static readonly IComparer<String> _ordinalComparer = StringComparer.OrdinalIgnoreCase;

        public static bool IsNullOrEmpty(Object fieldValue)
        {
            if (ReferenceEquals(fieldValue, DBNull.Value))
                return true;

            var str = Convert.ToString(fieldValue);
            return String.IsNullOrWhiteSpace(str);
        }

        public static FormDataUnit Transform(FormDataUnit source, IDictionary<String, String> names)
        {
            var target = new FormDataUnit();

            foreach (var pair in source)
            {
                var key = names.GetValueOrDefault(pair.Key) ?? pair.Key;
                target[key] = pair.Value;
            }

            return target;
        }
        public static IDictionary<String, Object> Transform(IDictionary<String, Object> source, IDictionary<String, String> names)
        {
            var target = new Dictionary<String, Object>(source.Count);

            foreach (var pair in source)
            {
                var key = names.GetValueOrDefault(pair.Key, pair.Key);
                target[key] = pair.Value;
            }

            return target;
        }

        public static bool MergeAndEquals(FormDataBase x, FormDataBase y)
        {
            return MergeAndCompare(x, y) == 0;
        }
        public static bool MergeAndEquals(FormDataBase x, FormDataBase y, ISet<String> @set)
        {
            return MergeAndCompare(x, y, @set) == 0;
        }

        public static int MergeAndCompare(FormDataBase x, FormDataBase y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x != null && y == null)
                return 1;

            if (x == null && y != null)
                return -1;

            var @set = new HashSet<String>();
            @set.UnionWith(x.Keys);
            @set.UnionWith(y.Keys);

            return MergeAndCompare(x, y, @set);
        }
        public static int MergeAndCompare(FormDataBase x, FormDataBase y, ISet<String> @set)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x != null && y == null)
                return 1;

            if (x == null && y != null)
                return -1;

            MergeFormDatas(x, y, @set);

            return FormDataCompare(x, y, @set);
        }

        public static bool FormItemEquals(Object xVal, Object yVal)
        {
            return FormItemCompare(xVal, yVal) == 0;
        }
        public static int FormItemCompare(Object xVal, Object yVal)
        {
            if (ReferenceEquals(xVal, yVal))
                return 0;

            if (Equals(xVal, yVal))
                return 0;

            if (xVal == null && yVal == null)
                return 0;

            if (xVal != null && yVal == null)
                return 1;

            if (xVal == null && yVal != null)
                return -1;

            if (xVal is FormDataListRef)
                return 1;

            if (yVal is FormDataListRef)
                return -1;

            if (xVal is FormDataBinary && yVal is FormDataBinary)
            {
                var xBin = (FormDataBinary)xVal;
                var yBin = (FormDataBinary)yVal;

                var order = _ordinalComparer.Compare(xBin.FileName, yBin.FileName);
                if (order == 0)
                    order = _bytesComparer.Compare(xBin.FileBytes, yBin.FileBytes);

                return order;
            }

            if (xVal is ICollection && yVal is ICollection)
            {
                var xColl = (IEnumerable)xVal;
                var yColl = (IEnumerable)yVal;

                var xList = ConvertToList(xColl);
                var yList = ConvertToList(yColl);

                var order = xList.Count.CompareTo(yList.Count);
                if (order != 0)
                    return order;

                var count = Math.Min(xList.Count, yList.Count);
                for (var i = 0; i < count; i++)
                {
                    order = _ordinalComparer.Compare(xList[i], yList[i]);
                    if (order != 0)
                        return order;
                }
            }

            if (xVal is FormDataBinary)
                return 1;

            if (yVal is FormDataBinary)
                return -1;

            var xs = Convert.ToString(xVal);
            var ys = Convert.ToString(yVal);

            var result = _ordinalComparer.Compare(xs, ys);
            return result;
        }

        private static IList<String> ConvertToList(IEnumerable collection)
        {
            var list = new List<String>();

            foreach (var item in collection)
            {
                var val = Convert.ToString(item);

                var index = list.BinarySearch(val, _ordinalComparer);
                if (index < 0)
                    index = ~index;

                list.Insert(index, val);
            }

            return list;
        }

        public static bool FormDataEquals(FormDataBase x, FormDataBase y)
        {
            return FormDataCompare(x, y) == 0;
        }
        public static bool FormDataEquals(FormDataBase x, FormDataBase y, ISet<String> @set)
        {
            return FormDataCompare(x, y, @set) == 0;
        }

        public static int FormDataCompare(FormDataBase x, FormDataBase y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x != null && y == null)
                return 1;

            if (x == null && y != null)
                return -1;

            var @set = new HashSet<String>();
            @set.UnionWith(x.Keys);
            @set.UnionWith(y.Keys);

            return FormDataCompare(x, y, @set);
        }
        public static int FormDataCompare(FormDataBase x, FormDataBase y, ISet<String> @set)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x != null && y == null)
                return 1;

            if (x == null && y != null)
                return -1;

            var order = 0;

            foreach (var key in @set)
            {
                var xVal = x[key];
                var yVal = y[key];

                xVal = (ReferenceEquals(xVal, DBNull.Value) ? null : xVal);
                yVal = (ReferenceEquals(yVal, DBNull.Value) ? null : yVal);

                if (xVal is FormDataListRef || yVal is FormDataListRef)
                {
                    //var xRef = (FormDataListRef)xVal;
                    //var yRef = (FormDataListRef)yVal;

                    //var xList = new FormDataLazyList(xRef);
                    //var xDict = xList.ToDictionary(n => n.ID);

                    //var yList = new FormDataLazyList(yRef);

                    //var xIds = xList.Select(n => n.ID);
                    //var yIds = yList.Select(n => n.ID);

                    //var xs = yIds.ToHashSet();

                    //foreach (var recID in xIds)
                    //{

                    //}
                }
                else
                {
                    order = FormItemCompare(xVal, yVal);
                    if (order != 0)
                        break;
                }
            }

            return order;
        }

        public static void MergeFormDatas(FormDataBase target, FormDataBase source)
        {
            if (target == null || source == null)
                return;

            var allKeys = new HashSet<String>();
            allKeys.UnionWith(target.Keys);
            allKeys.UnionWith(source.Keys);

            MergeFormDatas(target, source, allKeys);
        }
        public static void MergeFormDatas(FormDataBase target, FormDataBase source, ISet<String> fields)
        {
            if (target == null || source == null)
                return;

            foreach (var field in fields)
            {
                var newValue = target[field];
                if (!ReferenceEquals(newValue, DBNull.Value))
                    continue;

                var oldValue = source[field];
                if (ReferenceEquals(oldValue, DBNull.Value))
                    oldValue = null;

                target[field] = oldValue;
            }
        }

        public static bool IsGridOrTreeMissing(FormDataUnit formData, FormEntity formEntity)
        {
            var controls = FormStructureUtil.PreOrderFirstLevelTraversal(formEntity);

            var query = (from n in controls
                         where n is GridEntity || n is TreeEntity
                         let key = Convert.ToString(n.ID)
                         where !formData.ContainsKey(key)
                         select n);

            var missing = query.Any();
            return missing;
        }

        public static int CompareFormStatusDates(FormStatusUnit x, FormStatusUnit y)
        {
            if (x == null && y != null)
                return 1;

            if (x != null && y == null)
                return -1;

            if (x == null && y == null)
                return 0;

            if (x.DateOfStatus == null && y.DateOfStatus != null)
                return 1;

            if (x.DateOfStatus != null && y.DateOfStatus == null)
                return -1;

            if (x.DateOfStatus == null && y.DateOfStatus == null)
                return 0;

            var xDate = x.DateOfStatus.Value;
            var yDate = y.DateOfStatus.Value;

            return xDate.CompareTo(yDate);
        }

        public static IEnumerable<FormDataBase> MegreFormDatasFields
        (
            ContentEntity entity,
            ISet<String> treeAndGrids,
            ISet<String> subListsFields,
            FormDataUnit formData
        )
        {
            var allControls = FormStructureUtil.PreOrderFirstLevelTraversal(entity);

            var treesAndGridsQuery = from n in allControls
                                     where treeAndGrids.Contains(n.Name) ||
                                           treeAndGrids.Contains(n.Alias)
                                     from m in FormStructureUtil.PreOrderTraversal(n)
                                     select m;

            var queryForms = (from n in treesAndGridsQuery
                              let k = Convert.ToString(n.ID)
                              let d = formData[k]
                              select d);

            var list = new List<FormDataListBase>();

            foreach (var queryForm in queryForms)
            {
                var formDataList = queryForm as FormDataListBase;
                if (formDataList != null)
                    list.Add(formDataList);

                var listRef = queryForm as FormDataListRef;
                if (listRef != null)
                {
                    var lazyList = new FormDataLazyList(listRef);
                    list.Add(lazyList);
                }
            }

            return MegreFormDatasFields(entity, treeAndGrids, subListsFields, list);
        }

        public static IEnumerable<FormDataBase> MegreFormDatasFields
        (
            ContentEntity entity,
            ISet<String> treeAndGrids,
            ISet<String> subListsFields,
            IEnumerable<FormDataListBase> formDatas
        )
        {
            var collections = formDatas.Cast<IEnumerable<FormDataBase>>();
            return MegreFormDatasFields(entity, treeAndGrids, subListsFields, collections);
        }

        public static IEnumerable<FormDataBase> MegreFormDatasFields
        (
            ContentEntity entity,
            ISet<String> treeAndGrids,
            ISet<String> subListsFields,
            IEnumerable<IEnumerable<FormDataBase>> allFormDatas
        )
        {
            var allControls = FormStructureUtil.PreOrderFirstLevelTraversal(entity);

            var treesAndGridsQuery = from n in allControls
                                     where treeAndGrids.Contains(n.Name) ||
                                           treeAndGrids.Contains(n.Alias)
                                     from m in FormStructureUtil.PreOrderTraversal(n)
                                     select m;

            var treesAndGridsNamesQuery = from n in treesAndGridsQuery
                                          select new
                                          {
                                              Key = n.Name,
                                              Field = n
                                          };

            var treesAndGridsAliasQuery = from n in treesAndGridsQuery
                                          select new
                                          {
                                              Key = n.Alias,
                                              Field = n
                                          };

            var allFields = treesAndGridsNamesQuery.Union(treesAndGridsAliasQuery);

            var controlLp = allFields.ToLookup(n => n.Key, n => n.Field);

            foreach (var formDatas in allFormDatas)
            {
                foreach (var formData in formDatas)
                {
                    var result = new FormDataBase();

                    foreach (var fieldName in subListsFields)
                    {
                        var valQuery = (from n in controlLp[fieldName]
                                        let k = Convert.ToString(n.ID)
                                        where formData.ContainsKey(k)
                                        select formData[k]);

                        result[fieldName] = valQuery.FirstOrDefault();
                    }

                    yield return result;
                }
            }
        }
    }
}