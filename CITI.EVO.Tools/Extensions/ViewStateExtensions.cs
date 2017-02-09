using System;
using System.Web.UI;

namespace CITI.EVO.Tools.Extensions
{
    public static class ViewStateExtensions
    {
        public static TValue GetValue<TValue>(this StateBag viewState, String key)
        {
            var value = viewState[key];
            if (value == null)
            {
                return default(TValue);
            }

            return (TValue)value;
        }

        public static void SetValue(this StateBag viewState, String key, Object value)
        {
            viewState[key] = value;
        }
    }
}
