using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Common;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class AutoActiveLink : System.Web.UI.WebControls.HyperLink, ITranslatable, IPermissionDependent
    {
        private String text;
        private String trnKey;

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

        [Localizable(false), Bindable(true)]
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
