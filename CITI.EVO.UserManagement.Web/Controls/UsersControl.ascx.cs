using System;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class UsersControl : BaseUserControlExtend<UsersModel>
    {
        public event EventHandler<GenericEventArgs<Guid>> SetAttribute;
        protected virtual void OnSetAttribute(GenericEventArgs<Guid> e)
        {
            if (SetAttribute != null)
                SetAttribute(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> ViewAttributes;
        protected virtual void OnViewAttributes(GenericEventArgs<Guid> e)
        {
            if (ViewAttributes != null)
                ViewAttributes(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> NewMessage;
        protected virtual void OnNewMessage(GenericEventArgs<Guid> e)
        {
            if (NewMessage != null)
                NewMessage(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSetAttribute_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnSetAttribute(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnViewAttributes_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnViewAttributes(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected void btnNewMessage_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnNewMessage(new GenericEventArgs<Guid>(itemID.Value));
        }
    }
}