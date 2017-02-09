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
    [ControlValueProperty("Checked")]
    [DefaultEvent("CheckedChanged")]
    [DefaultProperty("Text")]
    [SupportsEventValidation]
    public class RadioButton : System.Web.UI.WebControls.RadioButton, ITranslatable, ICommand, IPermissionDependent
    {
        private String text;
        private String trnKey;

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool IncludeInResources { get; set; }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        public String TrnKey
        {
            get
            {
                if (String.IsNullOrEmpty(trnKey))
                {
                    return String.Empty;
                }

                return trnKey;
            }
            set
            {
                trnKey = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public override String Text
        {
            get
            {
                if (String.IsNullOrEmpty(text))
                {
                    return String.Empty;
                }

                return text;
            }
            set
            {
                text = value;
            }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        public String CommandArgument
        {
            get
            {
                return Convert.ToString(ViewState["CommandArgument"]);
            }
            set
            {
                ViewState["CommandArgument"] = value;
            }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        public String CommandName
        {
            get
            {
                return Convert.ToString(ViewState["CommandName"]);
            }
            set
            {
                ViewState["CommandName"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String PermissionKey { get; set; }

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		public bool DisableIfNoAccess { get; set; }

		protected override void RenderContents(HtmlTextWriter writer)
        {
            TranslationUtil.ApplyTranslation(this);
			PermissionUtil.ApplyPermission(this);

            base.Text = Text;
			base.RenderContents(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            TranslationUtil.ApplyTranslation(this);
			PermissionUtil.ApplyPermission(this);

            base.Text = Text;
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
