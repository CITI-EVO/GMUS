using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.ExpressionEngine
{
    public class AdvancedDataResolver : DefaultDataResolver
    {
        private readonly Func<String, Object> _varResolver;

        public AdvancedDataResolver()
            : this((IDataResolver)null)
        {
        }
        public AdvancedDataResolver(Func<String, Object> varResolver)
            : this(varResolver, null)
        {
        }
        public AdvancedDataResolver(IDataResolver parentResolver)
            : this(null, parentResolver)
        {
        }
        public AdvancedDataResolver(Func<String, Object> varResolver, IDataResolver parentResolver)
            : base(new Dictionary<String, Object>(), parentResolver)
        {
            _varResolver = varResolver;
        }

        public override bool TryGetValue(String name, out Object value)
        {
            if (base.TryGetValue(name, out value))
                return true;

            if (_varResolver != null)
            {
                value = _varResolver(name);
                return true;
            }

            return false;
        }
    }
}