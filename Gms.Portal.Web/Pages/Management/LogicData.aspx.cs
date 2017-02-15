using System;
using System.Linq;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Web.Bases;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Models;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.Management
{
	public partial class LogicData : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			var logicID = DataConverter.ToNullableGuid(Request["LogicID"]);
			var tableID = DataConverter.ToNullableGuid(Request["TableID"]);

			var logic = HbSession.Query<GM_Logic>().FirstOrDefault(n => n.ID == logicID);
			if (logic == null)
				return;

			var table = HbSession.Query<GM_Table>().FirstOrDefault(n => n.ID == tableID);
			if (table == null)
				return;

			var logicConverter = new LogicEntityModelConverter(HbSession);
			var tableConverter = new TableEntityModelConverter(HbSession);

			var logicModel = logicConverter.Convert(logic);
			var tableModel = tableConverter.Convert(table);

			var tableDataModel = new TableDataModel
			{
				Table = tableModel,
				Logic = logicModel,
			};

			tableDataControl.Model = tableDataModel;
		}
	}
}