using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Controls;
using CITI.EVO.Tools.Web.UI.Helpers;
using DevExpress.Web.ASPxTreeList;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Utils;
using HyperLink = CITI.EVO.Tools.Web.UI.Controls.HyperLink;

namespace Gms.Portal.Web.Helpers
{
    public class TreeListMetaDataColumn : TreeListDataColumn
    {
        private readonly IDictionary<Guid, FieldDataSourceUtil> _fieldDataSourceUtils;

        public TreeListMetaDataColumn(String dataField)
        {
            Caption = dataField;
            DataField = dataField;
        }
        public TreeListMetaDataColumn(Guid? userID, Guid? ownerID)
        {
            UserID = userID;
            OwnerID = ownerID;
        }
        public TreeListMetaDataColumn(Guid? userID, Guid? ownerID, FieldEntity fieldEntity, ContentEntity contentEntity)
        {
            UserID = userID;
            OwnerID = ownerID;
            FieldEntity = fieldEntity;
            ContentEntity = contentEntity;
            Caption = fieldEntity.Name;
            DataField = Convert.ToString(fieldEntity.ID);

            HeaderCaptionTemplate = new DefaultFieldTemplate(GetHeaderControl);
            DataCellTemplate = new DefaultFieldTemplate(GetItemControl);

            if (FieldEntity != null && !String.IsNullOrWhiteSpace(fieldEntity.GridFieldSummary))
                FooterCellTemplate = new DefaultFieldTemplate(GetFooterControl);

            _fieldDataSourceUtils = new Dictionary<Guid, FieldDataSourceUtil>();
        }

        public Guid? UserID { get; private set; }

        public Guid? OwnerID { get; private set; }

        public String DataField { get; set; }

        public FieldEntity FieldEntity { get; private set; }

        public ContentEntity ContentEntity { get; private set; }

        protected Control GetHeaderControl()
        {
            var label = new CITI.EVO.Tools.Web.UI.Controls.Label { Text = Caption };
            return label;
        }

        protected Control GetFooterControl()
        {
            var label = new System.Web.UI.WebControls.Label();
            label.DataBinding += footerLabel_DataBinding;

            return label;
        }

        protected Control GetItemControl()
        {
            if (FieldEntity != null && FieldEntity.Type == "FileUpload")
            {
                var hyperLink = new HyperLink();
                hyperLink.DataBinding += hyperLink_DataBinding;

                return hyperLink;
            }

            var label = new System.Web.UI.WebControls.Label();
            label.DataBinding += label_DataBinding;

            return label;
        }

        private void label_DataBinding(object sender, EventArgs e)
        {
            var label = sender as System.Web.UI.WebControls.Label;
            if (label == null)
                return;

            var descriptor = DataBoundHelper.GetDescriptor(label);
            if (descriptor == null)
                return;

            var value = GetLabelText(descriptor);

            var toolTip = Convert.ToString(value);
            var text = toolTip;

            if (toolTip.Length > 25)
                text = $"{toolTip.TrimLen(25)}...";

            label.Text = text;
            label.ToolTip = toolTip;
        }

        private void hyperLink_DataBinding(object sender, EventArgs e)
        {
            if (FieldEntity == null)
                return;

            var hyperLink = sender as HyperLink;
            if (hyperLink == null)
                return;

            hyperLink.Visible = false;

            var descriptor = DataBoundHelper.GetDescriptor(hyperLink);
            if (descriptor == null)
                return;

            var binary = descriptor.GetValue(DataField) as FormDataBinary;
            if (binary == null || binary.FileBytes == null || binary.FileBytes.Length == 0 || String.IsNullOrWhiteSpace(binary.FileName))
                return;

            var recordID = descriptor.GetValue(FormDataConstants.IDField);
            if (recordID == null)
                return;

            var downloadUrl = new UrlHelper("~/Handlers/Download.ashx")
            {
                [FormDataConstants.OwnerIDField] = OwnerID,
                [FormDataConstants.IDField] = recordID,
                ["FieldID"] = FieldEntity.ID
            };

            hyperLink.Text = binary.ToString();
            hyperLink.Target = "_blank";
            hyperLink.NavigateUrl = downloadUrl.ToEncodedUrl();

            hyperLink.Visible = true;
        }

        private void footerLabel_DataBinding(object sender, EventArgs e)
        {
            if (FieldEntity == null)
                return;

            var label = sender as System.Web.UI.WebControls.Label;
            if (label == null)
                return;

            var parents = UserInterfaceUtil.TraverseParents(label).OfType<DevExpress.Web.ASPxTreeList.ASPxTreeList>();

            var parent = parents.FirstOrDefault();
            if (parent == null)
                return;

            var source = parent.DataSource as DictionaryDataView;
            var value = GetSummaryValue(source);

            label.Text = $"{FieldEntity.GridFieldSummary}: {value:0.00}";
        }

        private Object GetLabelText(DictionaryItemDescriptor descriptor)
        {
            var value = descriptor.GetValue(DataField);
            if (FieldEntity == null)
                return value;

            if (FieldEntity.Type != "ComboBox" && FieldEntity.Type != "CheckBoxList")
                return value;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.UserIDField));

            var fieldDataSourceUtil = GetFieldDataSourceUtil(userID);

            var text = fieldDataSourceUtil.GetFieldText(value);
            return text;
        }

        private FieldDataSourceUtil GetFieldDataSourceUtil(Guid? userID)
        {
            var fieldDataSourceUtil = _fieldDataSourceUtils.GetValueOrDefault(userID.GetValueOrDefault());
            if (fieldDataSourceUtil == null)
            {
                fieldDataSourceUtil = new FieldDataSourceUtil(userID, ContentEntity, FieldEntity);
                _fieldDataSourceUtils.Add(userID.GetValueOrDefault(), fieldDataSourceUtil);
            }

            return fieldDataSourceUtil;
        }

        private decimal GetSummaryValue(DictionaryDataView source)
        {
            if (FieldEntity == null)
                return 0M;

            var values = (from n in source
                          let val = n.GetValue(DataField)
                          let dec = DataConverter.ToNullableDecimal(val)
                          select dec.GetValueOrDefault());

            switch (FieldEntity.GridFieldSummary)
            {
                case "Sum":
                    return values.Sum();
                case "Avg":
                    return values.Sum();
                case "Max":
                    return values.Sum();
                case "Min":
                    return values.Sum();
            }

            return 0M;
        }
    }
}