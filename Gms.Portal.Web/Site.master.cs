using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Helpers;
using Gms.Portal.Web.Utils;
using MongoDB.Driver;
using NHibernate.Linq;
using MongoDB.Driver.Linq;

namespace Gms.Portal.Web
{
    public partial class Site : MasterPageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var urlHelper = new UrlHelper(RequestUrl);

            urlHelper[LanguageUtil.RequestLanguageKey] = "en-US";
            btEngLang.NavigateUrl = urlHelper.ToEncodedUrl();

            urlHelper[LanguageUtil.RequestLanguageKey] = "ka-GE";
            btGeoLang.NavigateUrl = urlHelper.ToEncodedUrl();

            liAdmin.Visible = false;
            liTrnMode.Visible = false;

            if (UmUtil.Instance.IsLogged && UmUtil.Instance.CurrentUser.IsSuperAdmin)
            {
                liAdmin.Visible = true;
                liTrnMode.Visible = true;

                urlHelper[LanguageUtil.RequestLanguageKey] = LanguageUtil.GetLanguage();

                var trnMode = Convert.ToString(urlHelper[TranslationUtil.RequestTranslationModeKey]);

                if (String.IsNullOrWhiteSpace(trnMode))
                    urlHelper[TranslationUtil.RequestTranslationModeKey] = "ON";
                else
                    urlHelper.Remove(TranslationUtil.RequestTranslationModeKey);

                btTranslationMode.NavigateUrl = urlHelper.ToEncodedUrl();
            }
            else
            {
                btTranslationMode.NavigateUrl = "#";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UmUtil.Instance.IsLogged)
            {
                var loginToken = Convert.ToString(UmUtil.Instance.CurrentToken);

                foreach (var control in UserInterfaceUtil.TraverseChildren(this))
                {
                    var link = control as HyperLink;
                    if (link != null)
                        link.NavigateUrl = link.NavigateUrl.Replace("{loginToken}", loginToken);
                }
            }
            else
            {
                UmUtil.Instance.GoToLogin();
            }
        }

        private bool NeedToFillForm(Guid? mandatoryFormID)
        {
            if (UmUtil.Instance.HasAccess("Admin"))
                return false;

            var mandatoryFormStatus = ConfigurationManager.AppSettings["MandatoryFormStatus"];

            if (mandatoryFormID == null)
                return false;

            var userID = UserUtil.GetCurrentUserID();
            if (userID == null)
                return false;

            var collection = MongoDbUtil.GetCollection(mandatoryFormID);

            var query = (from n in collection.AsQueryable()
                         where n[FormDataConstants.UserIDField] == userID &&
                               n[FormDataConstants.DateChangedField] == (DateTime?)null
                         select n);

            var dataStatus = DataStatusCache.GetStatus(mandatoryFormStatus);
            if (dataStatus != null)
            {
                query = (from n in query
                         where n[FormDataConstants.StatusIDField] == dataStatus.ID
                         select n);
            }

            var doc = query.FirstOrDefault();
            if (doc == null)
                return true;

            return false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var query = from n in HbSession.Query<GM_Category>()
                        where n.DateDeleted == null &&
                              n.Visible == true
                        select n;

            var mandatoryFormID = DataConverter.ToNullableGuid(ConfigurationManager.AppSettings["MandatoryUserForm"]);

            if (NeedToFillForm(mandatoryFormID))
            {
                var formID = DataConverter.ToNullableGuid(RequestUrl["FormID"]);
                var ownerID = DataConverter.ToNullableGuid(RequestUrl["OwnerID"]);

                if (mandatoryFormID != formID && mandatoryFormID != ownerID)
                {
                    var urlHelper = new UrlHelper("~/Pages/User/FormDataGrid.aspx");
                    urlHelper["FormID"] = mandatoryFormID;
                    urlHelper["OwnerID"] = mandatoryFormID;

                    Response.Redirect(urlHelper.ToEncodedUrl());
                }

                query = (from n in query
                         from m in n.Forms
                         where m.ID == mandatoryFormID
                         select n);
            }

            query = (from n in query
                     orderby n.OrderIndex, n.Name
                     select n);

            var list = query.ToList();

            categoriesLinksControl.Model = GetCategotiesForms(list);
            categoriesLinksControl.DataBind();

            MakeActiveCurrentLink();
        }

        protected void btLogout_OnClick(object sender, EventArgs e)
        {
            UmUtil.Instance.GoToLogout();
        }

        protected void btChangePassword_Click(object sender, EventArgs e)
        {
            UmUtil.Instance.GoToChangePassword();
        }

        protected void MakeActiveCurrentLink()
        {
            var controls = UserInterfaceUtil.TraverseControls(menuDiv);
            var current = (from n in controls
                           let h = n as HyperLink
                           where h != null
                           let url = GetFullUrl(h.NavigateUrl)
                           where url == Request.Url.ToString()
                           select h).FirstOrDefault();

            if (current == null)
            {
                var directoryUrl = GetDirectoryUrl();

                current = (from n in controls
                           let h = n as HyperLink
                           where h != null
                           let url = GetFullUrl(h.NavigateUrl)
                           where url.StartsWith(directoryUrl) && !url.Contains("#")
                           select h).FirstOrDefault();

            }

            if (current != null)
            {
                var parents = UserInterfaceUtil.TraverseParents(current).OfType<HtmlControl>();

                var parentLi = parents.FirstOrDefault(n => n.TagName.ToLower() == "li");
                if (parentLi != null)
                    parentLi.Attributes["class"] += "active";

                var parentUl = parents.FirstOrDefault(n => n.TagName.ToLower() == "ul");
                if (parentUl != null)
                    parentUl.Attributes["class"] += " in";
            }


        }

        public string GetFullUrl(String relativeUrl)
        {
            var root = Request.Url.GetLeftPart(UriPartial.Authority);
            var fullUrl = root + Page.ResolveUrl(relativeUrl);
            return fullUrl;
        }

        public string GetDirectoryUrl()
        {
            var root = Request.Url.GetLeftPart(UriPartial.Authority);
            var fullUrl = root + Page.ResolveUrl("#");
            return fullUrl.TrimEnd('#');
        }


        protected CategoriesFormsModel GetCategotiesForms(IEnumerable<GM_Category> source)
        {
            var models = new CategoriesFormsModel
            {
                List = new List<CategoryFormModel>()
            };

            foreach (var item in source)
            {
                if (item.Forms == null)
                    continue;

                var forms = item.Forms.Where(n => n.DateDeleted == null);

                var model = new CategoryFormModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    Children = GetCategotiesForms(forms)
                };

                models.List.Add(model);
            }

            return models;
        }

        protected CategoriesFormsModel GetCategotiesForms(IEnumerable<GM_Form> source)
        {
            var models = new CategoriesFormsModel
            {
                List = new List<CategoryFormModel>()
            };

            foreach (var item in source)
            {
                var model = new CategoryFormModel
                {
                    ID = item.ID,
                    Name = item.Name,
                };

                models.List.Add(model);
            }

            return models;
        }
    }
}
