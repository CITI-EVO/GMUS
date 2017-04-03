using System;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class ElementMoveControl : BaseUserControlExtend<ElementMoveModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void SetModel(ElementMoveModel model)
        {
            cbxMovingElements.DataSource = model.FormTree;
            cbxMovingElements.DataBind();
            base.SetModel(model);
        }
    }
}