using System;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.UserManagement.Web.Bases;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Models;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

namespace Gms.Portal.Web.Pages.Management
{
	public partial class LogicsList : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			FillLogicsGrid();
		}

		protected void btnAddLogic_OnClick(object sender, EventArgs e)
		{
			Response.Redirect("~/Pages/Management/AddEditLogic.aspx");
		}

		private void FillLogicsGrid()
		{
			var entities = (from n in HbSession.Query<GM_Logic>()
                            where n.DateDeleted == null
							orderby n.DateCreated descending
							select n).ToList();

			var converter = new LogicEntityModelConverter(HbSession);

			var models = (from n in entities
						  let m = converter.Convert(n)
						  select m).ToList();

			var model = new LogicsModel();
			model.List = models;

			logicsControl.Model = model;
		}

		protected void logicsControl_OnEditItem(object sender, GenericEventArgs<Guid> e)
		{
			var url = String.Format("~/Pages/Management/AddEditLogic.aspx?LogicID={0}", e.Value);
			Response.Redirect(url);
		}

		protected void logicsControl_OnDeleteItem(object sender, GenericEventArgs<Guid> e)
		{
			var entity = HbSession.Query<GM_Logic>().FirstOrDefault(n => n.ID == e.Value);
			if (entity != null)
				entity.DateDeleted = DateTime.Now;

			HbSession.SubmitChanges(entity);

			FillLogicsGrid();
		}

		protected void logicsControl_OnViewItem(object sender, GenericEventArgs<Guid> e)
		{
			var url = String.Format("~/Pages/Management/AddEditLogic.aspx?LogicID={0}", e.Value);
			Response.Redirect(url);
		}
	}
}