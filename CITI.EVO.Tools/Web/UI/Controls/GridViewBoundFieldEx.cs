using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class GridViewBoundFieldEx : TemplateField
    {
        private readonly Func<TemplateField, Control> _headerFunc;
        private readonly Func<TemplateField, Control> _itemFunc;

        public GridViewBoundFieldEx(Func<TemplateField, Control> headerFunc, Func<TemplateField, Control> itemFunc)
        {
            _headerFunc = headerFunc;
            _itemFunc = itemFunc;
        }

        public String DataField { get; set; }

        public String FieldType { get; set; }

        public override bool Initialize(bool sortingEnabled, Control control)
        {
            HeaderTemplate = new DefaultFieldTemplate(GetHeaderControl);
            ItemTemplate = new DefaultFieldTemplate(GetItemControl);

            return base.Initialize(sortingEnabled, control);
        }

        protected Control GetHeaderControl()
        {
            return _headerFunc(this);
        }

        protected Control GetItemControl()
        {
            return _itemFunc(this);
        }
    }
}