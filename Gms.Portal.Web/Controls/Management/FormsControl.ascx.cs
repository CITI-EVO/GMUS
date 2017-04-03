using System;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class FormsControl : BaseUserControlExtend<FormsModel>
    {
        public event EventHandler<GenericEventArgs<Guid>> Copy;
        protected virtual void OnCopy(GenericEventArgs<Guid> e)
        {
            if (Copy != null)
                Copy(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCopy_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnCopy(new GenericEventArgs<Guid>(itemID.Value));
        }
    }
}