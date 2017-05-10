using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Utils;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Pages.User
{
    public partial class Register : BasePage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var urlHelper = new UrlHelper(Request.Url);

            urlHelper[LanguageUtil.RequestLanguageKey] = "en-US";
            btEngLang.NavigateUrl = urlHelper.ToEncodedUrl();

            urlHelper[LanguageUtil.RequestLanguageKey] = "ka-GE";
            btGeoLang.NavigateUrl = urlHelper.ToEncodedUrl();
        }

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

        protected void btOK_Click(object sender, EventArgs e)
        {
            var model = registerUserControl.Model;

            if (model.GroupID == null)
            {
                lblError.Text = "გთხოვთ, აირჩიეთ ჯგუფი";
                return;
            }

            if (String.IsNullOrWhiteSpace(model.LoginName))
            {
                lblError.Text = "გთხოვთ, შეავსოთ სავალდებულო ველები";
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

            var dbUser = (from n in HbSession.Query<UM_User>()
                          where n.DateDeleted == null &&
                                (
                                    n.LoginName.ToLower() == model.LoginName.ToLower() ||
                                    n.Email.ToLower() == model.LoginName.ToLower()
                                )
                          select n).FirstOrDefault();

            if (dbUser != null)
            {
                lblError.Text = "მომხმარებელი მითითებული ელ.ფოსტის მისამარტთ უკვე რეგისტრირებულია";
                return;
            }

            using (var transaction = HbSession.BeginTransaction())
            {
                dbUser = new UM_User
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
                    GroupUsers = new List<UM_GroupUser>()
                };

                HbSession.Save(dbUser);

                var @group = HbSession.Query<UM_Group>().FirstOrDefault(n => n.ID == model.GroupID);
                if (@group != null)
                {
                    var groupUser = new UM_GroupUser
                    {
                        ID = Guid.NewGuid(),
                        DateCreated = DateTime.Now,
                        GroupID = @group.ID,
                        UserID = dbUser.ID,
                        AccessLevel = 0
                    };

                    HbSession.Save(groupUser);
                }

                try
                {
                    if (ConfigUtil.UserActivationEnabled)
                        EmailUtil.SendActivationEmail(dbUser);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            pnlCompleted.Visible = true;
            pnlUserData.Visible = false;

            if (ConfigUtil.UserActivationEnabled)
                lblMessage.Text = "რეგისტრაცია წარმატებით დასრულდა, მომხმარებლის გასააქტიურებლად შეამოწმეთ ელ.ფოსტა";
            else
            {
                lblMessage.Text = "რეგისტრაცია წარმატებით დასრულდა, აკტივაციისთვის მიმართეთ ადმინისტრატორს";
                lblAdminEmail.Text = ConfigurationManager.AppSettings["AdminEmail"];
            }
        }
    }
}