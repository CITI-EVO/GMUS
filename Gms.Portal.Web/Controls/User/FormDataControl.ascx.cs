using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Controls;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.CollectionStructure;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using MongoDB.Driver;
using NHibernate.Linq;
using Label = CITI.EVO.Tools.Web.UI.Controls.Label;
using Panel = CITI.EVO.Tools.Web.UI.Controls.Panel;
using TextBox = CITI.EVO.Tools.Web.UI.Controls.TextBox;
using GridView = CITI.EVO.Tools.Web.UI.Controls.GridView;
using CheckBox = CITI.EVO.Tools.Web.UI.Controls.CheckBox;
using LinkButton = CITI.EVO.Tools.Web.UI.Controls.LinkButton;
using RadioButton = CITI.EVO.Tools.Web.UI.Controls.RadioButton;
using HtmlElement = CITI.EVO.Tools.Web.UI.Controls.HtmlElement;
using DropDownList = CITI.EVO.Tools.Web.UI.Controls.DropDownList;
using FieldEntity = Gms.Portal.Web.Entities.FormStructure.FieldEntity;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormDataControl : BaseUserControl
    {
        private const String FileIDFormat = "file_{0:n}";
        private const String FieldIDFormat = "field_{0:n}";
        private const String CheckIDFormat = "chk_{0:n}";
        private const String RadioIDFormat = "rb_{0:n}";

        private const String FieldPabelIDFormat = "fldPanel_{0:n}";

        private const String SecIDFormat = "sec_{0:n}";
        private const String GridIDFormat = "grid_{0:n}";
        private const String GroupIDFormat = "grp_{0:n}";
        private const String GridPanelIDFormat = "gridPnl_{0:n}";
        private const String TabHeaderIDFormat = "tab_header_{0:n}";
        private const String TabContentIDFormat = "tab_content_{0:n}";
        private const String TabContainerIDFormat = "tbc_{0:n}";

        private const String newCommandIDFormat = "newCmd_{0:n}";
        private const String viewCommandIDFormat = "viewCmd_{0:n}";
        private const String editCommandIDFormat = "editCmd_{0:n}";
        private const String deleteCommandIDFormat = "deleteCmd_{0:n}";

        private const String TransferDataCacheKey = "@{FormDataControl_TransferData}";
        private const String FindDataRecordCacheKey = "@{FormDataControl_FindDataRecord}";
        private const String CollectionEntityCacheKey = "@{FormDataControl_CollectionEntityCache}";

        private ContentEntity _contentEntity;

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

        private FormDataUnit _formData;
        public FormDataUnit FormData
        {
            get
            {
                if (_formData == null)
                    _formData = GetFormData();

                return _formData;
            }
        }

        private IEnumerable<Control> _allControls;
        protected IEnumerable<Control> AllControls
        {
            get
            {
                if (_allControls == null)
                    _allControls = UserInterfaceUtil.TraverseControls(pnlMain);

                return _allControls;
            }
        }

        private ILookup<String, Control> _allControlslp;
        protected ILookup<String, Control> AllControlsLp
        {
            get
            {
                if (_allControlslp == null)
                    _allControlslp = AllControls.ToLookup(n => n.ID);

                return _allControlslp;
            }
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

        private IDictionary<String, Object> _dependentRecords;
        protected IDictionary<String, Object> DependentRecords
        {
            get
            {
                if (_dependentRecords == null)
                    _dependentRecords = new Dictionary<String, Object>();

                return _dependentRecords;
            }
        }

        private ILookup<Guid?, ControlEntity> _dependentFields;
        protected ILookup<Guid?, ControlEntity> DependentFields
        {
            get
            {
                if (_dependentFields == null)
                    _dependentFields = GetDependentFields(_contentEntity);

                return _dependentFields;
            }
        }

        private IDictionary<Guid, ControlEntity> _firstLevelChildren;
        protected IDictionary<Guid, ControlEntity> FirstLevelChildren
        {
            get
            {
                if (_firstLevelChildren == null)
                {
                    var children = FormStructureUtil.PreOrderFirstLevelTraversal(_contentEntity);
                    _firstLevelChildren = children.ToDictionary(n => n.ID);
                }

                return _firstLevelChildren;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                ApplyViewMode();
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

        public ISet<Guid> GetActiveTabs()
        {
            var activeTabs = (from n in Request.Form.AllKeys
                              where n.Contains("tab_active")
                              let v = Request.Form[n]
                              let g = DataConverter.ToNullableGuid(v)
                              where g != null
                              select g.Value);

            var @set = activeTabs.ToHashSet();
            return @set;
        }

        public void SetActiveTabs(ISet<Guid> activeTabs)
        {
            if (activeTabs == null || activeTabs.Count == 0)
                return;

            var query = (from n in AllControls
                         let v = (n.ID ?? String.Empty)
                         where v.StartsWith("tab")
                         select n);

            var tabsDict = query.ToDictionary(n => n.ID);

            var children = FormStructureUtil.CreateTree(_contentEntity);

            var containersQuery = (from n in children
                                   where activeTabs.Contains(n.ID.GetValueOrDefault())
                                   select new
                                   {
                                       ID = n.ID.Value,
                                       ParentID = n.ParentID.Value
                                   });

            var containers = containersQuery.ToDictionary(n => n.ID, n => n.ParentID);

            foreach (var control in tabsDict.Values)
            {
                var webControl = control as WebControl;
                if (webControl != null)
                    webControl.CssClass = "tab-pane";

                var htmlControl = control as HtmlControl;
                if (htmlControl != null)
                    htmlControl.Attributes["class"] = String.Empty;
            }

            foreach (var tabID in activeTabs)
            {
                var headerID = String.Format("tab_header_{0:n}", tabID);
                var contentID = String.Format("tab_content_{0:n}", tabID);

                var header = tabsDict.GetValueOrDefault(headerID) as HtmlControl;
                if (header != null)
                    header.Attributes["class"] = "active";

                var content = tabsDict.GetValueOrDefault(contentID) as WebControl;
                if (content != null)
                    content.CssClass = "tab-pane active";

                if (containers.Count > 0)
                {
                    var parentID = containers[tabID];
                    var activeID = String.Format("tab_active_{0:n}", parentID);

                    var activeFld = tabsDict.GetValueOrDefault(activeID) as HiddenField;
                    if (activeFld != null)
                        activeFld.Value = String.Format("{0:n}", tabID);
                }
            }
        }

        public void InitStructure(FormEntity formEntity)
        {
            MetaControls.Clear();

            if (formEntity == null)
                return;

            _formData = null;
            _metaControls = null;
            _dependentFields = null;
            _dependentRecords = null;
            _firstLevelChildren = null;

            _contentEntity = formEntity;

            InitStructure(formEntity.Controls);
        }
        public void InitStructure(GridEntity gridEntity)
        {
            MetaControls.Clear();

            if (gridEntity == null)
                return;

            _formData = null;
            _metaControls = null;
            _dependentFields = null;
            _dependentRecords = null;
            _firstLevelChildren = null;

            _contentEntity = gridEntity;

            InitStructure(gridEntity.Controls);
        }
        public void InitStructure(IEnumerable<ControlEntity> entities)
        {
            MetaControls.Clear();

            if (entities == null)
                return;

            SetStructure(pnlMain, entities);
        }

        public void BindFormData(FormDataUnit formData)
        {
            BindFormData(formData, false);
        }
        public void BindFormData(FormDataUnit formData, bool keepPostBackData)
        {
            if (OwnerID == null || formData == null)
                return;

            if (keepPostBackData && IsPostBack)
            {
                var formDataCopy = new FormDataUnit(formData);

                var postBackData = FormData;
                foreach (var pair in postBackData)
                    formDataCopy[pair.Key] = pair.Value;

                formData = formDataCopy;
            }

            var privateFields = formData.PrivateFields;
            var isMyData = (formData.UserID == UserUtil.GetCurrentUserID() || UserUtil.IsSuperAdmin());

            foreach (var pair in formData)
            {
                var fieldKey = DataConverter.ToNullableGuid(pair.Key);

                if (privateFields != null && privateFields.Count > 0)
                {
                    if (privateFields.Contains(pair.Key))
                    {
                        var secControlID = String.Format(SecIDFormat, fieldKey);

                        var secControl = MetaControls.GetValueOrDefault(secControlID);
                        if (secControl != null)
                        {
                            var checkBox = (CheckBox)secControl;
                            checkBox.Checked = true;
                        }

                        if (!isMyData)
                            continue;
                    }
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

                    var columns = dataGrid.Columns.OfType<GridViewBoundField>();

                    var formDataList = (FormDataBaseList)null;

                    if (pair.Value is FormDataListRef)
                        formDataList = new FormDataLazyList((FormDataListRef)pair.Value);
                    else
                        formDataList = (FormDataBaseList)pair.Value;

                    var dictionatries = formDataList.Cast<IDictionary<String, Object>>();

                    var dataFieldSet = columns.Select(n => n.DataField).ToHashSet();

                    var dataView = new DictionaryDataView(dictionatries, dataFieldSet);

                    dataGrid.DataSource = dataView;
                    dataGrid.DataBind();
                }

                var fieldControlID = String.Format(FieldIDFormat, fieldKey);
                var checkControlID = String.Format(CheckIDFormat, fieldKey);
                var radioControlID = String.Format(RadioIDFormat, fieldKey);
                var fileControlID = String.Format(FileIDFormat, fieldKey);
                //var secControlID = String.Format(SecIDFormat, fieldKey);

                //var secControl = MetaControls.GetValueOrDefault(fieldControlID);
                //if (secControl != null)
                //{
                //    var checkBox = (CheckBox)secControl;

                //    var @bool = true;
                //    if (Convert.ToString(pair.Value) != "on")
                //        @bool = DataConverter.ToNullableBool(pair.Value).GetValueOrDefault();

                //    checkBox.Checked = @bool;
                //}

                var fieldControl = MetaControls.GetValueOrDefault(fieldControlID);
                if (fieldControl != null)
                {
                    if (fieldControl is TextBox)
                    {
                        var textBox = (TextBox)fieldControl;
                        textBox.Text = Convert.ToString(pair.Value);
                    }
                    else if (fieldControl is DropDownList)
                    {
                        var dropDownList = (DropDownList)fieldControl;
                        dropDownList.TrySetSelectedValue(pair.Value);
                    }
                }

                var fileControl = MetaControls.GetValueOrDefault(fileControlID);
                if (fileControl != null)
                {
                    if (fileControl is FileUpload)
                    {
                        var binary = pair.Value as FormDataBinary;
                        if (binary != null && binary.FileBytes != null && !String.IsNullOrWhiteSpace(binary.FileName))
                        {
                            var parents = UserInterfaceUtil.TraverseParents(fileControl);
                            var mainPanel = parents.FirstOrDefault(n => n.ID == "pnlFileData");

                            var children = UserInterfaceUtil.TraverseChildren(mainPanel);
                            var childrenLp = children.ToLookup(n => n.ID);

                            //var uploadPanel = childrenLp["pnlInput"].Single();
                            var downloadPanel = childrenLp["pnlOuput"].Single();

                            //uploadPanel.Visible = false;
                            downloadPanel.Visible = true;
                        }
                    }
                }

                var checkControl = MetaControls.GetValueOrDefault(checkControlID);
                if (checkControl != null)
                {
                    var checkBox = (CheckBox)checkControl;

                    var @bool = true;
                    if (Convert.ToString(pair.Value) != "on")
                        @bool = DataConverter.ToNullableBool(pair.Value).GetValueOrDefault();

                    checkBox.Checked = @bool;
                }

                var radioControl = MetaControls.GetValueOrDefault(radioControlID);
                if (radioControl != null)
                {
                    var radioButton = (RadioButton)radioControl;
                    radioButton.Checked = DataConverter.ToNullableBool(pair.Value).GetValueOrDefault();
                }
            }

            ApplyViewMode(formData);
        }

        public FormDataUnit GetFormData()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;

            var regex = new Regex(@"(?<type>\w+)_(?<elemID>.+)", RegexOptions.Compiled);

            var formData = new FormDataUnit(FormID, OwnerID, RecordID, ParentID, DateCreated);

            var allKeys = GetAllFieldKeys();

            var fieldsValues = from n in allKeys
                               where regex.IsMatch(n.Key)
                               let match = regex.Match(n.Key)
                               let type = match.Groups["type"].Value
                               let elemID = match.Groups["elemID"].Value
                               let fieldID = DataConverter.ToNullableGuid(elemID)
                               where fieldID != null
                               select new
                               {
                                   Type = type,
                                   Value = n.Value,
                                   FieldID = fieldID,
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

                var binary = new FormDataBinary();

                var file = item.Value as HttpPostedFile;
                if (file != null)
                {
                    file.InputStream.Seek(0, SeekOrigin.Begin);

                    binary.FileName = file.FileName;
                    binary.FileBytes = file.InputStream.ReadToEnd();
                }

                formData[key] = binary;
            }

            foreach (var item in valuesLp["chk"])
            {
                var key = Convert.ToString(item.FieldID);
                formData[key] = comparer.Equals(item.Value, "on");
            }

            foreach (var item in valuesLp["rb"])
            {
                var key = Convert.ToString(item.FieldID);
                formData[key] = comparer.Equals(item.Value, "on");
            }

            var @set = formData.PrivateFields;
            if (@set == null)
            {
                @set = new HashSet<String>();
                formData.PrivateFields = @set;
            }

            foreach (var item in valuesLp["sec"])
            {
                if (comparer.Equals(item.Value, "on"))
                {
                    var key = Convert.ToString(item.FieldID);
                    @set.Add(key);
                }
            }

            if (FirstLevelChildren != null)
            {
                var dataGrids = FirstLevelChildren.Values.OfType<GridEntity>();
                foreach (var dataGrid in dataGrids)
                {
                    var listRef = new FormDataListRef(FormID, dataGrid.ID, RecordID);

                    var key = Convert.ToString(dataGrid.ID);
                    formData[key] = listRef;
                }
            }

            return formData;
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

            var leftClass = String.Format("col-sm-{0} control-label", entity.CaptionSize.GetValueOrDefault(4));
            var rightClass = String.Format("col-sm-{0}", entity.ControlSize.GetValueOrDefault(8));

            var mainPanel = new Panel
            {
                ID = String.Format(FieldPabelIDFormat, entity.ID),
                CssClass = "form-group",
                Enabled = Enabled
            };

            var leftPanel = new Panel { CssClass = leftClass };
            mainPanel.Controls.Add(leftPanel);

            var rightPanel = new Panel { CssClass = rightClass };
            mainPanel.Controls.Add(rightPanel);

            var captionPanel = leftPanel;
            var valuePanel = rightPanel;

            if (entity.Inversion.GetValueOrDefault())
            {
                captionPanel = rightPanel;
                valuePanel = leftPanel;
            }

            if (entity.Mandatory.GetValueOrDefault())
            {
                var mandatorySpan = new System.Web.UI.WebControls.Label { ForeColor = Color.Red, Text = "*" };
                captionPanel.Controls.Add(mandatorySpan);
            }

            var captionSpan = new Label { Text = entity.Name, ToolTip = entity.Description };
            captionPanel.Controls.Add(captionSpan);

            var container = valuePanel;
            if (entity.Privacy.GetValueOrDefault())
                container = InitPrivacyGroup(valuePanel, entity);

            var autoPostBack = (DependentFields != null && DependentFields.Contains(entity.ID));

            var control = CreateControl(container, entity, autoPostBack);
            if (control != null)
                MetaControls.Add(control.ID, control);

            if (entity.DependentFieldID != null)
                mainPanel.Visible = false;

            parent.Controls.Add(mainPanel);
        }
        private void SetGroup(Control parent, GroupEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var titlePanel = new Panel { CssClass = "ibox-title" };
            var titleText = new HtmlElement("h5");
            var titleLabel = new Label { Text = entity.Name };

            var contentPanel = new Panel { CssClass = "ibox-content" };
            var rowPanel = new Panel { CssClass = "row" };

            var groupSize = String.Format("col-lg-{0}", entity.Size.GetValueOrDefault(12));

            var sizeDeteminerPanel = new Panel
            {
                ID = String.Format(GroupIDFormat, entity.ID),
                CssClass = groupSize
            };
            var marginsDeteminerPanel = new Panel { CssClass = "ibox float-e-margins" };

            if (entity.Controls != null)
                SetStructure(rowPanel, entity.Controls);

            titleText.Controls.Add(titleLabel);

            titlePanel.Controls.Add(titleText);
            contentPanel.Controls.Add(rowPanel);

            marginsDeteminerPanel.Controls.Add(titlePanel);
            marginsDeteminerPanel.Controls.Add(contentPanel);

            sizeDeteminerPanel.Controls.Add(marginsDeteminerPanel);

            if (entity.DependentFieldID != null)
                sizeDeteminerPanel.Visible = false;

            parent.Controls.Add(sizeDeteminerPanel);
        }
        private void SetGrid(Control parent, GridEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var rowPanel = new Panel { ID = String.Format(GridPanelIDFormat, entity.ID) };

            var titlePanel = new Panel { CssClass = "ibox-title" };
            var toolsPanel = new Panel { CssClass = "ibox-tools" };
            var contentPanel = new Panel { CssClass = "ibox-content" };
            var tablePanel = new Panel { CssClass = "table-responsive" };

            var dataGrid = new GridView
            {
                ID = String.Format(GridIDFormat, entity.ID),
                ShowHeaderWhenEmpty = true,
                AutoGenerateColumns = false,
                UseAccessibleHeader = true,
                TableSectionHeader = true,
                CssClass = "tableIntern table table-striped table-bordered table-hover dataTable"
            };

            SetCommands(dataGrid, entity);

            MetaControls.Add(dataGrid.ID, dataGrid);

            if (entity.Controls != null)
            {
                var dataTable = new DataTable();

                var fields = entity.Controls.Cast<FieldEntity>();

                var orderedVisibleFields = (from n in fields
                                            where n.Visible && n.DisplayOnGrid.GetValueOrDefault()
                                            orderby n.OrderIndex, n.Name
                                            select n);

                foreach (var field in orderedVisibleFields)
                {
                    var dataField = Convert.ToString(field.ID);

                    var column = new GridViewBoundField
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
                newButton.CssClass = (Enabled ? "btn btn-primary btn-sm fa fa-plus" : "btn btn-default btn-sm fa fa-plus");
                newButton.ClientIDMode = ClientIDMode.Static;
                newButton.CommandName = "New";
                newButton.CommandArgument = String.Format("{0:n}/@", entity.ID);

                toolsPanel.Controls.Add(newButton);
            }

            tablePanel.Controls.Add(dataGrid);

            titlePanel.Controls.Add(toolsPanel);
            contentPanel.Controls.Add(tablePanel);

            rowPanel.Controls.Add(titlePanel);
            rowPanel.Controls.Add(contentPanel);

            if (entity.DependentFieldID != null)
                rowPanel.Visible = false;

            parent.Controls.Add(rowPanel);
        }
        private void SetTabContainer(Control parent, TabContainerEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var tabContainer = new Panel { CssClass = "tabs-container" };

            var activeNameContainer = new HiddenField();
            activeNameContainer.ClientIDMode = ClientIDMode.Static;
            activeNameContainer.ID = String.Format("tab_active_{0:n}", entity.ID);

            var containerHeader = new HtmlGenericControl("ul");
            containerHeader.Attributes["class"] = "nav nav-tabs";

            var containerContent = new Panel { CssClass = "tab-content" };

            containerHeader.Controls.Add(activeNameContainer);
            tabContainer.Controls.Add(containerHeader);
            tabContainer.Controls.Add(containerContent);

            if (entity.Controls != null)
            {
                foreach (var child in entity.Controls)
                {
                    var tabPage = child as TabPageEntity;
                    if (tabPage == null)
                        throw new Exception();

                    SetTabPage(containerHeader, containerContent, entity.ID, tabPage);
                }

                if (entity.DependentFieldID != null)
                    tabContainer.Visible = false;

                parent.Controls.Add(tabContainer);
            }
        }
        private void SetTabPage(Control headers, Control contents, Guid parentID, TabPageEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var tabHeader = new HtmlElement("li")
            {
                ID = String.Format(TabHeaderIDFormat, entity.ID)
            };

            var headerLink = new HtmlElement("a");
            headerLink.Attributes["href"] = String.Format("#tab_content_{0:n}", entity.ID);
            headerLink.Attributes["onclick"] = "onTabClick(this);";

            headerLink.Attributes["data-toggle"] = "tab";
            headerLink.Attributes["aria-expanded"] = "true";

            headerLink.Attributes["tab-id"] = String.Format("{0:n}", entity.ID);
            headerLink.Attributes["active-store"] = String.Format("tab_active_{0:n}", parentID);

            var titleLabel = new Label { Text = entity.Name };

            var tabContent = new Panel
            {
                ID = String.Format(TabContentIDFormat, entity.ID),
                ClientIDMode = ClientIDMode.Static,
                CssClass = "tab-pane"
            };

            var panelBody = new Panel { CssClass = "panel-body" };
            var rowPanel = new Panel { CssClass = "row" };

            if (headers.Controls.Count == 0)
                tabHeader.Attributes["class"] = "active";

            if (contents.Controls.Count == 0)
                tabContent.CssClass = "tab-pane active";

            panelBody.Controls.Add(rowPanel);

            headerLink.Controls.Add(titleLabel);

            tabHeader.Controls.Add(headerLink);
            tabContent.Controls.Add(panelBody);

            if (entity.Controls != null)
                SetStructure(rowPanel, entity.Controls);

            if (entity.DependentFieldID != null)
            {
                tabHeader.Visible = false;
                tabContent.Visible = false;
            }

            headers.Controls.Add(tabHeader);
            contents.Controls.Add(tabContent);
        }

        private void SetCommands(GridView dataGrid, GridEntity entity)
        {
            var commandsColumn = new TemplateField
            {
                ItemTemplate = new GridViewFieldTemplate(() => CreateCommands(entity))
            };

            dataGrid.Columns.Add(commandsColumn);
        }

        private void ApplyViewMode()
        {
            ApplyViewMode(FormData);
        }
        private void ApplyViewMode(FormDataUnit formData)
        {
            SetupActiveTab();

            if (DependentFields == null || DependentFields.Count == 0 || formData == null)
                return;

            var query = (from n in FirstLevelChildren.Values
                         let v = formData[Convert.ToString(n.ID)]
                         select new
                         {
                             Key = n.Name,
                             Value = v
                         });

            var formDataLp = query.ToLookup(n => n.Key, n => n.Value);
            var formDataDict = formDataLp.ToDictionary(n => n.Key, n => n.FirstOrDefault());

            foreach (var fieldsGrp in DependentFields)
            {
                var dependentSource = FirstLevelChildren.GetValueOrDefault(fieldsGrp.Key.GetValueOrDefault());
                if (dependentSource == null)
                    continue;

                var sourceField = dependentSource as FieldEntity;
                if (sourceField == null)
                    continue;

                var sourceKey = Convert.ToString(sourceField.ID);

                foreach (var targetEntity in fieldsGrp)
                {
                    var elementIds = GetElementID(targetEntity);

                    var elements = (from n in elementIds
                                    from m in AllControlsLp[n]
                                    select m);

                    if (!elements.Any())
                        continue;

                    var dependentExp = targetEntity.DependentExp;
                    if (String.IsNullOrWhiteSpace(dependentExp))
                        dependentExp = @"!IsEmpty(@)";

                    var sourceValue = formData[sourceKey];

                    formDataDict["@"] = sourceValue;
                    formDataDict["@val"] = sourceValue;
                    formDataDict["@value"] = sourceValue;

                    SetTargetVisibilities(elements, formDataDict, dependentExp);
                    FillFieldData(targetEntity, sourceField, formDataDict, dependentExp, sourceValue);
                }
            }
        }

        private void SetupActiveTab()
        {
            var activeTabs = GetActiveTabs();
            SetActiveTabs(activeTabs);
        }

        private String GetFieldID(FieldEntity entity)
        {
            if (entity.Type == "CheckBox")
                return String.Format(CheckIDFormat, entity.ID);

            if (entity.Type == "RadioButton")
                return String.Format(RadioIDFormat, entity.ID);

            if (entity.Type == "FileIDFormat")
                return String.Format(FileIDFormat, entity.ID);

            return String.Format(FieldIDFormat, entity.ID);
        }

        private Control CreateCommands(GridEntity entity)
        {
            //var viewIcon = new Label { CssClass = "fa fa-file" };
            //var viewText = new Label { CssClass = "linkTitle", Text = "View" };

            //var editIcon = new Label { CssClass = "fa fa-edit" };
            //var editText = new Label { CssClass = "linkTitle", Text = "Edit" };

            //var deleteIcon = new Label { CssClass = "fa fa-trash-o" };
            //var deleteText = new Label { CssClass = "linkTitle", Text = "Delete" };

            var viewCommand = new LinkButton();
            viewCommand.ID = String.Format(viewCommandIDFormat, entity.ID);
            viewCommand.CssClass = "btn btn-info btn-sm fa fa-search";
            viewCommand.CommandName = "View";
            viewCommand.Command += command_OnCommand;
            viewCommand.DataBinding += command_OnDataBinding;
            //viewCommand.Controls.Add(viewIcon);
            //viewCommand.Controls.Add(viewText);

            var editCommand = new LinkButton();
            editCommand.ID = String.Format(editCommandIDFormat, entity.ID);
            editCommand.Visible = Enabled;
            editCommand.CssClass = (Enabled ? "btn btn-primary btn-sm fa fa-edit" : "btn btn-default btn-sm fa fa-edit");
            editCommand.CommandName = "Edit";
            editCommand.Command += command_OnCommand;
            editCommand.DataBinding += command_OnDataBinding;
            //editCommand.Controls.Add(editIcon);
            //editCommand.Controls.Add(editText);

            var deleteCommand = new LinkButton();
            deleteCommand.ID = String.Format(deleteCommandIDFormat, entity.ID);
            deleteCommand.Visible = Enabled;
            deleteCommand.CssClass = (Enabled ? "btn btn-danger btn-sm fa fa-trash-o" : "btn btn-default btn-sm fa fa-trash-o");
            deleteCommand.CommandName = "Delete";
            deleteCommand.Command += command_OnCommand;
            deleteCommand.OnClientClick = "return confirm('დარწმუნებული ხართ?/Are you sure?')";

            deleteCommand.DataBinding += command_OnDataBinding;
            //deleteCommand.Controls.Add(deleteIcon);
            //deleteCommand.Controls.Add(deleteText);

            var panel = new Panel();
            panel.Controls.Add(viewCommand);
            panel.Controls.Add(new LiteralControl("&nbsp;"));

            panel.Controls.Add(editCommand);
            panel.Controls.Add(new LiteralControl("&nbsp;"));

            panel.Controls.Add(deleteCommand);
            panel.Controls.Add(new LiteralControl("&nbsp;"));

            return panel;
        }

        private IDictionary<String, Object> GetAllFieldKeys()
        {
            var dict = new Dictionary<String, Object>(StringComparer.OrdinalIgnoreCase);

            foreach (var key in Request.Form.AllKeys)
            {
                var index = key.LastIndexOf("$", StringComparison.OrdinalIgnoreCase);
                if (index > -1)
                {
                    var fieldKey = key.Substring(index + 1);
                    dict.Add(fieldKey, Request.Form[key]);
                }
            }

            foreach (var key in Request.Files.AllKeys)
            {
                var index = key.LastIndexOf("$", StringComparison.OrdinalIgnoreCase);
                if (index > -1)
                {
                    var fieldKey = key.Substring(index + 1);
                    dict.Add(fieldKey, Request.Files[key]);
                }
            }

            foreach (var key in MetaControls.Keys)
            {
                if (!dict.ContainsKey(key))
                    dict.Add(key, null);
            }

            return dict;
        }

        private IEnumerable<String> GetElementID(ControlEntity entity)
        {
            if (entity is FieldEntity)
                yield return String.Format(FieldPabelIDFormat, entity.ID);
            else if (entity is GridEntity)
                yield return String.Format(GridPanelIDFormat, entity.ID);
            else if (entity is GroupEntity)
                yield return String.Format(GroupIDFormat, entity.ID);
            else if (entity is TabContainerEntity)
                yield return String.Format(TabContainerIDFormat, entity.ID);
            else if (entity is TabPageEntity)
            {
                yield return String.Format(TabHeaderIDFormat, entity.ID);
                yield return String.Format(TabContentIDFormat, entity.ID);
            }
        }

        private Panel InitPrivacyGroup(Panel panel, FieldEntity entity)
        {
            var inputGroup = new Panel { CssClass = "input-group" };
            panel.Controls.Add(inputGroup);

            panel = inputGroup;

            var privacySpan = new Label { CssClass = "input-group-addon" };
            var checkBox = new CheckBox
            {
                ID = String.Format(SecIDFormat, entity.ID),
                ClientIDMode = ClientIDMode.Static,
            };

            MetaControls.Add(checkBox.ID, checkBox);

            privacySpan.Controls.Add(checkBox);
            panel.Controls.Add(privacySpan);

            return panel;
        }

        private CollectionEntity GetCollectionEntity(Guid? collectionID)
        {
            var cache = CommonObjectCache.InitObjectCache(CollectionEntityCacheKey, ConcurrencyHelper.CreateDictionary<Guid?, CollectionEntity>);
            lock (cache)
            {
                CollectionEntity entity;
                if (cache.TryGetValue(collectionID, out entity))
                    return entity;

                var dbEntity = HbSession.Query<GM_Collection>().FirstOrDefault(n => n.ID == collectionID);
                if (dbEntity == null)
                    return null;

                var converter = new CollectionEntityModelConverter(HbSession);
                var model = converter.Convert(dbEntity);

                entity = model.Entity;
                if (entity == null || entity.Fields == null)
                    return null;

                cache[collectionID] = entity;

                return entity;
            }
        }

        private ILookup<Guid?, ControlEntity> GetDependentFields(ContentEntity parent)
        {
            var query = (from n in FormStructureUtil.PreOrderTraversal(parent)
                         where n.DependentFieldID != null
                         select n);

            var lp = query.ToLookup(n => n.DependentFieldID);
            return lp;
        }

        private Control CreateControl(Control parent, FieldEntity entity, bool autoPostBack)
        {
            if (entity.Type == "TextBox")
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control",
                    AutoPostBack = autoPostBack,
                };

                if (!String.IsNullOrWhiteSpace(entity.Mask))
                    control.Attributes["data-mask"] = entity.Mask;

                parent.Controls.Add(control);
                return control;
            }

            if (entity.Type == "TextArea")
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control",
                    AutoPostBack = autoPostBack,
                    TextMode = TextBoxMode.MultiLine,
                };

                if (!String.IsNullOrWhiteSpace(entity.Mask))
                    control.Attributes["data-mask"] = entity.Mask;

                parent.Controls.Add(control);
                return control;
            }

            if (entity.Type == "Number")
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "intSpinEdit",
                    AutoPostBack = autoPostBack,
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
                    CssClass = "form-control",
                    AutoPostBack = autoPostBack,
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
                    CssClass = "form-control",
                    AutoPostBack = autoPostBack,
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
                    //CssClass = "form-control",
                    AutoPostBack = autoPostBack,
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
                    GroupName = entity.Tag,
                    AutoPostBack = autoPostBack,
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
                    CssClass = "chosen-select form-control selectWidth",
                    AutoPostBack = autoPostBack,
                };

                if (entity.DataSourceID != null)
                {
                    var dataBinder = CreateDataBinder(entity.DataSourceID, entity);
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

                parent.Controls.Add(control);
                return control;
            }

            if (entity.Type == "FileUpload")
            {
                var fileOutputPanel = new Panel();
                fileOutputPanel.ID = String.Format("pnlOuput_{0:n}", entity.ID);
                fileOutputPanel.Visible = false;

                var downloadUrl = new UrlHelper("~/Handlers/Download.ashx");
                downloadUrl[FormDataConstants.OwnerIDField] = OwnerID;
                downloadUrl[FormDataConstants.IDField] = RecordID;
                downloadUrl["FieldID"] = entity.ID;

                var donwloadLink = new HyperLink();
                donwloadLink.NavigateUrl = downloadUrl.ToString();
                donwloadLink.Attributes["target"] = "_blank";
                donwloadLink.Controls.Add(new Label { Text = "Download" });

                fileOutputPanel.Controls.Add(donwloadLink);

                var fileInputPanel = new Panel { CssClass = "fileinput fileinput-new input-group" };
                fileInputPanel.ID = String.Format("pnlInput_{0:n}", entity.ID);
                fileInputPanel.Visible = true;
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

                var removeLabel = new Label { Text = "Remove" };

                var removeLink = new HyperLink();
                removeLink.CssClass = "input-group-addon btn btn-default fileinput-exists";
                removeLink.Attributes["data-dismiss"] = "fileinput";
                removeLink.Controls.Add(removeLabel);

                formControlPanel.Controls.Add(fileIconLabel);
                formControlPanel.Controls.Add(fileNameLabel);

                inputLabel.Controls.Add(selectLabel);
                inputLabel.Controls.Add(changeLabel);
                inputLabel.Controls.Add(fileUpload);

                fileInputPanel.Controls.Add(formControlPanel);
                fileInputPanel.Controls.Add(inputLabel);
                fileInputPanel.Controls.Add(removeLink);

                var fileDataPanel = new Panel();
                fileDataPanel.ID = String.Format("pnlFileData_{0:n}", entity.ID);
                fileDataPanel.Controls.Add(fileOutputPanel);
                fileDataPanel.Controls.Add(fileInputPanel);

                parent.Controls.Add(fileDataPanel);
                return fileUpload;
            }

            var message = String.Format("Unknwn element type '{0}'", entity.Type);
            throw new Exception(message);
        }

        private DictionaryDataBinder CreateDataBinder(Guid? collectionID, FieldEntity fieldEntity)
        {
            var textExp = fieldEntity.TextExpression;
            var valueExp = fieldEntity.ValueExpression;

            var records = LoadDataRecords(collectionID, fieldEntity);

            var dictionaryBinder = new DictionaryDataBinder(records, textExp, valueExp);
            return dictionaryBinder;
        }

        private IDictionary<String, Object> FindDataRecord(Guid? collectionID, String valueExp, Object value)
        {
            var recordKey = String.Format("{0}_{1}_{2}", collectionID, valueExp, value);

            var cache = CommonObjectCache.InitObjectCache(FindDataRecordCacheKey, ConcurrencyHelper.CreateDictionary<String, IDictionary<String, Object>>);
            lock (cache)
            {
                IDictionary<String, Object> record;
                if (cache.TryGetValue(recordKey, out record))
                    return record;

                var entity = GetCollectionEntity(collectionID);
                if (entity == null)
                    return null;

                var fields = entity.Fields.ToDictionary(n => Convert.ToString(n.ID), n => n.Name);
                fields.Add(FormDataConstants.IDField, FormDataConstants.IDField);

                var dictionaries = TransferDataRecords(collectionID, fields);

                var expNode = ExpressionParser.GetOrParse(valueExp);
                foreach (var dict in dictionaries)
                {
                    var result = ExpressionEvaluator.Eval(expNode, n => dict[n]);

                    if (Comparer.Default.Compare(result, value) == 0)
                    {
                        cache[recordKey] = dict;
                        return dict;
                    }
                }

                cache[recordKey] = null;
                return null;
            }
        }

        private IEnumerable<IDictionary<String, Object>> LoadDataRecords(Guid? collectionID, FieldEntity fieldEntity)
        {
            if (collectionID == null)
                return null; 

            var entity = GetCollectionEntity(collectionID);
            if (entity == null)
                return null;

            var fields = entity.Fields.ToDictionary(n => Convert.ToString(n.ID), n => n.Name);
            fields.Add(FormDataConstants.IDField, FormDataConstants.IDField);

            var dictionaries = TransferDataRecords(collectionID, fields);

            dictionaries = FilterData(dictionaries, fieldEntity);
            dictionaries = SortData(dictionaries, fieldEntity);

            return dictionaries;
        }
        private IEnumerable<IDictionary<String, Object>> TransferDataRecords(Guid? collectionID, IDictionary<String, String> fields)
        {
            var cache = CommonObjectCache.InitObjectCache(TransferDataCacheKey, ConcurrencyHelper.CreateDictionary<Guid?, IEnumerable<IDictionary<String, Object>>>);
            lock (cache)
            {
                IEnumerable<IDictionary<String, Object>> records;
                if (cache.TryGetValue(collectionID, out records))
                    return records;

                var collection = MongoDbUtil.GetCollection(collectionID);
                if (collection == null)
                {
                    cache[collectionID] = null;
                    return null;
                }

                var query = collection.AsQueryable();
                var dicts = BsonDocumentConverter.ConvertToDictionary(query);

                var results = new List<IDictionary<String, Object>>();

                foreach (var dict in dicts)
                {
                    var result = new Dictionary<String, Object>();
                    foreach (var pair in fields)
                    {
                        var val = dict.GetValueOrDefault(pair.Key);
                        result[pair.Value] = val;
                    }

                    results.Add(result);
                }

                cache[collectionID] = results;
                return results;
            }
        }

        private IEnumerable<IDictionary<String, Object>> SortData(IEnumerable<IDictionary<String, Object>> dictionaries, FieldEntity fieldEntity)
        {
            var expression = fieldEntity.DataSourceSortExp;
            if (String.IsNullOrWhiteSpace(expression))
                return dictionaries;

            var rx = new Regex(@"(?<name>.+?)(?<type>(asc|desc)?)($|,)", RegexOptions.Compiled);
            if (!rx.IsMatch(expression))
                return dictionaries;

            var fields = new Dictionary<String, String>();

            var matches = rx.Matches(expression);
            foreach (Match match in matches)
            {
                var name = match.Groups["name"].Value;
                var type = match.Groups["type"].Value;

                fields[name] = type;
            }

            var comparer = new DictionaryComparer(fields);
            dictionaries = dictionaries.OrderBy(n => n, comparer);

            return dictionaries;
        }

        private IEnumerable<IDictionary<String, Object>> FilterData(IEnumerable<IDictionary<String, Object>> dictionaries, FieldEntity fieldEntity)
        {
            if (dictionaries == null)
                return null;

            var filterExp = fieldEntity.DataSourceFilterExp;
            if (String.IsNullOrWhiteSpace(filterExp))
                return dictionaries;

            if (fieldEntity.DependentFieldID == null)
                return ApplyFilter(dictionaries, filterExp);

            var formData = FormData;
            if (formData == null)
                return dictionaries;

            var sourceField = FirstLevelChildren.GetValueOrDefault(fieldEntity.DependentFieldID.Value) as FieldEntity;
            if (sourceField == null)
                return dictionaries;

            var sourceKey = Convert.ToString(sourceField.ID);
            var sourceValue = formData[sourceKey];

            if (sourceField.Type == "ComboBox" &&
                sourceField.DataSourceID != null &&
                !String.IsNullOrWhiteSpace(sourceField.ValueExpression))
            {
                var dependentExp = sourceField.DependentExp;
                if (String.IsNullOrWhiteSpace(dependentExp))
                    dependentExp = @"!IsEmpty(@)";

                var recordKey = String.Format("{0}_{1}_({2})", sourceField.DataSourceID, sourceValue, dependentExp);

                var dataRecord = DependentRecords.GetValueOrDefault(recordKey) as IDictionary<String, Object>;
                if (dataRecord == null)
                {
                    var sourceCollectionID = sourceField.DataSourceID;
                    var sourceValueExp = sourceField.ValueExpression;

                    dataRecord = FindDataRecord(sourceCollectionID, sourceValueExp, sourceValue);
                    if (dataRecord != null)
                        DependentRecords[recordKey] = dataRecord;
                }

                if (dataRecord != null)
                    dictionaries = ApplyFilter(dictionaries, dataRecord, filterExp);
            }
            else
            {
                dictionaries = ApplyFilter(dictionaries, sourceValue, filterExp);
            }

            return dictionaries;
        }

        private IEnumerable<IDictionary<String, Object>> ApplyFilter(IEnumerable<IDictionary<String, Object>> target, String filterExp)
        {
            return ApplyFilter(target, (Object)null, filterExp);
        }
        private IEnumerable<IDictionary<String, Object>> ApplyFilter(IEnumerable<IDictionary<String, Object>> target, Object sourceValue, String filterExp)
        {
            var filterNode = ExpressionParser.GetOrParse(filterExp);

            foreach (var targetRecord in target)
            {
                var adpTargetRecord = new Dictionary<String, Object>();
                foreach (var pair in targetRecord)
                    adpTargetRecord[String.Format("${0}", pair.Key)] = pair.Value;

                var result = ExpressionEvaluator.Eval(filterNode, n =>
                {
                    if (n == "@" || n == "@val" || n == "@value")
                        return sourceValue;

                    return adpTargetRecord[n];
                });

                var @bool = DataConverter.ToNullableBool(result);
                if (@bool.GetValueOrDefault())
                    yield return targetRecord;
            }
        }
        private IEnumerable<IDictionary<String, Object>> ApplyFilter(IEnumerable<IDictionary<String, Object>> target, IDictionary<String, Object> sourceRecord, String filterExp)
        {
            var adpSourceRecord = new Dictionary<String, Object>();
            foreach (var pair in sourceRecord)
                adpSourceRecord[String.Format("@{0}", pair.Key)] = pair.Value;

            var filterNode = ExpressionParser.GetOrParse(filterExp);

            foreach (var targetRecord in target)
            {
                var adpTargetRecord = new Dictionary<String, Object>();
                foreach (var pair in targetRecord)
                    adpTargetRecord[String.Format("${0}", pair.Key)] = pair.Value;

                var result = ExpressionEvaluator.Eval(filterNode, n =>
                {
                    if (n.StartsWith("@"))
                        return adpSourceRecord[n];

                    return adpTargetRecord[n];
                });

                var @bool = DataConverter.ToNullableBool(result);
                if (@bool.GetValueOrDefault())
                    yield return targetRecord;
            }
        }

        private bool FillFieldData(FieldEntity entity, IDictionary<String, Object> content)
        {
            var fiedlID = GetFieldID(entity);
            var controls = AllControlsLp[fiedlID];

            var expNode = ExpressionParser.GetOrParse(entity.DependentFillExp);

            var expResult = ExpressionEvaluator.Eval(expNode, n => content[n]);
            if (expResult == null)
                return false;

            foreach (var control in controls)
            {
                var textBox = control as TextBox;
                if (textBox != null)
                    textBox.Text = Convert.ToString(expResult);

                var checkBox = control as CheckBox;
                if (checkBox != null)
                    checkBox.Checked = DataConverter.ToNullableBool(expResult).GetValueOrDefault();

                var redioButton = control as RadioButton;
                if (redioButton != null)
                    redioButton.Checked = DataConverter.ToNullableBool(expResult).GetValueOrDefault();

                var comboBox = control as DropDownList;
                if (comboBox != null)
                    comboBox.TrySetSelectedValue(expResult);
            }

            return true;
        }
        private void FillFieldData(ControlEntity targetEntity, FieldEntity sourceField, IDictionary<String, Object> formDataDict, String dependentExp, Object sourceValue)
        {
            if (sourceValue == null)
                return;

            var fieldEntity = targetEntity as FieldEntity;
            if (fieldEntity == null || String.IsNullOrWhiteSpace(fieldEntity.DependentFillExp))
                return;

            if (sourceField.Type == "ComboBox" &&
                sourceField.DataSourceID != null &&
                !String.IsNullOrWhiteSpace(sourceField.ValueExpression))
            {
                var recordKey = String.Format("{0}_({1})", sourceField.DataSourceID, dependentExp);

                var dataRecord = DependentRecords.GetValueOrDefault(recordKey) as IDictionary<String, Object>;
                if (dataRecord == null)
                {
                    var collectionID = sourceField.DataSourceID;
                    var valueExp = sourceField.ValueExpression;

                    dataRecord = FindDataRecord(collectionID, valueExp, sourceValue);
                    if (dataRecord != null)
                        DependentRecords[recordKey] = dataRecord;
                }

                if (dataRecord != null)
                {
                    if (!FillFieldData(fieldEntity, dataRecord))
                        FillFieldData(fieldEntity, formDataDict);
                }
            }
            else
            {
                FillFieldData(fieldEntity, formDataDict);
            }
        }

        private void SetTargetVisibilities(IEnumerable<Control> elements, IDictionary<String, Object> formData, String expression)
        {
            var result = ExpressionEvaluator.Eval(expression, n => formData.GetValueOrDefault(n));
            var visible = DataConverter.ToNullableBoolean(result);

            foreach (var element in elements)
                element.Visible = visible.GetValueOrDefault();
        }
    }
}