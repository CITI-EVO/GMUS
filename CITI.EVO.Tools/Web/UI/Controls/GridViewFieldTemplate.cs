using System;
using System.Web.UI;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class GridViewFieldTemplate : ITemplate
    {
        private readonly Func<Control> _func;

        public GridViewFieldTemplate(Func<Control> func)
        {
            _func = func;
        }

        public void InstantiateIn(Control container)
        {
            if (_func == null)
                return;

            var control = _func();
            if (control == null)
                return;

            container.Controls.Add(control);
        }
    }
}