using System.Web.UI;
using CITI.EVO.Tools.Web.UI.Helpers;
using DevExpress.Web.ASPxTreeList.Internal;

namespace Gms.Portal.Web.Helpers
{
    public static class DataBoundHelper
    {
        public static DictionaryItemDescriptor GetDescriptor(Control control)
        {
            var dataItemContainer = control.DataItemContainer as IDataItemContainer;
            if (dataItemContainer == null)
                return null;

            var descriptor = dataItemContainer.DataItem as DictionaryItemDescriptor;
            if (descriptor != null)
                return descriptor;

            var templateData = dataItemContainer.DataItem as TreeListTemplateDataItem;
            if (templateData == null)
                return null;

            var listRow = templateData.Row;
            if (listRow == null)
                return null;

            var boundNodeData = listRow.DataItem as TreeListBoundNodeDataItem;
            if (boundNodeData == null)
                return null;

            descriptor = boundNodeData.GetDataObject() as DictionaryItemDescriptor;
            return descriptor;
        }
    }
}