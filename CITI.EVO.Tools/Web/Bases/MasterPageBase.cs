using System;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using AjaxControlToolkit;
using CITI.EVO.Tools.Collections;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Controls;
using Button = System.Web.UI.WebControls.Button;
using Panel = System.Web.UI.WebControls.Panel;

namespace CITI.EVO.Tools.Web.Bases
{
    public class MasterPageBase : MasterPage
    {
        public PageBase BasePage
        {
            get { return Page as PageBase; }
        }

        private UrlHelper _requestUrl;
        public UrlHelper RequestUrl
        {
            get
            {
                _requestUrl = (_requestUrl ?? Request.RequestUrl());
                return _requestUrl;
            }
        }

        private Control _postBackControl;
        public Control PostBackControl
        {
            get
            {
                _postBackControl = (_postBackControl ?? GetPostBackControl());
                return _postBackControl;
            }
        }

        public NameObjectCollection PageSession
        {
            get { return BasePage.PageSession; }
        }

        protected override void OnInit(EventArgs e)
        {
            UmUtil.Instance.Login();
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            DisplayPopupsIfNeeded();
            base.OnLoad(e);
        }

        private void DisplayPopupsIfNeeded()
        {
            var enabled = DataConverter.ToNullableBool(ConfigurationManager.AppSettings["PopupsAutoControl"]);
            if (!enabled.GetValueOrDefault())
                return;

            var target = PostBackControl;
            if (target == null)
                return;

            var parents = UserInterfaceUtil.TraverseParents(target).OfType<Panel>();

            var parentPopup = parents.OfType<ModalPopup>().FirstOrDefault();
            if (parentPopup != null)
            {
                parentPopup.Show();
                return;
            }

            var root = ((Control)base.Master ?? this);

            var popups = UserInterfaceUtil.TraverseChildren(root).OfType<ModalPopupExtender>();

            var @set = parents.Select(n => n.ID).ToHashSet();
            var pops = popups.Where(n => @set.Contains(n.PopupControlID));

            foreach (var item in pops)
                item.Show();
        }

        private Control GetPostBackControl()
        {
            Control control = null;
            var request = Page.Request;

            var ctrlName = request.Params["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(ctrlName))
            {
                control = Page.FindControl(ctrlName);
            }
            else
            {
                //handle the Button control postbacks
                foreach (String ctl in request.Form)
                {
                    var ctrl = Page.FindControl(ctl);
                    if (ctrl is Button)
                    {
                        control = ctrl;
                        break;
                    }
                }
            }

            //handle the ImageButton control postbacks
            if (control == null)
            {
                for (int i = 0; i < request.Form.Count; i++)
                {
                    var key = request.Form.Keys[i];
                    if (!String.IsNullOrWhiteSpace(key) && (key.EndsWith(".x") || key.EndsWith(".y")))
                    {
                        control = Page.FindControl(request.Form.Keys[i].Substring(0, key.Length - 2));
                        return control;
                    }
                }
            }

            return control;
        }

    }
}