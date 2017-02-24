using System;
using System.Linq;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class UserDataControl : BaseUserControlExtend<UserModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillUserCategories();
        }

        protected void chkChangePassword_CheckChanged(object sender, EventArgs e)
        {
            var model = Model;

            if (chkChangePassword.Checked)
            {
                var user = HbSession.Query<UM_User>().FirstOrDefault(u => u.ID == model.ID);
                if (user == null)
                    return;

                tbPassword.Enabled = true;
                tbPassword.Text = user.Password;
            }
            else
            {
                tbPassword.Enabled = false;
                tbPassword.Text = String.Empty;
            }

            OnDataChanged(e);
        }

        protected void FillUserCategories()
        {
            var userCategoris = HbSession.Query<UM_UserCategory>().Where(n => n.DateDeleted == null).ToList();

            var item = new UM_UserCategory();
            item.ID = Guid.Empty;
            item.DateCreated = DateTime.Now;
            item.Name = "ყველა";

            userCategoris.Insert(0, item);

            cbxUserCategory.BindData(userCategoris);
        }
    }
}