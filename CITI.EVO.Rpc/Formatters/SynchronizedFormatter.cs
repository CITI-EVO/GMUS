using System;
using System.IO;
using System.Runtime.Serialization;

namespace CITI.EVO.Rpc.Formatters
{
    public class SynchronizedFormatter : IFormatter
    {
        private readonly Object _syncRoot;
        private readonly IFormatter _formatter;

        public SynchronizedFormatter(IFormatter formatter)
            : this(formatter, new Object())
        {
        }
        public SynchronizedFormatter(IFormatter formatter, Object syncRoot)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException("formatter");
            }

            if (syncRoot == null)
            {
                throw new ArgumentNullException("syncRoot");
            }

            _syncRoot = syncRoot;
            _formatter = formatter;
        }

        public Object _SyncRoot
        {
            get { return _syncRoot; }
        }

        public IFormatter BaseFormatter
        {
            get { return _formatter; }
        }

        public ISurrogateSelector SurrogateSelector
        {
            get
            {
                lock (_syncRoot)
                {
                    return _formatter.SurrogateSelector;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _formatter.SurrogateSelector = value;
                }
            }
        }

        public SerializationBinder Binder
        {
            get
            {
                lock (_syncRoot)
                {
                    return _formatter.Binder;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _formatter.Binder = value;
                }
            }
        }

        public StreamingContext Context
        {
            get
            {
                lock (_syncRoot)
                {
                    return _formatter.Context;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _formatter.Context = value;
                }
            }
        }

        public Object Deserialize(Stream serializationStream)
        {
            lock (_syncRoot)
            {
                return _formatter.Deserialize(serializationStream);
            }
        }

        public void Serialize(Stream serializationStream, Object graph)
        {
            lock (_syncRoot)
            {
                _formatter.Serialize(serializationStream, graph);
            }
        }
    }
}
