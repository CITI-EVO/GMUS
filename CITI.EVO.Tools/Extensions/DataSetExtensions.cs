using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using CITI.EVO.Tools.Helpers;

namespace CITI.EVO.Tools.Extensions
{
    public static class DataSetExtensions
    {
        public static DataSet ToDataSet(this IEnumerable items)
        {
            var itemsDict = new Dictionary<Type, IList>();

            foreach (var item in items)
            {
                if (item == null)
                {
                    throw new NullReferenceException();
                }

                var type = item.GetType();
                if (type.IsValueType)
                {
                    throw new InvalidOperationException();
                }

                IList list;

                if (!itemsDict.TryGetValue(type, out list))
                {
                    list = new ArrayList();
                    itemsDict.Add(type, list);
                }

                list.Add(item);
            }

            var dataSet = new DataSet();

            foreach (var pair in itemsDict)
            {
                var itemsType = pair.Key;
                var collection = pair.Value;

                var dataTable = collection.ToDataTable(itemsType);

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }

        public static DataTable ToDataTable<TItem>(this IEnumerable<TItem> items)
        {
            var itemType = typeof(TItem);
            return items.ToDataTable(itemType);
        }

        public static DataTable ToDataTable(this IEnumerable items, Type type)
        {
            var propertiesArr = type.GetProperties();

            var propertiesList = new List<PropertyInfo>();
            foreach (var propertyInfo in propertiesArr)
            {
                if (!propertyInfo.CanRead)
                {
                    continue;
                }

                var getterMethod = propertyInfo.GetGetMethod(false);
                if (getterMethod == null)
                {
                    continue;
                }

                propertiesList.Add(propertyInfo);
            }

            var tableName = GetTableName(type);
            var dataTable = new DataTable(tableName);

            var mapping = new Dictionary<String, String>();

            foreach (var propertyInfo in propertiesList)
            {
                var propertyType = propertyInfo.PropertyType;
                var columnName = GetColumnName(propertyInfo);

                var dataColumn = new DataColumn(columnName);

                dataColumn.DataType = propertyType;
                dataColumn.AllowDBNull = (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>));

                if (dataColumn.AllowDBNull)
                {
                    dataColumn.DataType = Nullable.GetUnderlyingType(propertyType);
                }

                dataTable.Columns.Add(dataColumn);

                mapping[propertyInfo.Name] = columnName;
            }

            foreach (var item in items)
            {
                var dataRow = dataTable.NewRow();

                foreach (var propertyInfo in propertiesList)
                {
                    var columnName = mapping[propertyInfo.Name];
                    var propertyValue = propertyInfo.GetValue(item, null);

                    dataRow[columnName] = propertyValue;
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }



        public static TObject ToObject<TObject>(this DataRow dataRow, IDictionary<String, String> mapping)
        {
            return (TObject)dataRow.ToObject(typeof(TObject), mapping, null);
        }
        public static TObject ToObject<TObject>(this DataRow dataRow, IDictionary<String, String> mapping, Func<Object, String, String, Object> valueHandler)
        {
            return (TObject)dataRow.ToObject(typeof(TObject), mapping, valueHandler);
        }

        public static Object ToObject(this DataRow dataRow, Type type, IDictionary<String, String> mapping)
        {
            return dataRow.ToObject(type, mapping, null);
        }
        public static Object ToObject(this DataRow dataRow, Type type, IDictionary<String, String> mapping, Func<Object, String, String, Object> valueHandler)
        {
            var newObj = Activator.CreateInstance(type);

            foreach (var pair in mapping)
            {
                var propertyInfo = type.GetProperty(pair.Key);
                if (propertyInfo == null)
                {
                    throw new Exception(String.Format("{0}", pair.Key));
                }

                var dataColumn = dataRow.Table.Columns[pair.Value];
                if (dataColumn == null)
                {
                    throw new Exception(String.Format("{0}", pair.Value));
                }

                var value = dataRow.Field<Object>(dataColumn);
                if (valueHandler != null)
                {
                    value = valueHandler(value, pair.Key, pair.Value);
                }

                propertyInfo.SetValue(newObj, value, null);
            }

            return newObj;
        }

        public static IEnumerable<TObject> ToObjects<TObject>(this DataTable dataTable, IDictionary<String, String> mapping)
        {
            return dataTable.ToObjects<TObject>(mapping, null);
        }
        public static IEnumerable<TObject> ToObjects<TObject>(this DataTable dataTable, IDictionary<String, String> mapping, Func<Object, String, String, Object> valueHandler)
        {
            var collection = dataTable.ToObjects(typeof(TObject), mapping, valueHandler);
            return collection.Cast<TObject>();
        }

        public static IEnumerable ToObjects(this DataTable dataTable, Type type, IDictionary<String, String> mapping)
        {
            return dataTable.ToObjects(type, mapping, null);
        }
        public static IEnumerable ToObjects(this DataTable dataTable, Type type, IDictionary<String, String> mapping, Func<Object, String, String, Object> valueHandler)
        {
            var collection = dataTable.AsEnumerable().Select(row => row.ToObject(type, mapping, valueHandler));
            return collection;
        }



        public static void WriteDatabase(this DataSet dataSet, String connectionString)
        {
            dataSet.WriteDatabase(connectionString, IsolationLevel.Unspecified, null);
        }
        public static void WriteDatabase(this DataSet dataSet, String connectionString, IsolationLevel isolationLevel)
        {
            dataSet.WriteDatabase(connectionString, isolationLevel, null);
        }

        public static void WriteDatabase(this DataSet dataSet, String connectionString, BulkCopyCallback bulkCopyCallback)
        {
            dataSet.WriteDatabase(connectionString, IsolationLevel.Unspecified, bulkCopyCallback);
        }
        public static void WriteDatabase(this DataSet dataSet, String connectionString, IsolationLevel isolationLevel, BulkCopyCallback bulkCopyCallback)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                dataSet.WriteDatabase(connection, isolationLevel, bulkCopyCallback);
            }
        }

        public static void WriteDatabase(this DataSet dataSet, SqlConnection connection)
        {
            dataSet.WriteDatabase(connection, IsolationLevel.Unspecified, null);
        }
        public static void WriteDatabase(this DataSet dataSet, SqlConnection connection, IsolationLevel isolationLevel)
        {
            dataSet.WriteDatabase(connection, isolationLevel, null);
        }

        public static void WriteDatabase(this DataSet dataSet, SqlConnection connection, BulkCopyCallback bulkCopyCallback)
        {
            dataSet.WriteDatabase(connection, IsolationLevel.Unspecified, bulkCopyCallback);
        }
        public static void WriteDatabase(this DataSet dataSet, SqlConnection connection, IsolationLevel isolationLevel, BulkCopyCallback bulkCopyCallback)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (var transaction = connection.BeginTransaction(isolationLevel))
            {
                try
                {
                    dataSet.WriteDatabase(connection, transaction, bulkCopyCallback);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public static void WriteDatabase(this DataSet dataSet, SqlConnection connection, SqlTransaction transaction)
        {
            dataSet.WriteDatabase(connection, transaction, null);
        }
        public static void WriteDatabase(this DataSet dataSet, SqlConnection connection, SqlTransaction transaction, BulkCopyCallback bulkCopyCallback)
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
                if (dataTable.Rows.Count > 0)
                {
                    dataTable.WriteDatabase(connection, transaction, bulkCopyCallback);
                }
            }
        }

        public static void WriteDatabase(this DataTable dataTable, String connectionString)
        {
            dataTable.WriteDatabase(connectionString, IsolationLevel.Unspecified, null);
        }
        public static void WriteDatabase(this DataTable dataTable, String connectionString, IsolationLevel isolationLevel)
        {
            dataTable.WriteDatabase(connectionString, isolationLevel, null);
        }

        public static void WriteDatabase(this DataTable dataTable, String connectionString, BulkCopyCallback bulkCopyCallback)
        {
            dataTable.WriteDatabase(connectionString, IsolationLevel.Unspecified, bulkCopyCallback);
        }
        public static void WriteDatabase(this DataTable dataTable, String connectionString, IsolationLevel isolationLevel, BulkCopyCallback bulkCopyCallback)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                dataTable.WriteDatabase(connection, isolationLevel, bulkCopyCallback);
            }
        }

        public static void WriteDatabase(this DataTable dataTable, SqlConnection connection)
        {
            dataTable.WriteDatabase(connection, IsolationLevel.Unspecified, null);
        }
        public static void WriteDatabase(this DataTable dataTable, SqlConnection connection, IsolationLevel isolationLevel)
        {
            dataTable.WriteDatabase(connection, isolationLevel, null);
        }

        public static void WriteDatabase(this DataTable dataTable, SqlConnection connection, BulkCopyCallback bulkCopyCallback)
        {
            dataTable.WriteDatabase(connection, IsolationLevel.Unspecified, bulkCopyCallback);
        }
        public static void WriteDatabase(this DataTable dataTable, SqlConnection connection, IsolationLevel isolationLevel, BulkCopyCallback bulkCopyCallback)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (var transaction = connection.BeginTransaction(isolationLevel))
            {
                try
                {
                    dataTable.WriteDatabase(connection, transaction, bulkCopyCallback);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public static void WriteDatabase(this DataTable dataTable, SqlConnection connection, SqlTransaction transaction)
        {
            dataTable.WriteDatabase(connection, transaction, null);
        }
        public static void WriteDatabase(this DataTable dataTable, SqlConnection connection, SqlTransaction transaction, BulkCopyCallback bulkCopyCallback)
        {
            //const SqlBulkCopyOptions bulkCopyOptions = SqlBulkCopyOptions.KeepNulls |
            //                                           SqlBulkCopyOptions.KeepIdentity |
            //                                           SqlBulkCopyOptions.TableLock |
            //                                           SqlBulkCopyOptions.FireTriggers |
            //                                           SqlBulkCopyOptions.CheckConstraints;

            const SqlBulkCopyOptions bulkCopyOptions = SqlBulkCopyOptions.KeepNulls |
                                                       SqlBulkCopyOptions.KeepIdentity |
                                                       SqlBulkCopyOptions.FireTriggers |
                                                       SqlBulkCopyOptions.CheckConstraints;

            //const SqlBulkCopyOptions bulkCopyOptions = SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.KeepIdentity;

            using (var sqlBulkCopy = new SqlBulkCopy(connection, bulkCopyOptions, transaction))
            {
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    sqlBulkCopy.ColumnMappings.Add(dataColumn.ColumnName, dataColumn.ColumnName);
                }

                var rowsCount = dataTable.Rows.Count;
                var tableName = dataTable.TableName;

                sqlBulkCopy.DestinationTableName = tableName;
                sqlBulkCopy.BulkCopyTimeout = 3600;
                sqlBulkCopy.BatchSize = Math.Min(2500, rowsCount);
                sqlBulkCopy.NotifyAfter = GetBulkCopyNotifyCount(rowsCount, sqlBulkCopy.BatchSize);

                sqlBulkCopy.SqlRowsCopied += (sender, args) => DoBulkCopyCallback(sender, args, bulkCopyCallback);
                sqlBulkCopy.WriteToServer(dataTable);

                DoBulkCopyCallback(sqlBulkCopy, new SqlRowsCopiedEventArgs(rowsCount), bulkCopyCallback);

                sqlBulkCopy.Close();
            }
        }

        private static String GetTableName(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(TableAttribute), true);

            var tableAttr = (TableAttribute)attributes.FirstOrDefault();
            if (tableAttr != null && !String.IsNullOrWhiteSpace(tableAttr.Name))
            {
                return tableAttr.Name;
            }

            return type.Name;
        }

        private static String GetColumnName(PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), true);

            var columnAttr = (ColumnAttribute)attributes.FirstOrDefault();
            if (columnAttr != null && !String.IsNullOrWhiteSpace(columnAttr.Name))
            {
                return columnAttr.Name;
            }

            return propertyInfo.Name;
        }

        private static void DoBulkCopyCallback(Object sender, SqlRowsCopiedEventArgs args, BulkCopyCallback callback)
        {
            var sqlBulkCopy = sender as SqlBulkCopy;
            if (sqlBulkCopy == null || callback == null || args == null || callback.Count == args.RowsCopied)
            {
                return;
            }

            callback.Name = sqlBulkCopy.DestinationTableName;
            callback.Count = args.RowsCopied;

            var result = callback.DoCallback();
            if (result != null)
            {
                args.Abort = result.Value;
            }
        }

        private static int GetBulkCopyNotifyCount(int rowsCount, int batchSize)
        {
            if (rowsCount <= batchSize)
            {
                return rowsCount;
            }

            var count = (int)Math.Truncate(rowsCount / 100D * 10D);
            return count;
        }
    }
}
