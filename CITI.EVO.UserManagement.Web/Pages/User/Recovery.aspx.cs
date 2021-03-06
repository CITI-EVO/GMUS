﻿using System;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Pages.User
{
    public partial class Recovery : BasePage
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
        }

        protected void btnOK_OnClick(object sender, EventArgs e)
        {
            var userCode = Request["userCode"];
            if (String.IsNullOrWhiteSpace(userCode))
            {
                lblError.Text = "მომხმარებელი ვერ მოიძებნა";
                return;
            }

            var user = (from n in HbSession.Query<UM_User>()
                        where n.DateDeleted == null &&
                              n.UserCode == userCode
                        select n).FirstOrDefault();

            if (user == null)
            {
                lblError.Text = "მომხმარებელი ვერ მოიძებნა";
                return;
            }

            var model = recoveryControl.Model;

            if (String.IsNullOrWhiteSpace(model.NewPassword))
            {
                lblError.Text = "გთხოვთ შეიყვანეთ პაროლი";
                return;
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                lblError.Text = "პაროლები არ ემთხვევა ერთმანეთს";
                return;
            }

            user.Password = model.NewPassword;
            user.PasswordExpirationDate = DateTime.Now.AddMonths(1);
            user.UserCode = null;

            pnlCompleted.Visible = true;
            pnlUserData.Visible = false;

            HbSession.SubmitUpdate(user);
        }
    }
}