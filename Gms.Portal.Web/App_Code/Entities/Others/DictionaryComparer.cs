using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Comparers;

namespace Gms.Portal.Web.Entities.Others
{
    public class DictionaryComparer : IComparer<IDictionary<String, Object>>
    {
        private readonly StringComparer _ordinalComparer;
        private readonly StringLogicalComparer _logicalcomparer;

        private readonly IDictionary<String, String> _fields;

        public DictionaryComparer(IDictionary<String, String> fields)
        {
            _fields = fields;

            _ordinalComparer = StringComparer.OrdinalIgnoreCase;
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
                    if (_ordinalComparer.Equals(pair.Value, "desc"))
                        return -order;
                }
            }

            return 0;
        }
    }
}