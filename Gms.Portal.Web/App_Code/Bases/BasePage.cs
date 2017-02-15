using System;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.Bases;
using NHibernate;


namespace CITI.EVO.UserManagement.Web.Bases
{
    public class BasePage : PageBase
    {
        public BasePage()
        {
        }

        private ISession hbSession;
        public ISession HbSession
        {
            get
            {
                hbSession = (hbSession ?? Hb8Factory.CreateSession());
                return hbSession;
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            if (hbSession != null)
                hbSession.Dispose();

            base.OnUnload(e);
        }
    }
}