using System;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class FilesDownloadControl : BaseUserControlExtend<FilesModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public event EventHandler<GenericEventArgs<Guid>> Download;
        protected virtual void OnDownload(GenericEventArgs<Guid> e)
        {
            if (Download != null)
                Download(this, e);
        }


        protected void btnDownload_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnDownload(new GenericEventArgs<Guid>(itemID.Value));
        }
    }
}