using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using Gms.Portal.Web.Bases;

public partial class Pages_Report_Report : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    protected void mainChart_OnDataBound(object sender, EventArgs e)
    {

    }

    protected void lstReportTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        var chartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlReportTypes.SelectedValue);
        mainChart.Series["Testing"].ChartType = chartType;
        mainChart.DataBind();
    }

    private void BindData()
    {
        ddlReportTypes.DataSource = Enum.GetValues(typeof(SeriesChartType)).Cast<SeriesChartType>().Select(i => new ListItem(i.ToString(), i.ToString()));
        ddlReportTypes.DataBind();
    }
}