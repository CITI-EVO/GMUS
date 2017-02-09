using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CITI.EVO.Tools.Extensions;

namespace CITI.EVO.Tools.Helpers
{
    public class DictionaryDataReader : IDataReader
    {
        private readonly IEnumerator<IDictionary<String, Object>> _enumerator;
        private readonly IList<String> _fields;

        public DictionaryDataReader(ISet<String> fieldSet, IEnumerable<IDictionary<String, Object>> enumerable)
        {
            _enumerator = enumerable.GetEnumerator();
            _fields = fieldSet.ToList();
        }

        public void Dispose()
        {
        }

        public string GetName(int i)
        {
            return _fields[i];
        }

        public string GetDataTypeName(int i)
        {
            return GetFieldType(i).FullName;
        }

        public Type GetFieldType(int i)
        {
            return typeof(Object);
        }

        public object GetValue(int i)
        {
            var dict = _enumerator.Current;
            var field = GetName(i);

            return dict.GetValueOrDefault(field);
        }

        public int GetValues(object[] values)
        {
            int i = 0;

            while (i < values.Length && i < FieldCount)
            {
                values[i] = GetValue(i);

                i++;
            }

            return i;
        }

        public int GetOrdinal(string name)
        {
            return _fields.IndexOf(name);
        }

        public bool GetBoolean(int i)
        {
            return (bool)GetValue(i);
        }

        public byte GetByte(int i)
        {
            return (byte)GetValue(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            var array = (byte[])GetValue(i);

            var count = 0;
            while ((count++) < length)
            {
                if (fieldOffset >= array.Length)
                {
                    break;
                }

                buffer[bufferOffset++] = array[fieldOffset++];
            }

            return count;
        }

        public char GetChar(int i)
        {
            return (char)GetValue(i);
        }

        public long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            var array = (char[])GetValue(i);

            var count = 0;
            while ((count++) < length)
            {
                if (fieldOffset >= array.Length)
                {
                    break;
                }

                buffer[bufferOffset++] = array[fieldOffset++];
            }

            return count;
        }

        public Guid GetGuid(int i)
        {
            return (Guid)GetValue(i);
        }

        public short GetInt16(int i)
        {
            return (short)GetValue(i);
        }

        public int GetInt32(int i)
        {
            return (int)GetValue(i);
        }

        public long GetInt64(int i)
        {
            return (long)GetValue(i);
        }

        public float GetFloat(int i)
        {
            return (float)GetValue(i);
        }

        public double GetDouble(int i)
        {
            return (double)GetValue(i);
        }

        public string GetString(int i)
        {
            return (string)GetValue(i);
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)GetValue(i);
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)GetValue(i);
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();

            //var enumerable = (IEnumerable<IDictionary<String, Object>>)GetValue(i);
            //var firstDict = enumerable.FirstOrDefault();

            //var fields = new HashSet<String>(firstDict.Keys);

            //var reader = new Class1(fields, enumerable);
            //return reader;
        }

        public bool IsDBNull(int i)
        {
            var value = GetValue(i);
            return Convert.IsDBNull(value);
        }

        public int FieldCount
        {
            get { return _fields.Count; }
        }

        public Object this[int i]
        {
            get { return GetValue(i); }
        }

        public Object this[string name]
        {
            get
            {
                var index = GetOrdinal(name);
                return GetValue(index);
            }
        }

        public void Close()
        {
        
        }

        public DataTable GetSchemaTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Name");

            foreach (var field in _fields)
            {
                var dataRow = dataTable.NewRow();
                dataRow["Name"] = field;

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        public bool NextResult()
        {
            return false;
        }

        public bool Read()
        {
            return _enumerator.MoveNext();
        }

        public int Depth { get; private set; }

        public bool IsClosed { get; private set; }

        public int RecordsAffected { get; private set; }
    }
}