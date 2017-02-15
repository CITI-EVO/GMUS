using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormDataControl : System.Web.UI.UserControl
    {
        private const String FieldIDFormat = "field_{0:n}";
        private const String GridIDFormat = "grid_{0:n}";

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

        public bool Enabled
        {
            get { return DataConverter.ToNullableBool(hdEnabled.Value).GetValueOrDefault(); }
            set { hdEnabled.Value = Convert.ToString(value); }
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

            foreach (var controlEntity in entities)
                SetStructure(this, controlEntity);
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

            var captionPanel = new Panel();
            captionPanel.Controls.Add(new Label { Text = entity.Name });

            var control = CreateControl(entity);
            control.ClientIDMode = ClientIDMode.Static;
            control.ID = String.Format(FieldIDFormat, entity.ID);

            MetaControls.Add(control.ID, control);

            var valuePanel = new Panel();
            valuePanel.Controls.Add(control);

            var mainPanel = new Panel();
            mainPanel.Enabled = Enabled;

            mainPanel.Controls.Add(captionPanel);
            mainPanel.Controls.Add(valuePanel);

            parent.Controls.Add(mainPanel);
        }

        private void SetGroup(Control parent, GroupEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var fieldSet = new HtmlGenericControl("fieldset");

            var legend = new HtmlGenericControl("legend");
            legend.InnerText = entity.Name;

            fieldSet.Controls.Add(legend);

            if (entity.Controls != null)
            {
                foreach (var child in entity.Controls)
                    SetStructure(fieldSet, child);
            }

            parent.Controls.Add(fieldSet);
        }

        private void SetGrid(Control parent, GridEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var dataGrid = new GridView();
            dataGrid.ID = String.Format(GridIDFormat, entity.ID);
            dataGrid.AutoGenerateColumns = false;

            SetCommands(dataGrid, entity);

            MetaControls.Add(dataGrid.ID, dataGrid);

            if (entity.Controls != null)
            {
                foreach (var child in entity.Controls)
                {
                    var field = child as FieldEntity;
                    if (field == null)
                        throw new Exception();

                    var column = new BoundField();
                    column.HeaderText = field.Name;
                    column.DataField = Convert.ToString(field.ID);

                    dataGrid.Columns.Add(column);
                }
            }

            var newButton = new LinkButton();
            newButton.ID = String.Format(newCommandIDFormat, entity.ID);
            newButton.Text = "New";
            newButton.Click += New_OnClick;
            newButton.Enabled = Enabled;
            newButton.ClientIDMode = ClientIDMode.Static;
            newButton.CommandName = "New";
            newButton.CommandArgument = String.Format("{0:n}/@", entity.ID);

            var newBtnPanel = new Panel();
            newBtnPanel.Controls.Add(newButton);

            var mainPanel = new Panel();
            mainPanel.Controls.Add(newBtnPanel);
            mainPanel.Controls.Add(dataGrid);

            mainPanel.Visible = (RecordID != null);

            parent.Controls.Add(mainPanel);
        }

        private void SetTabContainer(Control parent, TabContainerEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var tabContainer = new TabContainer();

            if (entity.Controls != null)
            {
                foreach (var child in entity.Controls)
                {
                    var tabPage = child as TabPageEntity;
                    if (tabPage == null)
                        throw new Exception();

                    SetTabPage(tabContainer, tabPage);
                }

                parent.Controls.Add(tabContainer);
            }
        }

        private void SetTabPage(Control parent, TabPageEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var tabContainer = parent as TabContainer;
            if (tabContainer == null)
                return;

            var tabPage = new TabPanel();
            tabPage.HeaderText = entity.Name;

            if (entity.Controls != null)
            {
                foreach (var child in entity.Controls)
                    SetStructure(tabPage, child);
            }

            tabContainer.Tabs.Add(tabPage);
        }

        private void SetCommands(GridView dataGrid, GridEntity entity)
        {
            var commandsColumn = new TemplateField();
            commandsColumn.ItemTemplate = new GridColumnTemplate(() => CreateCommands(entity));

            dataGrid.Columns.Add(commandsColumn);
        }

        private IEnumerable<Control> CreateCommands(GridEntity entity)
        {
            var viewCommand = new LinkButton();
            viewCommand.ID = String.Format(viewCommandIDFormat, entity.ID);
            viewCommand.Text = "View";
            viewCommand.CommandName = "View";
            viewCommand.Command += command_OnCommand;
            viewCommand.DataBinding += command_OnDataBinding;

            var editCommand = new LinkButton();
            editCommand.ID = String.Format(editCommandIDFormat, entity.ID);
            editCommand.Text = "Edit";
            editCommand.Enabled = Enabled;
            editCommand.CommandName = "Edit";
            editCommand.Command += command_OnCommand;
            editCommand.DataBinding += command_OnDataBinding;

            var deleteCommand = new LinkButton();
            deleteCommand.ID = String.Format(deleteCommandIDFormat, entity.ID);
            deleteCommand.Text = "Delete";
            deleteCommand.Enabled = Enabled;
            deleteCommand.CommandName = "Delete";
            deleteCommand.Command += command_OnCommand;
            deleteCommand.DataBinding += command_OnDataBinding;

            return new[] { viewCommand, editCommand, deleteCommand };
        }

        private Control CreateControl(FieldEntity entity)
        {
            if (entity.Type == "TextBox")
            {
                var textBox = new TextBox();
                return textBox;
            }

            if (entity.Type == "CheckBox")
            {
                var checkBox = new CheckBox();
                return checkBox;
            }

            if (entity.Type == "RagioButton")
            {
                var ragioButton = new RadioButton();
                ragioButton.GroupName = entity.Tag;

                return ragioButton;
            }

            if (entity.Type == "ComboBox")
            {
                var dropDown = new DropDownList();
                //TODO: Init Possible Values
                return dropDown;
            }

            if (entity.Type == "FileUpload")
            {
                var fileUpload = new FileUpload();
                return fileUpload;
            }

            throw new Exception();
        }

        public void BindFormData(FormDataUnit formData)
        {
            if (OwnerID == null || formData == null)
                return;

            foreach (var pair in formData)
            {
                var fieldKey = DataConverter.ToNullableGuid(pair.Key);

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
            var regex = new Regex(@"(field_|grid_)(?<elemID>.*)", RegexOptions.Compiled);

            var formData = new FormDataUnit(FormID, OwnerID, RecordID, ParentID, DateCreated);

            var formValues = from n in Request.Form.AllKeys
                             let v = Request.Form[n]
                             where regex.IsMatch(n)
                             select new
                             {
                                 Key = n,
                                 Value = v
                             };

            foreach (var item in formValues)
            {
                var fieldID = GetFieldID(regex, item.Key);
                if (fieldID == null)
                    continue;

                var key = Convert.ToString(fieldID);
                formData[key] = item.Value;
            }

            var gridsID = (from n in MetaControls
                           where n.Value is GridView
                           let fieldID = GetFieldID(regex, n.Key)
                           select fieldID);

            foreach (var gridID in gridsID)
            {
                var listRef = new FormDataListRef(FormID, gridID, RecordID);

                var key = Convert.ToString(gridID);
                formData[key] = listRef;
            }

            return formData;
        }

        private Guid? GetFieldID(Regex regex, String text)
        {
            var match = regex.Match(text);
            var value = match.Groups["elemID"].Value;

            var fieldID = DataConverter.ToNullableGuid(value);
            return fieldID;
        }
    }
}