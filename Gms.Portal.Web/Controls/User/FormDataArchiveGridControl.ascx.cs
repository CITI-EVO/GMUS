using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Enums;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormDataArchiveGridControl : BaseUserControlExtend<FormDataArchiveGridModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected bool GetPrintVisible(object dataItem)
        {
            return true;
        }

        protected String GetFormName(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return String.Empty;

            var formID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FormIDField));
            if (formID == null)
                return String.Empty;

            var formsCache = CommonObjectCache.InitObject("Forms", CommonCacheStore.Request, GetFormNames);
            return formsCache.GetValueOrDefault(formID.Value);
        }

        protected String GetCommandArg(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var recordID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.IDField));
            if (recordID == null)
                return null;

            var formID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FormIDField));
            if (formID == null)
                return null;

            return $"{recordID}/{formID}";
        }

        protected String GetViewUrl(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return "#";

            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = Convert.ToString(FormMode.View),
                ["FormID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["OwnerID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["RecordID"] = descriptor.GetValue(FormDataConstants.IDField),
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
            };

            return urlHelper.ToEncodedUrl();
        }

        protected String GetPrintUrl(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return "#";

            var urlHelper = new UrlHelper("~/Handlers/PrintFormData.ashx")
            {
                ["FormID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["RecordID"] = descriptor.GetValue(FormDataConstants.IDField),
                ["LoginToken"] = UmUtil.Instance.CurrentToken,
            };

            return urlHelper.ToEncodedUrl();
        }

        protected IDictionary<Guid?, String> GetFormNames()
        {
            var query = (from n in HbSession.Query<GM_Form>()
                         where n.DateDeleted == null
                         select new IDNameEntity
                         {
                             ID = n.ID,
                             Name = n.Name
                         });

            var dict = new Dictionary<Guid?, String>();

            foreach (var item in query)
                dict.Add(item.ID, item.Name);

            return dict;
        }
    }
}