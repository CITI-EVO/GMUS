using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Utils;
using DevExpress.Web;

namespace CITI.EVO.Tools.Extensions
{
    public static class UIExtensions
	{
		public static void InsertEmptyItem<TItem>(this IList<TItem> list, Action<TItem> action)
		{
			if (list == null)
			{
				return;
			}

			var instance = Activator.CreateInstance<TItem>();
			action(instance);

			list.Insert(0, instance);
		}

		public static Object GetFormViewState(StateBag viewState)
		{
			var stackTrace = new StackTrace();

			var frames = stackTrace.GetFrames();
			if (frames == null)
			{
				throw new Exception();
			}

			var frame = frames[1];
			var method = frame.GetMethod();

			if (!method.Name.StartsWith("get_"))
			{
				throw new Exception();
			}

			return null;
		}

		public static bool TrySetSelectedValue(this ListControl list, Object value)
		{
			if (list == null)
				return false;

			var flag = false;

			foreach (ListItem listItem in list.Items)
			{
				var equals = (listItem.Value == Convert.ToString(value));
				listItem.Selected = equals;

				if (!flag)
				{
					flag = equals;
				}
			}

			return flag;
		}
		public static bool TrySetSelectedValue(this ASPxComboBox list, Object value)
		{
			if (list == null || list.Items == null)
			{
				return false;
			}

			if (value == null)
			{
				list.SelectedItem = null;
				return true;
			}

			if (list.ValueType != null)
			{
				var type = value.GetType();
				if (list.ValueType != type)
				{
					var message = String.Format("ValueType ({0}) of ASPxComboBox ({1}) and type of setting value ({2}) to set is not same", list.ID, list.ValueType, type);
					throw new Exception(message);
				}
			}

			list.SelectedItem = list.Items.FindByValue(value);
			return true;
		}


		public static String TryGetStringValue(this ASPxComboBox list)
		{
			var selectedItem = list.SelectedItem;
			if (selectedItem == null)
			{
				return null;
			}

			var value = DataConverter.ToString(selectedItem.Value);
			return value;
		}

		public static String TryGetStringValue(this ListControl list)
		{
			var selectedItem = list.SelectedItem;
			if (selectedItem == null)
			{
				return null;
			}

			var value = DataConverter.ToString(selectedItem.Value);
			return value;
		}

		public static Guid? TryGetGuidValue(this ListControl list, bool emptyGuidAsNull = true)
		{
			var selectedItem = list.SelectedItem;
			if (selectedItem == null)
			{
				return null;
			}

			var value = DataConverter.ToNullableGuid(selectedItem.Value);
			if (value == Guid.Empty && emptyGuidAsNull)
			{
				value = null;
			}

			return value;
		}

		public static Guid? TryGetGuidValue(this ASPxComboBox list, bool emptyGuidAsNull = true)
		{
			var selectedItem = list.SelectedItem;
			if (selectedItem == null)
			{
				return null;
			}

			var value = DataConverter.ToNullableGuid(selectedItem.Value);
			if (value == Guid.Empty && emptyGuidAsNull)
			{
				value = null;
			}

			return value;
		}

		public static int? TryGetIntValue(this ASPxComboBox list)
		{
			var selectedItem = list.SelectedItem;
			if (selectedItem == null)
			{
				return null;
			}

			var value = DataConverter.ToNullableInt(selectedItem.Value);

			return value;
		}

		public static int? TryGetIntValue(this ITextControl textControl)
		{
			if (textControl == null)
			{
				return null;
			}

			return DataConverter.ToNullableInt(textControl.Text);
		}

		public static DateTime? TryGetDateTimeValue(this ITextControl textControl)
		{
			if (textControl == null)
			{
				return null;
			}

			return DataConverter.ToNullableDateTime(textControl.Text);
		}

		public static double? TryGetDoubleValue(this ITextControl textControl)
		{
			if (textControl == null)
			{
				return null;
			}

			return DataConverter.ToNullableDouble(textControl.Text);
		}

		public static decimal? TryGetDecimalValue(this ITextControl textControl)
		{
			if (textControl == null)
			{
				return null;
			}

			return DataConverter.ToNullableDecimal(textControl.Text);
		}

        public static void BindData(this ListControl control, IEnumerable source)
        {
            var selValue = control.TryGetStringValue();

            control.Items.Clear();

            control.DataSource = source;
            control.DataBind();

            control.Items.Insert(0, new ListItem("Select an Option", String.Empty));

            control.TrySetSelectedValue(selValue);
        }
        public static void DataBind(this BaseDataBoundControl control, Object dataSource)
		{
			control.DataSource = dataSource;
			control.DataBind();
		}
		public static void DataBind(this ASPxDataWebControlBase control, Object dataSource)
		{
			control.DataSource = dataSource;
			control.DataBind();
		}

		public static String RenderString(this Control control)
		{
			using (var stringWriter = new StringWriter())
			{
				using (var htmlWriter = new HtmlTextWriter(stringWriter))
				{
					control.RenderControl(htmlWriter);

					htmlWriter.Flush();
					stringWriter.Flush();

					return stringWriter.ToString();
				}
			}
		}
	}
}
