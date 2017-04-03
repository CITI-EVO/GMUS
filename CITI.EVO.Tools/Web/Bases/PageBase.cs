using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using CITI.EVO.Tools.Collections;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.Tools.Web.Bases
{
    public class PageBase : Page
    {
        private ILookup<String, Control> _controlsCache;

        public MasterPageBase BaseMaster
        {
            get { return base.Master as MasterPageBase; }
        }

        public String PageTitle
        {
            get { return Title; }
            set { Title = value; }
        }

        private Control _postBackControl;
        public Control PostBackControl
        {
            get
            {
                if (BaseMaster != null)
                    return BaseMaster.PostBackControl;

                _postBackControl = (_postBackControl ?? GetPostBackControl());
                return _postBackControl;
            }
        }

        private UrlHelper _requestUrl;
        public UrlHelper RequestUrl
        {
            get
            {
                if (Master != null)
                    return BaseMaster.RequestUrl;

                _requestUrl = (_requestUrl ?? Request.RequestUrl());
                return _requestUrl;
            }
        }

        public NameObjectCollection PageSession
        {
            get { return InitPageSessionData(); }
        }

        protected override void OnInit(EventArgs e)
        {
            if (base.Master == null)
                UmUtil.Instance.Login();

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (BaseMaster == null)
                DisplayPopupsIfNeeded();

            base.OnLoad(e);
        }

        protected override Object LoadPageStateFromPersistenceMedium()
        {
            var compress = ConfigurationManager.AppSettings["PageSettings:CompressViewState"];
            compress = (compress ?? String.Empty).ToLower();

            if (compress != "lz" && compress != "def")
            {
                return base.LoadPageStateFromPersistenceMedium();
            }

            var viewState = Request.Form["__VSTATE"];
            var bytes = Convert.FromBase64String(viewState);

            var formatter = new LosFormatter();

            switch (compress.ToLower())
            {
                case "lz":
                    bytes = bytes.DecompressLZ();
                    break;
                case "def":
                    bytes = bytes.DecompressDef();
                    break;
            }

            var pageState = formatter.Deserialize(Convert.ToBase64String(bytes));
            return pageState;
        }

        protected override void SavePageStateToPersistenceMedium(Object viewState)
        {
            var compress = ConfigurationManager.AppSettings["PageSettings:CompressViewState"];
            compress = (compress ?? String.Empty).ToLower();

            if (compress != "lz" && compress != "def")
            {
                base.SavePageStateToPersistenceMedium(viewState);
                return;
            }

            using (var writer = new StringWriter())
            {
                var formatter = new LosFormatter();
                formatter.Serialize(writer, viewState);

                var viewStateString = writer.ToString();
                var bytes = Convert.FromBase64String(viewStateString);

                switch (compress.ToLower())
                {
                    case "lz":
                        bytes = bytes.CompressLZ();
                        break;
                    case "def":
                        bytes = bytes.CompressDef();
                        break;
                }

                ClientScript.RegisterHiddenField("__VSTATE", Convert.ToBase64String(bytes));
            }
        }

        public Control FindControlRec(String controlID)
        {
            if (_controlsCache == null)
            {
                var allControls = UserInterfaceUtil.TraverseChildren(this);
                _controlsCache = allControls.ToLookup(n => n.ID);
            }

            var result = _controlsCache[controlID];
            return result.FirstOrDefault();
        }

        protected void DisplayPopupsIfNeeded()
        {
            var enabled = DataConverter.ToNullableBool(ConfigurationManager.AppSettings["PopupsAutoControl"]);
            if (!enabled.GetValueOrDefault())
                return;

            var target = PostBackControl;
            if (target == null)
                return;

            var root = ((Control)base.Master ?? this);

            var popups = UserInterfaceUtil.TraverseChildren(root).OfType<ModalPopupExtender>();
            var parents = UserInterfaceUtil.TraverseParents(target).OfType<Panel>();

            var @set = parents.Select(n => n.ID).ToHashSet();
            var pops = popups.Where(n => @set.Contains(n.PopupControlID));

            foreach (var item in pops)
                item.Show();
        }

        private NameObjectCollection InitPageSessionData()
        {
            var sessinID = InitPageSessionID();

            var nameObjectCollection = Session[sessinID] as NameObjectCollection;
            if (nameObjectCollection == null)
            {
                nameObjectCollection = new NameObjectCollection();
                Session[sessinID] = nameObjectCollection;
            }

            return nameObjectCollection;
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

        private String InitPageSessionID()
        {
            var sessinID = Convert.ToString(RequestUrl["PageSession"]);
            if (String.IsNullOrWhiteSpace(sessinID))
            {
                //throw new Exception("Invalid page session ID");

                sessinID = Convert.ToString(ViewState["PageSession"]);
                if (String.IsNullOrWhiteSpace(sessinID))
                {
                    sessinID = Convert.ToString(Guid.NewGuid());
                }
            }

            ViewState["PageSession"] = sessinID;

            return sessinID;
        }
    }
}