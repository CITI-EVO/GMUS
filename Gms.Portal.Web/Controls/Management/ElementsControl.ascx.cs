using System;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models.Helpers;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class ElementsControl : BaseUserControlExtend<ElementNodesModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
    }
}