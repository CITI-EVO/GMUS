using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.Services;
using CITI.EVO.Tools.Web.UI.Controls;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.ServiceStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.User
{
    public partial class ExternalServiceControl : BaseUserControl
    {
        public Guid MethodID { get; private set; }

        public ServiceModel ServiceModel { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            mpeSearch.Show();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var parameters = GetParametersData();
            var serviceInvoker = new ExternalServiceInvoker(ServiceModel, MethodID);

            var result = serviceInvoker.Invoke(parameters);

        }

        protected void btnSelectOK_Click(object sender, EventArgs e)
        {
            var selectedVal = lstResults.SelectedValue;

        }

        protected void btnSelectCancel_OnClick(object sender, EventArgs e)
        {
            mpeSearch.Hide();
        }

        public void InitStructure(ServiceModel serviceModel, Guid methodID)
        {
            ServiceModel = serviceModel;
            MethodID = methodID;

            var serviceEntity = serviceModel.Entity;
            var methodEntity = serviceEntity.Methods.FirstOrDefault(n => n.ID == methodID);

            if (methodEntity == null || methodEntity.Parameters == null)
                return;

            var parameters = methodEntity.Parameters.OrderBy(n => n.OrderIndex);

            foreach (var parameter in parameters)
            {
                var mainPanel = new Panel { CssClass = "form-group" };

                var leftPanel = new Panel { CssClass = "col-sm-4 control-label" };
                mainPanel.Controls.Add(leftPanel);

                var rightPanel = new Panel { CssClass = "col-sm-8" };
                mainPanel.Controls.Add(rightPanel);

                var captionSpan = new Label { Text = parameter.Name };
                leftPanel.Controls.Add(captionSpan);

                var inputControl = new TextBox
                {
                    ID = $"param_{parameter.ID}",
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control",
                    EnableViewState = false,
                };

                rightPanel.Controls.Add(inputControl);

                pnlParameters.Controls.Add(mainPanel);
            }
        }

        private IDictionary<String, Object> GetParametersData()
        {
            var resultDict = new Dictionary<String, Object>();

            if (!IsPostBack)
                return resultDict;

            var fieldsValues = from n in Request.Form.AllKeys
                               where RegexUtil.FormValueParserRx.IsMatch(n)
                               let val = Request.Form[n]
                               let match = RegexUtil.FormValueParserRx.Match(n)
                               let type = match.Groups["type"].Value
                               let elemID = match.Groups["elemID"].Value
                               let fieldID = DataConverter.ToNullableGuid(elemID)
                               where fieldID != null
                               select new
                               {
                                   Type = type,
                                   Value = val,
                                   FieldID = fieldID.Value,
                               };

            var valuesLp = fieldsValues.ToLookup(n => n.FieldID);

            foreach (var valuesGrp in valuesLp)
            {
                var key = Convert.ToString(valuesGrp.Key);
                resultDict[key] = valuesGrp.First();
            }

            return resultDict;
        }
    }
}