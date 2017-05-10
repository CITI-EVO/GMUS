using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Enums;

namespace CITI.EVO.Tools.Comparers
{
    public class DictionaryComparer : IComparer<IDictionary<String, Object>>
    {
        private readonly StringLogicalComparer _logicalcomparer;

        private readonly IDictionary<String, SortOrder> _fields;

        public DictionaryComparer(IDictionary<String, SortOrder> fields)
        {
            _fields = fields;
            _logicalcomparer = new StringLogicalComparer();
        }

        public int Compare(IDictionary<String, Object> x, IDictionary<String, Object> y)
        {
            if (x == null && y == null)
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return 1;

            foreach (var pair in _fields)
            {
                var xVal = Convert.ToString(x[pair.Key]);
                var yVal = Convert.ToString(y[pair.Key]);

                var order = _logicalcomparer.Compare(xVal, yVal);
                if (order != 0)
                {
                    if (pair.Value == SortOrder.Desc)
                        order = -order;

                    return order;
                }
            }

            return 0;
        }
    }

}
