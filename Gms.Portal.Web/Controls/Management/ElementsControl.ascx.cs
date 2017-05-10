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
        protected virtual void OnMove(GenericEventArgs<Guid> e)
        {
            if (Move != null)
                Move(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Paste;
        protected virtual void OnPaste(GenericEventArgs<Guid> e)
        {
            if (Paste != null)
                Paste(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Copy;
        protected virtual void OnCopy(GenericEventArgs<Guid> e)
        {
            if (Copy != null)
                Copy(this, e);
        }
        


        protected void btnMove_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnMove(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnPaste_Command(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnPaste(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnCopy_Command(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnCopy(new GenericEventArgs<Guid>(itemID.Value));
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

        protected bool GetCopyVisible(object eval)
        {
            return eval != null;
        }

        protected bool GetMoveVisible(object eval)
        {
            return eval != null;
        }

        protected bool GetPasteVisible(object eval)
        {
            var type = Convert.ToString(eval);
            if (type == "Field")
                return false;

            return true;
        }


    }
}