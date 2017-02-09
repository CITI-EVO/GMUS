using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;

namespace CITI.EVO.Tools.Utils
{
	public static class UserInterfaceUtil
	{
        public static String FormatForHtml(String text)
        {
            text = (text ?? String.Empty);

            text = text.Replace("\t", "&nbsp&nbsp&nbsp&nbsp");
            text = text.Replace(" ", "&nbsp");
            text = text.Replace("\n", "<br />");

            return text;
        }

        public static Object GetTruncatedLabel(Object eval)
        {
            return GetTruncatedLabel(eval, 20);
        }

        public static Object GetTruncatedLabel(Object eval, int visibleCharCount)
        {
            var text = Convert.ToString(eval);

            var label = new Label();
            label.Text = text;

            if (text.Length > visibleCharCount)
            {
                label.Text = String.Format("{0}...", text.Substring(0, visibleCharCount - 3));
                label.ToolTip = text;

                label.ForeColor = Color.DodgerBlue;
                label.CssClass = "tooltip";

                label.Attributes["style"] = "cursor: help;";
            }

            return label.RenderString();
        }

        public static IEnumerable<Control> TraverseParents(Control control)
        {
            return TraverseParents(control, null);
        }
        public static IEnumerable<Control> TraverseParents(Control control, Predicate<Control> skipChildren)
        {
            while (control != null)
            {
                if (skipChildren == null || !skipChildren(control))
                    yield return control;

                control = control.Parent;
            }
        }

        public static IEnumerable<Control> TraverseChildren(Control control)
        {
            return TraverseChildren(control, null);
        }
        public static IEnumerable<Control> TraverseChildren(Control control, Predicate<Control> skipChildren)
        {
            var stack = new Stack<Control>();

            foreach (Control child in control.Controls)
                stack.Push(child);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (current.Controls.Count > 0)
                {
                    if (skipChildren == null || !skipChildren(current))
                    {
                        foreach (Control child in current.Controls)
                            stack.Push(child);
                    }
                }

                yield return current;
            }
        }

        public static IEnumerable<Control> TraverseControls(Control control)
        {
            return TraverseControls(control, null);
        }
        public static IEnumerable<Control> TraverseControls(Control control, Predicate<Control> skipChildren)
        {
            var stack = new Stack<Control>();

            foreach (Control child in control.Controls)
                stack.Push(child);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (current.Controls.Count > 0)
                {
                    if (skipChildren == null || !skipChildren(current))
                    {
                        foreach (Control child in current.Controls)
                            stack.Push(child);
                    }
                }

                yield return current;
            }
        }

        public static String GetAttributeValue(Control control, String attributeName)
        {
            var webControl = control as WebControl;
            if (webControl != null)
            {
                return webControl.Attributes[attributeName];
            }

            var userControl = control as UserControl;
            if (userControl != null)
            {
                return userControl.Attributes[attributeName];
            }

            return null;
        }
    }
}
