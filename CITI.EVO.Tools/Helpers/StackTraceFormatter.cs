using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.Tools.Helpers
{
    public class StackTraceFormatter
    {
        private readonly ISet<String> primitiveTypes = new HashSet<String>(StringComparer.OrdinalIgnoreCase)
        {
            "bool",
            "boolean",
            "system.boolean",

            "sbyte",
            "system.sbyte",

            "byte",
            "system.byte",

            "short",
            "int16",

            "system.int16",
            "ushort",
            "uint16",
            "system.uint16",

            "int",
            "int32",
            "system.int32",

            "uint",
            "uint32",
            "system.uint32",

            "long",
            "int64",
            "system.int64",

            "ulong",
            "uint64",
            "system.uint64",

            "float",
            "single",
            "system.single",

            "double",
            "system.double",

            "decimal",
            "system.decimal",

            "guid",
            "system.guid",

            "datetime",
            "system.datetime",

            "timespan",
            "system.timespan",

            "string",
            "system.string",
        };

        private readonly Regex stackLineRx = new Regex(@"(^at (?<method>.*?) in (?<file>.*?)$)|(^at (?<method>.*?)$)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex methodSigRx = new Regex(@"(?<method>.*?)\((?<params>.*)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public WebControl TryFormatStack(String text)
        {
            try
            {
                return FormatStack(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public WebControl FormatStack(String text)
        {
            var stackControl = new Panel();

            var reader = new StringReader(text);
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                line = line.Trim();

                var match = stackLineRx.Match(line);
                var method = match.Groups["method"].Value;
                var file = match.Groups["file"].Value;

                var methodPanel = new Panel();

                var methodControl = FormatMethod(method);
                methodPanel.Controls.Add(methodControl);

                if (!String.IsNullOrWhiteSpace(file))
                {
                    var filePanel = new Panel();
                    filePanel.Style["padding-left"] = "15px";
                    filePanel.Controls.Add(new Label { Text = "&nbsp in &nbsp", ForeColor = Color.Black });
                    filePanel.Controls.Add(new Label { Text = file, ForeColor = Color.Brown });

                    methodPanel.Controls.Add(filePanel);
                }

                stackControl.Controls.Add(methodPanel);
            }

            return stackControl;
        }

        private Control FormatMethod(String text)
        {
            var match = methodSigRx.Match(text);

            var methodFullName = match.Groups["method"].Value;
            var methodParams = match.Groups["params"].Value;

            var methodArr = methodFullName.Split(new[] { '.' });

            var classFullName = String.Join(".", methodArr, 0, methodArr.Length - 1);
            var methoName = methodArr[methodArr.Length - 1];

            var atLabel = CreateDelimControl("at&nbsp");
            var classLabel = CreateClassLabel(classFullName);
            var delimLabel = CreateDelimControl(".");
            var methodLabel = new Label { Text = methoName, ForeColor = Color.Black };
            var openParLabel = CreateDelimControl("(");
            var paramsLabel = CreateParamsLabel(methodParams);
            var closeParLabel = CreateDelimControl(")");

            var methodSigLabel = new Label();
            methodSigLabel.Controls.Add(atLabel);
            methodSigLabel.Controls.Add(classLabel);
            methodSigLabel.Controls.Add(delimLabel);
            methodSigLabel.Controls.Add(methodLabel);
            methodSigLabel.Controls.Add(openParLabel);
            methodSigLabel.Controls.Add(paramsLabel);
            methodSigLabel.Controls.Add(closeParLabel);

            return methodSigLabel;
        }

        private Control CreateParamsLabel(String allParam)
        {
            var paramsArr = allParam.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var paramsLabel = new Label();

            foreach (var item in paramsArr)
            {
                var paramLabel = CreateParamLabel(item.Trim());
                var delimLabel = CreateDelimControl(",&nbsp");

                paramsLabel.Controls.Add(paramLabel);
                paramsLabel.Controls.Add(delimLabel);
            }

            if (paramsLabel.Controls.Count > 1)
            {
                paramsLabel.Controls.RemoveAt(paramsLabel.Controls.Count - 1);
            }

            return paramsLabel;
        }

        private WebControl CreateParamLabel(String paramFullName)
        {
            var paramArr = paramFullName.Split(new[] { ' ' });
            var classFullName = paramArr[0];
            var paramName = (paramArr.Length > 1 ? paramArr[1] : String.Empty);

            var classLabel = CreateClassLabel(classFullName);
            var delimLabel = CreateDelimControl("&nbsp");
            var nameLabel = new Label { Text = paramName, ForeColor = Color.Black };

            var paramLabel = new Label();

            paramLabel.Controls.Add(classLabel);
            paramLabel.Controls.Add(delimLabel);
            paramLabel.Controls.Add(nameLabel);

            return paramLabel;
        }

        private Control CreateDelimControl(String delimiter)
        {
            var g = new HtmlGenericControl { InnerHtml = delimiter, };
            g.Style["color"] = "black";

            return g;
        }

        private WebControl CreateClassLabel(String classFullName)
        {
            var classArr = classFullName.Split(new[] { '.' });

            var classNs = String.Join(".", classArr, 0, classArr.Length - 1);
            var className = classArr[classArr.Length - 1];

            var classLabel = CreateClassLabel(classNs, className);
            return classLabel;
        }

        private WebControl CreateClassLabel(String classNs, String className)
        {
            var classLabel = new Label();

            var color = GetClassColor(classNs, className);
            var nameLabel = new Label { Text = className, ForeColor = color };

            if (String.IsNullOrWhiteSpace(classNs))
            {
                classLabel.Controls.Add(nameLabel);
            }
            else
            {
                var nsLabel = new Label { Text = classNs, ForeColor = Color.DarkMagenta };
                var delimLabel = CreateDelimControl(".");

                classLabel.Controls.Add(nsLabel);
                classLabel.Controls.Add(delimLabel);
                classLabel.Controls.Add(nameLabel);
            }

            return classLabel;
        }

        private Color GetClassColor(String classNs, String className)
        {
            if (primitiveTypes.Contains(className))
                return Color.Blue;

            var list = new List<String> { classNs, className };
            list.Remove(String.Empty);

            var fullName = String.Join(".", list);

            var type = ReflectionUtil.FindType(fullName);
            if (type != null)
            {
                if (type.IsValueType)
                    return Color.DarkGoldenrod;

                if (type.IsInterface)
                    return Color.CornflowerBlue;
            }

            return Color.CadetBlue;
        }
    }
}
