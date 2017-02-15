using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace CITI.EVO.Core.Common
{
    public abstract class BulkModelConverterBase<TSource, TTarget>
    {
        public BulkModelConverterBase(ISession hbSession)
        {
            HbSession = hbSession;
        }

        public ISession HbSession { get; private set; }

        public abstract IEnumerable<TTarget> Convert(IQueryable<TSource> source);
    }
}