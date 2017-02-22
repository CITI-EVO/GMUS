using System;
using System.ComponentModel;
using System.Web.UI;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Common;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    [ParseChildren(false)]
    [DefaultProperty("Text")]
    [ControlValueProperty("Text")]
    public class HtmlElement : System.Web.UI.HtmlControls.HtmlGenericControl, ITranslatable, IPermissionDependent
    {
        public HtmlElement(String tag) : base(tag)
        {
        }

        private String trnKey;

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        public String TrnKey
        {
            get
            {
                if (String.IsNullOrEmpty(trnKey))
                    return String.Empty;

                return trnKey;
            }
            set
            {
                trnKey = value;
            }
        }

        [Localizable(false), Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public String Text
        {
            get
            {
                if (!IsLiteralContent())
                    return null;

                return InnerText;
            }
            set
            {
                if (IsLiteralContent())
                    InnerText = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String CssClass
        {
            get { return Attributes["class"]; }
            set { Attributes["class"] = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String PermissionKey { get; set; }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool DisableIfNoAccess { get; set; }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            TranslationUtil.ApplyTranslation(this);
            PermissionUtil.ApplyPermission(this);

            base.Render(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            TranslationUtil.ApplyTranslation(this);
            PermissionUtil.ApplyPermission(this);

            base.OnPreRender(e);
        }

        public bool HasAccess()
        {
            return PermissionUtil.HasAccess(this);
        }

        StateBag ITranslatable.ViewState
        {
            get { return ViewState; }
        }
    }
}
