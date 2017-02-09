using System;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;
using CITI.EVO.UserManagement.Web.Models.Helpers;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class GroupsControl : BaseUserControlExtend<GroupUnitsModel>
    {
        public event EventHandler<GenericEventArgs<Guid>> AddUser;
        protected virtual void OnAddUser(GenericEventArgs<Guid> e)
        {
            if (AddUser != null)
                AddUser(this, e);
        }

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

        public event EventHandler<GenericEventArgs<Guid>> Messages;
        protected virtual void OnMessages(GenericEventArgs<Guid> e)
        {
            if (Messages != null)
                Messages(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddUser_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnAddUser(new GenericEventArgs<Guid>(itemID.Value));
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

        protected void btnMessage_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnMessages(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected String GetImageUrl(object eval)
        {
            var val = Convert.ToString(eval);
            if (val == "Project")
                return "~/App_Themes/Default/Images/projects.png";

            if (val == "Group")
                return "~/App_Themes/Default/Images/groups.png";

            if (val == "User")
                return "~/App_Themes/Default/Images/users.png";

            return "#";
        }

        protected bool GetEditVisible(object eval)
        {
            var val = Convert.ToString(eval);
            if (val == "Group")
                return true;

            return false;
        }

        protected bool GetDeleteVisible(object eval)
        {
            var val = Convert.ToString(eval);
            if (val == "Group" || val == "User")
                return true;

            return false;
        }

        protected bool GetAddUserVisible(object eval)
        {
            var val = Convert.ToString(eval);
            if (val == "Group")
                return true;

            return false;
        }

        protected bool GetNewGroupVisible(object eval)
        {
            var val = Convert.ToString(eval);
            if (val == "Group" || val == "Project")
                return true;

            return false;
        }

        protected bool GetAttrSetVisible(object eval)
        {
            var val = Convert.ToString(eval);
            if (val == "Group")
                return true;

            return false;
        }

        protected bool GetAttrViewVisible(object eval)
        {
            var val = Convert.ToString(eval);
            if (val == "Group")
                return true;

            return false;
        }

        protected bool GetMessageVisible(object eval)
        {
            var val = Convert.ToString(eval);
            if (val == "Group")
                return true;

            return false;
        }
    }
}