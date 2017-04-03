using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CITI.EVO.Tools.Helpers
{
    public class UrlHelper : IEnumerable<KeyValuePair<String, Object>>
    {
        private static readonly Regex nameRegex = new Regex(@"(?<name>[\W]params[\W])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex pairRegex = new Regex(@"(?<Key>.*?)=(?<Value>.*?)(&|$)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private const String PairDelimiter = "=";
        private const String ItemDelimiter = "&";

        private const String DefaultParamName = "[params]";

        private readonly String _urlBase;
        private readonly IDictionary<String, Object> _urlParams;

        public UrlHelper(Uri uri) : this(uri, true)
        {
        }
        public UrlHelper(Uri uri, bool mayBeEncoded) : this(Convert.ToString(uri), mayBeEncoded)
        {
        }

        public UrlHelper(String url) : this(url, true)
        {
        }
        public UrlHelper(String url, bool mayBeEncoded)
        {
            _urlBase = Uri.UnescapeDataString(url);
            _urlParams = new Dictionary<String, Object>(StringComparer.OrdinalIgnoreCase);

            if (String.IsNullOrWhiteSpace(_urlBase))
            {
                return;
            }

            var urlParts = SplitBasePart(_urlBase);
            _urlBase = urlParts[0];

            if (urlParts.Length > 1)
            {
                TryParseParams(urlParts[1]);
                TryDecode(mayBeEncoded);
            }
        }

        public UrlHelper(UrlHelper urlHelper)
        {
            _urlBase = urlHelper._urlBase;
            _urlParams = new Dictionary<String, Object>(urlHelper._urlParams, StringComparer.OrdinalIgnoreCase);
        }

        public String HandlerUrl
        {
            get { return _urlBase; }
        }

        public Object this[String key]
        {
            get
            {
                Object parameter;
                if (!_urlParams.TryGetValue(key, out parameter))
                {
                    return null;
                }

                return parameter;
            }
            set
            {
                _urlParams[key] = value;
            }
        }

        public bool Remove(String key)
        {
            return _urlParams.Remove(key);
        }

        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            foreach (var pair in _urlParams)
            {
                yield return new KeyValuePair<String, Object>(pair.Key, pair.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override String ToString()
        {
            return ConvertToString(_urlBase, _urlParams);
        }

        public Uri ToUri()
        {
            return new Uri(ToString());
        }

        public String ToEncodedUrl()
        {
            return ToEncodedUrl(null);
        }
        public String ToEncodedUrl(params String[] exceptParams)
        {
            exceptParams = (exceptParams ?? new String[0]);

            var comparer = StringComparer.OrdinalIgnoreCase;
            var @set = new HashSet<String>(exceptParams, comparer);

            var outputParams = new Dictionary<String, Object>(comparer);
            var encodeParams = new Dictionary<String, Object>(comparer);

            foreach (var pair in _urlParams)
            {
                if (@set.Contains(pair.Key))
                    outputParams[pair.Key] = pair.Value;
                else
                    encodeParams[pair.Key] = pair.Value;
            }

            var encodeItems = encodeParams.Select(n => String.Concat(n.Key, PairDelimiter, n.Value));
            var encodeItemsText = String.Join(ItemDelimiter, encodeItems);

            var base64 = GetEncodedForUrl(encodeItemsText);
            outputParams[DefaultParamName] = base64;

            var encodedUrl = ConvertToString(_urlBase, outputParams);
            return encodedUrl;
        }

        public Uri ToEncodedUri()
        {
            return new Uri(ToEncodedUrl());
        }
        public Uri ToEncodedUri(params String[] exceptParams)
        {
            return new Uri(ToEncodedUrl(exceptParams));
        }

        private String[] SplitBasePart(String url)
        {
            var index = url.IndexOf("?", StringComparison.Ordinal);
            if (index > 0)
            {
                var @base = url.Substring(0, index);
                var @params = url.Substring(index + 1);

                if (String.IsNullOrWhiteSpace(@params))
                {
                    return new[] { @base };
                }

                var array = new[]
                {
                    @base,
                    @params
                };

                return array;
            }

            return new[] { url };
        }

        private void TryParseParams(String urlParams)
        {
            var collection = HttpUtility.ParseQueryString(urlParams);
            if (collection.Count == 0)
                return;

            foreach (var paramKey in collection.AllKeys)
            {
                var key = (paramKey ?? String.Empty);

                var paramValue = collection[paramKey];
                SetValue(key, paramValue);
            }
        }

        private void SetValue(String key, Object value)
        {
            Object oldVal;
            if (_urlParams.TryGetValue(key, out oldVal))
                value = String.Join(",", oldVal, value);

            _urlParams[key] = value;
        }

        private void TryDecode(bool mayBeEncoded)
        {
            if (!mayBeEncoded)
                return;

            var name = GetEncodedParamName(_urlParams.Keys);
            if (String.IsNullOrWhiteSpace(name))
                return;

            Object val;
            if (!_urlParams.TryGetValue(name, out val))
                return;

            var @params = ConvertFromBase64(Convert.ToString(val));
            if (String.IsNullOrWhiteSpace(@params))
                return;

            _urlParams.Remove(name);

            var extracted = ExtractEncoded(@params);
            foreach (var pair in extracted)
            {
                var key = pair.Key;
                var value = pair.Value;

                SetValue(key, value);
            }
        }

        private String GetEncodedParamName(IEnumerable<String> collection)
        {
            foreach (var item in collection)
            {
                if (nameRegex.IsMatch(item))
                    return item;
            }

            return null;
        }

        private String GetEncodedForUrl(String text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);

            var encoded = HttpServerUtility.UrlTokenEncode(bytes);
            return encoded;
        }

        private String ConvertFromBase64(String text)
        {
            try
            {
                var bytes = HttpServerUtility.UrlTokenDecode(text);

                var @string = Encoding.UTF8.GetString(bytes);
                return @string;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private String ConvertToString(String handlerUrl, IDictionary<String, Object> @params)
        {
            var sb = new StringBuilder(handlerUrl);

            if (@params.Count > 0)
            {
                sb.Append("?");

                foreach (var pair in @params)
                {
                    sb.Append(pair.Key);
                    sb.Append("=");

                    var value = pair.Value;
                    if (value is Array)
                    {
                        var array = ((IEnumerable)value).Cast<Object>();
                        value = String.Join(",", array);
                    }

                    sb.Append(value);

                    sb.Append("&");
                }

                sb = sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        private IEnumerable<KeyValuePair<String, Object>> ExtractEncoded(String text)
        {
            if (pairRegex.IsMatch(text))
            {
                var matches = pairRegex.Matches(text);
                foreach (Match match in matches)
                {
                    var key = match.Groups["Key"].Value;
                    var value = match.Groups["Value"].Value;

                    var pair = new KeyValuePair<String, Object>(key, value);
                    yield return pair;
                }
            }
        }
    }

}
