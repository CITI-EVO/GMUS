using System;
using System.ComponentModel;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Common;
using CITI.EVO.Tools.Web.UI.Configs.ImageLinkButton;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [ParseChildren(true, "Text")]
    [ToolboxData("<{0}:ImageLinkButton runat=\"server\"></{0}:ImageLinkButton>")]
    [SupportsEventValidation]
    public class ImageLinkButton : System.Web.UI.WebControls.LinkButton, ITranslatable, ICommand, IPermissionDependent
    {
        #region Configuration

        private static readonly ImageLinkButtonSection configSection = (ImageLinkButtonSection)ConfigurationManager.GetSection("imageLinkButton");

        #endregion

        #region Private Controls

        private readonly Image imageControl;
        private readonly Label labelControl;
        private readonly HyperLink hyperLink;

        #endregion

        #region Public Constructors

        public ImageLinkButton()
        {
            imageControl = new Image();
            labelControl = new Label();
            hyperLink = new HyperLink();
            IncludeInResources = true;
        }

        #endregion

        #region Public Properties

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public new String Text
        {
            get { return labelControl.Text; }
            set { labelControl.Text = value; }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        public String ImageKey
        {
            get
            {
                return Convert.ToString(ViewState["ImageKey"]);
            }
            set
            {
                var currentImageKey = Convert.ToString(ViewState["ImageKey"]);
                if (String.Equals(currentImageKey, value, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                ViewState["ImageKey"] = value;

                ChangeCurrentImageUrl();
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [UrlProperty]
        public String DefaultImageUrl
        {
            get
            {
                return Convert.ToString(ViewState["DefaultImageUrl"]);
            }
            set
            {
                ViewState["DefaultImageUrl"] = value;

                ChangeCurrentImage();
            }
        }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool IncludeInResources { get; set; }

        [Category("Appearance")]
        [DefaultValue("")]
        [UrlProperty]
        public String DisabledImageUrl
        {
            get
            {
                return Convert.ToString(ViewState["DisabledImageUrl"]);
            }
            set
            {
                ViewState["DisabledImageUrl"] = value;

                ChangeCurrentImage();
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [UrlProperty]
        public String DefaultCssClass
        {
            get
            {
                return Convert.ToString(ViewState["DefaultCssClass"]);
            }
            set
            {
                ViewState["DefaultCssClass"] = value;

                ChangeCurrentCssClass();
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [UrlProperty]
        public String DisabledButtonCssClass
        {
            get
            {
                return Convert.ToString(ViewState["DisabledCssClass"]);
            }
            set
            {
                ViewState["DisabledCssClass"] = value;

                ChangeCurrentCssClass();
            }
        }

        [Category("Navigation")]
        [DefaultValue("")]
        [UrlProperty]
        public String NavigateUrl
        {
            get { return hyperLink.NavigateUrl; }
            set { hyperLink.NavigateUrl = value; }
        }

        [Category("Navigation")]
        [DefaultValue("")]
        [UrlProperty]
        public String Target
        {
            get { return hyperLink.Target; }
            set { hyperLink.Target = value; }
        }

        [Category("Behavior")]
        [DefaultValue("")]
        [UrlProperty]
        public new String ToolTip
        {
            get { return base.ToolTip; }
            set { base.ToolTip = hyperLink.ToolTip = value; }
        }

        [Category("Appearance")]
        [DefaultValue(true)]
        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = hyperLink.Enabled = value;

                ChangeCurrentImage();
                ChangeCurrentCssClass();
            }
        }

        StateBag ITranslatable.ViewState
        {
            get { return ViewState; }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        public String TrnKey
        {
            get { return labelControl.TrnKey; }
            set { labelControl.TrnKey = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String PermissionKey { get; set; }

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		public bool DisableIfNoAccess { get; set; }

		#endregion

		#region Private Properties

		#endregion

		#region Methods

		private ImageUrlElement GetImageUrlElement(String key)
        {
            if (configSection != null && configSection.ImageUrls != null)
            {
                foreach (ImageUrlElement imageUrlElement in configSection.ImageUrls)
                {
                    if (String.Equals(imageUrlElement.ImageKey, key, StringComparison.OrdinalIgnoreCase))
                    {
                        return imageUrlElement;
                    }
                }
            }

            return null;
        }

        private void ChangeCurrentImageUrl()
        {
            DefaultImageUrl = null;
            DisabledImageUrl = null;

            var imageUrlElement = GetImageUrlElement(ImageKey);
            if (imageUrlElement != null)
            {
                DefaultImageUrl = imageUrlElement.DefaultImageUrl;
                DisabledImageUrl = imageUrlElement.DisabledImageUrl;
            }

            ChangeCurrentImage();
        }

        private void ChangeCurrentImage()
        {
            imageControl.ImageUrl = DefaultImageUrl;

            if (!Enabled && !String.IsNullOrEmpty(DisabledImageUrl))
            {
                imageControl.ImageUrl = DisabledImageUrl;
            }
        }

        private void ChangeCurrentCssClass()
        {
            base.CssClass = DefaultCssClass;
            if (!Enabled && !String.IsNullOrEmpty(DisabledButtonCssClass))
            {
                base.CssClass = DisabledButtonCssClass;
            }
        }

		public bool HasAccess()
		{
			return PermissionUtil.HasAccess(this);
		}

		#endregion

		#region Protected Overrides

		public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!Enabled)
            {
                OnClientClick = String.Empty;
            }

            Control renderControl = this;
            if (!String.IsNullOrWhiteSpace(NavigateUrl))
            {
                renderControl = hyperLink;
            }

            if (!String.IsNullOrWhiteSpace(imageControl.ImageUrl))
            {
                renderControl.Controls.Add(imageControl);
            }

            renderControl.Controls.Add(labelControl);

            if (String.IsNullOrWhiteSpace(NavigateUrl))
            {
                base.RenderBeginTag(writer);
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (String.IsNullOrWhiteSpace(NavigateUrl))
            {
                base.RenderEndTag(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            TranslationUtil.ApplyTranslation(this);
			PermissionUtil.ApplyPermission(this);

			if (!Enabled)
            {
                OnClientClick = String.Empty;
            }

            if (!String.IsNullOrWhiteSpace(NavigateUrl))
            {
                hyperLink.RenderControl(writer);
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            TranslationUtil.ApplyTranslation(this);
			PermissionUtil.ApplyPermission(this);

			if (!Enabled)
            {
                OnClientClick = String.Empty;
            }

            base.OnPreRender(e);
        }

        #endregion
    }
}
