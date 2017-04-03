using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Common;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    [ParseChildren(false)]
    [DefaultProperty("Text")]
    [ControlValueProperty("Text")]
    public class HtmlLabel : WebControl, ITextControl, ITranslatable, IPermissionDependent
    {
        private String trnKey;
        private String permissionKey;
        private LiteralControl control;

        public HtmlLabel() : base("label")
        {
            control = new LiteralControl();
        }

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
                if (String.IsNullOrEmpty(control.Text))
                    return String.Empty;

                return control.Text;
            }
            set
            {
                control.Text = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String PermissionKey
        {
            get
            {
                if (String.IsNullOrEmpty(permissionKey))
                    return String.Empty;

                return permissionKey;
            }
            set
            {
                permissionKey = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool DisableIfNoAccess { get; set; }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            Controls.Add(control);

            base.RenderBeginTag(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            TranslationUtil.ApplyTranslation(this);
            PermissionUtil.ApplyPermission(this);

            base.RenderContents(writer);
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