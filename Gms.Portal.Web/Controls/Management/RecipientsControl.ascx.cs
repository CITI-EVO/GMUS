using System;
using System.Web.UI.WebControls;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class RecipientsControl : BaseUserControlExtend<RecipientsModel>
    {
        public bool CommandsVisible
        {
            get
            {
                var commandsColumn = gvData.Columns[0];
                return commandsColumn.Visible;
            }
            set
            {
                var commandsColumn = gvData.Columns[0];
                commandsColumn.Visible = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}