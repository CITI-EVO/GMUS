using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class PhaseControl : BaseUserControlExtend<PhaseModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var forms = (from n in HbSession.Query<GM_Form>()
                         where n.DateDeleted == null
                         orderby n.OrderIndex, n.Name
                         select n).ToList();

            var @set = (from n in cbxForm.Items.OfType<ListItem>()
                        where n.Selected
                        select n.Value).ToHashSet();

            cbxForm.Items.Clear();

            cbxForm.DataSource = forms;
            cbxForm.DataBind();

            foreach (var item in cbxForm.Items.OfType<ListItem>())
                item.Selected = @set.Contains(item.Value);
        }
    }
}