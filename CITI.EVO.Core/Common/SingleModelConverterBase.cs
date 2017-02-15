
using NHibernate;

namespace CITI.EVO.Core.Common
{
    public abstract class SingleModelConverterBase<TSource, TTarget>
    {
        public SingleModelConverterBase(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; private set; }

        public abstract TTarget Convert(TSource source);

        public abstract void FillObject(TTarget target, TSource source);
    }
}