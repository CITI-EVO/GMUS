using System;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models.Helpers;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class ElementsControl : BaseUserControlExtend<ElementNodesModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public event EventHandler<GenericEventArgs<Guid>> Move;
        protected void btnMove_OnCommand(object sender, CommandEventArgs e)
        {
            var id = DataConverter.ToNullableGuid(e.CommandArgument);
            if (id != null && Move != null)
            {
                Move(sender, new GenericEventArgs<Guid>(id.Value));
            }
        }

        protected String GetImageClass(object eval)
        {
            var type = Convert.ToString(eval);

            if (type == "Form")
                return "fa fa-drivers-license-o";

            if (type == "Grid")
                return "fa fa-table";

            if (type == "Field")
                return "fa fa-pencil-square-o";

            if (type == "Group")
                return "fa fa-list-alt";

            if (type == "TabPage")
                return "fa fa-cube";

            if (type == "TabContainer")
                return "fa fa-cubes";

            return null;
        }

        protected bool GetEditVisible(object eval)
        {
            return true;
        }

        protected bool GetDeleteVisible(object eval)
        {
            return true;
        }

        protected bool GetNewVisible(object eval)
        {
            var type = Convert.ToString(eval);
            if (type == "Field")
                return false;

            return true;
        }

        protected bool GetMoveVisible(object eval)
        {
            return eval != null;
        }


    }
}