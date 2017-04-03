using System;
using NHibernate;

namespace Gms.Portal.Web.Bases
{
    public class MasterPageBase : CITI.EVO.Tools.Web.Bases.MasterPageBase
    {
        private BasePage CurrentBasePage
        {
            get
            {
                return Page as BasePage;
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
    }
}