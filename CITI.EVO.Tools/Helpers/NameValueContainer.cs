using System;
using System.Text.RegularExpressions;

namespace CITI.EVO.Tools.Helpers
{
    public class NameValueContainer
    {
        private readonly static Regex _expRegex = new Regex(@"^(?<name>.*?)=(?<value>.*?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly String _name;
        private readonly String _value;

        public NameValueContainer(String name, String value)
        {
            _name = name;
            _value = value;
        }

        public String Name
        {
            get { return _name; }
        }

        public String Value
        {
            get { return _value; }
        }

        public override bool Equals(object obj)
        {
            var other = (NameValueContainer)obj;
            return String.Equals(Name, other.Name) && String.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            var nameHash = (_name ?? String.Empty).GetHashCode();
            var valueHash = (_value ?? String.Empty).GetHashCode();

            return nameHash ^ valueHash;
        }

        public override String ToString()
        {
            return String.Format("{0} = {1}", Name, Value);
        }

        public static implicit operator String(NameValueContainer d)
        {
            if (d == null)
                return null;

            return d.ToString();
        }

        public static implicit operator NameValueContainer(String d)
        {
            if (!_expRegex.IsMatch(d))
                throw new Exception("Unable to parse expression for NameValueDataContainer");

            var match = _expRegex.Match(d);

            var name = match.Groups["name"].Value;
            var value = match.Groups["name"].Value;

            var result = new NameValueContainer(name, value);
            return result;
        }
    }
}
