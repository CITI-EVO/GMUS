using System;

namespace CITI.EVO.Tools.Web.UI.Helpers
{
    public class DictionaryBinderItem
    {
        public DictionaryBinderItem(Object text, Object value)
        {
            Text = text;
            Value = value;
        }

        public Object Text { get; private set; }

        public Object Value { get; private set; }
    }
}
