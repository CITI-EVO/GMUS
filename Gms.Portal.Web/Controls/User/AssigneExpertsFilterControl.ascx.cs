using System;
using System.Linq;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Models;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

namespace Gms.Portal.Web.Controls.User
{
    public partial class AssigneExpertsFilterControl : BaseUserControlExtend<AssigneExpertsFilterModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillRecipientGroupsGrid();
        }

        protected void FillRecipientGroupsGrid()
        {
            var entities = (from n in HbSession.Query<GM_RecipientGroup>()
                            where n.DateDeleted == null
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new RecipientGroupEntityModelConverter(HbSession);

            var groups = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            ddlRecipientGroups.BindData(groups);
        }
    }
}