using System;
using System.ComponentModel;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;
using DevExpress.Web;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class ASPxGridViewDataTextColumn : GridViewDataTextColumn, IPermissionDependent
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String PermissionKey { get; set; }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool DisableIfNoAccess { get; set; }

        public bool Enabled
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public bool HasAccess()
        {
            return PermissionUtil.HasAccess(this);
        }
    }
}
