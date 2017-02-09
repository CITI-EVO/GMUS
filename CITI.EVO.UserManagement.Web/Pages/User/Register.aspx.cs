using System;
using System.Drawing;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Utils;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Pages.User
{
    public partial class Register : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ConfigUtil.UserRegisterEnabled)
            {
                lblMessage.Text = "რეგისტრაცია გათიშულია";
                lblMessage.ForeColor = Color.Red;

                pnlCompleted.Visible = true;
                pnlUserData.Visible = false;
            }
            else
                pnlUserData.Visible = true;
        }

        protected void btnOK_OnClick(object sender, EventArgs e)
        {
            var model = registerUserControl.Model;

            if (String.IsNullOrWhiteSpace(model.LoginName))
            {
                lblError.Text = "გთხოვთ შეიყვანეთ იდენტიფიკატორი";
                return;
            }

            if (String.IsNullOrWhiteSpace(model.Email))
            {
                lblError.Text = "გთხოვთ შეიყვანეთ ელ.ფოსტის მისამართი";
                return;
            }

            if (String.IsNullOrWhiteSpace(model.Password))
            {
                lblError.Text = "გთხოვთ შეიყვანეთ პაროლი";
                return;
            }

            if (model.Password != model.ConfirmPassword)
            {
                lblError.Text = "პაროლები არ ემთხვევა ერთმანეთს";
                return;
            }

            var user = (from n in HbSession.Query<UM_User>()
                        where n.DateDeleted == null &&
                              n.LoginName == model.LoginName
                        select n).FirstOrDefault();

            if (user != null)
            {
                lblError.Text = "მომხმარებელი მითითებული ელ.ფოსტის მისამარტთ უკვე რეგისტრირებულია";
                return;
            }

            user = new UM_User
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                IsActive = false,
                LoginName = model.LoginName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                //BirthDate = deBirthDate.Date
                Password = model.Password,
                UserCode = Convert.ToString(Guid.NewGuid()),
                PasswordExpirationDate = DateTime.Now.AddDays(30),
            };

            var @group = HbSession.Query<UM_Group>().FirstOrDefault(n => n.ID == ConfigUtil.UserRegisterGroupID);
            if (@group != null)
            {
                var groupUser = new UM_GroupUser
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    GroupID = @group.ID,
                };

                user.GroupUsers.Add(groupUser);
            }

            if (ConfigUtil.UserActivationEnabled)
                EmailUtil.SendActivationEmail(user);

            HbSession.SubmitInsert(user);

            pnlCompleted.Visible = true;
            pnlUserData.Visible = false;

            if (ConfigUtil.UserActivationEnabled)
                lblMessage.Text = "რეგისტრაცია წარმატებით დასრულდა, აკტივაციისთვის შეამოწმეთ ელ.ფოსტა";
            else
                lblMessage.Text = "რეგისტრაცია წარმატებით დასრულდა, აკტივაციისთვის მიმართეთ ადმინისტრატორს";
        }
    }
}