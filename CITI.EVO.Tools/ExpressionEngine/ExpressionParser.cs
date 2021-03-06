using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.ExpressionEngine.Common;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;

namespace CITI.EVO.Tools.ExpressionEngine
{
    public static class ExpressionParser
    {
        #region internal data

        private const String CacheKey = "@{ExpressionNodes}";

        private static readonly ISet<char> operationalSymbols = new HashSet<char>
        {
            '(', ')', '[', ']', ',', '"', '\'',
        };

        private static readonly ISet<char> reservedSymbols = new HashSet<char>
        {
            '%', '^', '&', '*', '-', '=', '+', '/', '\\', '!', '<', '>', '|'
        };

        private static readonly IDictionary<String, int> operators = new Dictionary<String, int>
        {
            {"%" , 0}, {"^" , 0},
            {"*" , 1}, {"/" , 1}, {"\\", 1},
            {"!" , 3}, {"+" , 3}, {"-" , 3}, {"<" , 3}, {">" , 3}, {"&" , 3}, {"=" , 3},
            {"++", 3}, {"--", 3}, {"==", 3}, {"!=", 3}, {"<>", 3}, {"<=", 3}, {">=", 3},
            {"&&", 4},
            {"||", 5},
        };

        private static readonly Regex escapeRx;
        #endregion

        static ExpressionParser()
        {
            var escapeSymbols = new HashSet<char>
            {
                '.'
            };

            escapeSymbols.UnionWith(operationalSymbols);
            escapeSymbols.UnionWith(reservedSymbols);

            var escapeSymText = String.Join(@"\", escapeSymbols);
            var escapePattern = $@"[{escapeSymText}]";

            escapeRx = new Regex(escapePattern, RegexOptions.Compiled);
        }

        public static String Escape(String name)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            return escapeRx.Replace(name, "_");
        }

        public static ExpressionNode GetOrParse(String expression)
        {
            var hashCode = expression.ComputeMd5();
            var cache = CommonObjectCache.InitObject(CacheKey, ConcurrencyHelper.CreateDictionary<String, ExpressionNode>);

            ExpressionNode node;
            if (!cache.TryGetValue(hashCode, out node))
            {
                node = Parse(expression);
                cache[hashCode] = node;
            }

            return node;
        }

        public static ExpressionNode Parse(String expression)
        {
            var rootNode = new ExpressionNode { Container = true };

            var expTokens = ExpressionTokenizer(expression);

            foreach (var exp in expTokens)
            {
                var node = InternalParse(exp);
                rootNode.Params.Add(node);
            }

            return rootNode;
        }

        private static ExpressionNode InternalParse(String expression)
        {
            if (String.IsNullOrEmpty(expression))
                return new OperatorNode { Value = double.NaN };

            expression = expression.Trim();

            if (ExpressionHelper.IsNumber(expression))
            {
                var node = new ExpressionNode
                {
                    Value = ExpressionHelper.GetNumber(expression),
                    ValueType = ValueTypes.Number,
                };

                return node;
            }

            if (ExpressionHelper.IsString(expression))
            {
                var node = new StringValueNode
                {
                    Value = ExpressionHelper.GetString(expression),
                    Quota = ExpressionHelper.GetQuota(expression),
                    ValueType = ValueTypes.String,
                };

                return node;
            }

            if (ExpressionHelper.IsDateTime(expression))
            {
                var node = new ExpressionNode
                {
                    Value = ExpressionHelper.GetDateTime(expression),
                    ValueType = ValueTypes.DateTime,
                };

                return node;
            }

            int parentheses = 0;

            var leftExp = String.Empty;
            var rightExp = String.Empty;

            var currOperator = String.Empty;
            var currPriority = -1;

            var singleQuoteOpen = false;
            var doubleQuoteOpen = false;

            for (int i = 0; i < expression.Length; i++)
            {
                var @char = expression[i];
                if (@char == '\'' && !doubleQuoteOpen)
                    singleQuoteOpen = !singleQuoteOpen;

                if (@char == '"' && !singleQuoteOpen)
                    doubleQuoteOpen = !doubleQuoteOpen;

                if (singleQuoteOpen || doubleQuoteOpen)
                    continue;

                if (@char == '(')
                    parentheses++;
                else if (@char == ')')
                    parentheses--;
                else if (parentheses == 0)
                {
                    if (reservedSymbols.Contains(@char) && i != 0)
                    {
                        int operatorStart = i;

                        while (reservedSymbols.Contains(@char))
                            @char = expression[++i];

                        int operatorEnd = i--;

                        var @operator = expression.Substring(operatorStart, operatorEnd - operatorStart).Trim();

                        int priority;
                        if (!operators.TryGetValue(@operator, out priority))
                            throw new Exception($"Unknown operator \"{@operator}\"");

                        if (currPriority < priority)
                        {
                            currOperator = @operator;
                            currPriority = priority;

                            leftExp = expression.Substring(0, operatorStart).Trim();
                            rightExp = expression.Substring(operatorEnd).Trim();
                        }
                    }
                }
            }

            if (parentheses > 0)
                throw new Exception($"Missing ) in \"{expression}\"");

            if (parentheses < 0)
                throw new Exception($"Too many )s in \"{expression}\"");

            if (String.IsNullOrWhiteSpace(leftExp) &&
                String.IsNullOrWhiteSpace(currOperator) &&
                String.IsNullOrWhiteSpace(rightExp))
            {
                if (expression.StartsWith("(") && expression.EndsWith(")"))
                {
                    var firstClosingBracketIndex = expression.IndexOf(")", StringComparison.OrdinalIgnoreCase);
                    var nextOpeningBracketIndex = expression.IndexOf("(", 1, StringComparison.OrdinalIgnoreCase);

                    if (nextOpeningBracketIndex < firstClosingBracketIndex)
                    {
                        var subNode = InternalParse(expression.Substring(1, expression.Length - 2));
                        return subNode;
                    }
                }
            }

            if (ExpressionHelper.IsEmptyOrSpace(currOperator))
            {
                var actionName = String.Empty;
                var innerExp = String.Empty;

                for (int i = 0; i < expression.Length; i++)
                {
                    if (!reservedSymbols.Contains(expression[i]))
                    {
                        innerExp = expression.Substring(i);
                        break;
                    }

                    actionName += expression[i];
                }

                if (!String.IsNullOrEmpty(actionName) &&
                    actionName != "-" &&
                    actionName != "--" &&
                    actionName != "+" &&
                    actionName != "++" &&
                    actionName != "!")
                {
                    throw new Exception();
                }

                var openingBracketIndex = expression.IndexOf("(", StringComparison.OrdinalIgnoreCase);
                if (openingBracketIndex > 0 && expression.EndsWith(")"))
                {
                    if (operators.ContainsKey(actionName))
                    {
                        var subNode = new OperatorNode
                        {
                            Action = actionName,
                            ActionType = ActionTypes.Operator,

                            Right = InternalParse(innerExp)
                        };

                        return subNode;
                    }

                    actionName = expression.Substring(0, openingBracketIndex);
                    innerExp = expression.Substring(openingBracketIndex + 1, innerExp.Length - openingBracketIndex - 2);

                    var funcNode = new FunctionNode
                    {
                        Action = actionName,
                        ActionType = ActionTypes.Function,
                    };

                    if (!ExpressionHelper.IsEmptyOrSpace(innerExp))
                    {
                        var paramsArr = SplitParams(innerExp);
                        foreach (var paramExp in paramsArr)
                        {
                            if (ExpressionHelper.IsEmptyOrSpace(paramExp))
                            {
                                var node = new ExpressionNode();
                                funcNode.Params.Add(node);
                            }
                            else
                            {
                                var node = InternalParse(paramExp);
                                funcNode.Params.Add(node);
                            }
                        }
                    }

                    return funcNode;
                }

                var varNode = new ExpressionNode
                {
                    Action = innerExp,
                    ValueType = ValueTypes.Variable,
                };

                if (String.IsNullOrEmpty(actionName))
                    return varNode;

                var actNode = new OperatorNode
                {
                    Action = actionName,
                    ActionType = ActionTypes.Operator,
                    Right = varNode,
                };

                return actNode;
            }

            var operNode = new OperatorNode
            {
                Action = currOperator,
                ActionType = ActionTypes.Operator,

                Left = InternalParse(leftExp),
                Right = InternalParse(rightExp)
            };

            return operNode;
        }

        private static IEnumerable<String> SplitParams(String expression)
        {
            var parentheses = 0;
            var buffer = new StringBuilder();

            var singleQuoteOpen = false;
            var doubleQuoteOpen = false;

            foreach (var @char in expression)
            {
                if (@char == '\'' && !doubleQuoteOpen)
                    singleQuoteOpen = !singleQuoteOpen;

                if (@char == '"' && !singleQuoteOpen)
                    doubleQuoteOpen = !doubleQuoteOpen;

                if (!singleQuoteOpen && !doubleQuoteOpen)
                {
                    switch (@char)
                    {
                        case '(':
                            {
                                parentheses++;
                                break;
                            }
                        case ')':
                            {
                                parentheses--;
                                break;
                            }
                        case ',':
                            {
                                if (parentheses == 0)
                                {
                                    var item = buffer.ToString();
                                    yield return item.Trim();

                                    buffer.Clear();
                                    continue;
                                }

                                break;
                            }
                    }
                }

                buffer.Append(@char);
            }

            if (buffer.Length > 0)
            {
                var item = buffer.ToString();
                yield return item.Trim();
            }
        }

        private static IEnumerable<String> ExpressionTokenizer(String expression)
        {
            var buffer = new StringBuilder();

            var singleQuoteOpen = false;
            var doubleQuoteOpen = false;

            foreach (var @char in expression)
            {
                if (@char == '\'' && !doubleQuoteOpen)
                    singleQuoteOpen = !singleQuoteOpen;

                if (@char == '"' && !singleQuoteOpen)
                    doubleQuoteOpen = !doubleQuoteOpen;

                if (@char == ';' && !singleQuoteOpen && !doubleQuoteOpen)
                {
                    var item = buffer.ToString();
                    yield return item.Trim();

                    buffer.Clear();
                    continue;
                }

                buffer.Append(@char);
            }

            if (buffer.Length > 0)
            {
                var item = buffer.ToString();
                yield return item.Trim();
            }
        }
    }
}