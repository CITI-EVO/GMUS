using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using DevExpress.XtraPrinting.Native;
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
using CITI.EVO.Tools.Extensions;

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
            liReport.Visible = false;

            if (UmUtil.Instance.IsLogged && UmUtil.Instance.CurrentUser.IsSuperAdmin)
            {
                liAdmin.Visible = true;
                liTrnMode.Visible = true;
                liReport.Visible = true;

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
            if (!UmUtil.Instance.IsLogged)
            {
                UmUtil.Instance.GoToLogin();
                return;
            }

            if (UmUtil.Instance.IsPasswordExpired)
            {
                UmUtil.Instance.GoToChangePassword();
                return;
            }

            liDataApprove.Visible = UmUtil.Instance.HasAccess("Org");

            var loginToken = Convert.ToString(UmUtil.Instance.CurrentToken);

            foreach (var control in UserInterfaceUtil.TraverseChildren(this))
            {
                var link = control as HyperLink;
                if (link != null)
                    link.NavigateUrl = link.NavigateUrl.Replace("{loginToken}", loginToken);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var query = from n in HbSession.Query<GM_Category>()
                        where n.DateDeleted == null &&
                              n.Visible == true
                        select n;

            var mandatoryFormID = UserUtil.GetMandatoryFormID();
            if (NeedToFillForm(mandatoryFormID))
            {
                var formID = DataConverter.ToNullableGuid(RequestUrl["FormID"]);
                var ownerID = DataConverter.ToNullableGuid(RequestUrl["OwnerID"]);

                if (mandatoryFormID != formID && mandatoryFormID != ownerID)
                {
                    var urlHelper = new UrlHelper("~/Pages/User/FormDataGrid.aspx")
                    {
                        ["FormID"] = mandatoryFormID,
                        ["OwnerID"] = mandatoryFormID
                    };

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
                var formID = DataConverter.ToNullableGuid(RequestUrl["FormID"]);
                current = (from n in controls
                           let h = n as HyperLink
                           where h != null
                           let Id = GetFormID(h.NavigateUrl)
                           where formID == Id
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

        protected Guid? GetFormID(String relativeUrl)
        {
            var helper = new UrlHelper(relativeUrl);
            return DataConverter.ToNullableGuid(helper["FormID"]);
        }

        protected String GetFullUrl(String relativeUrl)
        {
            var root = Request.Url.GetLeftPart(UriPartial.Authority);
            var fullUrl = root + Page.ResolveUrl(relativeUrl);
            return fullUrl;
        }

        protected bool NeedToFillForm(Guid? mandatoryFormID)
        {
            if (mandatoryFormID == null || UmUtil.Instance.HasAccess("Admin"))
                return false;

            var mandatoryFormStatus = UserUtil.GetMandatoryFormStatus();
            if (mandatoryFormStatus == null)
                return false;

            var userID = UserUtil.GetCurrentUserID();
            if (userID == null)
                return false;

            var collection = MongoDbUtil.GetCollection(mandatoryFormID);

            var query = (from n in collection.AsQueryable()
                         where n[FormDataConstants.UserIDField] == userID &&
                               n[FormDataConstants.DateDeletedField] == (DateTime?)null
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

        protected IEnumerable<GM_Form> GetCategoryForms(GM_Category item)
        {
            var forms = (from n in item.Forms
                         where n.DateDeleted == null
                         orderby n.OrderIndex, n.Name
                         select n);

            foreach (var form in forms)
            {
                if (!form.Visible.GetValueOrDefault())
                    continue;

                if (!UmUtil.Instance.CurrentUser.IsSuperAdmin && !string.IsNullOrWhiteSpace(form.VisibleExpression))
                {
                    var expNode = ExpressionParser.GetOrParse(form.VisibleExpression);
                    var expGlobals = new ExpressionGlobalsUtil();

                    Object eval;
                    if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out eval))
                        continue;

                    var result = DataConverter.ToNullableBoolean(eval);
                    if (!result.GetValueOrDefault())
                        continue;
                }

                yield return form;
            }

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

                var forms = GetCategoryForms(item);

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
