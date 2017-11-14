using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class ModalPopup : Panel
    {
        private bool? _open;
        private String _scriptKey;

        private const String _scriptFormat = "<script type='text/javascript'>$('#{0}').modal('{1}');</script>";

        public ModalPopup()
        {
            _scriptKey = $"popup_{Guid.NewGuid():n}";
        }

        public void Show()
        {
            _open = true;
        }

        public void Hide()
        {
            _open = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (_open != null)
            {
                var script = String.Format(_scriptFormat, ClientID, "show");
                if (!_open.GetValueOrDefault())
                    script = String.Format(_scriptFormat, ClientID, "hide");

                Page.ClientScript.RegisterStartupScript(GetType(), _scriptKey, script);
            }

            base.OnPreRender(e);
        }
    }
}