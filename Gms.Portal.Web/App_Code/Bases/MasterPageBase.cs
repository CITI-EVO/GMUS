using System;
using CITI.EVO.UserManagement.Web.Bases;
using NHibernate;

namespace Gms.Portal.Web.Bases
{
    /// <summary>
    /// Summary description for MasterPageBase
    /// </summary>
    public class MasterPageBase : CITI.EVO.Tools.Web.Bases.MasterPageBase
    {
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
    }
}