using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gms.Portal.Web.Bases;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using DropDownList = CITI.EVO.Tools.Web.UI.Controls.DropDownList;
using FieldEntity = Gms.Portal.Web.Entities.FormStructure.FieldEntity;
using Label = CITI.EVO.Tools.Web.UI.Controls.Label;
using Panel = CITI.EVO.Tools.Web.UI.Controls.Panel;
using TextBox = CITI.EVO.Tools.Web.UI.Controls.TextBox;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormGridFilterControl : BaseUserControl
    {
        private const String FieldIDFormat = "fltFld_{0}_{1:n}";
        private const String FieldPabelIDFormat = "fltFldPanel_{0:n}";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void InitStructure(ContentEntity container)
        {
            var allControls = FormStructureUtil.PreOrderFirstLevelTraversal(container);
            var filterControls = (from n in allControls.OfType<FieldEntity>()
                                  where n.DisplayOnFilter.GetValueOrDefault()
                                  orderby n.OrderIndex, n.Name
                                  select n);
            int index = 0;
            foreach (var entity in filterControls)
                SetFilterField(entity, index++ % 2);
        }

        private void SetFilterField(FieldEntity entity, int side)
        {
            var mainPanel = new Panel
            {
                ID = String.Format(FieldPabelIDFormat, entity.ID),
                CssClass = "form-group"
            };

            var leftClass = "col-sm-3 control-label";
            var rightClass = "col-sm-9";

            var leftPanel = new Panel { CssClass = leftClass };
            mainPanel.Controls.Add(leftPanel);

            var rightPanel = new Panel { CssClass = rightClass };
            mainPanel.Controls.Add(rightPanel);

            var captionPanel = leftPanel;
            var valuePanel = rightPanel;

            var captionSpan = new Label { Text = entity.Name, ToolTip = entity.Description };
            captionPanel.Controls.Add(captionSpan);

            var controls = CreateControl(entity);
            foreach (var control in controls)
                valuePanel.Controls.Add(control);

            if (side == 0)
                pnlLeft.Controls.Add(mainPanel);
            else
                pnlRight.Controls.Add(mainPanel);
        }

        private IEnumerable<Control> CreateControl(FieldEntity entity)
        {
            if (entity.Type == "Date" ||
                entity.Type == "Time" ||
                entity.Type == "Number")
            {
                var startControl = CreateControl(entity, 0);
                var endControl = CreateControl(entity, 1);

                var startPanel = new Panel { CssClass = "col-xs-6" };
                var endPanel = new Panel { CssClass = "col-xs-6" };

                startPanel.Controls.Add(startControl);
                endPanel.Controls.Add(endControl);

                yield return startPanel;
                yield return endPanel;
            }
            else
            {
                yield return CreateControl(entity, 0);
            }
        }


        private Control CreateControl(FieldEntity entity, int index)
        {
            if (!String.IsNullOrWhiteSpace(entity.DataSourceID) && String.IsNullOrWhiteSpace(entity.DependentFillExp))
            {
                var control = new DropDownList
                {
                    ID = String.Format(FieldIDFormat, index, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "chosen-select form-control selectWidth",
                };

                if (!String.IsNullOrWhiteSpace(entity.DataSourceID))
                {
                    var dataBinder = CreateDataBinder(entity);
                    if (dataBinder != null)
                    {
                        control.Items.Clear();

                        var collection = dataBinder.GetItems();

                        var emptyItem = new ListItem("-Select an Option-", "");
                        control.Items.Add(emptyItem);

                        foreach (var item in collection)
                        {
                            var text = Convert.ToString(item.Text);
                            var value = Convert.ToString(item.Value);

                            var listItem = new ListItem(text, value);
                            control.Items.Add(listItem);
                        }
                    }
                }

                return control;
            }
            else if (entity.Type == "Date")
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, index, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control",
                    EnableViewState = false,
                };

                if (!String.IsNullOrWhiteSpace(entity.Mask))
                    control.Attributes["data-mask"] = entity.Mask;

                var panel = new Panel { CssClass = "input-group date" };
                var label = new Label { CssClass = "input-group-addon" };
                var icon = new Label { CssClass = "fa fa-calendar" };

                label.Controls.Add(icon);

                panel.Controls.Add(label);
                panel.Controls.Add(control);

                return panel;
            }
            else if (entity.Type == "Time")
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, index, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control",
                    EnableViewState = false,
                };

                if (!String.IsNullOrWhiteSpace(entity.Mask))
                    control.Attributes["data-mask"] = entity.Mask;

                var panel = new Panel { CssClass = "input-group clockpicker" };
                var label = new Label { CssClass = "input-group-addon" };
                //var icon = new Label { CssClass = "fa fa-clock-o" };

                //label.Controls.Add(icon);

                panel.Controls.Add(label);
                panel.Controls.Add(control);

                return panel;
            }
            else if (entity.Type == "Number")
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, index, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "decSpinEdit",
                    EnableViewState = false,
                };

                if (!String.IsNullOrWhiteSpace(entity.Mask))
                    control.Attributes["data-mask"] = entity.Mask;

                return control;
            }
            else
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, index, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control",
                    EnableViewState = false,
                };

                if (!String.IsNullOrWhiteSpace(entity.Mask))
                    control.Attributes["data-mask"] = entity.Mask;

                return control;
            }
        }


        public void BindData(IDictionary<String, Object> data)
        {
            if (data == null)
                return;

            var allControls = UserInterfaceUtil.TraverseControls(pnlMain);
            var controlsLp = allControls.ToLookup(n => n.ID);

            foreach (var pair in data)
            {
                var key = pair.Key;

                var fieldID = DataConverter.ToNullableGuid(pair.Key);
                if (fieldID != null)
                    key = fieldID.Value.ToString("n");

                if (pair.Value is Object[])
                {
                    var array = (Object[])pair.Value;
                    for (int i = 0; i < array.Length; i++)
                    {
                        var controlKey = String.Format(FieldIDFormat, i, key);
                        var control = controlsLp[controlKey].OfType<TextBox>().First();

                        control.Text = Convert.ToString(array[i]);
                    }
                }
                else
                {
                    var controlKey = String.Format(FieldIDFormat, 0, key);
                    var control = controlsLp[controlKey].OfType<TextBox>().First();

                    control.Text = Convert.ToString(pair.Value);
                }
            }
        }

        public IDictionary<String, Object> GetData()
        {
            var resultDict = new Dictionary<String, Object>();

            if (!IsPostBack)
                return resultDict;

            var fieldsValues = from n in Request.Form.AllKeys
                               where RegexUtil.FilterElementParserRx.IsMatch(n)
                               let val = Request.Form[n]
                               let match = RegexUtil.FilterElementParserRx.Match(n)
                               let type = match.Groups["type"].Value
                               let index = match.Groups["index"].Value
                               let elemID = match.Groups["elemID"].Value
                               let fieldID = DataConverter.ToNullableGuid(elemID)
                               where fieldID != null
                               select new
                               {
                                   Type = type,
                                   Value = val,
                                   Index = index,
                                   FieldID = fieldID.Value,
                               };

            var valuesLp = fieldsValues.ToLookup(n => n.FieldID);

            foreach (var valuesGrp in valuesLp)
            {
                var query = (from n in valuesGrp
                             orderby n.Index
                             select n.Value);

                var key = Convert.ToString(valuesGrp.Key);

                if (valuesGrp.Count() > 1)
                {
                    var array = query.ToArray();
                    var tuple = new Tuple<Object, Object>(array[0], array[1]);

                    resultDict[key] = tuple;
                }
                else
                    resultDict[key] = query.First();
            }

            return resultDict;
        }

        private DictionaryDataBinder CreateDataBinder(FieldEntity fieldEntity)
        {
            var textExp = fieldEntity.TextExpression;
            var valueExp = fieldEntity.ValueExpression;

            var userID = (Guid?)null;
            if (!UserUtil.IsSuperAdmin())
                userID = UserUtil.GetCurrentUserID();

            var dataSourceHelper = new DataSourceHelper(userID, fieldEntity);
            var dataRecords = dataSourceHelper.LoadDataRecords();

            var dictionaryBinder = new DictionaryDataBinder(dataRecords, textExp, valueExp);
            return dictionaryBinder;
        }
    }
}