using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;

namespace CITI.EVO.Rpc.Formatters
{
    public class ConcurrentFormatter : IFormatter
    {
        private readonly IFormatter[] _formatters;

        public ConcurrentFormatter(Func<IFormatter> initializer)
            : this(Environment.ProcessorCount * 2, initializer)
        {
        }
        public ConcurrentFormatter(int concurrencyLevel, Func<IFormatter> initializer)
        {
            if (concurrencyLevel < 2)
            {
                throw new ArgumentOutOfRangeException("concurrencyLevel");
            }

            if (initializer == null)
            {
                throw new ArgumentNullException("initializer");
            }

            _formatters = new IFormatter[concurrencyLevel];

            for (int i = 0; i < concurrencyLevel; i++)
            {
                var formatter = initializer();
                var syncFormatter = new SynchronizedFormatter(formatter);

                _formatters[i] = syncFormatter;
            }
        }

        public ISurrogateSelector SurrogateSelector
        {
            get
            {
                var formatter = GetFormatterForCurrentThread();
                return formatter.SurrogateSelector;
            }
            set
            {
                var formatter = GetFormatterForCurrentThread();
                formatter.SurrogateSelector = value;
            }
        }

        public SerializationBinder Binder
        {
            get
            {
                var formatter = GetFormatterForCurrentThread();
                return formatter.Binder;
            }
            set
            {
                var formatter = GetFormatterForCurrentThread();
                formatter.Binder = value;
            }
        }

        public StreamingContext Context
        {
            get
            {
                var formatter = GetFormatterForCurrentThread();
                return formatter.Context;
            }
            set
            {
                var formatter = GetFormatterForCurrentThread();
                formatter.Context = value;
            }
        }

        public Object Deserialize(Stream serializationStream)
        {
            var formatter = GetFormatterForCurrentThread();
            return formatter.Deserialize(serializationStream);
        }

        public void Serialize(Stream serializationStream, Object graph)
        {
            var formatter = GetFormatterForCurrentThread();
            formatter.Serialize(serializationStream, graph);
        }

        private IFormatter GetFormatterForCurrentThread()
        {
            var hashCode = Thread.CurrentThread.ManagedThreadId & int.MaxValue;
            var index = hashCode % _formatters.Length;

            var formatter = _formatters[index];
            return formatter;
        }
    }
}
