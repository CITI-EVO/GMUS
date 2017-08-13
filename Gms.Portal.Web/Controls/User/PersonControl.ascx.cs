using System;
using System.Collections.Generic;
using CITI.EVO.Proxies;
using Gms.Portal.Web.Helpers;

namespace Gms.Portal.Web.Controls.User
{
    public partial class PersonControl : System.Web.UI.UserControl, ICustomMetaControl
    {
        public Object Value
        {
            get { return tbxPersonalID.Text; }
            set { tbxPersonalID.Text = Convert.ToString(value); }
        }

        public IDictionary<String, Object> GetFullContent()
        {
            var dict = new Dictionary<String, Object>();

            if (String.IsNullOrWhiteSpace(tbxPersonalID.Text) ||
                String.IsNullOrWhiteSpace(seBirthYear.Text))
                return dict;

            var personData = CommonProxy.GetPerson(tbxPersonalID.Text, seBirthYear.Text);
            if (personData == null)
                return dict;

            dict["LastName"] = personData.LastName;
            dict["FirstName"] = personData.FirstName;
            dict["BirthDate"] = personData.BirthDate;
            dict["PersonalID"] = personData.PersonalID;
            dict["PersonStatus"] = personData.PersonStatus;
            dict["PersonStatusId"] = personData.PersonStatusId;
            dict["CitizenshipCountry"] = personData.CitizenshipCountry;
            dict["CitizenshipCountryID"] = personData.CitizenshipCountryID;

            return dict;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnOK_OnClick(object sender, EventArgs e)
        {
        }
    }
}