using System;

namespace CITI.EVO.Tools.ExpressionEngine
{
    public interface IDataResolver
    {
        Object GetValue(String name);

        bool TryGetValue(String name, out Object value);

        bool SetValue(String name, Object value);

        bool TrySetValue(String name, Object value);

        bool Contains(String name);

        bool Remove(String name);

        void Clear();
    }
}
