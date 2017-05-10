using System;
using System.Linq;
using System.Net.Mime;
using System.Web;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Models;
using NHibernate.Linq;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class NotificationHistory : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillNotificationsGrid();
        }

        private void FillNotificationsGrid()
        {
            var recipientId = DataConverter.ToNullableGuid(RequestUrl["RecipientID"]);

            var entities = (from n in HbSession.Query<GM_NotificationHistory>()
                            where n.DateDeleted == null && n.RecipientID == recipientId
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new NotificationHistoryEntityModelConverter(HbSession);

            var models = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            var mailModel = new NotificationsHistoryModel();
            mailModel.List = models.Where(n => n.ContactType == "Mail" || n.ContactType == "All").ToList();

            var phoneModel = new NotificationsHistoryModel();
            phoneModel.List = models.Where(n => n.ContactType == "Sms" || n.ContactType == "All").ToList();

            mailNotificationsHistoryControl.Model = mailModel;
            mailNotificationsHistoryControl.DataBind();

            phoneNotificationsHistoryControl.Model = phoneModel;
            phoneNotificationsHistoryControl.DataBind();

        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            var returnUrl = DataConverter.ToString(RequestUrl["ReturnUrl"]);
            var cleanUrl = GmsCommonUtil.ConvertFromBase64(returnUrl);

            var returnUrlHelper = new UrlHelper(cleanUrl);
            Response.Redirect(returnUrlHelper.ToEncodedUrl());
        }


    }
}