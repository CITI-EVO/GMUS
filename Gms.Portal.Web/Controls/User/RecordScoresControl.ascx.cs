using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Utils;
using DevExpress.XtraPrinting.Native;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using Panel = CITI.EVO.Tools.Web.UI.Controls.Panel;
using TextBox = CITI.EVO.Tools.Web.UI.Controls.TextBox;
using CITI.EVO.Tools.Extensions;

namespace Gms.Portal.Web.Controls.User
{
    public partial class RecordScoresControl : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void InitStructure(IEnumerable<RateEntity> rates)
        {
            var ratesLp = rates.ToLookup(n => n.ParentID);

            var parents = ratesLp[null];
            foreach (var parent in parents)
            {
                var children = ratesLp[parent.ID];
                if (children.Count() > 0)
                {
                    var header = CreateHeaderPanel(parent);
                    pnlMain.Controls.Add(header);

                    foreach (var child in children)
                    {
                        var item = CreateItemPanel(child);
                        pnlMain.Controls.Add(item);
                    }
                }
            }
        }

        public void BindData(IDictionary<String, Object> data)
        {
            if (data == null)
                return;

            tbxComment.Text = Convert.ToString(data.GetValueOrDefault("RatingComment"));

            var controls = UserInterfaceUtil.TraverseControls(pnlMain);
            var controlsLp = controls.ToLookup(n => n.ID);

            foreach (var pair in data)
            {
                if (!RegexUtil.FormValueParserRx.IsMatch(pair.Key))
                    continue;

                var match = RegexUtil.FormValueParserRx.Match(pair.Key);

                var type = match.Groups["type"].Value;
                if (type != "score" && type != "comment")
                    continue;

                var elemID = match.Groups["elemID"].Value;

                var fieldID = DataConverter.ToNullableGuid(elemID);
                if (fieldID == null)
                    continue;

                var controlID = $"{type}_{fieldID:N}";

                var target = controlsLp[controlID].OfType<TextBox>().FirstOrDefault();
                if (target == null)
                    continue;

                target.Text = Convert.ToString(pair.Value);
            }
        }

        public IDictionary<String, Object> GetFormData()
        {
            if (!IsPostBack)
                return null;

            var dict = new Dictionary<String, Object>
            {
                {"RatingComment", tbxComment.Text }
            };

            foreach (var key in Request.Form.AllKeys)
            {
                if (!RegexUtil.FormValueParserRx.IsMatch(key))
                    continue;

                var match = RegexUtil.FormValueParserRx.Match(key);

                var type = match.Groups["type"].Value;
                if (type != "score" && type != "comment")
                    continue;

                var elemID = match.Groups["elemID"].Value;

                var fieldID = DataConverter.ToNullableGuid(elemID);
                if (fieldID == null)
                    continue;

                var dataKey = $"{type}_{fieldID}";
                var dataVal = Request.Form[key];

                dict[dataKey] = dataVal;
            }

            return dict;
        }

        protected Panel CreateHeaderPanel(RateEntity entity)
        {
            var rowPanel = new Panel { CssClass = "row" };
            rowPanel.Attributes["style"] = "background-color: cadetblue;padding: 5px; color: white; font-weight: bold;";

            rowPanel.Controls.Add(CreateLabelPanel(entity.Number, 1));
            rowPanel.Controls.Add(CreateLabelPanel(entity.Name, 11));

            return rowPanel;
        }

        protected Panel CreateItemPanel(RateEntity entity)
        {
            var rowPanel = new Panel { CssClass = "row" };
            rowPanel.Attributes["style"] = "padding: 5px;";

            rowPanel.Controls.Add(CreateLabelPanel(entity.Number, 1));
            rowPanel.Controls.Add(CreateLabelPanel(entity.Name, 4));
            rowPanel.Controls.Add(CreateScorePanel(entity, 3));
            rowPanel.Controls.Add(CreateCommentPanel(entity.ID, 4));

            return rowPanel;
        }

        protected Panel CreateScorePanel(RateEntity rateEntity, int size)
        {
            var panel = new Panel { CssClass = $"col-lg-{size}" };
            var minMax = $"Min: {rateEntity.MinScore}; Max: {rateEntity.MaxScore}";

            var control = new TextBox
            {
                ID = $"score_{rateEntity.ID:N}",
                ClientIDMode = ClientIDMode.Static,
                CssClass = "intSpinEdit",
                EnableViewState = false,
                Text = Convert.ToString(rateEntity.MinScore),
                ToolTip = minMax
            };

            panel.Controls.Add(control);

            return panel;
        }

        protected Panel CreateCommentPanel(Guid? itemID, int size)
        {
            var mainPanel = new Panel { CssClass = $"col-lg-{size}" };
            var basePanel = new Panel {CssClass = "form-group has-error" };

            var control = new TextBox
            {
                ID = $"comment_{itemID:N}",
                ClientIDMode = ClientIDMode.Static,
                TextMode = TextBoxMode.MultiLine,
                EnableViewState = false,
                CssClass = "form-control"
            };

            basePanel.Controls.Add(control);
            mainPanel.Controls.Add(basePanel);

            return mainPanel;
        }

        protected Panel CreateLabelPanel(String text, int size)
        {
            var panel = new Panel { CssClass = $"col-lg-{size}" };

            var literal = new LiteralControl(text);
            panel.Controls.Add(literal);

            return panel;
        }
    }
}