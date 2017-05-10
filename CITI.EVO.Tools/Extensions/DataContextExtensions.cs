using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.Tools.Extensions
{
    public static class DataContextExtensions
    {
        private static readonly Regex _queryWhereRegex;

        static DataContextExtensions()
        {
            _queryWhereRegex = new Regex(@"\[\w\d*\].", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static IEnumerable<TEntity> GetUpdates<TEntity>(this DataContext dataContext) where TEntity : class
        {
            var changeSet = dataContext.GetChangeSet();
            return changeSet.Updates.Where(update => update.GetType() == typeof(TEntity)).Select(update => update as TEntity);
        }

        public static IEnumerable<TEntity> GetInserts<TEntity>(this DataContext dataContext) where TEntity : class
        {
            var changeSet = dataContext.GetChangeSet();
            return changeSet.Inserts.Where(update => update.GetType() == typeof(TEntity)).Select(update => update as TEntity);
        }

        public static IEnumerable<TEntity> GetDeletes<TEntity>(this DataContext dataContext) where TEntity : class
        {
            var changeSet = dataContext.GetChangeSet();
            return changeSet.Deletes.Where(update => update.GetType() == typeof(TEntity)).Select(update => update as TEntity);
        }

        public static int ExecuteUpdate<TItem>(this DataContext dataContext, IQueryable<TItem> query, Expression<Func<TItem, Object>> expression, Object value)
        {
            return ExecuteUpdate(dataContext, query, expression, value, null);
        }
        public static int ExecuteUpdate<TItem>(this DataContext dataContext, IQueryable<TItem> query, Expression<Func<TItem, Object>> expression, Object value, int? commandTimeout)
        {
            var memberInfo = ReflectionUtil.MemberOf(expression);
            if (memberInfo == null)
            {
                throw new Exception();
            }

            var columnName = memberInfo.Name;

            var columnAttr = Attribute.GetCustomAttribute(memberInfo, typeof(ColumnAttribute)) as ColumnAttribute;
            if (columnAttr != null && !String.IsNullOrWhiteSpace(columnAttr.Name))
            {
                columnName = columnAttr.Name;
            }

            return ExecuteUpdate(dataContext, query, columnName, value, commandTimeout);
        }

        public static int ExecuteUpdate(this DataContext dataContext, IQueryable query, String column, Object value)
        {
            return ExecuteUpdate(dataContext, query, column, value, null);
        }
        public static int ExecuteUpdate(this DataContext dataContext, IQueryable query, String column, Object value, int? commandTimeout)
        {
            var updateParams = new Dictionary<String, Object>
            {
                { column, value }
            };

            return dataContext.ExecuteUpdate(query, updateParams, commandTimeout);
        }

        public static int ExecuteUpdate(this DataContext dataContext, IQueryable query, IDictionary<String, Object> updateParams)
        {
            return ExecuteUpdate(dataContext, query, updateParams, null);
        }
        public static int ExecuteUpdate(this DataContext dataContext, IQueryable query, IDictionary<String, Object> updateParams, int? commandTimeout)
        {
            using (var command = CreateUpdateCommand(dataContext, query, updateParams))
            {
                PrepareCommand(dataContext, command, commandTimeout);

                var count = command.ExecuteNonQuery();
                return count;
            }
        }

        public static bool TryBeginTransaction(this DataContext dataContext)
        {
            return TryBeginTransaction(dataContext, null);
        }
        public static bool TryBeginTransaction(this DataContext dataContext, IsolationLevel? isolationLevel)
        {
            if (dataContext.Transaction != null)
            {
                return false;
            }

            var connection = dataContext.Connection;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            if (isolationLevel == null)
            {
                dataContext.Transaction = connection.BeginTransaction();
            }
            else
            {
                dataContext.Transaction = connection.BeginTransaction(isolationLevel.Value);
            }

            return true;
        }

        public static bool TryCommitTransaction(this DataContext dataContext)
        {
            if (dataContext.Transaction == null)
            {
                return false;
            }

            try
            {
                dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception commitEx)
            {
                Console.WriteLine(commitEx);

                try
                {
                    dataContext.Transaction.Rollback();
                }
                catch (Exception rollbackEx)
                {
                    Console.WriteLine(rollbackEx);
                }

                return false;
            }
        }

        public static bool TryRollbackTransaction(this DataContext dataContext)
        {
            if (dataContext.Transaction == null)
            {
                return false;
            }

            try
            {
                dataContext.Transaction.Rollback();
                return true;
            }
            catch (Exception commitEx)
            {
                Console.WriteLine(commitEx);

                return false;
            }
        }

        public static IEnumerable<TResult> ExecuteQuery<TResult>(this DataContext dataContext, IQueryable query)
        {
            return ExecuteQuery<TResult>(dataContext, query, null);
        }
        public static IEnumerable<TResult> ExecuteQuery<TResult>(this DataContext dataContext, IQueryable query, int? commandTimeout)
        {
            var result = ExecuteQuery(dataContext, typeof(TResult), query, commandTimeout);

            var collection = result.Cast<TResult>();
            return collection;
        }

        public static IEnumerable ExecuteQuery(this DataContext dataContext, Type elementType, IQueryable query)
        {
            return ExecuteQuery(dataContext, elementType, query, null);
        }
        public static IEnumerable ExecuteQuery(this DataContext dataContext, Type elementType, IQueryable query, int? commandTimeout)
        {
            using (var command = CreateSelectCommand(dataContext, query))
            {
                PrepareCommand(dataContext, command, commandTimeout);

                using (var reader = command.ExecuteReader())
                {
                    var result = dataContext.Translate(elementType, reader);

                    foreach (var item in result)
                    {
                        yield return item;
                    }
                }
            }
        }

        public static TResult ExecuteScalar<TResult>(this DataContext dataContext, IQueryable query)
        {
            return ExecuteScalar<TResult>(dataContext, query, null);
        }
        public static TResult ExecuteScalar<TResult>(this DataContext dataContext, IQueryable query, int? commandTimeout)
        {
            var result = ExecuteScalar(dataContext, typeof(TResult), query, commandTimeout);
            return (TResult)result;
        }

        public static Object ExecuteScalar(this DataContext dataContext, Type elementType, IQueryable query)
        {
            return ExecuteScalar(dataContext, elementType, query, null);
        }
        public static Object ExecuteScalar(this DataContext dataContext, Type elementType, IQueryable query, int? commandTimeout)
        {
            using (var command = CreateSelectCommand(dataContext, query))
            {
                PrepareCommand(dataContext, command, commandTimeout);

                var result = command.ExecuteScalar();
                return result;
            }
        }

        public static bool ExecuteAny(this DataContext dataContext, IQueryable query)
        {
            return ExecuteAny(dataContext, query, null);
        }
        public static bool ExecuteAny(this DataContext dataContext, IQueryable query, int? commandTimeout)
        {
            using (var command = CreateSelectCommand(dataContext, query))
            {
                MakeAnyCommand(command);
                PrepareCommand(dataContext, command, commandTimeout);

                var result = command.ExecuteScalar();
                return Convert.ToBoolean(result);
            }
        }

        public static long ExecuteCount(this DataContext dataContext, IQueryable query)
        {
            return ExecuteCount(dataContext, query, null);
        }
        public static long ExecuteCount(this DataContext dataContext, IQueryable query, int? commandTimeout)
        {
            using (var command = CreateSelectCommand(dataContext, query))
            {
                MakeCountCommand(command);
                PrepareCommand(dataContext, command, commandTimeout);

                var result = command.ExecuteScalar();
                return Convert.ToInt64(result);
            }
        }

        private static DbCommand CreateUpdateCommand(DataContext dataContext, IQueryable query, IDictionary<String, Object> updateColumns)
        {
            var dbCommand = CreateSelectCommand(dataContext, query);
            var tableName = GetTableName(query.ElementType);

            var updatePart = $"UPDATE {tableName} SET";

            var setParams = new List<String>(updateColumns.Count);

            foreach (var pair in updateColumns)
            {
                var formatedValue = GetFormatedValue(pair.Value);
                var param = $"{pair.Key} = {formatedValue}";

                setParams.Add(param);
            }

            var setPart = String.Join(", ", setParams);

            var wherePart = String.Empty;

            var whereIndex = dbCommand.CommandText.IndexOf("WHERE", StringComparison.InvariantCultureIgnoreCase);
            if (whereIndex > 0)
            {
                wherePart = dbCommand.CommandText.Substring(whereIndex);
            }

            wherePart = _queryWhereRegex.Replace(wherePart, String.Empty);

            var updateQuery = $"{updatePart}{Environment.NewLine}{setPart}{Environment.NewLine}{wherePart}";
            dbCommand.CommandText = updateQuery;

            return dbCommand;
        }

        private static DbCommand CreateSelectCommand(DataContext dataContext, IQueryable query)
        {
            var appSettingKey = $"{dataContext.GetType().Name}.InQueryMode";

            var inQueryMode = ConfigurationManager.AppSettings[appSettingKey];
            inQueryMode = (inQueryMode ?? String.Empty);

            var dbCommand = dataContext.GetCommand(query);

            if (!ReferenceEquals(dataContext.Transaction, dbCommand.Transaction))
            {
                dbCommand.Transaction = dataContext.Transaction;
            }

            switch (inQueryMode.ToLower())
            {
                case "xml":
                    {
                        var optimizer = GetDbCommandOptimizer(dataContext);
                        optimizer.Optimize(dbCommand);
                    }
                    break;
                default:
                    {
                        var sqlQuery = GetDbCommandText(dbCommand);

                        dbCommand.CommandText = sqlQuery;
                        dbCommand.Parameters.Clear();
                    }
                    break;
            }

            return dbCommand;
        }

        private static IDictionary<String, Object> GetDbParamsDict(DbCommand sqlCommand)
        {
            var sqlParams = sqlCommand.Parameters;

            var paramsDict = new Dictionary<String, Object>(sqlParams.Count);

            for (int i = 0; i < sqlParams.Count; i++)
            {
                var dbParam = sqlParams[i];

                var name = dbParam.ParameterName;
                var value = GetFormatedValue(dbParam);

                paramsDict.Add(name, value);
            }

            return paramsDict;
        }

        private static DbCommandOptimizer GetDbCommandOptimizer(DataContext dataContext)
        {
            var contextTypeName = dataContext.GetType().Name;

            var dbOptimizer = CommonObjectCache.InitObject(contextTypeName, () => CreateDbOptimizer(dataContext));
            return dbOptimizer;
        }

        private static DbCommandOptimizer CreateDbOptimizer(DataContext dataContext)
        {
            var converters = GetDbXmlConverters(dataContext);

            var optimizer = new DbCommandOptimizer(converters);
            return optimizer;
        }

        private static IDictionary<String, String> GetDbXmlConverters(DataContext dataContext)
        {

            var prefix = $"{dataContext.GetType().Name}.XmlConverter.";
            var result = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (!String.IsNullOrWhiteSpace(key) && key.StartsWith(prefix))
                {
                    var typeName = key.Replace(prefix, String.Empty);
                    var converterName = ConfigurationManager.AppSettings[key];

                    if (!String.IsNullOrWhiteSpace(typeName) && !String.IsNullOrWhiteSpace(converterName))
                        result[typeName] = converterName;
                }
            }

            return result;
        }

        private static void PrepareCommand(DataContext dataContext, DbCommand command, int? commandTimeout)
        {
            var isolationLevel = GetIsolationLevel(dataContext);
            if (!String.IsNullOrWhiteSpace(isolationLevel))
            {
                var sqlQuery = $"SET TRANSACTION ISOLATION LEVEL {isolationLevel}";
                command.CommandText = String.Concat(sqlQuery, Environment.NewLine, command.CommandText);
            }

            if (commandTimeout != null)
            {
                command.CommandTimeout = commandTimeout.Value;
            }

            var connection = command.Connection;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private static void MakeAnyCommand(DbCommand sqlCommand)
        {
            const String countQueryFormat = "SELECT (CASE WHEN EXISTS({0}) THEN 1 ELSE 0 END) AS {1}";

            var resultName = $"[r{(uint) Guid.NewGuid().GetHashCode()}]";

            sqlCommand.CommandText = String.Format(countQueryFormat, sqlCommand.CommandText, resultName);
        }

        private static void MakeCountCommand(DbCommand sqlCommand)
        {
            const String countQueryFormat = "SELECT COUNT(*) FROM ({0}) AS {1}";

            var hashCode = (uint)sqlCommand.CommandText.GetHashCode();
            var resultName = $"[t_{hashCode}]";

            sqlCommand.CommandText = String.Format(countQueryFormat, sqlCommand.CommandText, resultName);
        }

        private static String GetDbCommandText(DbCommand dbCommand)
        {
            var sqlQuery = dbCommand.CommandText;

            var paramsDict = GetDbParamsDict(dbCommand);

            var querySb = new StringBuilder();

            var parens = false;
            var nameBegin = false;
            var paramName = (String)null;

            foreach (var @char in sqlQuery)
            {
                switch (@char)
                {
                    case '\0':
                        break;
                    case '\\':
                        {
                            parens = !parens;
                        }
                        break;
                    case '@':
                        {
                            if (parens || nameBegin)
                            {
                                continue;
                            }

                            nameBegin = true;
                            paramName = "@";
                        }
                        break;
                    default:
                        {
                            if ((@char >= '0' && @char <= '9') ||
                                (@char >= 'a' && @char <= 'z') ||
                                (@char >= 'A' && @char <= 'Z'))
                            {
                                if (nameBegin)
                                {
                                    paramName += @char;
                                }
                            }
                            else
                            {
                                if (nameBegin)
                                {
                                    var paramValue = paramsDict[paramName];
                                    querySb.Append(paramValue);

                                    nameBegin = false;
                                    paramName = null;
                                }
                            }
                        }
                        break;
                }

                if (!nameBegin)
                {
                    querySb.Append(@char);
                }
            }

            if (nameBegin && !String.IsNullOrWhiteSpace(paramName))
            {
                var paramValue = paramsDict[paramName];
                querySb.Append(paramValue);
            }

            querySb = querySb.Replace("\0", String.Empty);

            return querySb.ToString();
        }

        private static String GetTableName(Type type)
        {
            var tableAttribute = Attribute.GetCustomAttribute(type, typeof(TableAttribute)) as TableAttribute;
            if (tableAttribute == null)
            {
                throw new InvalidCastException("invalid table attribute, attribute not exists");
            }

            return tableAttribute.Name;
        }

        private static Object GetFormatedValue(Object value)
        {
            if (value == null)
            {
                return "NULL";
            }

            if (value is DateTime)
            {
                return $"'{value:yyyy-MM-dd HH:mm:ss.fff}'";
            }

            if (value is Guid)
            {
                return $"'{value}'";
            }

            if (value is bool)
            {
                var boolValue = Convert.ToBoolean(value);
                var intValue = Convert.ToInt32(boolValue);

                return Convert.ToString(intValue);
            }

            if (value is String)
            {
                return $"N'{value}'";
            }

            if (value is sbyte ||
                value is byte ||
                value is short ||
                value is ushort ||
                value is int ||
                value is uint ||
                value is long ||
                value is ulong ||
                value is float ||
                value is double ||
                value is decimal)
            {
                var numberFormatInfo = new NumberFormatInfo
                {
                    NumberDecimalSeparator = "."
                };

                return String.Format(numberFormatInfo, "{0}", value);
            }

            throw new Exception();
        }

        private static Object GetFormatedValue(DbParameter sqlParam)
        {
            if (sqlParam.Value == null)
            {
                return "NULL";
            }

            switch (sqlParam.DbType)
            {
                case DbType.Guid:
                    return String.Format(DateTimeFormatInfo.InvariantInfo, "'{0}'", sqlParam.Value);
                case DbType.Time:
                    return String.Format(DateTimeFormatInfo.InvariantInfo, "'{0:HH:mm:ss.fff}'", sqlParam.Value);
                case DbType.Date:
                    return String.Format(DateTimeFormatInfo.InvariantInfo, "'{0:yyyy-MM-dd}'", sqlParam.Value);
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    return String.Format(DateTimeFormatInfo.InvariantInfo, "'{0:yyyy-MM-dd HH:mm:ss.fff}'", sqlParam.Value);
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                    return String.Format(CultureInfo.InvariantCulture, "N'{0}'", sqlParam.Value);
                case DbType.Boolean:
                    {
                        var boolValue = Convert.ToBoolean(sqlParam.Value);
                        var intValue = Convert.ToInt32(boolValue);

                        return String.Format(DateTimeFormatInfo.InvariantInfo, "{0}", intValue);
                    }
                case DbType.Byte:
                case DbType.Currency:
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.Single:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                    return String.Format(NumberFormatInfo.InvariantInfo, "{0}", sqlParam.Value);
                case DbType.Xml:
                    return String.Format(CultureInfo.InvariantCulture, "N'{0}'", sqlParam.Value);
                case DbType.Object:
                    {
                        if (sqlParam.Value is String)
                        {
                            return String.Format(CultureInfo.InvariantCulture, "N'{0}'", sqlParam.Value);
                        }

                        if (sqlParam.Value is Guid || sqlParam.Value is DateTime)
                        {
                            return String.Format(CultureInfo.InvariantCulture, "'{0}'", sqlParam.Value);
                        }

                        return sqlParam.Value;
                    }
            }

            throw new Exception();
        }

        private static String GetIsolationLevel(DataContext dataContext)
        {
            var appSettingsKey = $"{dataContext.GetType().Name}.AutoTransaction";
            var autoTransaction = ConfigurationManager.AppSettings[appSettingsKey];

            IsolationLevel isolationLevel;
            if (Enum.TryParse(autoTransaction, true, out isolationLevel))
            {
                switch (isolationLevel)
                {
                    case IsolationLevel.ReadCommitted:
                        return "READ COMMITTED";
                    case IsolationLevel.ReadUncommitted:
                        return "READ UNCOMMITTED";
                    case IsolationLevel.RepeatableRead:
                        return "REPEATABLE READ";
                    case IsolationLevel.Serializable:
                        return "SNAPSHOT";
                    case IsolationLevel.Snapshot:
                        return "SERIALIZABLE";
                }
            }

            return null;
        }

    }
}
