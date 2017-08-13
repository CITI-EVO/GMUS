using System;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.User
{
    public partial class ChooseTemplateControl : BaseUserControlExtend<ChooseTemplateModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindTemplates(FormEntity formEntity)
        {
            if (formEntity == null || formEntity.Templates == null)
                return;

            var query = (from n in formEntity.Templates
                         where String.IsNullOrWhiteSpace(n.Role) ||
                               UmUtil.Instance.HasAccess(n.Role)
                         select n);

            cbxTemplates.BindData(query);
        }
    }
}