using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using CITI.EVO.Rpc.Formatters;
using CITI.EVO.Rpc.Helpers;

namespace CITI.EVO.Rpc.Utils
{
    public static class SerializationUtil
    {
        private static IFormatter _defaultFormatter;
        public static IFormatter DefaultFormatter
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                _defaultFormatter = (_defaultFormatter ?? CreateFormatter());
                return _defaultFormatter;
            }
        }

        private static IFormatter CreateFormatter()
        {
            var formatterName = ConfigurationManager.AppSettings["Rpc.DefaultFormatter"];
            formatterName = (formatterName ?? String.Empty);

            var makeFormatterThreadSafe = ConfigurationManager.AppSettings["Rpc.MakeFormatterThreadSafe"];
            makeFormatterThreadSafe = (makeFormatterThreadSafe ?? String.Empty);

            if (!String.IsNullOrWhiteSpace(formatterName))
            {
                var formatterType = ReflectionUtil.FindType(formatterName);
                if (formatterType != null)
                {
                    if (typeof(IFormatter).IsAssignableFrom(formatterType))
                    {
                        if (makeFormatterThreadSafe == "true" || makeFormatterThreadSafe == "yes" || makeFormatterThreadSafe == "1")
                        {
                            var concurrentFormatter = ConcurrencyHelper.CreateFormatter(() => (IFormatter)Activator.CreateInstance(formatterType));
                            return concurrentFormatter;
                        }

                        var formatterInst = (IFormatter)Activator.CreateInstance(formatterType);
                        if (formatterInst != null)
                        {
                            return formatterInst;
                        }
                    }
                }
            }

            var formatter = ConcurrencyHelper.CreateFormatter(() => new AdvancedBinaryFormatter());
            return formatter;
        }

        public static byte[] Serialize(Object obj)
        {
            using (var stream = new MemoryStream())
            {
                DefaultFormatter.Serialize(stream, obj);

                stream.Flush();

                return stream.ToArray();
            }
        }

        public static TItem Deserialize<TItem>(byte[] data)
        {
            return (TItem)Deserialize(data);
        }

        public static Object Deserialize(byte[] data)
        {
            using (var srcStream = new MemoryStream(data))
            {
                return DefaultFormatter.Deserialize(srcStream);
            }
        }
    }
}
