using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class TreeGrid : System.Web.UI.WebControls.GridView, IPermissionDependent
    {
        private const String _scriptKey = "treeGrid";

        private const String _parentNodeFormat = "treegrid-{0}";
        private const String _childNodeFormat = "treegrid-{0} treegrid-parent-{1}";

        private const String _scriptText = @"<script type='text/javascript'>
                                                 $(document).ready(function() {
                                                      $('.tree').treegrid({'initialState':'collapsed'});
                                                 });
                                             </script>";

        private Regex _regex;
        private Object _dataSource;

        public TreeGrid()
        {
            _regex = new Regex(@"\W", RegexOptions.Compiled);
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String PrimaryKeyField { get; set; }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String ParentKeyField { get; set; }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String PermissionKey { get; set; }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool DisableIfNoAccess { get; set; }

        [Bindable(true)]
        [Themeable(false)]
        [Category("Data")]
        [DefaultValue(null)]
        public override Object DataSource
        {
            get
            {
                var stack = new StackTrace();

                var frame = stack.GetFrame(1);
                var method = frame.GetMethod();

                if (method.DeclaringType == typeof(DataBoundControl))
                    return CreateTreeOrderedList().ToList();

                return _dataSource;
            }
            set
            {
                _dataSource = value;
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(CssClass))
                CssClass = "tree";
            else
                CssClass += " tree";

            base.OnDataBinding(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), _scriptKey, _scriptText);

            base.OnPreRender(e);
        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);

            var gridRow = e.Row;
            if (gridRow.RowType != DataControlRowType.DataRow)
                return;

            var dataItem = gridRow.DataItem;

            var primaryKey = Convert.ToString(DataBinder.Eval(dataItem, PrimaryKeyField));
            var parentKey = Convert.ToString(DataBinder.Eval(dataItem, ParentKeyField));

            primaryKey = _regex.Replace(primaryKey, String.Empty);
            parentKey = _regex.Replace(parentKey, String.Empty);

            var classFormat = (String.IsNullOrWhiteSpace(parentKey) ? _parentNodeFormat : _childNodeFormat);

            var cssClass = String.Format(classFormat, primaryKey, parentKey);
            if (String.IsNullOrWhiteSpace(gridRow.CssClass))
                gridRow.CssClass = cssClass;
            else
                gridRow.CssClass += cssClass;
        }

        public bool HasAccess()
        {
            return PermissionUtil.HasAccess(this);
        }

        private IEnumerable<Object> CreateTreeOrderedList()
        {
            var collection = DataSource as IEnumerable;
            if (collection == null)
                yield break;

            var list = (from n in collection.Cast<Object>()
                        let primaryKey = DataBinder.Eval(n, PrimaryKeyField)
                        let parentKey = DataBinder.Eval(n, ParentKeyField)
                        select new TreeNodeItem
                        {
                            PrimaryKey = primaryKey,
                            ParentKey = parentKey,
                            DataItem = n
                        }).ToList();

            var treeLp = list.ToLookup(n => n.ParentKey);

            foreach (var item in list)
            {
                if (item.ParentKey != null)
                    continue;

                var children = CreateTreeOrderedList(item, treeLp);
                foreach (var child in children)
                    yield return child;
            }
        }

        private IEnumerable<Object> CreateTreeOrderedList(TreeNodeItem parent, ILookup<Object, TreeNodeItem> treeLp)
        {
            if (parent == null || parent.DataItem == null)
                yield break;

            yield return parent.DataItem;

            var children = treeLp[parent.PrimaryKey];
            foreach (var child in children)
            {
                foreach (var subChild in CreateTreeOrderedList(child, treeLp))
                    yield return subChild;
            }
        }

        private class TreeNodeItem
        {
            public Object PrimaryKey { get; set; }

            public Object ParentKey { get; set; }

            public Object DataItem { get; set; }
        }
    }
}