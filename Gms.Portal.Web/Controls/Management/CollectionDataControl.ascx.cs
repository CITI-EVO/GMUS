using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Web.UI.Controls;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Controls.Common;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Models;
using Panel = System.Web.UI.WebControls.Panel;
using TextBox = System.Web.UI.WebControls.TextBox;
using HtmlElement = CITI.EVO.Tools.Web.UI.Controls.HtmlElement;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class CollectionDataControl : BaseUserControlExtend<CollectionDataModel>
    {
        private const String propertyFormat = "{CollectionDataModel.Data[@dataKey]=@propName}";
        private const String controlIDFormat = "field_@hashCode";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void InitStructure(IDictionary<Object, String> fields)
        {
            foreach (var pair in fields)
            {
                var fieldKey = Convert.ToString(pair.Key);
                var fieldID = String.Format("{0:n}", pair.Key);

                var property = propertyFormat.Replace("@dataKey", fieldKey);


                var controlID = controlIDFormat.Replace("@hashCode", fieldID);

                if (fieldKey == FormDataUnit.IDField)
                {
                    property = property.Replace("@propName", "Value");

                    var hiddenField = (HiddenFieldValueControl)LoadControl("~/Controls/Common/HiddenFieldValueControl.ascx");

                    hiddenField.Attributes["Property"] = property;
                    hiddenField.ClientIDMode = ClientIDMode.Static;
                    hiddenField.ID = controlID;

                    pnlMain.Controls.Add(hiddenField);
                    continue;
                }

                property = property.Replace("@propName", "Text");

                var panel = new Panel { CssClass = "form-group" };
                var subPanel = new Panel { CssClass = "col-lg-10" };

                var label = new HtmlElement("label") { CssClass = "col-lg-2 control-label", InnerHtml = pair.Value };

                var textBox = new TextBox();
                textBox.Attributes["Property"] = property;
                textBox.ClientIDMode = ClientIDMode.Static;
                textBox.CssClass = "form-control";
                textBox.ID = controlID;

                subPanel.Controls.Add(textBox);

                panel.Controls.Add(label);
                panel.Controls.Add(subPanel);

                pnlMain.Controls.Add(panel);
            }
        }
    }
}