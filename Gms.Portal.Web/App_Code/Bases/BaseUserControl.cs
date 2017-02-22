using System;
using System.Web.UI;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.TwoWayModel.UIBases.Generic;
using NHibernate;

namespace Gms.Portal.Web.Bases
{
    public class BaseUserControl : UserControl
    {
        public BaseUserControl()
        {
        }

        private BasePage CurrentBasePage
        {
            get
            {
                return base.Page as BasePage;
            }
        }

        public ISession HbSession
        {
            get
            {
                if (Page == null)
                    throw new InvalidCastException("Can not convert to base page type.");

                return CurrentBasePage.HbSession;
            }
        }

        protected Control PagePostBackControl
        {
            get
            {
                return CurrentBasePage.PostBackControl;
            }
        }
    }

    public class BaseUserControl<TModel> : UserControlModelBase<TModel> where TModel : class, new()
    {
        public BaseUserControl()
        {

        }

        public event EventHandler DataChanged;
        protected virtual void OnDataChanged(EventArgs e)
        {
            if (DataChanged != null)
                DataChanged(this, e);
        }

        protected UrlHelper RequestUrl
        {
            get
            {
                return CurrentBasePage.RequestUrl;
            }
        }

        private BasePage CurrentBasePage
        {
            get
            {
                return base.Page as BasePage;
            }
        }

        public ISession HbSession
        {
            get
            {
                if (Page == null)
                    throw new InvalidCastException("Can not convert to base page type.");

                return CurrentBasePage.HbSession;
            }
        }

        protected Control PagePostBackControl
        {
            get
            {
                return CurrentBasePage.PostBackControl;
            }
        }

        protected Object GetTrimedText(object eval)
        {
            return GetTrimedText(eval, 28);
        }

        protected Object GetTrimedText(object eval, int length)
        {
            var text = Convert.ToString(eval);
            if (String.IsNullOrWhiteSpace(text))
                return String.Empty;

            if (text.Length <= length)
                return text;

            var part = String.Concat(text.Substring(0, length - 3), "...");
            return part;
        }
    }
}