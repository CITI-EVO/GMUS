using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CITI.EVO.Tools.Extensions;

namespace CITI.EVO.Tools.Helpers
{
    public class EntityBulkCopySession
    {
        private static readonly IDictionary<Object, EntityBulkCopySession> sessionsDict = new Dictionary<Object, EntityBulkCopySession>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool ExistsSession(Object key)
        {
            return sessionsDict.ContainsKey(key);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static EntityBulkCopySession LoadSession(Object key)
        {
            return sessionsDict[key];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static EntityBulkCopySession CreateSession(Object key, params Type[] prepareTypes)
        {
            var bulkCopySession = new EntityBulkCopySession
            {
                Key = key
            };

            foreach (var prepareType in prepareTypes)
            {
                bulkCopySession.Prepare(prepareType);
            }

            sessionsDict.Add(key, bulkCopySession);

            return bulkCopySession;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static EntityBulkCopySession CreateOrLoadSession(Object key, params Type[] prepareTypes)
        {
            EntityBulkCopySession bulkCopySession;
            if (!sessionsDict.TryGetValue(key, out bulkCopySession))
            {
                bulkCopySession = new EntityBulkCopySession
                {
                    Key = key
                };

                foreach (var prepareType in prepareTypes)
                {
                    bulkCopySession.Prepare(prepareType);
                }

                sessionsDict.Add(key, bulkCopySession);
            }

            return bulkCopySession;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ReleaseSession(Object key)
        {
            sessionsDict.Remove(key);
        }

        private readonly DataSet dataSet = new DataSet();
        private readonly IDictionary<Type, ISet<PropertyInfo>> propertiesMapping = new Dictionary<Type, ISet<PropertyInfo>>();

        public Object Key { get; private set; }

        public BulkCopyCallback Callback { get; set; }

        public void Prepare<TEntity>()
        {
            var type = typeof(TEntity);

            var tableName = GetDbTableName(type);
            if (dataSet.Tables.Contains(tableName))
            {
                return;
            }

            var dataTable = GenerateDataTable(type);
            dataSet.Tables.Add(dataTable);
        }
        public void Prepare(Type entityType)
        {
            var tableName = GetDbTableName(entityType);
            if (dataSet.Tables.Contains(tableName))
            {
                return;
            }

            var dataTable = GenerateDataTable(entityType);
            dataSet.Tables.Add(dataTable);
        }

        public void SetRelation<TParentEntity, TChildEntity>(String parentPropertyName, String childPropertyName)
        {
            var parentEntityType = typeof(TParentEntity);
            var childEntityType = typeof(TChildEntity);

            SetRelation(parentEntityType, childEntityType, parentPropertyName, childPropertyName);
        }
        public void SetRelation(Type parentEntityType, Type childEntityType, String parentPropertyName, String childPropertyName)
        {
            Prepare(parentEntityType);
            Prepare(childEntityType);

            var parentTableName = GetDbTableName(parentEntityType);
            var childTableName = GetDbTableName(childEntityType);

            var parentDataTable = dataSet.Tables[parentTableName];
            var childDataTable = dataSet.Tables[childTableName];

            var relationName = $"{childDataTable.TableName}_To_{parentDataTable.TableName}";
            if (dataSet.Relations.Contains(relationName))
            {
                return;
            }

            var parentProperties = propertiesMapping[parentEntityType];
            var childProperties = propertiesMapping[childEntityType];

            var parentProperty = parentProperties.FirstOrDefault(n => String.Equals(n.Name, parentPropertyName, StringComparison.OrdinalIgnoreCase));
            var childProperty = childProperties.FirstOrDefault(n => String.Equals(n.Name, childPropertyName, StringComparison.OrdinalIgnoreCase));

            var parentColumnName = GetDbColumnName(parentProperty);
            var childColumnName = GetDbColumnName(childProperty);

            var parentDataColumn = parentDataTable.Columns[parentColumnName];
            var childDataColumn = childDataTable.Columns[childColumnName];

            var dataRelation = new DataRelation(relationName, parentDataColumn, childDataColumn);

            dataSet.Relations.Add(dataRelation);
        }

        public void RemoveRelation(Type parentEntityType, Type childEntityType, String parentPropertyName, String childPropertyName)
        {
            Prepare(parentEntityType);
            Prepare(childEntityType);

            var parentTableName = GetDbTableName(parentEntityType);
            var childTableName = GetDbTableName(childEntityType);

            var parentDataTable = dataSet.Tables[parentTableName];
            var childDataTable = dataSet.Tables[childTableName];

            var relationName = $"{childDataTable.TableName}_To_{parentDataTable.TableName}";

            dataSet.Relations.Remove(relationName);
        }

        public void ClearRelation()
        {
            dataSet.Relations.Clear();
        }

        public void Insert<TEntity>(TEntity entity)
        {
            var entityType = typeof(TEntity);

            if (typeof(IEnumerable).IsAssignableFrom(typeof(TEntity)))
            {
                if (entityType.IsGenericType)
                {
                    var genericArgs = entityType.GetGenericArguments();
                    if (genericArgs.Length == 0 || genericArgs.Length > 1)
                    {
                        throw new Exception("Invalid generic type");
                    }

                    entityType = genericArgs[0];
                }
                else if (entityType.IsArray)
                {
                    entityType = entityType.GetElementType();
                }
                else
                {
                    throw new Exception("Invalid entity type");
                }

                Insert(entityType, (IEnumerable)entity);
            }
            else
            {
                Insert(entityType, entity);
            }
        }
        public void Insert(Type entityType, Object entity)
        {
            if (entity is IEnumerable)
            {
                Insert(entityType, (IEnumerable)entity);
                return;
            }

            Prepare(entityType);

            var tableName = GetDbTableName(entityType);

            var dataTable = dataSet.Tables[tableName];
            var dataRow = dataTable.NewRow();

            var propertiesSet = propertiesMapping[entityType];

            foreach (var propertyInfo in propertiesSet)
            {
                var columnAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(ColumnAttribute)) as ColumnAttribute;
                if (columnAttribute == null)
                {
                    continue;
                }

                var columnName = (!String.IsNullOrEmpty(columnAttribute.Name) ? columnAttribute.Name : propertyInfo.Name);

                var columnValue = propertyInfo.GetValue(entity, null);
                if (columnValue is Binary)
                {
                    columnValue = ((Binary)columnValue).ToArray();
                }

                dataRow.SetField(columnName, columnValue);
            }

            dataTable.Rows.Add(dataRow);
        }

        public void Insert<TEntity>(IEnumerable<TEntity> entities)
        {
            var entityType = typeof(TEntity);
            Insert(entityType, entities);
        }
        public void Insert(Type entityType, IEnumerable entities)
        {
            Prepare(entityType);

            var tableName = GetDbTableName(entityType);

            var dataTable = dataSet.Tables[tableName];

            var propertiesSet = propertiesMapping[entityType];

            foreach (var entity in entities)
            {
                var dataRow = dataTable.NewRow();

                foreach (var propertyInfo in propertiesSet)
                {
                    var columnAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(ColumnAttribute)) as ColumnAttribute;
                    if (columnAttribute == null)
                    {
                        continue;
                    }

                    var columnName = (!String.IsNullOrEmpty(columnAttribute.Name) ? columnAttribute.Name : propertyInfo.Name);

                    var columnValue = propertyInfo.GetValue(entity, null);
                    if (columnValue is Binary)
                    {
                        columnValue = ((Binary)columnValue).ToArray();
                    }

                    dataRow.SetField(columnName, columnValue);
                }

                dataTable.Rows.Add(dataRow);
            }
        }

        public int Count<TEntity>()
        {
            var entityType = typeof(TEntity);
            return Count(entityType);
        }
        public int Count(Type entityType)
        {
            Prepare(entityType);

            var tableName = GetDbTableName(entityType);
            var dataTable = dataSet.Tables[tableName];

            return dataTable.Rows.Count;
        }

        public void Clear()
        {
            dataSet.Clear();
        }
        public void Clear<TEntity>()
        {
            var entityType = typeof(TEntity);
            Clear(entityType);
        }
        public void Clear(Type entityType)
        {
            Prepare(entityType);

            var tableName = GetDbTableName(entityType);
            var dataTable = dataSet.Tables[tableName];

            dataTable.Clear();
        }

        public DataTable GetDataTable<TEntity>()
        {
            var entityType = typeof(TEntity);
            return GetDataTable(entityType);
        }
        public DataTable GetDataTable(Type entityType)
        {
            Prepare(entityType);

            var tableName = GetDbTableName(entityType);
            var dataTable = dataSet.Tables[tableName];

            return dataTable;
        }

        public void Commit(SqlConnection connection)
        {
            using (var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                Commit(connection, transaction);

                try
                {
                    transaction.Commit();
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception)
                    {
                    }

                    throw;
                }
            }
        }
        public void Commit(SqlConnection connection, SqlTransaction transaction)
        {
            if (!IsDataSetEmpty())
            {
                dataSet.WriteDatabase(connection, transaction, Callback);
            }

            dataSet.Clear();
        }

        public void Release()
        {
            ReleaseSession(Key);
        }

        private DataTable GenerateDataTable(Type entityType)
        {
            var tableName = GetDbTableName(entityType);
            if (String.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("Unable to get entity table name");
            }

            ISet<PropertyInfo> propertiesSet;
            if (!propertiesMapping.TryGetValue(entityType, out propertiesSet))
            {
                propertiesSet = new HashSet<PropertyInfo>();
                propertiesMapping.Add(entityType, propertiesSet);
            }

            var dataTable = new DataTable(tableName);

            var properties = entityType.GetProperties();
            foreach (var propertyInfo in properties)
            {
                var columnAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(ColumnAttribute)) as ColumnAttribute;
                if (columnAttribute == null)
                {
                    continue;
                }

                propertiesSet.Add(propertyInfo);

                var propertyType = propertyInfo.PropertyType;
                if (propertyType == typeof(Binary))
                {
                    propertyType = typeof(byte[]);
                }
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                }

                var columnName = (!String.IsNullOrEmpty(columnAttribute.Name) ? columnAttribute.Name : propertyInfo.Name);
                var allowNull = columnAttribute.CanBeNull;
                var isPrimary = columnAttribute.IsPrimaryKey;

                var dataColumn = new DataColumn();
                dataColumn.ColumnName = columnName;
                dataColumn.AllowDBNull = allowNull;
                dataColumn.DataType = propertyType;

                dataTable.Columns.Add(dataColumn);

                if (isPrimary)
                {
                    var constraintName = $"PK_{dataColumn.ColumnName}";
                    dataTable.Constraints.Add(constraintName, dataColumn, true);
                }
            }

            return dataTable;
        }

        private String GetDbTableName(Type entityType)
        {
            if (entityType == null)
            {
                return null;
            }

            var tableAttribute = Attribute.GetCustomAttribute(entityType, typeof(TableAttribute)) as TableAttribute;
            if (tableAttribute == null)
            {
                return null;
            }

            return tableAttribute.Name;
        }

        private String GetDbColumnName(PropertyInfo propertyInfo)
        {
            var columnAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(ColumnAttribute)) as ColumnAttribute;
            if (columnAttribute == null)
            {
                return null;
            }

            var columnName = (!String.IsNullOrEmpty(columnAttribute.Name) ? columnAttribute.Name : propertyInfo.Name);
            return columnName;
        }

        private bool IsDataSetEmpty()
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
                if (dataTable.Rows.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
