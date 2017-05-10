using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Xml;

namespace CITI.EVO.Tools.Helpers
{
    public class DbCommandOptimizer
    {
        private class Entry
        {
            public int Index { get; set; }

            public int Start { get; set; }
            public int End { get; set; }

            public String Text { get; set; }
        }

        private const String _converterSqlBatch = "SELECT * FROM {0}(@{1})";
        private const String _standardSqlBatch = "SELECT node.value('.', '{0}') FROM @{1}.nodes('/Val') {1}(node)";

        public DbCommandOptimizer()
        {
            Converters = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);
        }
        public DbCommandOptimizer(IDictionary<String, String> converters)
        {
            Converters = new Dictionary<String, String>(converters, StringComparer.OrdinalIgnoreCase);
        }

        public IDictionary<String, String> Converters { get; private set; }

        public void Optimize(DbCommand command)
        {
            var sb = new StringBuilder();

            var query = command.CommandText;
            var @params = ParseParams(String.Intern(query));

            var prev = (Entry)null;
            foreach (var entry in @params)
            {
                var xmlDoc = new XmlDocument();

                var itemsElem = xmlDoc.CreateElement("Items");
                xmlDoc.AppendChild(itemsElem);

                var dbTypes = new List<DbType>();

                var queryParams = IterateParams(String.Intern(query), entry.Start, entry.End);
                foreach (var nameOrValue in queryParams)
                {
                    var valElem = xmlDoc.CreateElement("Val");
                    itemsElem.AppendChild(valElem);

                    var realNameOrValue = nameOrValue.Trim();

                    DbType dbType;

                    if (command.Parameters.Contains(realNameOrValue))
                    {
                        var dbParam = command.Parameters[realNameOrValue];
                        var dbValue = dbParam.Value;

                        valElem.InnerText = Convert.ToString(dbValue);

                        dbType = dbParam.DbType;

                        command.Parameters.Remove(dbParam);
                    }
                    else
                    {
                        dbType = GetValueType(realNameOrValue);

                        var realValue = GetCleanValue(realNameOrValue);
                        valElem.InnerText = Convert.ToString(realValue.Trim());
                    }

                    if (dbTypes.Count == 0)
                    {
                        dbTypes.Add(dbType);
                    }
                    else
                    {
                        var index = dbTypes.BinarySearch(dbType);
                        if (index < 0)
                        {
                            throw new Exception("More then one type parameters");
                        }
                    }
                }

                var xmlName = $"xml_{entry.Index}";
                var paramName = $"@{xmlName}";

                var xmlParam = command.CreateParameter();
                xmlParam.ParameterName = paramName;
                xmlParam.DbType = DbType.Xml;
                xmlParam.Value = GetXmlText(xmlDoc);

                command.Parameters.Add(xmlParam);

                var typeName = GetSqlTypeName(dbTypes[0]);

                var repText = FormatQuery(typeName, xmlName);

                var prevEnd = (prev == null ? 0 : prev.End);

                var len = entry.Start - prevEnd;
                var chunk = query.Substring(prevEnd, len);

                sb.Append(chunk);
                sb.Append(repText);

                prev = entry;
            }

            if (prev != null && prev.End != query.Length)
            {
                var chunk = query.Substring(prev.End);
                sb.Append(chunk);
            }

            if (sb.Length > 0)
            {
                command.CommandText = sb.ToString();
            }
        }

        private String FormatQuery(String typeName, String xmlName)
        {
            String converter;
            if (Converters.TryGetValue(typeName, out converter) && !String.IsNullOrWhiteSpace(converter))
            {
                return String.Format(_converterSqlBatch, converter, xmlName);
            }

            return String.Format(_standardSqlBatch, typeName, xmlName);
        }

        private IEnumerable<Entry> ParseParams(String query)
        {
            var index = 0;

            var openPars = false;
            var beginArr = false;

            const String beginParamsPattern = " IN (";
            const String endParamsPattern = ")";

            var start = 0;
            var end = 0;
            var sb = new StringBuilder();

            for (int i = 0; i < query.Length; i++)
            {
                if (query[i] == '\'')
                {
                    openPars = !openPars;
                }

                if (!openPars)
                {
                    if (!beginArr)
                    {
                        var order = CompareRange(query, i, beginParamsPattern, 0, beginParamsPattern.Length);
                        if (order == 0)
                        {
                            i += beginParamsPattern.Length;

                            start = i;
                            beginArr = true;
                        }
                    }
                    else
                    {
                        var order = CompareRange(query, i, endParamsPattern, 0, endParamsPattern.Length);
                        if (order == 0)
                        {
                            end = i;
                            beginArr = false;

                            var entry = new Entry();
                            entry.Index = index++;
                            entry.Start = start;
                            entry.End = end;
                            entry.Text = sb.ToString();

                            yield return entry;

                            sb.Clear();
                        }
                    }
                }

                if (beginArr)
                {
                    sb.Append(query[i]);
                }
            }
        }

        private IEnumerable<String> IterateParams(String text, int start, int end)
        {
            var openPars = false;
            var nameOrValueSb = new StringBuilder();

            for (int i = start; i < end; i++)
            {
                var @char = text[i];
                switch (@char)
                {
                    case '\'':
                        {
                            nameOrValueSb.Append(@char);
                            openPars = !openPars;
                        }
                        break;
                    case ',':
                        {
                            if (!openPars)
                            {
                                yield return nameOrValueSb.ToString();

                                nameOrValueSb.Clear();
                            }
                        }
                        break;
                    default:
                        {
                            nameOrValueSb.Append(@char);
                        }
                        break;
                }
            }

            if (nameOrValueSb.Length > 0)
            {
                yield return nameOrValueSb.ToString();
            }
        }

        private int CompareRange(String x, int xIndex, String y, int yIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                var xChar = char.ToLower(x[xIndex++]);
                var yChar = char.ToLower(y[yIndex++]);

                var order = xChar.CompareTo(yChar);
                if (order != 0)
                    return order;
            }

            return 0;
        }

        private String GetXmlText(XmlDocument xmlDoc)
        {
            if (xmlDoc == null)
                return null;

            if (xmlDoc.DocumentElement == null)
                return xmlDoc.InnerXml;

            return xmlDoc.DocumentElement.InnerXml;
        }

        private DbType GetValueType(String value)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                if (value.StartsWith("'") || value.StartsWith("N'"))
                {
                    value = GetCleanValue(value);

                    Guid guid;
                    if (Guid.TryParse(value, out guid))
                    {
                        return DbType.Guid;
                    }

                    DateTime dateTime;
                    if (DateTime.TryParse(value, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dateTime))
                    {
                        return DbType.DateTime;
                    }

                    return DbType.String;
                }

                if (value.Contains(".") || value.Contains(","))
                {
                    decimal dec;
                    if (decimal.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out dec))
                    {
                        return DbType.Double;
                    }
                }

                long @long;
                if (long.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out @long))
                {
                    return DbType.Int64;
                }
            }

            throw new Exception();
        }

        private String GetSqlTypeName(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                    return "VARCHAR(MAX)";

                case DbType.AnsiStringFixedLength:
                    return "CHAR(MAX)";

                case DbType.Binary:
                    return "VARBINARY(MAX)";

                case DbType.Boolean:
                    return "BIT";

                case DbType.SByte:
                case DbType.Byte:
                    return "TINYINT";

                case DbType.Currency:
                    return "MONEY";

                case DbType.Date:
                    return "DATE";

                case DbType.DateTime:
                    return "DATETIME";

                case DbType.DateTime2:
                    return "DATETIME2";

                case DbType.DateTimeOffset:
                    return "DATETIMEOFFSET";

                case DbType.Decimal:
                    return "DECIMAL(18, 0)";

                case DbType.Double:
                    return "FLOAT";

                case DbType.Guid:
                    return "UNIQUEIDENTIFIER";

                case DbType.UInt16:
                case DbType.Int16:
                    return "SMALLINT";

                case DbType.UInt32:
                case DbType.Int32:
                    return "INT";

                case DbType.UInt64:
                case DbType.Int64:
                    return "BITINT";

                case DbType.Object:
                    return "SQL_VARIANT";

                case DbType.Single:
                    return "REAL";

                case DbType.String:
                    return "NVARCHAR(MAX)";

                case DbType.StringFixedLength:
                    return "NCHAR(MAX)";

                case DbType.Time:
                    return "TIME";

                case DbType.Xml:
                    return "XML";
            }

            throw new Exception();
        }

        private String GetCleanValue(String value)
        {
            value = (value ?? String.Empty);

            if ((value.StartsWith("'") || value.StartsWith("N'")) && value.EndsWith("'"))
            {
                if (value.StartsWith("'"))
                    value = value.Substring(1);
                else if (value.StartsWith("N'"))
                    value = value.Substring(1);

                return value.Substring(0, value.Length - 1);
            }

            return value;
        }
    }
}
