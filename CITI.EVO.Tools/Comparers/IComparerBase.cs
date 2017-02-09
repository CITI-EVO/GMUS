using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Comparers
{
    public interface IComparerBase<TObject> : IComparer, IComparer<TObject>, IEqualityComparer, IEqualityComparer<TObject>
    {
    }
}
