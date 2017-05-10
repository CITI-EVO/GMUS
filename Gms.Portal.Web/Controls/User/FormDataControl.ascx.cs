using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Controls;
using CITI.EVO.Tools.Web.UI.Helpers;
using DevExpress.Web.ASPxTreeList;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Enums;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using MongoDB.Driver;
using Label = CITI.EVO.Tools.Web.UI.Controls.Label;
using Panel = CITI.EVO.Tools.Web.UI.Controls.Panel;
using ListBox = CITI.EVO.Tools.Web.UI.Controls.ListBox;
using TextBox = CITI.EVO.Tools.Web.UI.Controls.TextBox;
using GridView = CITI.EVO.Tools.Web.UI.Controls.GridView;
using CheckBox = CITI.EVO.Tools.Web.UI.Controls.CheckBox;
using LinkButton = CITI.EVO.Tools.Web.UI.Controls.LinkButton;
using RadioButton = CITI.EVO.Tools.Web.UI.Controls.RadioButton;
using HtmlElement = CITI.EVO.Tools.Web.UI.Controls.HtmlElement;
using DropDownList = CITI.EVO.Tools.Web.UI.Controls.DropDownList;
using FieldEntity = Gms.Portal.Web.Entities.FormStructure.FieldEntity;
using ASPxTreeList = CITI.EVO.Tools.Web.UI.Controls.ASPxTreeList;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormDataControl : BaseUserControl
    {
        private const String FileIDFormat = "file_{0:n}";
        private const String FieldIDFormat = "field_{0:n}";
        private const String ButtonIDFormat = "btn_{0:n}";
        private const String CheckIDFormat = "chk_{0:n}";
        private const String RadioIDFormat = "rb_{0:n}";

        //private const String LookupBtnIDFormat = "lpBtn_{0:n}";
        private const String CheckListIDFormat = "chkLst_{0:n}";
        private const String FieldPabelIDFormat = "fldPanel_{0:n}";
        private const String FileDataPanelFormat = "pnlFileData_{0:n}";
        private const String FileInputPanelFormat = "pnlInput_{0:n}";
        private const String FileOuputPanelFormat = "pnlOuput_{0:n}";

        private const String SecIDFormat = "sec_{0:n}";
        private const String InspIDFormat = "insp_{0:n}";
        private const String TreeIDFormat = "tree_{0:n}";
        private const String GridIDFormat = "grid_{0:n}";
        private const String GroupIDFormat = "grp_{0:n}";
        private const String GridPanelIDFormat = "gridPnl_{0:n}";
        private const String TreePanelIDFormat = "treePnl_{0:n}";
        private const String TabHeaderIDFormat = "tab_header_{0:n}";
        private const String TabContentIDFormat = "tab_content_{0:n}";
        private const String TabContainerIDFormat = "tbc_{0:n}";

        private const String newCommandIDFormat = "newCmd_{0:n}";
        private const String viewCommandIDFormat = "viewCmd_{0:n}";
        private const String editCommandIDFormat = "editCmd_{0:n}";
        private const String cloneCommandIDFormat = "cloneCmd_{0:n}";
        private const String deleteCommandIDFormat = "deleteCmd_{0:n}";
        private const String inspectCommandIDFormat = "inspectCmd_{0:n}";

        private const String childRecordCommandArgFormat = "{0:n}/{1:n}/{2:n}";

        private FormDataUnit _lastFormData;
        private ContentEntity _contentEntity;
        private ContentEntity _parentContentEntity;

        public event EventHandler<CommandEventArgs> Command;
        protected virtual void OnCommand(CommandEventArgs e)
        {
            if (Command != null)
                Command(this, e);
        }

        public FormMode? Mode
        {
            get { return DataConverter.ToNullableEnum<FormMode>(hdMode.Value); }
            set { hdMode.Value = Convert.ToString(value); }
        }

        public bool Enabled
        {
            get { return DataConverter.ToNullableBool(hdEnabled.Value).GetValueOrDefault(); }
            set { hdEnabled.Value = Convert.ToString(value); }
        }

        public Guid? UserID
        {
            get { return DataConverter.ToNullableGuid(hdUserID.Value); }
            set { hdUserID.Value = Convert.ToString(value); }
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

        public Guid? StatusID
        {
            get { return DataConverter.ToNullableGuid(hdStatusID.Value); }
            set { hdStatusID.Value = Convert.ToString(value); }
        }

        public Guid? ContainerID
        {
            get { return DataConverter.ToNullableGuid(hdContainerID.Value); }
            set { hdContainerID.Value = Convert.ToString(value); }
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

        private FormDataUnit _parentFormData;
        protected FormDataUnit ParentFormData
        {
            get
            {
                if (ParentID == null || FormID == null)
                    return null;

                if (_parentFormData == null)
                    _parentFormData = LoadFormData(FormID, ParentID);

                return _parentFormData;
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

        private ILookup<Guid?, FieldEntity> _dependentFields;
        protected ILookup<Guid?, FieldEntity> DependentFields
        {
            get
            {
                if (_dependentFields == null)
                {
                    var query = DataFields.Values.Where(n => n.DependentFieldID != null);
                    _dependentFields = query.ToLookup(n => n.DependentFieldID);
                }

                return _dependentFields;
            }
        }

        private IDictionary<Guid, FieldEntity> _dataFields;
        protected IDictionary<Guid, FieldEntity> DataFields
        {
            get
            {
                if (_dataFields == null)
                    _dataFields = Children.Values.OfType<FieldEntity>().ToDictionary(n => n.ID);

                return _dataFields;
            }
        }

        private IDictionary<Guid, GridEntity> _dataGrids;
        protected IDictionary<Guid, GridEntity> DataGrids
        {
            get
            {
                if (_dataGrids == null)
                    _dataGrids = Children.Values.OfType<GridEntity>().ToDictionary(n => n.ID);

                return _dataGrids;
            }
        }

        private IDictionary<Guid, TreeEntity> _treeLists;
        protected IDictionary<Guid, TreeEntity> TreeLists
        {
            get
            {
                if (_treeLists == null)
                    _treeLists = Children.Values.OfType<TreeEntity>().ToDictionary(n => n.ID);

                return _treeLists;
            }
        }

        private IDictionary<Guid, ControlEntity> _children;
        protected IDictionary<Guid, ControlEntity> Children
        {
            get
            {
                if (_children == null)
                {
                    var children = FormStructureUtil.PreOrderTraversal(_contentEntity);
                    _children = children.ToDictionary(n => n.ID);
                }

                return _children;
            }
        }

        private IDictionary<Guid, ControlEntity> _firstLevelChildren;
        protected IDictionary<Guid, ControlEntity> FirstLevelChildren
        {
            get
            {
                if (_firstLevelChildren == null)
                {
                    var firstLevelChildren = FormStructureUtil.PreOrderFirstLevelTraversal(_contentEntity);
                    _firstLevelChildren = firstLevelChildren.ToDictionary(n => n.ID);
                }

                return _firstLevelChildren;
            }
        }

        private IDictionary<Guid, ElementTreeNodeEntity> _elementTree;
        protected IDictionary<Guid, ElementTreeNodeEntity> ElementTree
        {
            get
            {
                if (_elementTree == null)
                {
                    var children = FormStructureUtil.CreateTree(_contentEntity);
                    _elementTree = children.ToDictionary(n => n.ID.GetValueOrDefault());
                }

                return _elementTree;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var formData = (_lastFormData ?? FormData);
            ApplyViewMode(formData);
        }

        protected void command_OnClick(object sender, EventArgs eventArgs)
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

            var descriptor = DataBoundHelper.GetDescriptor(linkButton);
            if (descriptor == null)
                return;

            var ownerID = descriptor.GetValue(FormDataConstants.OwnerIDField);
            var recordID = descriptor.GetValue(FormDataConstants.IDField);
            var containerID = descriptor.GetValue(FormDataConstants.ContainerIDField);

            linkButton.CommandArgument = String.Format(childRecordCommandArgFormat, ownerID, recordID, containerID);
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

            var containersQuery = (from n in ElementTree.Values
                                   where activeTabs.Contains(n.ID.GetValueOrDefault())
                                   select new
                                   {
                                       ID = n.ID.GetValueOrDefault(),
                                       ParentID = n.ParentID.GetValueOrDefault()
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
                var headerID = $"tab_header_{tabID:n}";
                var contentID = $"tab_content_{tabID:n}";

                var header = tabsDict.GetValueOrDefault(headerID) as HtmlControl;
                if (header != null)
                    header.Attributes["class"] = "active";

                var content = tabsDict.GetValueOrDefault(contentID) as WebControl;
                if (content != null)
                    content.CssClass = "tab-pane active";

                if (containers.Count > 0)
                {
                    var parentID = containers[tabID];
                    var activeID = $"tab_active_{parentID:n}";

                    var activeFld = tabsDict.GetValueOrDefault(activeID) as HiddenField;
                    if (activeFld != null)
                        activeFld.Value = $"{tabID:n}";
                }
            }
        }

        public void InitStructure(ContentEntity entity)
        {
            InitStructure(entity, null, null);
        }
        public void InitStructure(ContentEntity entity, ContentEntity parent)
        {
            InitStructure(entity, parent, null);
        }

        public void InitStructure(ContentEntity entity, FormDataUnit formData)
        {
            InitStructure(entity, null, formData);
        }
        public void InitStructure(ContentEntity entity, ContentEntity parent, FormDataUnit formData)
        {
            MetaControls.Clear();

            if (entity == null)
                return;

            _formData = null;
            _children = null;
            _metaControls = null;
            _dependentFields = null;

            _lastFormData = formData;
            _contentEntity = entity;
            _parentContentEntity = parent;

            InitStructure(entity.Controls);
        }

        private void InitStructure(IEnumerable<ControlEntity> entities)
        {
            MetaControls.Clear();

            if (entities == null)
                return;

            SetStructure(pnlMain, entities);
        }

        public void MarkChangedFields(IDictionary<Guid, Object> fields)
        {
            foreach (var pair in fields)
            {
                var entity = Children.GetValueOrDefault(pair.Key) as FieldEntity;
                if (entity == null)
                    continue;

                var controlID = GetFieldID(entity);

                var control = MetaControls.GetValueOrDefault(controlID) as WebControl;
                if (control == null)
                    continue;

                control.Attributes["title"] = Convert.ToString(pair.Value);

                var elementIds = GetElementID(entity);

                var elements = (from n in elementIds
                                from m in AllControlsLp[n]
                                select m).OfType<Panel>();

                foreach (var element in elements)
                    element.CssClass = $"{element.CssClass} has-error";
            }
        }

        public FormDataUnit GetFormData()
        {
            if (!IsPostBack)
                return null;

            var comparer = StringComparer.OrdinalIgnoreCase;
            var formData = new FormDataUnit(FormID, OwnerID, RecordID, ParentID, DateCreated);

            var fieldsValues = GetAllFieldValues();

            var valuesLp = fieldsValues.ToLookup(n => n.Type);

            var lists = valuesLp["chkLst"];
            foreach (var item in lists)
            {
                var key = Convert.ToString(item.Key);

                if (ReferenceEquals(item.Value, FormNoData.Value))
                    formData[key] = item.Value;
                else
                {
                    var value = Convert.ToString(item.Value);

                    var parts = (from n in value.Split(',')
                                 where !String.IsNullOrWhiteSpace(n)
                                 select (Object)n.Trim());

                    formData[key] = parts.Distinct().ToArray();
                }
            }

            var fields = valuesLp["sys"].Union(valuesLp["field"]);
            foreach (var item in fields)
            {
                var key = Convert.ToString(item.Key);
                formData[key] = item.Value;
            }

            var checkBoxes = valuesLp["chk"];
            foreach (var item in checkBoxes)
            {
                var key = Convert.ToString(item.Key);
                var value = item.Value;

                if (!ReferenceEquals(value, FormNoData.Value))
                    formData[key] = comparer.Equals(item.Value, "on");
                else
                    formData[key] = value;
            }

            var radioBtns = valuesLp["rb"];
            foreach (var item in radioBtns)
            {
                var key = Convert.ToString(item.Key);
                var value = item.Value;

                if (!ReferenceEquals(value, FormNoData.Value))
                    formData[key] = comparer.Equals(item.Value, "on");
                else
                    formData[key] = false;
            }

            foreach (var item in valuesLp["file"])
            {
                var key = Convert.ToString(item.Key);

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

            foreach (var dataGrid in DataGrids.Values)
            {
                var listRef = new FormDataListRef(FormID, dataGrid.ID, RecordID);

                var key = Convert.ToString(dataGrid.ID);
                formData[key] = listRef;
            }

            foreach (var treeList in TreeLists.Values)
            {
                var listRef = new FormDataListRef(FormID, treeList.ID, RecordID);

                var key = Convert.ToString(treeList.ID);
                formData[key] = listRef;
            }

            var privates = formData[FormDataConstants.PrivacyFields];
            if (!ReferenceEquals(privates, FormNoData.Value))
            {
                var privateFields = privates as ISet<String>;
                if (privateFields == null)
                {
                    privateFields = new HashSet<String>();
                    formData.PrivateFields = privateFields;
                }

                foreach (var item in valuesLp["sec"])
                {
                    if (comparer.Equals(item.Value, "on"))
                    {
                        var key = Convert.ToString(item.Key);
                        privateFields.Add(key);
                    }
                }
            }

            var reviews = formData[FormDataConstants.ReviewFields];
            if (!ReferenceEquals(reviews, FormNoData.Value))
            {
                var reviewFields = reviews as ISet<String>;
                if (reviewFields == null)
                {
                    reviewFields = new HashSet<String>();
                    formData.ReviewFields = reviewFields;
                }

                foreach (var item in valuesLp["insp"])
                {
                    if (comparer.Equals(item.Value, "on"))
                    {
                        var key = Convert.ToString(item.Key);
                        reviewFields.Add(key);
                    }
                }
            }

            var dataFields = FirstLevelChildren.Values.OfType<FieldEntity>();
            var expGlobals = new ExpressionGlobalsUtil(UserID, _contentEntity, formData);

            foreach (var entity in dataFields)
            {
                var fieldKey = Convert.ToString(entity.ID);

                if (String.IsNullOrWhiteSpace(entity.FieldValueExpression))
                    continue;

                var expNode = ExpressionParser.GetOrParse(entity.FieldValueExpression);

                Object result;
                if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out result))
                    continue;

                formData[fieldKey] = result;
            }

            return formData;
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
            else
            {
                formData = new FormDataUnit(formData);
            }

            _lastFormData = formData;

            StatusID = (StatusID ?? formData.StatusID);

            var reviewFields = formData.ReviewFields;
            var privateFields = formData.PrivateFields;

            var isMyData = (formData.UserID == UserUtil.GetCurrentUserID() || UserUtil.IsSuperAdmin());

            var fieldKeys = formData.Keys.ToList();

            foreach (var fieldKey in fieldKeys)
            {
                var fieldID = DataConverter.ToNullableGuid(fieldKey);
                if (fieldID == null)
                    continue;

                var fieldValue = formData[fieldKey];

                if (ReferenceEquals(fieldValue, FormNoData.Value))
                    fieldValue = null;

                if (Mode == FormMode.Inspect)
                {
                    if (reviewFields != null && reviewFields.Count > 0 && reviewFields.Contains(fieldKey))
                    {
                        var inspectControlID = String.Format(InspIDFormat, fieldID);

                        var inspectControl = MetaControls.GetValueOrDefault(inspectControlID);
                        if (inspectControl != null)
                        {
                            var checkBox = (CheckBox)inspectControl;
                            checkBox.Checked = true;
                        }
                    }
                }

                if (privateFields != null && privateFields.Count > 0)
                {
                    if (privateFields.Contains(fieldKey))
                    {
                        var secControlID = String.Format(SecIDFormat, fieldID);

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

                if (fieldValue is FormDataListBase || fieldValue is FormDataListRef)
                {

                    var gridControlID = String.Format(GridIDFormat, fieldID);
                    var treeControlID = String.Format(TreeIDFormat, fieldID);

                    var control = MetaControls.GetValueOrDefault(gridControlID);
                    if (control == null)
                    {
                        control = MetaControls.GetValueOrDefault(treeControlID);
                        if (control == null)
                            continue;
                    }

                    var formDataList = GetFormDataList(fieldValue);
                    var dictionatries = formDataList.Cast<IDictionary<String, Object>>();

                    var dataFieldSet = GetContainerFields(control);
                    var dataView = new DictionaryDataView(dictionatries, dataFieldSet);

                    if (control is GridView)
                    {
                        var dataGrid = (GridView)control;

                        dataGrid.DataSource = dataView;
                        dataGrid.DataBind();
                    }
                    else if (control is ASPxTreeList)
                    {
                        var treeList = (ASPxTreeList)control;

                        treeList.DataSource = dataView;
                        treeList.DataBind();
                    }
                }

                var checkListControlID = String.Format(CheckListIDFormat, fieldID);
                var fieldControlID = String.Format(FieldIDFormat, fieldID);
                var checkControlID = String.Format(CheckIDFormat, fieldID);
                var radioControlID = String.Format(RadioIDFormat, fieldID);
                var fileControlID = String.Format(FileIDFormat, fieldID);

                var fieldControl = MetaControls.GetValueOrDefault(checkListControlID);
                if (fieldControl == null)
                    fieldControl = MetaControls.GetValueOrDefault(fieldControlID);

                if (fieldControl == null)
                    fieldControl = MetaControls.GetValueOrDefault(checkControlID);

                if (fieldControl == null)
                    fieldControl = MetaControls.GetValueOrDefault(radioControlID);

                if (fieldControl == null)
                    fieldControl = MetaControls.GetValueOrDefault(fileControlID);

                if (fieldControl is TextBox)
                {
                    var textBox = (TextBox)fieldControl;
                    textBox.Text = Convert.ToString(fieldValue);
                }
                else if (fieldControl is RadioButton)
                {
                    var radioButton = (RadioButton)fieldControl;
                    radioButton.Checked = DataConverter.ToNullableBool(fieldValue).GetValueOrDefault();
                }
                else if (fieldControl is CheckBox)
                {
                    var checkBox = (CheckBox)fieldControl;

                    var @bool = true;
                    if (Convert.ToString(fieldValue) != "on")
                        @bool = DataConverter.ToNullableBool(fieldValue).GetValueOrDefault();

                    checkBox.Checked = @bool;
                }
                else if (fieldControl is DropDownList)
                {
                    var dropDownList = (DropDownList)fieldControl;
                    if (!dropDownList.TrySetSelectedValue(fieldValue))
                        formData[fieldKey] = null;
                }
                else if (fieldControl is ListBox)
                {
                    var listBox = (ListBox)fieldControl;
                    var listItems = listBox.Items.OfType<ListItem>();

                    var @set = new HashSet<String>(StringComparer.OrdinalIgnoreCase);

                    if (!(fieldValue is String) && fieldValue is IEnumerable)
                    {
                        var list = (IEnumerable)fieldValue;
                        var items = list.Cast<Object>().Select(Convert.ToString);

                        @set.UnionWith(items);
                    }

                    foreach (var listItem in listItems)
                    {
                        var equals = @set.Contains(listItem.Value);
                        listItem.Selected = equals;
                    }
                }
                else if (fieldControl is FileUpload)
                {
                    var binary = fieldValue as FormDataBinary;
                    if (binary != null && binary.FileBytes != null && !String.IsNullOrWhiteSpace(binary.FileName))
                    {
                        var mainPanelID = String.Format(FileDataPanelFormat, fieldID);
                        //var inputPanelID = String.Format(FileInputPanelFormat, fieldID);
                        var outputPanelID = String.Format(FileOuputPanelFormat, fieldID);

                        var parents = UserInterfaceUtil.TraverseParents(fieldControl);
                        var mainPanel = parents.FirstOrDefault(n => n.ID == mainPanelID);

                        var children = UserInterfaceUtil.TraverseChildren(mainPanel);
                        var childrenLp = children.ToLookup(n => n.ID);

                        //var uploadPanel = childrenLp[inputPanelID].Single();
                        var downloadPanel = childrenLp[outputPanelID].Single();

                        //uploadPanel.Visible = false;
                        downloadPanel.Visible = true;
                    }
                }
            }
        }

        protected FormDataUnit LoadFormData(Guid? ownerID, Guid? recordID)
        {
            if (ownerID == null || recordID == null)
                return null;

            var collection = MongoDbUtil.GetCollection(ownerID);

            var document = collection.AsQueryable().FirstOrDefault(n => n[FormDataConstants.IDField] == recordID);
            if (document == null)
                return null;

            return BsonDocumentConverter.ConvertToFormDataUnit(document);
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
            else if (entity is TreeEntity)
            {
                var treeEntity = (TreeEntity)entity;
                if (treeEntity.Visible)
                    SetTree(parent, treeEntity);
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

            var leftClass = $"col-sm-{entity.CaptionSize.GetValueOrDefault(4)} control-label";
            var rightClass = $"col-sm-{entity.ControlSize.GetValueOrDefault(8)}";

            var enabled = Enabled;
            if (enabled && entity.ReadOnly.GetValueOrDefault())
                enabled = false;

            var mainPanel = new Panel
            {
                ID = String.Format(FieldPabelIDFormat, entity.ID),
                CssClass = "form-group",
                Enabled = enabled
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

            var elementsContainer = valuePanel;
            if (entity.Privacy.GetValueOrDefault())
                elementsContainer = InitPrivacyGroup(valuePanel, entity);

            var autoPostBack = (DependentFields != null && DependentFields.Contains(entity.ID));

            var control = CreateControl(elementsContainer, entity, autoPostBack);
            if (control != null)
            {
                MetaControls.Add(control.ID, control);

                if (entity.DependentFieldID != null)
                    mainPanel.Visible = false;

                var mainControl = (Control)mainPanel;
                if (Mode == FormMode.Inspect)
                    mainControl = InitInspectionGroup(mainPanel, entity);

                parent.Controls.Add(mainControl);
            }
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

            var groupSize = $"col-lg-{entity.Size.GetValueOrDefault(12)}";
            var groupBgColor = entity.BgColor;
            var groupTextColor = entity.TextColor;

            if (!String.IsNullOrWhiteSpace(groupBgColor))
                titlePanel.Style["background-color"] = groupBgColor;

            if (!String.IsNullOrWhiteSpace(groupTextColor))
                titlePanel.Style["color"] = groupTextColor;

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

            var rowPanel = new Panel { ID = String.Format(GridPanelIDFormat, entity.ID), CssClass = "col-lg-12" };

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
                TableSectionFooter = true,
                CssClass = "tableIntern table table-striped table-bordered table-hover dataTable"
            };

            dataGrid.RowEditing += (s, e) => { };
            dataGrid.RowDeleting += (s, e) => { };

            SetCommands(dataGrid, entity);

            MetaControls.Add(dataGrid.ID, dataGrid);

            if (entity.Controls != null)
            {
                var dataTable = new DataTable();

                foreach (var defaultField in FormDataBase.DefaultFields)
                {
                    var column = new GridViewMetaBoundField(defaultField)
                    {
                        HeaderText = defaultField,
                        Visible = false
                    };

                    dataTable.Columns.Add(defaultField);
                    dataGrid.Columns.Add(column);
                }

                var fields = entity.Controls.Cast<FieldEntity>();

                var orderedVisibleFields = (from n in fields
                                            where n.Visible && (n.DisplayOnGrid == "Always" || n.DisplayOnGrid == "Conditional")
                                            orderby n.OrderIndex, n.Name
                                            select n);

                foreach (var field in orderedVisibleFields)
                {
                    var column = new GridViewMetaBoundField(UserID, entity.ID, field, _contentEntity);

                    dataTable.Columns.Add(column.DataField);
                    dataGrid.Columns.Add(column);
                }

                var footerFields = (from n in orderedVisibleFields
                                    where !string.IsNullOrWhiteSpace(n.GridFieldSummary)
                                    select n);

                dataGrid.ShowFooter = !footerFields.IsNullOrEmpty();

                dataGrid.DataSource = dataTable;
                dataGrid.DataBind();
            }

            if (RecordID == null)
            {
                var label = new Label
                {
                    ForeColor = Color.Red,
                    Text = "Please save master data to add new record in grid or copy from an other grid"
                };

                toolsPanel.Controls.Add(label);
            }
            else
            {
                var newButton = new LinkButton
                {
                    ID = String.Format(newCommandIDFormat, entity.ID),
                    Enabled = Enabled,
                    Visible = (RecordID != null),
                    CssClass = (Enabled ? "btn btn-primary btn-sm fa fa-plus" : "btn btn-default btn-sm fa fa-plus"),
                    ClientIDMode = ClientIDMode.Static,
                    CommandName = "Edit",
                    CommandArgument = String.Format(childRecordCommandArgFormat, entity.ID, "@", "@")
                };

                newButton.Click += command_OnClick;

                toolsPanel.Controls.Add(newButton);

                if (IsGridCloneExists(entity))
                {
                    var cloneButton = new LinkButton
                    {
                        ID = String.Format(cloneCommandIDFormat, entity.ID),
                        Enabled = Enabled,
                        Visible = (RecordID != null),
                        CssClass = (Enabled ? "btn btn-primary btn-sm fa fa-copy" : "btn btn-default btn-sm fa fa-copy"),
                        ClientIDMode = ClientIDMode.Static,
                        CommandName = "Clone",
                        CommandArgument = String.Format(childRecordCommandArgFormat, entity.ID, "@", "@")
                    };

                    cloneButton.Click += command_OnClick;

                    toolsPanel.Controls.Add(cloneButton);
                }
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
        private void SetTree(Control parent, TreeEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var rowPanel = new Panel { ID = String.Format(TreePanelIDFormat, entity.ID), CssClass = "col-lg-12" };

            var titlePanel = new Panel { CssClass = "ibox-title" };
            var toolsPanel = new Panel { CssClass = "ibox-tools" };
            var contentPanel = new Panel { CssClass = "ibox-content" };

            var treeList = new ASPxTreeList
            {
                ID = String.Format(TreeIDFormat, entity.ID),
                KeyFieldName = "ID",
                ParentFieldName = "ContainerID",
                AutoGenerateColumns = false,
            };

            SetCommands(treeList, entity);

            MetaControls.Add(treeList.ID, treeList);

            if (entity.Controls != null)
            {
                var dataTable = new DataTable();

                foreach (var defaultField in FormDataBase.DefaultFields)
                {
                    var column = new TreeListMetaDataColumn(defaultField)
                    {
                        FieldName = defaultField,
                        Caption = defaultField,
                        Visible = false
                    };

                    dataTable.Columns.Add(defaultField);
                    treeList.Columns.Add(column);
                }

                var fields = entity.Controls.Cast<FieldEntity>();

                var orderedVisibleFields = (from n in fields
                                            where n.Visible && (n.DisplayOnGrid == "Always" || n.DisplayOnGrid == "Conditional")
                                            orderby n.OrderIndex, n.Name
                                            select n);

                foreach (var field in orderedVisibleFields)
                {
                    var column = new TreeListMetaDataColumn(UserID, entity.ID, field, _contentEntity);

                    dataTable.Columns.Add(column.DataField);
                    treeList.Columns.Add(column);
                }

                //var footerFields = (from n in orderedVisibleFields
                //                    where !string.IsNullOrWhiteSpace(n.GridFieldSummary)
                //                    select n);

                //treeList.ShowFooter = !footerFields.IsNullOrEmpty();

                treeList.DataSource = dataTable;
                treeList.DataBind();
            }

            if (RecordID == null)
            {
                var label = new Label
                {
                    ForeColor = Color.Red,
                    Text = "Please save master data to add new record in grid or copy from an other grid"
                };

                toolsPanel.Controls.Add(label);
            }
            else
            {
                var newButton = new LinkButton
                {
                    ID = String.Format(newCommandIDFormat, entity.ID),
                    Enabled = Enabled,
                    Visible = (RecordID != null),
                    CssClass = (Enabled ? "btn btn-primary btn-sm fa fa-plus" : "btn btn-default btn-sm fa fa-plus"),
                    ClientIDMode = ClientIDMode.Static,
                    CommandName = "Edit",
                    CommandArgument = String.Format(childRecordCommandArgFormat, entity.ID, "@", "@")
                };

                newButton.Click += command_OnClick;

                toolsPanel.Controls.Add(newButton);

                //if (IsGridCloneExists(entity))
                //{
                //    var cloneButton = new LinkButton
                //    {
                //        ID = String.Format(cloneCommandIDFormat, entity.ID),
                //        Enabled = Enabled,
                //        Visible = (RecordID != null),
                //        CssClass = (Enabled ? "btn btn-primary btn-sm fa fa-copy" : "btn btn-default btn-sm fa fa-copy"),
                //        ClientIDMode = ClientIDMode.Static,
                //        CommandName = "Clone",
                //        CommandArgument = $"{entity.ID:n}/@"
                //    };

                //    cloneButton.Click += command_OnClick;

                //    toolsPanel.Controls.Add(cloneButton);
                //}
            }

            titlePanel.Controls.Add(toolsPanel);
            contentPanel.Controls.Add(treeList);

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
            activeNameContainer.ID = $"tab_active_{entity.ID:n}";

            var containerHeader = new HtmlGenericControl("ul");
            containerHeader.Attributes["class"] = "nav nav-tabs";

            var containerContent = new Panel { CssClass = "tab-content" };

            containerHeader.Controls.Add(activeNameContainer);
            tabContainer.Controls.Add(containerHeader);
            tabContainer.Controls.Add(containerContent);

            if (entity.Controls != null)
            {
                var tabPages = (from n in entity.Controls
                                orderby n.OrderIndex, n.Name
                                select n);

                foreach (var child in tabPages)
                {
                    var tabPage = child as TabPageEntity;
                    if (tabPage == null)
                    {
                        var genEventArg = new GenericEventArgs<String>($"TabContainer '{entity.Name}' contains not TabPage");
                        OnError(genEventArg);

                        continue;
                    }

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
            headerLink.Attributes["href"] = $"#tab_content_{entity.ID:n}";
            headerLink.Attributes["onclick"] = "onTabClick(this);";

            headerLink.Attributes["data-toggle"] = "tab";
            headerLink.Attributes["aria-expanded"] = "true";

            headerLink.Attributes["tab-id"] = $"{entity.ID:n}";
            headerLink.Attributes["active-store"] = $"tab_active_{parentID:n}";

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
                ItemTemplate = new DefaultFieldTemplate(() => CreateCommands(entity))
            };

            dataGrid.Columns.Add(commandsColumn);
        }
        private void SetCommands(ASPxTreeList treeList, TreeEntity entity)
        {
            var commandsColumn = new TreeListDataColumn
            {
                DataCellTemplate = new DefaultFieldTemplate(() => CreateCommands(entity))
            };

            treeList.Columns.Add(commandsColumn);
        }

        private void ApplyViewMode(FormDataUnit formData)
        {
            SetupActiveTab();

            ProcessDependencyFields(formData);
            ProcessDefaultFieldValues(formData);

            ProcessEnabledStatuses(formData);
        }

        private void ProcessEnabledStatuses(FormDataUnit formData)
        {

            var dataFields = FirstLevelChildren.Values.OfType<FieldEntity>();
            foreach (var entity in dataFields)
            {
                var enabled = Enabled;

                if (enabled)
                    enabled = !entity.ReadOnly.GetValueOrDefault();

                if (enabled)
                {
                    if (formData != null && formData.StatusID != DataStatusCache.None.ID)
                        enabled = !entity.FirstTimeFill;
                }

                if (enabled)
                {

                    if (formData != null && formData.ReviewFields != null && formData.ReviewFields.Count > 0)
                    {
                        if (Mode == FormMode.Review && StatusID == DataStatusCache.Rejected.ID)
                        {
                            var fieldKey = Convert.ToString(entity.ID);

                            if (!formData.ReviewFields.Contains(fieldKey))
                                enabled = false;
                        }
                    }
                }

                if (!enabled)
                {
                    var fieldID = GetFieldID(entity);
                    var elementsID = GetElementID(entity);

                    var control = MetaControls.GetValueOrDefault(fieldID);
                    if (control == null)
                        continue;

                    //MetaControls.Remove(fieldID);

                    var container = new HiddenField
                    {
                        ID = control.ID,
                        Value = GetControlValue(control),
                        ClientIDMode = ClientIDMode.Static,
                        EnableViewState = false,
                    };

                    control.ID = $"{control.ID}_disabled";

                    //MetaControls.Add(control.ID, control);
                    //MetaControls.Add(container.ID, container);

                    control.Parent.Controls.Add(container);


                    var elements = (from n in elementsID
                                    from m in AllControlsLp[n]
                                    select m).OfType<Panel>();

                    foreach (var element in elements)
                        element.Enabled = false;
                }
            }
        }

        private void ProcessDependencyFields(FormDataUnit formData)
        {
            if (DependentFields == null || DependentFields.Count == 0)
                return;

            var currFormData = (formData ?? new FormDataUnit());
            var parentFormData = (ParentFormData ?? new FormDataUnit());

            IDictionary<String, Object>[] sources =
            {
                new Dictionary<String, Object>(currFormData),
                new Dictionary<String, Object>(parentFormData),
            };

            var fields = (IEnumerable<ControlEntity>)Children.Values;
            if (_parentContentEntity != null)
                fields = FormStructureUtil.PreOrderTraversal(_parentContentEntity);

            var expGlobals = new ExpressionGlobalsUtil(UserID, fields, sources);

            var grandParents = (from n in fields
                                where n.DependentFieldID == null &&
                                      DependentFields.Contains(n.ID)
                                select n);

            ProcessDependencyFields(grandParents, expGlobals);
        }

        private void ProcessDependencyFields(IEnumerable<ControlEntity> parents, ExpressionGlobalsUtil expGlobals)
        {
            foreach (var parent in parents)
                ProcessDependencyFields(parent, expGlobals);
        }

        private void ProcessDependencyFields(ControlEntity parent, ExpressionGlobalsUtil expGlobals)
        {
            var sourceField = parent as FieldEntity;
            if (sourceField == null)
                return;

            var sourceKey = Convert.ToString(sourceField.ID);
            var dependents = DependentFields[sourceField.ID];

            foreach (var targetEntity in dependents)
            {
                var dependentExp = targetEntity.DependentExp;
                if (String.IsNullOrWhiteSpace(dependentExp))
                    dependentExp = @"!IsEmpty(@)";

                expGlobals.SetAssociation("@", sourceKey);
                expGlobals.SetAssociation("@val", sourceKey);
                expGlobals.SetAssociation("@value", sourceKey);

                var node = ElementTree.GetValueOrDefault(targetEntity.ID);
                if (node.ParentID != null)
                {
                    var parentControl = Children.GetValueOrDefault(node.ParentID.Value);

                    var parentGrid = parentControl as GridEntity;
                    if (parentGrid != null)
                    {
                        var fieldEntity = targetEntity;
                        if (fieldEntity.DisplayOnGrid == "Conditional")
                        {
                            var dataGridID = String.Format(GridIDFormat, parentGrid.ID);

                            var dataGrid = MetaControls.GetValueOrDefault(dataGridID) as GridView;
                            if (dataGrid != null)
                            {
                                var columns = dataGrid.Columns.OfType<GridViewMetaBoundField>();

                                var column = columns.FirstOrDefault(n => n.FieldEntity != null && n.FieldEntity.ID == targetEntity.ID);
                                if (column != null)
                                    SetTargetVisibilities(column, expGlobals, dependentExp);

                                continue;
                            }
                        }
                    }

                    var parentTree = parentControl as TreeEntity;
                    if (parentTree != null)
                    {
                        var fieldEntity = targetEntity;
                        if (fieldEntity.DisplayOnGrid == "Conditional")
                        {
                            var treeListID = String.Format(TreeIDFormat, parentTree.ID);

                            var treeList = MetaControls.GetValueOrDefault(treeListID) as ASPxTreeList;
                            if (treeList != null)
                            {
                                var columns = treeList.Columns.OfType<TreeListMetaDataColumn>();

                                var column = columns.FirstOrDefault(n => n.FieldEntity != null && n.FieldEntity.ID == targetEntity.ID);
                                if (column != null)
                                    SetTargetVisibilities(column, expGlobals, dependentExp);

                                continue;
                            }
                        }
                    }
                }

                var elementIds = GetElementID(targetEntity);

                var elements = (from n in elementIds
                                from m in AllControlsLp[n]
                                select m).ToList();

                if (elements.Any())
                {
                    SetTargetVisibilities(elements, expGlobals, dependentExp);
                    FillFieldData(targetEntity, sourceField, expGlobals);
                }

                ProcessDependencyFields(targetEntity, expGlobals);
            }
        }

        private void ProcessDefaultFieldValues(FormDataUnit formData)
        {
            if (formData == null)
                return;

            var dataFields = FirstLevelChildren.Values.OfType<FieldEntity>();
            var expGlobals = new ExpressionGlobalsUtil(UserID, _contentEntity, formData);

            foreach (var entity in dataFields)
            {
                if (String.IsNullOrWhiteSpace(entity.FieldValueExpression))
                    continue;

                var expNode = ExpressionParser.GetOrParse(entity.FieldValueExpression);

                Object result;
                if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out result))
                    continue;

                var controlID = GetFieldID(entity);
                var control = MetaControls.GetValueOrDefault(controlID);

                if (control is ITextControl)
                {
                    var textBox = control as ITextControl;
                    textBox.Text = Convert.ToString(result, CultureInfo.InvariantCulture);
                }

                if (control is CheckBox)
                {
                    var checkBox = control as CheckBox;
                    checkBox.Checked = DataConverter.ToNullableBoolean(result).GetValueOrDefault();
                }

                if (control is RadioButton)
                {
                    var radioButton = control as RadioButton;
                    radioButton.Checked = DataConverter.ToNullableBoolean(result).GetValueOrDefault();
                }
            }
        }

        private void SetupActiveTab()
        {
            var activeTabs = GetActiveTabs();
            SetActiveTabs(activeTabs);
        }

        private bool IsDisabled(Control control)
        {
            var parents = UserInterfaceUtil.TraverseParents(control).OfType<WebControl>();
            return parents.Any(n => !n.Enabled);
        }

        private String GetFieldID(FieldEntity entity)
        {
            if (entity.Type == "CheckBox")
                return String.Format(CheckIDFormat, entity.ID);

            if (entity.Type == "RagioButton")
                return String.Format(RadioIDFormat, entity.ID);

            if (entity.Type == "FileIDFormat")
                return String.Format(FileIDFormat, entity.ID);

            return String.Format(FieldIDFormat, entity.ID);
        }

        private String GetControlValue(Control control)
        {
            //DropDown, ListBox, CheckBoxList, RadioButtonList is child of ListControl
            var listControl = control as ListControl;
            if (listControl != null)
            {
                var query = (from ListItem n in listControl.Items
                             where n.Selected
                             select n.Value);

                var values = String.Join(",", query);
                return values;
            }

            //RadioButton is child of CheckBox
            var checkable = control as System.Web.UI.WebControls.CheckBox;
            if (checkable != null)
                return Convert.ToString(checkable.Checked);

            //Label, TextBox is child of ITextControl
            var textBox = control as ITextControl;
            if (textBox != null)
                return textBox.Text;

            throw new Exception("Unable to get value of control because control is unknown");
        }

        private Control CreateCommands(ControlEntity entity)
        {
            var editMode = (Mode == FormMode.Review ? Mode : FormMode.Edit);

            var list = new List<LinkButton>
            {
                new LinkButton
                {
                    ID = String.Format(inspectCommandIDFormat, entity.ID),
                    CssClass = "btn btn-info btn-sm fa fa-search",
                    CommandName = Convert.ToString(FormMode.Inspect),
                    Visible = (Mode == FormMode.Inspect)
                },
                new LinkButton
                {
                    ID = String.Format(viewCommandIDFormat, entity.ID),
                    CssClass = "btn btn-info btn-sm fa fa-search",
                    CommandName = Convert.ToString(FormMode.View)
                },
                new LinkButton
                {
                    ID = String.Format(editCommandIDFormat, entity.ID),
                    Visible = Enabled,
                    CssClass = (Enabled ? "btn btn-primary btn-sm fa fa-edit" : "btn btn-default btn-sm fa fa-edit"),
                    CommandName = Convert.ToString(editMode)
                },
                new LinkButton
                {
                    ID = String.Format(deleteCommandIDFormat, entity.ID),
                    Visible = Enabled,
                    CssClass = (Enabled ? "btn btn-danger btn-sm fa fa-trash-o" : "btn btn-default btn-sm fa fa-trash-o"),
                    CommandName = "Delete",
                    OnClientClick = @"return confirm('დარწმუნებული ხართ?/Are you sure?')"
                },
            };

            if (entity is TreeEntity)
            {
                var newButton = new LinkButton
                {
                    ID = String.Format(newCommandIDFormat, entity.ID),
                    CssClass = "btn btn-primary btn-sm fa fa-plus",
                    CommandName = "New",
                    Visible = (Mode == FormMode.Edit)
                };

                list.Insert(0, newButton);
            }

            var panel = new Panel();

            foreach (var button in list)
            {
                button.Command += command_OnCommand;
                button.DataBinding += command_OnDataBinding;

                panel.Controls.Add(button);
                panel.Controls.Add(new LiteralControl("&nbsp;"));
            }

            return panel;
        }

        private FormDataListBase GetFormDataList(Object value)
        {
            if (value is FormDataListRef)
                return new FormDataLazyList((FormDataListRef)value);

            return (FormDataListBase)value;
        }

        private ISet<String> GetContainerFields(Control control)
        {
            var dataGrid = control as GridView;
            if (dataGrid != null)
            {
                var columns = dataGrid.Columns.OfType<GridViewMetaBoundField>();

                var dataFieldSet = columns.Select(n => n.DataField).ToHashSet();
                return dataFieldSet;

            }
            var treeList = control as ASPxTreeList;
            if (treeList != null)
            {
                var columns = treeList.Columns.OfType<TreeListMetaDataColumn>();

                var dataFieldSet = columns.Select(n => n.DataField).ToHashSet();
                return dataFieldSet;
            }

            return null;
        }

        private bool IsGridCloneExists(GridEntity primaryGrid)
        {
            var primaryGridFields = primaryGrid.Controls.Select(n => n.Name).ToHashSet();

            foreach (var otherGrid in DataGrids.Values)
            {
                if (otherGrid.ID == primaryGrid.ID)
                    continue;

                var otherGridFields = otherGrid.Controls.Select(n => n.Name).ToHashSet();
                if (primaryGridFields.SetEquals(otherGridFields))
                    return true;
            }

            return false;
        }

        private bool IsTreeCloneExists(TreeEntity primaryTree)
        {
            var primaryTreeFields = primaryTree.Controls.Select(n => n.Name).ToHashSet();

            foreach (var otherTree in TreeLists.Values)
            {
                if (otherTree.ID == primaryTree.ID)
                    continue;

                var otherTreeFields = otherTree.Controls.Select(n => n.Name).ToHashSet();
                if (primaryTreeFields.SetEquals(otherTreeFields))
                    return true;
            }

            return false;
        }

        private IEnumerable<HtmlFormValue> GetAllFieldValues()
        {
            var dict = new Dictionary<String, ISet<Guid?>>();

            foreach (var item in GetRequestValues(dict))
                yield return item;

            foreach (var item in GetRequestFiles(dict))
                yield return item;

            foreach (var item in GetOutOfRequestValues(dict))
                yield return item;

            foreach (var item in GetSystemFieldValues())
                yield return item;
        }

        private IEnumerable<HtmlFormValue> GetSystemFieldValues()
        {
            var defaultFields = new HashSet<String>
            {
                FormDataConstants.DescriptionField,
                FormDataConstants.UserStatusesFields,
                FormDataConstants.DateCreatedField,
                FormDataConstants.DateChangedField,
                FormDataConstants.DateDeletedField,
                FormDataConstants.DateOfAcceptField,
                FormDataConstants.DateOfSubmitField,
                FormDataConstants.StatusChangeDateField
            };

            if (Mode != FormMode.Inspect)
                defaultFields.Add(FormDataConstants.ReviewFields);

            foreach (var field in defaultFields)
            {
                var entity = new HtmlFormValue
                {
                    Type = "sys",
                    Key = field,
                    Value = FormNoData.Value,
                };

                yield return entity;
            }
        }

        private IEnumerable<HtmlFormValue> GetRequestFiles(IDictionary<String, ISet<Guid?>> dict)
        {
            foreach (var key in Request.Files.AllKeys)
            {
                if (!RegexUtil.FormValueParserRx.IsMatch(key))
                    continue;

                var match = RegexUtil.FormValueParserRx.Match(key);

                var type = match.Groups["type"].Value;
                var elemID = match.Groups["elemID"].Value;

                var fieldVal = Request.Files[key];

                var @set = dict.GetValueOrDefault(type);
                if (@set == null)
                {
                    @set = new HashSet<Guid?>();
                    dict.Add(type, @set);
                }

                var fieldID = DataConverter.ToNullableGuid(elemID);
                if (fieldID == null || !@set.Add(fieldID))
                    continue;

                var entity = new HtmlFormValue
                {
                    Type = type,
                    Key = fieldID,
                    Value = fieldVal,
                };

                yield return entity;
            }
        }

        private IEnumerable<HtmlFormValue> GetRequestValues(IDictionary<String, ISet<Guid?>> dict)
        {
            foreach (var key in Request.Form.AllKeys)
            {
                var fieldKey = key;
                var fieldVal = (Request.Form[key] ?? String.Empty);

                if (fieldVal.StartsWith("rb_") && RegexUtil.FormValueParserRx.IsMatch(fieldVal))
                {
                    fieldKey = fieldVal;
                    fieldVal = "on";
                }

                if (!RegexUtil.FormValueParserRx.IsMatch(fieldKey))
                    continue;

                var match = RegexUtil.FormValueParserRx.Match(fieldKey);

                var type = match.Groups["type"].Value;
                var elemID = match.Groups["elemID"].Value;

                var @set = dict.GetValueOrDefault(type);
                if (@set == null)
                {
                    @set = new HashSet<Guid?>();
                    dict.Add(type, @set);
                }

                var fieldID = DataConverter.ToNullableGuid(elemID);
                if (fieldID == null || !@set.Add(fieldID))
                    continue;

                var entity = new HtmlFormValue
                {
                    Type = type,
                    Key = fieldID,
                    Value = fieldVal,
                };

                yield return entity;
            }
        }

        private IEnumerable<HtmlFormValue> GetOutOfRequestValues(IDictionary<String, ISet<Guid?>> dict)
        {
            foreach (var pair in MetaControls)
            {
                if (!RegexUtil.FormValueParserRx.IsMatch(pair.Key))
                    continue;

                var match = RegexUtil.FormValueParserRx.Match(pair.Key);

                var type = match.Groups["type"].Value;
                var elemID = match.Groups["elemID"].Value;

                var @set = dict.GetValueOrDefault(type);
                if (@set == null)
                {
                    @set = new HashSet<Guid?>();
                    dict.Add(type, @set);
                }

                var fieldID = DataConverter.ToNullableGuid(elemID);
                if (fieldID == null || !@set.Add(fieldID))
                    continue;

                var value = (Object)FormNoData.Value;

                var checkable = pair.Value as System.Web.UI.WebControls.CheckBox;
                if (checkable != null && !IsDisabled(checkable))
                    value = false;

                var entity = new HtmlFormValue
                {
                    Type = type,
                    Key = fieldID,
                    Value = value,
                };

                yield return entity;
            }
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

        private Control InitInspectionGroup(Panel panel, FieldEntity entity)
        {
            var checkBox = new CheckBox { ID = String.Format(InspIDFormat, entity.ID) };

            var fieldSet = new HtmlGenericControl("FieldSet");

            var legend = new HtmlGenericControl("Legend");
            legend.Controls.Add(checkBox);

            fieldSet.Controls.Add(legend);
            fieldSet.Controls.Add(legend);

            fieldSet.Controls.Add(panel);

            MetaControls.Add(checkBox.ID, checkBox);

            return fieldSet;
        }

        private DictionaryDataBinder CreateDataBinder(FieldEntity entity)
        {
            var textExp = entity.TextExpression;
            var valueExp = entity.ValueExpression;

            if (String.IsNullOrWhiteSpace(textExp) || String.IsNullOrWhiteSpace(valueExp))
                return null;

            var userID = (UserID ?? UserUtil.GetCurrentUserID());
            var formData = (FormData ?? _lastFormData);

            var dataSourceHelper = new DataSourceHelper(userID, entity, formData, Children);
            var dataRecords = dataSourceHelper.LoadDataRecords();

            var dictionaryBinder = new DictionaryDataBinder(dataRecords, textExp, valueExp);
            return dictionaryBinder;
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
                    EnableViewState = false,
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
                    EnableViewState = false,
                };

                if (!String.IsNullOrWhiteSpace(entity.Mask))
                    control.Attributes["data-mask"] = entity.Mask;

                parent.Controls.Add(control);
                return control;
            }

            if (entity.Type == "Lookup")
            {
                var groupPanel = new Panel
                {
                    CssClass = "input-group"
                };

                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control",
                    EnableViewState = false,
                };

                if (!String.IsNullOrWhiteSpace(entity.Mask))
                    control.Attributes["data-mask"] = entity.Mask;

                groupPanel.Controls.Add(control);

                var buttonPanel = new Panel
                {
                    CssClass = "input-group-btn"
                };

                var button = new ImageLinkButton
                {
                    ID = String.Format(ButtonIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "btn btn-primary",
                    Text = "OK"
                };

                buttonPanel.Controls.Add(button);

                groupPanel.Controls.Add(buttonPanel);
                parent.Controls.Add(groupPanel);

                return control;
            }

            if (entity.Type == "Label")
            {
                var control = new Label
                {
                    ID = String.Format(FieldIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "form-control-static",
                    Text = entity.Description,
                    EnableViewState = false,
                };

                parent.Controls.Add(control);
                return control;
            }

            if (entity.Type == "Number")
            {
                var control = new TextBox
                {
                    ID = String.Format(FieldIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "decSpinEdit",
                    AutoPostBack = autoPostBack,
                    EnableViewState = false,
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
                    EnableViewState = false,
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
                    EnableViewState = false,
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
                    EnableViewState = false,
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
                    EnableViewState = false,
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
                    EnableViewState = false,
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

                parent.Controls.Add(control);
                return control;
            }

            if (entity.Type == "CheckBoxList")
            {
                var control = new ListBox
                {
                    ID = String.Format(CheckListIDFormat, entity.ID),
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "chosen-select form-control selectWidth",
                    SelectionMode = ListSelectionMode.Multiple,
                    AutoPostBack = autoPostBack,
                    EnableViewState = false,
                };

                if (!String.IsNullOrWhiteSpace(entity.DataSourceID))
                {
                    var dataBinder = CreateDataBinder(entity);
                    if (dataBinder != null)
                    {
                        control.Items.Clear();

                        var collection = dataBinder.GetItems();

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
                var fileOutputPanel = new Panel
                {
                    ID = String.Format(FileOuputPanelFormat, entity.ID),
                    EnableViewState = false,
                    Visible = false,
                };

                var downloadUrl = new UrlHelper("~/Handlers/Download.ashx")
                {
                    [FormDataConstants.OwnerIDField] = OwnerID,
                    [FormDataConstants.IDField] = RecordID,
                    ["FieldID"] = entity.ID
                };

                var donwloadLink = new HyperLink();
                donwloadLink.NavigateUrl = downloadUrl.ToString();
                donwloadLink.Attributes["target"] = "_blank";
                donwloadLink.Controls.Add(new Label { Text = "Download" });

                fileOutputPanel.Controls.Add(donwloadLink);

                var fileInputPanel = new Panel { CssClass = "fileinput fileinput-new input-group" };
                fileInputPanel.ID = String.Format(FileInputPanelFormat, entity.ID);
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
                fileDataPanel.ID = String.Format(FileDataPanelFormat, entity.ID);
                fileDataPanel.Controls.Add(fileOutputPanel);
                fileDataPanel.Controls.Add(fileInputPanel);

                parent.Controls.Add(fileDataPanel);
                return fileUpload;
            }

            var genEventArgs = new GenericEventArgs<String>($"Unknwn element type '{entity.Type}' of field '{entity.Name}'");
            OnError(genEventArgs);

            return null;
        }

        private bool FillFieldData(FieldEntity sourceField, ExpressionGlobalsUtil expGlobals)
        {
            var fiedlID = GetFieldID(sourceField);
            var control = MetaControls[fiedlID];

            var expNode = ExpressionParser.GetOrParse(sourceField.DependentFillExp);

            Object result;
            if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out result))
                return false;

            var textBox = control as TextBox;
            if (textBox != null)
                textBox.Text = Convert.ToString(result);

            var checkBox = control as CheckBox;
            if (checkBox != null)
                checkBox.Checked = DataConverter.ToNullableBool(result).GetValueOrDefault();

            var redioButton = control as RadioButton;
            if (redioButton != null)
                redioButton.Checked = DataConverter.ToNullableBool(result).GetValueOrDefault();

            var listControl = control as ListControl;
            if (listControl != null)
            {
                if (!listControl.TrySetSelectedValue(result))
                    return false;
            }

            return true;
        }

        private bool FillFieldData(ControlEntity targetField, FieldEntity sourceField, ExpressionGlobalsUtil expGlobals)
        {
            var sourceValue = expGlobals["@value"];
            if (sourceValue == null)
                return false;

            var fieldEntity = targetField as FieldEntity;
            if (fieldEntity == null || String.IsNullOrWhiteSpace(fieldEntity.DependentFillExp))
                return false;

            if ((sourceField.Type == "ComboBox" || sourceField.Type == "Lookup") &&
                !String.IsNullOrWhiteSpace(sourceField.DataSourceID) &&
                !String.IsNullOrWhiteSpace(sourceField.ValueExpression))
            {
                var userID = (Guid?)null;
                var formData = (FormData ?? _lastFormData);

                if (formData != null)
                    userID = formData.UserID;

                if (!UserUtil.IsSuperAdmin())
                    userID = UserUtil.GetCurrentUserID();

                var result = false;

                var dataSourceHelper = new DataSourceHelper(userID, sourceField);

                var dataRecord = dataSourceHelper.FindDataRecord(sourceValue);
                if (dataRecord != null)
                {
                    expGlobals.AddSource(dataRecord);

                    result = FillFieldData(fieldEntity, expGlobals);

                    expGlobals.RemoveSource(dataRecord);
                }

                return result;
            }

            return FillFieldData(fieldEntity, expGlobals);
        }

        private void SetTargetVisibilities(IEnumerable<Control> elements, ExpressionGlobalsUtil expGlobals, String expression)
        {
            var expNode = ExpressionParser.GetOrParse(expression);

            Object result;
            if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out result))
                return;

            var visible = DataConverter.ToNullableBoolean(result);

            foreach (var element in elements)
                element.Visible = visible.GetValueOrDefault();
        }

        private void SetTargetVisibilities(TreeListMetaDataColumn treeColumn, ExpressionGlobalsUtil expGlobals, String expression)
        {
            var expNode = ExpressionParser.GetOrParse(expression);

            Object result;
            if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out result))
                return;

            var visible = DataConverter.ToNullableBoolean(result);
            treeColumn.Visible = visible.GetValueOrDefault();
        }

        private void SetTargetVisibilities(GridViewMetaBoundField boundField, ExpressionGlobalsUtil expGlobals, String expression)
        {
            var expNode = ExpressionParser.GetOrParse(expression);

            Object result;
            if (!ExpressionEvaluator.TryEval(expNode, expGlobals.Eval, out result))
                return;

            var visible = DataConverter.ToNullableBoolean(result);
            boundField.Visible = visible.GetValueOrDefault();
        }
    }
}