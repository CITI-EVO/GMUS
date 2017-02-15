using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Gms.Portal.Web.Helpers
{
    public class GridColumnTemplate : ITemplate
    {
        private readonly Func<IEnumerable<Control>> _func;

        public GridColumnTemplate(Func<IEnumerable<Control>> func)
        {
            _func = func;
        }

        public void InstantiateIn(Control container)
        {
            if (_func == null)
                return;

            var controls = _func();
            if (controls == null)
                return;

            foreach (var control in controls)
                container.Controls.Add(control);
        }
    }
}