using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models.Common;

namespace Gms.Portal.Web.Controls.Common
{
	public partial class NamedExpressionsListControl : BaseUserControlExtend<NamedExpressionsListModel>
	{
		public List<NamedExpressionModel> Expressions
		{
			get
			{
				var list = ViewState["Expressions"] as List<NamedExpressionModel>;
				if (list == null)
				{
					list = new List<NamedExpressionModel>();
					ViewState["Expressions"] = list;
				}

				return list;
			}
			set { ViewState["Expressions"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			gvExpressions.DataSource = Expressions;
			gvExpressions.DataBind();
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			gvExpressions.DataSource = Expressions;
			gvExpressions.DataBind();
		}

		protected void btnSave_OnClick(object sender, EventArgs e)
		{
			var model = namedExpressionControl.Model;
			if (String.IsNullOrWhiteSpace(model.Expression))
				return;

			if (model.Key != null)
			{
				var oldModel = Expressions.FirstOrDefault(n => n.Key == model.Key);
				if (oldModel != null)
				{
					oldModel.Name = model.Name;
					oldModel.Expression = model.Expression;
					oldModel.OutputType = model.OutputType;
				}
			}
			else
			{
				var newModel = new NamedExpressionModel
				{
					Key = Guid.NewGuid(),
					Name = model.Name,
					Expression = model.Expression,
					OutputType = model.OutputType
				};

				Expressions.Add(newModel);
			}

			mpeExpression.Hide();
		}

		protected void btnAdd_OnClick(object sender, EventArgs e)
		{
			namedExpressionControl.Model = new NamedExpressionModel();

			mpeExpression.Show();
		}

		protected override void btnDelete_OnCommand(object sender, CommandEventArgs e)
		{
			var key = DataConverter.ToNullableGuid(e.CommandArgument);
			if (key == null)
				return;

			var model = Expressions.FirstOrDefault(n => n.Key == key);
			if (model == null)
				return;

			Expressions.Remove(model);
		}

		protected override void btnEdit_OnCommand(object sender, CommandEventArgs e)
		{
			var key = DataConverter.ToNullableGuid(e.CommandArgument);
			if (key == null)
				return;

			var model = Expressions.FirstOrDefault(n => n.Key == key);
			if (model == null)
				return;

			namedExpressionControl.Model = model;

			mpeExpression.Show();
		}

        public override NamedExpressionsListModel GetModel()
        {
            var model = (NamedExpressionsListModel)GetModel(typeof(NamedExpressionsListModel));
            model.Expressions = Expressions;

            return model;
        }

        public override void SetModel(NamedExpressionsListModel model)
		{
            base.SetModel(model);
			Expressions = model.Expressions;
		}

	    protected void btnCancel_OnClick(object sender, EventArgs e)
	    {
	        mpeExpression.Hide();
	    }
	}
}