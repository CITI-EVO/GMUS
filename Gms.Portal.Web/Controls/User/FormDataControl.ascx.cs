using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using Label = CITI.EVO.Tools.Web.UI.Controls.Label;
using Panel = CITI.EVO.Tools.Web.UI.Controls.Panel;
using TextBox = CITI.EVO.Tools.Web.UI.Controls.TextBox;
using GridView = CITI.EVO.Tools.Web.UI.Controls.GridView;
using CheckBox = CITI.EVO.Tools.Web.UI.Controls.CheckBox;
using LinkButton = CITI.EVO.Tools.Web.UI.Controls.LinkButton;
using RadioButton = CITI.EVO.Tools.Web.UI.Controls.RadioButton;
using HtmlElement = CITI.EVO.Tools.Web.UI.Controls.HtmlElement;
using DropDownList = CITI.EVO.Tools.Web.UI.Controls.DropDownList;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormDataControl : BaseUserControl
    {
        private const String FieldIDFormat = "field_{0:n}";
        private const String FileIDFormat = "file_{0:n}";
        private const String CheckIDFormat = "chk_{0:n}";
        private const String RadioIDFormat = "rb_{0:n}";

        private const String GridIDFormat = "grid_{0:n}";
        private const String SecIDFormat = "sec_{0:n}";

        private const String newCommandIDFormat = "newCmd_{0:n}";
        private const String viewCommandIDFormat = "viewCmd_{0:n}";
        private const String editCommandIDFormat = "editCmd_{0:n}";
        private const String deleteCommandIDFormat = "deleteCmd_{0:n}";

        public event EventHandler<CommandEventArgs> Command;
        protected virtual void OnCommand(CommandEventArgs e)
        {
            if (Command != null)
                Command(this, e);
        }

        public bool Enabled
        {
            get { return DataConverter.ToNullableBool(hdEnabled.Value).GetValueOrDefault(); }
            set { hdEnabled.Value = Convert.ToString(value); }
        }

        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(hdFormID.Value); }
            set { hdFormID.Value = Convert.ToString(value); }
        }

        public Guid? OwnerID
        {
            get { return DataConverter.ToNullableGuid(hdOwnerID.Value); }
            set { hdOwnerID.Value = Convert.ToString(value); }
        }

        public Guid? RecordID
        {
            get { return DataConverter.ToNullableGuid(hdRecordID.Value); }
            set { hdRecordID.Value = Convert.ToString(value); }
        }

        public Guid? ParentID
        {
            get { return DataConverter.ToNullableGuid(hdParentID.Value); }
            set { hdParentID.Value = Convert.ToString(value); }
        }

        public DateTime? DateCreated
        {
            get { return DataConverter.ToNullableDateTime(hdDateCreated.Value); }
            set { hdDateCreated.Value = Convert.ToString(value); }
        }

        private IDictionary<String, Control> _metaControls;
        protected IDictionary<String, Control> MetaControls
        {
            get
            {
                _metaControls = (_metaControls ?? new Dictionary<String, Control>());
                return _metaControls;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void New_OnClick(object sender, EventArgs eventArgs)
        {
            var linkButton = sender as LinkButton;
            if (linkButton == null)
                return;

            var commandEventArg = new CommandEventArgs(linkButton.CommandName, linkButton.CommandArgument);
            OnCommand(commandEventArg);
        }

        protected void command_OnCommand(object sender, CommandEventArgs e)
        {
            OnCommand(e);
        }

        protected void command_OnDataBinding(object sender, EventArgs eventArgs)
        {
            var linkButton = sender as LinkButton;
            if (linkButton == null)
                return;

            var container = linkButton.NamingContainer as GridViewRow;
            if (container == null)
                return;

            var descriptor = container.DataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return;

            var ownerID = descriptor.GetValue("OwnerID");
            var recordID = descriptor.GetValue("ID");

            linkButton.CommandArgument = String.Format("{0:n}/{1:n}", ownerID, recordID);
        }

        public void InitStructure(FormEntity formEntity)
        {
            MetaControls.Clear();

            if (formEntity == null)
                return;

            InitStructure(formEntity.Controls);
        }
        public void InitStructure(GridEntity gridEntity)
        {
            MetaControls.Clear();

            if (gridEntity == null)
                return;

            InitStructure(gridEntity.Controls);
        }

        public void InitStructure(IEnumerable<ControlEntity> entities)
        {
            MetaControls.Clear();

            if (entities == null)
                return;

            SetStructure(pnlMain, entities);
        }

        private void SetStructure(Control parent, IEnumerable<ControlEntity> entities)
        {
            var ordered = entities.OrderBy(n => n.OrderIndex);

            foreach (var controlEntity in ordered)
                SetStructure(parent, controlEntity);
        }

        private void SetStructure(Control parent, ControlEntity entity)
        {
            if (entity is FieldEntity)
            {
                var fieldEntity = (FieldEntity)entity;
                if (fieldEntity.Visible)
                    SetField(parent, fieldEntity);
            }
            else if (entity is GroupEntity)
            {
                var groupEntity = (GroupEntity)entity;
                if (groupEntity.Visible)
                    SetGroup(parent, groupEntity);
            }
            else if (entity is GridEntity)
            {
                var gridEntity = (GridEntity)entity;
                if (gridEntity.Visible)
                    SetGrid(parent, gridEntity);
            }
            else if (entity is TabContainerEntity)
            {
                var tabContainerEntity = (TabContainerEntity)entity;
                if (tabContainerEntity.Visible)
                    SetTabContainer(parent, tabContainerEntity);
            }
            else
            {
                throw new Exception();
            }
        }

        private void SetField(Control parent, FieldEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var captionLabel = new HtmlElement("label") { CssClass = "col-sm-2 control-label" };
            var captionSpan = new Label { Text = entity.Name, ToolTip = entity.Description };

            var captionPanel = new Panel();
            captionPanel.Controls.Add(captionLabel);

            var valuePanel = new Panel { CssClass = "col-sm-10" };
            var mainPanel = new Panel { CssClass = "form-group", Enabled = Enabled };

            var container = valuePanel;
            if (entity.Privacy.GetValueOrDefault())
            {
                var inputGroup = new Panel { CssClass = "input-group m-b" };
                container.Controls.Add(inputGroup);

                container = inputGroup;

                var privacySpan = new Label { CssClass = "input-group-addon" };
                var checkBox = new CheckBox
                {
                    ID = String.Format(SecIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                };

                privacySpan.Controls.Add(checkBox);
                container.Controls.Add(privacySpan);
            }

            var control = CreateControl(container, entity);
            if (control != null)
                MetaControls.Add(control.ID, control);

            if (entity.Mandatory.GetValueOrDefault())
            {
                var requiredValidator = new RequiredFieldValidator
                {
                    ControlToValidate = control.ID,
                    ErrorMessage = "This is a required field",
                    ForeColor = Color.Red,
                };

                var mandatorySpan = new Label { ForeColor = Color.Red, Text = "*" };
                captionLabel.Controls.Add(mandatorySpan);

                valuePanel.Controls.Add(requiredValidator);
            }

            captionLabel.Controls.Add(captionSpan);

            mainPanel.Controls.Add(captionLabel);
            mainPanel.Controls.Add(valuePanel);

            parent.Controls.Add(mainPanel);
        }

        private void SetGroup(Control parent, GroupEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            //var groupPanel = new Panel { CssClass = "row" };

            var titlePanel = new Panel { CssClass = "ibox-title" };
            var titleText = new HtmlElement("h5");
            var titleLabel = new Label { Text = entity.Name };

            var contentPanel = new Panel { CssClass = "ibox-content" };
            var rowPanel = new Panel { CssClass = "row" };

            var groupSize = String.Format("col-lg-{0}", entity.Size.GetValueOrDefault(12));

            var sizeDeteminerPanel = new Panel { CssClass = groupSize };
            var marginsDeteminerPanel = new Panel { CssClass = "ibox float-e-margins" };

            if (entity.Controls != null)
                SetStructure(rowPanel, entity.Controls);

            titleText.Controls.Add(titleLabel);

            titlePanel.Controls.Add(titleText);
            contentPanel.Controls.Add(rowPanel);

            marginsDeteminerPanel.Controls.Add(titlePanel);
            marginsDeteminerPanel.Controls.Add(contentPanel);

            sizeDeteminerPanel.Controls.Add(marginsDeteminerPanel);

            //groupPanel.Controls.Add(sizeDeteminerPanel);

            parent.Controls.Add(sizeDeteminerPanel);
        }

        private void SetGrid(Control parent, GridEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var rowPanel = new Panel();
            //var rowPanel = new Panel { CssClass = "row" };

            var titlePanel = new Panel { CssClass = "ibox-title" };
            var toolsPanel = new Panel { CssClass = "ibox-tools" };
            var contentPanel = new Panel { CssClass = "ibox-content" };
            var tablePanel = new Panel { CssClass = "table-responsive" };

            var dataGrid = new GridView();
            dataGrid.ID = String.Format(GridIDFormat, entity.ID);
            dataGrid.ShowHeaderWhenEmpty = true;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.UseAccessibleHeader = true;
            dataGrid.TableSectionHeader = true;
            dataGrid.CssClass = "table table-striped table-bordered table-hover dataTable";

            SetCommands(dataGrid, entity);

            MetaControls.Add(dataGrid.ID, dataGrid);

            if (entity.Controls != null)
            {
                var dataTable = new DataTable();

                var fields = entity.Controls.Cast<FieldEntity>();

                var orderedVisibleFields = (from n in fields
                                            where n.Visible && n.DisplayOnGrid
                                            orderby n.OrderIndex, n.Name
                                            select n);

                foreach (var field in orderedVisibleFields)
                {
                    var dataField = Convert.ToString(field.ID);

                    var column = new BoundField
                    {
                        HeaderText = field.Name,
                        DataField = dataField
                    };

                    dataTable.Columns.Add(dataField);
                    dataGrid.Columns.Add(column);
                }

                dataGrid.DataSource = dataTable;
                dataGrid.DataBind();
            }

            var newIcon = new Label { CssClass = "fa fa-plus" };
            var newText = new Label { CssClass = "linkTitle", Text = "New" };

            if (RecordID == null)
            {
                var label = new Label
                {
                    ForeColor = Color.Red,
                    Text = "Please save master data to add new record in grid"
                };

                toolsPanel.Controls.Add(label);
            }
            else
            {
                var newButton = new LinkButton();
                newButton.ID = String.Format(newCommandIDFormat, entity.ID);
                newButton.Click += New_OnClick;
                newButton.Enabled = Enabled;
                newButton.Visible = (RecordID != null);
                newButton.CssClass = (Enabled ? "btn btn-primary btn-sm" : "btn btn-default btn-sm");
                newButton.ClientIDMode = ClientIDMode.Static;
                newButton.CommandName = "New";
                newButton.CommandArgument = String.Format("{0:n}/@", entity.ID);
                newButton.Controls.Add(newIcon);
                newButton.Controls.Add(newText);

                toolsPanel.Controls.Add(newButton);
            }

            tablePanel.Controls.Add(dataGrid);

            titlePanel.Controls.Add(toolsPanel);
            contentPanel.Controls.Add(tablePanel);

            rowPanel.Controls.Add(titlePanel);
            rowPanel.Controls.Add(contentPanel);

            parent.Controls.Add(rowPanel);
        }

        private void SetTabContainer(Control parent, TabContainerEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var tabContainer = new Panel { CssClass = "tabs-container" };

            var containerHeader = new HtmlGenericControl("ul");
            containerHeader.Attributes["class"] = "nav nav-tabs";

            var containerContent = new Panel { CssClass = "tab-content" };

            tabContainer.Controls.Add(containerHeader);
            tabContainer.Controls.Add(containerContent);

            if (entity.Controls != null)
            {
                foreach (var child in entity.Controls)
                {
                    var tabPage = child as TabPageEntity;
                    if (tabPage == null)
                        throw new Exception();

                    SetTabPage(containerHeader, containerContent, tabPage);
                }

                parent.Controls.Add(tabContainer);
            }
        }

        private void SetTabPage(Control header, Control content, TabPageEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var number = Math.Abs(Guid.NewGuid().GetHashCode());

            var tabHeader = new HtmlElement("li");

            var headerLink = new HtmlElement("a");
            headerLink.Attributes["href"] = String.Format("#tab-{0}", number);
            headerLink.Attributes["data-toggle"] = "tab";
            headerLink.Attributes["aria-expanded"] = "true";

            var titleLabel = new Label { Text = entity.Name };


            var tabPane = new Panel
            {
                ID = String.Format("tab-{0}", number),
                ClientIDMode = ClientIDMode.Static,
                CssClass = "tab-pane"
            };

            var panelBody = new Panel { CssClass = "panel-body" };
            var rowPanel = new Panel { CssClass = "row" };

            if (header.Controls.Count == 0)
                tabHeader.Attributes["class"] = "active";

            if (content.Controls.Count == 0)
                tabPane.CssClass = "tab-pane active";

            panelBody.Controls.Add(rowPanel);

            headerLink.Controls.Add(titleLabel);

            tabHeader.Controls.Add(headerLink);
            tabPane.Controls.Add(panelBody);

            if (entity.Controls != null)
                SetStructure(rowPanel, entity.Controls);

            header.Controls.Add(tabHeader);
            content.Controls.Add(tabPane);
        }

        private void SetCommands(GridView dataGrid, GridEntity entity)
        {
            var commandsColumn = new TemplateField();
            commandsColumn.ItemTemplate = new GridColumnTemplate(() => CreateCommands(entity));

            dataGrid.Columns.Add(commandsColumn);
        }

        private IEnumerable<Control> CreateCommands(GridEntity entity)
        {
            var viewIcon = new Label { CssClass = "fa fa-file" };
            var viewText = new Label { CssClass = "linkTitle", Text = "View" };

            var editIcon = new Label { CssClass = "fa fa-edit" };
            var editText = new Label { CssClass = "linkTitle", Text = "Edit" };

            var deleteIcon = new Label { CssClass = "fa fa-trash-o" };
            var deleteText = new Label { CssClass = "linkTitle", Text = "Delete" };

            var viewCommand = new LinkButton();
            viewCommand.ID = String.Format(viewCommandIDFormat, entity.ID);
            viewCommand.CssClass = "btn btn-info btn-xs";
            viewCommand.CommandName = "View";
            viewCommand.Command += command_OnCommand;
            viewCommand.DataBinding += command_OnDataBinding;
            viewCommand.Controls.Add(viewIcon);
            viewCommand.Controls.Add(viewText);

            var editCommand = new LinkButton();
            editCommand.ID = String.Format(editCommandIDFormat, entity.ID);
            editCommand.Visible = Enabled;
            editCommand.CssClass = (Enabled ? "btn btn-primary btn-xs" : "btn btn-default btn-xs");
            editCommand.CommandName = "Edit";
            editCommand.Command += command_OnCommand;
            editCommand.DataBinding += command_OnDataBinding;
            editCommand.Controls.Add(editIcon);
            editCommand.Controls.Add(editText);

            var deleteCommand = new LinkButton();
            deleteCommand.ID = String.Format(deleteCommandIDFormat, entity.ID);
            deleteCommand.Visible = Enabled;
            deleteCommand.CssClass = (Enabled ? "btn btn-danger btn-xs" : "btn btn-default btn-xs");
            deleteCommand.CommandName = "Delete";
            deleteCommand.Command += command_OnCommand;
            deleteCommand.DataBinding += command_OnDataBinding;
            deleteCommand.Controls.Add(deleteIcon);
            deleteCommand.Controls.Add(deleteText);

            return new Control[] { viewCommand, editCommand, deleteCommand };
        }

        private Control CreateControl(Control parent, FieldEntity entity)
        {
            if (entity.Type == "TextBox")
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control"
                };

                if (!String.IsNullOrWhiteSpace(entity.Mask))
                    control.Attributes["data-mask"] = entity.Mask;

                parent.Controls.Add(control);
                return control;
            }

            if (entity.Type == "Date")
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control"
                };

                var panel = new Panel { CssClass = "input-group date" };
                var label = new Label { CssClass = "input-group-addon" };
                var icon = new Label { CssClass = "fa fa-calendar" };

                label.Controls.Add(icon);

                panel.Controls.Add(label);
                panel.Controls.Add(control);

                parent.Controls.Add(panel);
                return control;
            }

            if (entity.Type == "Time")
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control"
                };

                var panel = new Panel { CssClass = "input-group clockpicker" };
                var label = new Label { CssClass = "input-group-addon" };
                var icon = new Label { CssClass = "fa fa-clock-o" };

                label.Controls.Add(icon);

                panel.Controls.Add(label);
                panel.Controls.Add(control);

                parent.Controls.Add(panel);
                return control;
            }

            if (entity.Type == "CheckBox")
            {
                var control = new CheckBox
                {
                    ID = String.Format(CheckIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control",
                };

                parent.Controls.Add(control);
                return control;
            }

            if (entity.Type == "RagioButton")
            {
                var control = new RadioButton
                {
                    ID = String.Format(RadioIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control",
                    GroupName = entity.Tag
                };

                parent.Controls.Add(control);
                return control;
            }

            if (entity.Type == "ComboBox")
            {
                var control = new DropDownList
                {
                    ID = String.Format(FieldIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "chosen-select"
                };

                if (entity.DataSourceID != null)
                {
                    var dataView = GetDataView(entity.DataSourceID);
                    if (dataView != null)
                    {
                        control.DataTextField = entity.TextExpression;
                        control.DataValueField = entity.ValueExpression;
                        control.DataSource = dataView;

                        control.DataBind();
                    }
                }

                parent.Controls.Add(control);
                return control;
            }

            if (entity.Type == "FileUpload")
            {
                var fileInputPanel = new Panel { CssClass = "fileinput fileinput-new input-group" };
                fileInputPanel.Attributes["data-provides"] = "fileinput";

                var formControlPanel = new Panel { CssClass = "form-control" };
                formControlPanel.Attributes["data-trigger"] = "fileinput";

                var fileIconLabel = new Label { CssClass = "glyphicon glyphicon-file fileinput-exists" };
                var fileNameLabel = new Label { CssClass = "fileinput-filename" };

                var inputLabel = new Label { CssClass = "input-group-addon btn btn-default btn-file" };
                var changeLabel = new Label { CssClass = "fileinput-exists", Text = "Change" };
                var selectLabel = new Label { CssClass = "fileinput-new", Text = "Select file" };

                var fileUpload = new FileUpload
                {
                    ID = String.Format(FileIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                };

                var removeLink = new HtmlGenericControl("a");
                removeLink.Attributes["class"] = "input-group-addon btn btn-default fileinput-exists";
                removeLink.Attributes["data-dismiss"] = "fileinput";
                removeLink.InnerHtml = "Remove";

                formControlPanel.Controls.Add(fileIconLabel);
                formControlPanel.Controls.Add(fileNameLabel);

                inputLabel.Controls.Add(selectLabel);
                inputLabel.Controls.Add(changeLabel);
                inputLabel.Controls.Add(fileUpload);

                fileInputPanel.Controls.Add(formControlPanel);
                fileInputPanel.Controls.Add(inputLabel);
                fileInputPanel.Controls.Add(removeLink);

                parent.Controls.Add(fileInputPanel);
                return fileUpload;
            }

            throw new Exception();
        }

        public void BindFormData(FormDataUnit formData)
        {
            if (OwnerID == null || formData == null)
                return;

            var privateFields = formData.PrivateFields;
            var isMyData = (formData.UserID == UserUtil.GetCurrentUserID() || UserUtil.IsSuperAdmin());

            foreach (var pair in formData)
            {
                var fieldKey = DataConverter.ToNullableGuid(pair.Key);

                if (!isMyData && privateFields != null && privateFields.Count > 0)
                {
                    if (privateFields.Contains(pair.Key))
                        continue;
                }

                if (pair.Value is FormDataBaseList || pair.Value is FormDataListRef)
                {
                    var gridControlID = String.Format(GridIDFormat, fieldKey);

                    var control = MetaControls.GetValueOrDefault(gridControlID);
                    if (control == null)
                        continue;

                    var dataGrid = control as GridView;
                    if (dataGrid == null)
                        continue;

                    var columns = dataGrid.Columns.Cast<DataControlField>();

                    var formDataList = (FormDataBaseList)null;

                    if (pair.Value is FormDataListRef)
                        formDataList = new FormDataLazyList((FormDataListRef)pair.Value);
                    else
                        formDataList = (FormDataBaseList)pair.Value;

                    var dictionatries = formDataList.Cast<IDictionary<String, Object>>();

                    var @set = (from n in columns
                                let b = n as BoundField
                                where b != null
                                select b.DataField).ToHashSet();

                    var dataView = new DictionaryDataView(dictionatries, @set);

                    dataGrid.DataSource = dataView;
                }
                else
                {
                    var fieldControlID = String.Format(FieldIDFormat, fieldKey);

                    var control = MetaControls.GetValueOrDefault(fieldControlID);
                    if (control == null)
                        continue;

                    if (control is TextBox)
                    {
                        var textBox = (TextBox)control;
                        textBox.Text = Convert.ToString(pair.Value);
                    }
                    else if (control is RadioButton)
                    {
                        var radioButton = (RadioButton)control;
                        radioButton.Checked = DataConverter.ToNullableBool(pair.Value).GetValueOrDefault();
                    }
                    else if (control is CheckBox)
                    {
                        var checkBox = (CheckBox)control;

                        var @bool = true;
                        if (Convert.ToString(pair.Value) != "on")
                            @bool = DataConverter.ToNullableBool(pair.Value).GetValueOrDefault();

                        checkBox.Checked = @bool;
                    }
                    else if (control is DropDownList)
                    {
                        var dropDownList = (DropDownList)control;
                        dropDownList.TrySetSelectedValue(pair.Value);
                    }
                    else if (control is FileUpload)
                    {
                    }
                }
            }
        }

        public FormDataUnit GetFormData()
        {
            var regex = new Regex(@"(?<type>\w+)_(?<elemID>.+)", RegexOptions.Compiled);

            var formData = new FormDataUnit(FormID, OwnerID, RecordID, ParentID, DateCreated);

            var fieldsValues = from key in Request.Form.AllKeys
                               where regex.IsMatch(key)
                               let value = Request.Form[key]
                               let match = regex.Match(key)
                               let type = match.Groups["type"].Value
                               let elemID = match.Groups["elemID"].Value
                               let fieldID = DataConverter.ToNullableGuid(elemID)
                               where fieldID != null
                               select new
                               {
                                   Type = type,
                                   FieldID = fieldID,
                                   Value = value
                               };

            var valuesLp = fieldsValues.ToLookup(n => n.Type);

            foreach (var item in valuesLp["field"])
            {
                var key = Convert.ToString(item.FieldID);
                formData[key] = item.Value;
            }

            foreach (var item in valuesLp["file"])
            {
                var key = Convert.ToString(item.FieldID);
                formData[key] = item.Value;
            }

            foreach (var item in valuesLp["chk"])
            {
                var key = Convert.ToString(item.FieldID);
                formData[key] = StringComparer.OrdinalIgnoreCase.Equals(item.Value, "on");
            }

            foreach (var item in valuesLp["rb"])
            {
                var key = Convert.ToString(item.FieldID);
                formData[key] = StringComparer.OrdinalIgnoreCase.Equals(item.Value, "on");
            }

            var @set = formData.PrivateFields;
            if (@set == null)
            {
                @set = new HashSet<String>();
                formData.PrivateFields = @set;
            }

            foreach (var item in valuesLp["sec"])
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(item.Value, "on"))
                {
                    var key = Convert.ToString(item.FieldID);
                    @set.Add(key);
                }
            }

            var gridsID = (from n in MetaControls
                           where n.Value is GridView && regex.IsMatch(n.Key)
                           let match = regex.Match(n.Key)
                           let type = match.Groups["type"].Value
                           let elemID = match.Groups["elemID"].Value
                           let fieldID = DataConverter.ToNullableGuid(elemID)
                           where type == "grid" &&
                                 fieldID != null
                           select fieldID);

            foreach (var gridID in gridsID)
            {
                var listRef = new FormDataListRef(FormID, gridID, RecordID);

                var key = Convert.ToString(gridID);
                formData[key] = listRef;
            }

            return formData;
        }

        private DictionaryDataView GetDataView(Guid? collectionID)
        {
            if (collectionID == null)
                return null;

            var entity = GetEntity(collectionID);
            if (entity == null)
                return null;

            var fields = entity.Fields.ToDictionary(n => Convert.ToString(n.ID), n => n.Name);
            fields.Add(FormDataUnit.IDField, FormDataUnit.IDField);

            var @set = fields.Values.ToHashSet();

            var dictionaries = TransferData(collectionID, fields);

            var dataView = new DictionaryDataView(dictionaries, @set);
            return dataView;
        }

        protected Entities.CollectionStructure.CollectionEntity GetEntity(Guid? collectionID)
        {
            var dbEntity = HbSession.Get<GM_Collection>(collectionID);
            if (dbEntity == null)
                return null;

            var converter = new CollectionEntityModelConverter(HbSession);
            var model = converter.Convert(dbEntity);

            var entity = model.Entity;
            if (entity == null || entity.Fields == null)
                return null;

            return entity;
        }

        protected IEnumerable<IDictionary<String, Object>> TransferData(Guid? collectionID, IDictionary<String, String> fields)
        {
            var collection = MongoDbUtil.GetCollection(collectionID);
            if (collection == null)
                yield break;

            var query = collection.AsQueryable();
            var dicts = BsonDocumentConverter.ConvertToDictionary(query);

            var empty = new Dictionary<String, Object>();
            foreach (var pair in fields)
                empty[pair.Value] = "Select an Option";

            yield return empty;

            foreach (var dict in dicts)
            {
                var result = new Dictionary<String, Object>();
                foreach (var pair in fields)
                {
                    var val = dict.GetValueOrDefault(pair.Key);
                    result[pair.Value] = val;
                }

                yield return result;
            }
        }
    }
}