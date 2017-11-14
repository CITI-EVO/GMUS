using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Web.UI.Controls;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using DevExpress.Web.ASPxTreeList;
using Gms.Portal.Web.Entities.Monitoring;
using Gms.Portal.Web.Entities.Others;
using ASPxTreeList = CITI.EVO.Tools.Web.UI.Controls.ASPxTreeList;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class MonitoringProjectsGridsControl : BaseUserControl
    {
        private const String TreeIDFormat = "tree_{0:n}";
        private const String GridIDFormat = "grid_{0:n}";

        private const String GridPanelIDFormat = "gridPnl_{0:n}";
        private const String TreePanelIDFormat = "treePnl_{0:n}";

        private ContentEntity _contentEntity;

        private ISet<Guid?> _projectTasks;

        public Guid? UserID
        {
            get { return DataConverter.ToNullableGuid(hdUserID.Value); }
            set { hdUserID.Value = Convert.ToString(value); }
        }

        private IDictionary<Guid, Control> _formControls;
        public IDictionary<Guid, Control> FormControls
        {
            get
            {
                _formControls = (_formControls ?? new Dictionary<Guid, Control>());
                return _formControls;
            }
        }

        private IDictionary<Guid, ControlEntity> _entities;
        public IDictionary<Guid, ControlEntity> Entities
        {
            get
            {
                _entities = (_entities ?? new Dictionary<Guid, ControlEntity>());
                return _entities;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindData(FormDataUnit formData, IEnumerable<ProjectTaskEntity> entities)
        {
            var query = (from n in entities
                         where n.DateDeleted == null &&
                               n.TaskID != null &&
                               n.Status == MonitoringItemStatuses.Accepted
                         select n.TaskID);

            _projectTasks = query.ToHashSet();

            foreach (var pair in Entities)
            {
                var controlKey = Convert.ToString(pair.Key);

                var fieldValue = formData[controlKey];
                if (fieldValue == null)
                    continue;

                if (fieldValue is FormDataListBase || fieldValue is FormDataListRef)
                {
                    var control = FormControls.GetValueOrDefault(pair.Key);
                    if (control == null)
                        continue;

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
            }
        }

        public void InitStructure(ControlEntity entity)
        {
            _contentEntity = entity as ContentEntity;

            if (_contentEntity == null)
                return;

            var comparer = StringLogicalComparer.OrdinalIgnoreCase;
            var controls = FormStructureUtil.PreOrderIndexedTraversal(_contentEntity);

            var projectTasksEntity = (from n in controls
                                      where (n is GridEntity || n is TreeEntity) &&
                                            comparer.Equals(n.Alias, "ProjectTasks")
                                      select n).SingleOrDefault();

            var errors = new List<String>();

            if (projectTasksEntity == null)
                errors.Add(@"Unable to find 'ProjectTasks'");
            else
                SetTreeOrGrid(projectTasksEntity);

            if (errors.Count > 0)
                lblError.Text = String.Join("<br/>", errors);
        }

        private void SetTreeOrGrid(ControlEntity entity)
        {
            var gridEntity = entity as GridEntity;
            if (gridEntity != null)
            {
                SetGrid(pnlMain, gridEntity);
                return;
            }

            var treeEntity = entity as TreeEntity;
            if (treeEntity != null)
            {
                SetTree(pnlMain, treeEntity);
                return;
            }
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

            tablePanel.Controls.Add(dataGrid);

            titlePanel.Controls.Add(toolsPanel);
            contentPanel.Controls.Add(tablePanel);

            rowPanel.Controls.Add(titlePanel);
            rowPanel.Controls.Add(contentPanel);

            if (entity.DependentFieldID != null)
                rowPanel.Visible = false;

            parent.Controls.Add(rowPanel);

            Entities.Add(entity.ID, entity);
            FormControls.Add(entity.ID, dataGrid);
        }

        private void SetTree(Control parent, TreeEntity entity)
        {
            if (entity == null || !entity.Visible)
                return;

            var rowPanel = new Panel { ID = String.Format(TreePanelIDFormat, entity.ID), CssClass = "col-lg-12" };

            var titlePanel = new Panel { CssClass = "ibox-title" };
            var toolsPanel = new Panel { CssClass = "ibox-tools" };
            var contentPanel = new Panel { CssClass = "ibox-content" };

            var treeList = new ASPxTreeList();
            treeList.ID = String.Format(TreeIDFormat, entity.ID);
            treeList.KeyFieldName = "ID";
            treeList.ParentFieldName = "ContainerID";
            treeList.AutoGenerateColumns = false;
            treeList.HtmlRowPrepared += treeList_HtmlRowPrepared;

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

                treeList.DataSource = dataTable;
                treeList.DataBind();
            }

            titlePanel.Controls.Add(toolsPanel);
            contentPanel.Controls.Add(treeList);

            rowPanel.Controls.Add(titlePanel);
            rowPanel.Controls.Add(contentPanel);

            if (entity.DependentFieldID != null)
                rowPanel.Visible = false;

            parent.Controls.Add(rowPanel);

            Entities.Add(entity.ID, entity);
            FormControls.Add(entity.ID, treeList);
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

        private void treeList_HtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
        {
            var taskID = DataConverter.ToNullableGuid(e.GetValue("ID"));
            if (_projectTasks.Contains(taskID.GetValueOrDefault()))
                e.Row.BackColor = Color.FromArgb(0, 210, 255, 191);
        }
    }
}