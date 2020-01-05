using ExTra.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExTra
{
    public abstract class ExpressionTranslator<T>
    {
        /// <summary>
        /// Translate an expression.
        /// </summary>
        /// <typeparam name="TExpression"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T Translate<TExpression, TResult>(Expression<Func<TExpression, TResult>> expression)
        {
            T output = default;
            AnalyzeExpression(expression.Body, ref output);
            return output;
        }

        /// <summary>
        /// Analyzes the expression based on its components.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="output"></param>
        private void AnalyzeExpression(Expression e, ref T output)
        {
            if (e is BinaryExpression binaryExpression)
            {
                AnalyzeExpression(binaryExpression.Left, ref output);
                output = Append(output, Translate(binaryExpression.NodeType));
                AnalyzeExpression(binaryExpression.Right, ref output);
            }
            else if (e is MemberExpression memberExpression)
            {
                output = Append(output, Translate(AnalyzeMemberExpression(memberExpression)));
            }
            else if (e is ConstantExpression constantExpression)
            {
                output = Append(output, Translate(constantExpression.Value));
            }
            else if (e is MethodCallExpression methodCallExpression)
            {
                string method = methodCallExpression.Method.Name;

                List<object> arguments = methodCallExpression.Arguments.Where(x => x is ConstantExpression ce).Select(y => (y as ConstantExpression).Value).ToList();

                T caller = methodCallExpression.Object switch
                {
                    MemberExpression me => Translate(AnalyzeMemberExpression(me).ToList()),
                    ConstantExpression ce => Translate(ce.Value),
                    _ => throw new Exception("Unable to analyze object.")
                };

                output = Append(output, Translate(new MethodCall(method, caller.ToString(), arguments)));
            }
            else
            {
                output = Append(output, Translate(e));
            }
        }

        /// <summary>
        /// Get a list of all expressions that make up <paramref name="memberExpression"/>.
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        protected IEnumerable<PropertyName> AnalyzeMemberExpression(MemberExpression memberExpression)
        {
            List<PropertyName> propertyList = new List<PropertyName>();
            RecursiveMemberExpressionAnalysis(memberExpression, ref propertyList);
            return propertyList;
        }

        /// <summary>
        /// Recursively looks through <paramref name="memberExpression"/> until it can no longer find an attached expressions.
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <param name="propertyList"></param>
        private void RecursiveMemberExpressionAnalysis(MemberExpression memberExpression, ref List<PropertyName> propertyList)
        {
            MemberExpression innerExpression = memberExpression?.GetType().GetProperty("Expression")?.GetValue(memberExpression) as MemberExpression;
            if (innerExpression is MemberExpression innerMemberExpression)
            {
                RecursiveMemberExpressionAnalysis(innerMemberExpression, ref propertyList);
            }
            propertyList.Add(new PropertyName(memberExpression.Member.Name));
        }

        /// <summary>
        /// Appends <paramref name="addition"/> onto <paramref name="origin"/>.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="addition"></param>
        /// <returns></returns>
        protected abstract T Append(T origin, T addition);

        /// <summary>
        /// Translate an object from the expression.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        protected abstract T Translate(object o);
    }
}
