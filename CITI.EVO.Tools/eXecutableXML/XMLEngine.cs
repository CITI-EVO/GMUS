using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using CITI.EVO.Tools.ExpressionEngine;

namespace CITI.EVO.Tools.eXecutableXML
{
    public class XMLEngine
    {
        private readonly XElement _rootXElem;
        private readonly XMLContext _globalContext;
        private readonly IDictionary<String, XElement> _methods;

        public XMLEngine(XElement rootXElem)
        {
            _rootXElem = rootXElem;
            _globalContext = new XMLContext();
            _methods = new Dictionary<String, XElement>(StringComparer.OrdinalIgnoreCase);

            foreach (var xElem in rootXElem.Elements())
            {
                if (xElem.Name == "func")
                {
                    var name = (String)xElem.Attribute("name");
                    if (String.IsNullOrWhiteSpace(name))
                        throw new Exception();

                    if (_methods.ContainsKey(name))
                        throw new Exception();

                    _methods.Add(name, xElem);
                }
            }
        }

        public Object Exec()
        {
            return Exec(null);
        }
        public Object Exec(IDictionary<String, Object> context)
        {
            if (context != null)
            {
                foreach (var pair in context)
                    _globalContext.SetValue(pair.Key, pair.Value);
            }

            var xmlReturn = ExecContent(_rootXElem, _globalContext) as XMLReturn;
            if (xmlReturn != null)
                return xmlReturn.Value;

            return null;
        }

        private XMLResult ExecContent(XElement rootXElem, XMLContext context)
        {
            var opersXElems = rootXElem.Elements();
            foreach (var operXElem in opersXElems)
            {
                var xmlResult = (XMLResult)null;

                var operation = operXElem.Name.ToString();
                switch (operation)
                {
                    case "add":
                        xmlResult = ExecAdd(operXElem, context);
                        break;
                    case "insert":
                        xmlResult = ExecInsert(operXElem, context);
                        break;
                    case "remove":
                        xmlResult = ExecRemove(operXElem, context);
                        break;
                    case "removeAt":
                        xmlResult = ExecRemoveAt(operXElem, context);
                        break;
                    case "clear":
                        xmlResult = ExecClear(operXElem, context);
                        break;
                    case "set":
                        xmlResult = ExecSet(operXElem, context);
                        break;
                    case "out":
                        xmlResult = ExecOut(operXElem, context);
                        break;
                    case "print":
                        xmlResult = ExecPrint(operXElem, context);
                        break;
                    case "return":
                        xmlResult = ExecReturn(operXElem, context);
                        break;
                    case "check":
                        xmlResult = ExecCheck(operXElem, context);
                        break;
                    case "for":
                        xmlResult = ExecFor(operXElem, context);
                        break;
                    case "foreach":
                        xmlResult = ExecForEach(operXElem, context);
                        break;
                    case "while":
                        xmlResult = ExecWhile(operXElem, context);
                        break;
                    case "call":
                        xmlResult = ExecCall(operXElem, context);
                        break;
                    case "break":
                        return XMLBreak.Value;
                    case "func":
                        break;
                }

                if (xmlResult != null)
                    return xmlResult;
            }

            return null;
        }

        private XMLResult ExecAdd(XElement xElem, XMLContext context)
        {
            var @var = (String)xElem.Attribute("var");
            if (String.IsNullOrWhiteSpace(@var))
                throw new Exception();

            var list = EvalObj(@var, context) as IList;
            if (list == null)
                throw new Exception();

            var value = (String)xElem.Attribute("value");
            if (String.IsNullOrWhiteSpace(value))
                throw new Exception();

            var item = EvalObj(value, context);

            list.Add(item);
            return null;
        }

        private XMLResult ExecInsert(XElement xElem, XMLContext context)
        {
            var @var = (String)xElem.Attribute("var");
            if (String.IsNullOrWhiteSpace(@var))
                throw new Exception();

            var list = EvalObj(@var, context) as IList;
            if (list == null)
                throw new Exception();

            int index;
            if (!int.TryParse((String)xElem.Attribute("index"), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out index))
                throw new Exception();

            var value = (String)xElem.Attribute("value");
            if (String.IsNullOrWhiteSpace(value))
                throw new Exception();

            var item = EvalObj(value, context);

            list.Insert(index, item);
            return null;
        }

        private XMLResult ExecRemove(XElement xElem, XMLContext context)
        {
            var @var = (String)xElem.Attribute("var");
            if (String.IsNullOrWhiteSpace(@var))
                throw new Exception();

            var list = EvalObj(@var, context) as IList;
            if (list == null)
                throw new Exception();

            var value = (String)xElem.Attribute("value");
            if (String.IsNullOrWhiteSpace(value))
                throw new Exception();

            var item = EvalObj(value, context);

            list.Remove(item);
            return null;
        }

        private XMLResult ExecRemoveAt(XElement xElem, XMLContext context)
        {
            var @var = (String)xElem.Attribute("var");
            if (String.IsNullOrWhiteSpace(@var))
                throw new Exception();

            var list = EvalObj(@var, context) as IList;
            if (list == null)
                throw new Exception();

            int index;
            if (!int.TryParse((String)xElem.Attribute("index"), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out index))
                throw new Exception();

            list.RemoveAt(index);
            return null;
        }

        private XMLResult ExecClear(XElement xElem, XMLContext context)
        {
            var @var = (String)xElem.Attribute("var");
            if (String.IsNullOrWhiteSpace(@var))
                throw new Exception();

            var list = EvalObj(@var, context) as IList;
            if (list == null)
                throw new Exception();

            int index;
            if (!int.TryParse((String)xElem.Attribute("index"), NumberStyles.Any, NumberFormatInfo.CurrentInfo, out index))
                throw new Exception();

            list.Clear();
            return null;
        }

        private XMLResult ExecCheck(XElement xElem, XMLContext context)
        {
            var ifXElem = xElem.Element("if");
            if (ifXElem == null)
                throw new Exception();

            var elseifsXElems = xElem.Elements("elseif");
            var elseXElem = xElem.Element("else");

            var ifTest = (String)ifXElem.Attribute("test");

            var ifRes = EvalLogic(ifTest, context);
            if (ifRes)
            {
                var subContent = new XMLContext(context);
                var xmlResult = ExecContent(ifXElem, subContent);

                subContent.Clear();
                return xmlResult;
            }

            foreach (var elseifsXElem in elseifsXElems)
            {
                var elseIfTest = (String)elseifsXElem.Attribute("test");

                var elseIfRes = EvalLogic(elseIfTest, context);
                if (elseIfRes)
                {
                    var subContent = new XMLContext(context);
                    var xmlResult = ExecContent(elseifsXElem, subContent);

                    subContent.Clear();
                    return xmlResult;
                }
            }

            if (elseXElem != null)
            {
                var subContent = new XMLContext(context);
                var xmlResult = ExecContent(elseXElem, subContent);

                subContent.Clear();
                return xmlResult;
            }

            return null;
        }

        private XMLResult ExecOut(XElement xElem, XMLContext context)
        {
            return null;
        }

        private XMLResult ExecPrint(XElement xElem, XMLContext context)
        {
            var value = (String)xElem.Attribute("value");
            var varVal = (Object)null;

            if (!String.IsNullOrWhiteSpace(value))
                varVal = EvalObj(value, context);

            Console.WriteLine(varVal);
            return null;
        }

        private XMLResult ExecReturn(XElement xElem, XMLContext context)
        {
            var value = (String)xElem.Attribute("value");
            var varVal = (Object)null;

            if (!String.IsNullOrWhiteSpace(value))
                varVal = EvalObj(value, context);

            return new XMLReturn(varVal);
        }

        private XMLResult ExecSet(XElement xElem, XMLContext context)
        {
            var name = (String)xElem.Attribute("name");
            if (String.IsNullOrWhiteSpace(name))
                throw new Exception();

            var value = (String)xElem.Attribute("value");
            var varVal = (Object)null;

            if (!String.IsNullOrWhiteSpace(value))
                varVal = EvalObj(value, context);
            else
            {
                var type = (String)xElem.Attribute("type");
                if (type == "list")
                    varVal = new ArrayList();
            }

            context.SetValue(name, varVal);
            return null;
        }

        private XMLResult ExecFor(XElement xElem, XMLContext context)
        {
            var @var = (String)xElem.Attribute("var");
            var start = (String)xElem.Attribute("start");
            var end = (String)xElem.Attribute("end");
            var step = (String)xElem.Attribute("step");

            if (String.IsNullOrWhiteSpace(@var))
                throw new Exception();

            var startVal = Convert.ToString(EvalObj(start, context));
            var endVal = Convert.ToString(EvalObj(end, context));
            var stepVal = Convert.ToString(EvalObj(step, context));

            double startDbl;
            if (!double.TryParse(startVal, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out startDbl))
                throw new Exception();

            double endDbl;
            if (!double.TryParse(endVal, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out endDbl))
                throw new Exception();

            double stepDbl;
            if (!double.TryParse(stepVal, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out stepDbl))
                stepDbl = 1D;

            var subContent = new XMLContext(context);

            for (double i = startDbl; i < endDbl; i += stepDbl)
            {
                subContent.SetValue(@var, i);

                var xmlResult = ExecContent(xElem, subContent);

                if (ReferenceEquals(xmlResult, XMLBreak.Value))
                    break;

                if (xmlResult != null)
                    return xmlResult;
            }

            subContent.Clear();
            return null;
        }

        private XMLResult ExecForEach(XElement xElem, XMLContext context)
        {
            var @var = (String)xElem.Attribute("var");
            if (String.IsNullOrWhiteSpace(@var))
                throw new Exception();

            var items = (String)xElem.Attribute("items");
            if (String.IsNullOrWhiteSpace(items))
                throw new Exception();

            var list = EvalObj(items, context) as IEnumerable;
            if (list == null)
                throw new Exception();

            var subContent = new XMLContext(context);

            foreach (var item in items)
            {
                subContent.SetValue(@var, item);

                var xmlResult = ExecContent(xElem, subContent);

                if (ReferenceEquals(xmlResult, XMLBreak.Value))
                    break;

                if (xmlResult != null)
                    return xmlResult;
            }

            subContent.Clear();
            return null;
        }

        private XMLResult ExecWhile(XElement xElem, XMLContext context)
        {
            var test = (String)xElem.Attribute("test");
            if (String.IsNullOrWhiteSpace(test))
                throw new Exception();

            var subContent = new XMLContext(context);

            while (EvalLogic(test, context))
            {
                var xmlResult = ExecContent(xElem, subContent);

                if (ReferenceEquals(xmlResult, XMLBreak.Value))
                    break;

                if (xmlResult != null)
                    return xmlResult;
            }

            subContent.Clear();
            return null;
        }

        private XMLResult ExecCall(XElement xElem, XMLContext context)
        {
            var name = (String)xElem.Attribute("name");
            var result = (String)xElem.Attribute("result");

            var @params = new XMLContext(_globalContext);

            foreach (var paramXElem in xElem.Elements("param"))
            {
                var paramName = (String)paramXElem.Attribute("name");
                var paramValue = CreateVarValue(paramXElem, context);

                @params.SetValue(paramName, paramValue);
            }

            XElement methodXElem;
            if (!_methods.TryGetValue(name, out methodXElem))
                throw new Exception();

            var xmlReturn = ExecContent(methodXElem, @params) as XMLReturn;
            if (!String.IsNullOrWhiteSpace(result))
            {
                if (xmlReturn != null)
                    context.SetValue(result, xmlReturn.Value);
            }

            return null;
        }

        private Object CreateVarValue(XElement xElem, XMLContext context)
        {
            var value = (String)xElem.Attribute("value");
            var type = (String)xElem.Attribute("type");

            return CreateVarValue(value, context, type);
        }
        private Object CreateVarValue(String expression, XMLContext context)
        {
            return CreateVarValue(expression, context, null);
        }
        private Object CreateVarValue(String expression, XMLContext context, String type)
        {
            if (!String.IsNullOrWhiteSpace(expression))
                return EvalObj(expression, context);

            if (type == "list")
                return new ArrayList();

            return null;
        }

        private bool EvalLogic(String expression, XMLContext context)
        {
            var result = EvalObj(expression, context);
            if (result != null && !Equals(result, false))
                return true;

            return false;
        }

        private Object EvalObj(String expression, XMLContext context)
        {
            if (String.IsNullOrEmpty(expression))
                return null;

            var advResolver = new AdvancedDataResolver(context.GetValue);
            var expNode = ExpressionParser.GetOrParse(expression);

            var result = ExpressionEvaluator.Eval(expNode, advResolver);

            return result;
        }
    }
}
