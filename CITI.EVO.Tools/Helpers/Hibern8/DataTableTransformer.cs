using System;
using System.Collections;
using System.Data;
using NHibernate.Transform;

namespace CITI.EVO.Tools.Helpers.Hibern8
{
    public class DataTableTransformer : IResultTransformer
    {
        private readonly DataTable _dataTable;
        private readonly DataTableTransformerMode _mode;

        private bool _dataTableInited;

        public DataTableTransformer(DataTable dataTable) : this(dataTable, DataTableTransformerMode.None)
        {
        }
        public DataTableTransformer(DataTable dataTable, DataTableTransformerMode mode)
        {
            _dataTable = dataTable;
            _mode = mode;
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            if (!_dataTableInited)
            {
                if (_mode.HasFlag(DataTableTransformerMode.InitDataTable))
                {
                    foreach (var name in aliases)
                    {
                        var dataColumn = _dataTable.Columns.Add(name);
                        dataColumn.AllowDBNull = true;
                    }
                }

                _dataTableInited = true;
            }

            if (_mode.HasFlag(DataTableTransformerMode.RecheckColumns))
            {
                foreach (var name in aliases)
                {
                    if (!_dataTable.Columns.Contains(name))
                    {
                        var dataColumn = _dataTable.Columns.Add(name);
                        dataColumn.AllowDBNull = true;
                    }
                }
            }

            var dataRow = _dataTable.NewRow();

            for (int i = 0; i < aliases.Length; i++)
            {
                var key = aliases[i];
                var value = tuple[i];

                dataRow[key] = (value ?? DBNull.Value);
            }

            if (_mode.HasFlag(DataTableTransformerMode.FillDataTable))
                _dataTable.Rows.Add(dataRow);

            return dataRow;
        }

        public IList TransformList(IList collection)
        {
            return collection;
        }
    }
}