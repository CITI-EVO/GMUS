using System;
using System.Linq;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

namespace Gms.Portal.Web.Controls.User
{
    public partial class RecordStatusControl : BaseUserControlExtend<RecordStatusModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillDataStatusList();
        }

        protected void comboBox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            OnDataChanged(e);
        }

        protected void FillDataStatusList()
        {
            var categories = (from n in HbSession.Query<GM_DataStatus>()
                              where n.DateDeleted == null
                              orderby n.Name
                              select n).ToList();

            cbxDataStatus.BindData(categories);
        }
    }
}