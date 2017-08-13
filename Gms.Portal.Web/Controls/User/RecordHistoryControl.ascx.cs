using System;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.User
{
    public partial class RecordHistoryControl : BaseUserControlExtend<RecordHistoryModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected String GetUserName(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue("UserID"));
            if (userID == null)
                return null;

            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            var name = $"{user.LoginName} - {user.FirstName} {user.LastName}";
            return name;
        }

        protected String GetDownloadUrl(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var itemID = DataConverter.ToNullableGuid(descriptor.GetValue("ID"));
            if (itemID == null)
                return null;

            var url = new UrlHelper("~/Handlers/Download.ashx")
            {
                ["HistoryID"] = itemID,
                ["LoginToken"] = UmUtil.Instance.CurrentToken
            };

            return url.ToEncodedUrl();
        }
    }
}