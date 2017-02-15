using System;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models.Helpers;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class FormStructureControl : BaseUserControlExtend<FormUnitsModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string GetImageUrl(object eval)
        {
            return "#";
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