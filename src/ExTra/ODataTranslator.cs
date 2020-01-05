using ExTra.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExTra
{
    public class ODataTranslator : ExpressionTranslator<string>
    {
        protected override string Append(string origin, string addition)
        {
            return origin + addition;
        }

        protected override string Translate(object o)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberDecimalSeparator = ".";

            return o switch
            {
                null => "null",
                bool b => b.ToString(nfi),
                byte b => b.ToString(nfi),
                DateTime d => $"datetime'{d.ToString("yyyy-mm-ddThh:mm:ss.fffffff")}",
                decimal d => $"{d.ToString(nfi)}m",
                double d => $"{d.ToString(nfi)}d",
                Guid g => $"guid'{g.ToString()}",
                int i => i.ToString(nfi),
                long l => $"{l.ToString(nfi)}l",
                sbyte s => s.ToString(nfi),
                short s => s.ToString(nfi),
                string s => $"'{s}'",

                ExpressionType t when t == ExpressionType.AndAlso => " and ",
                ExpressionType t when t == ExpressionType.Equal => " eq ",
                ExpressionType t when t == ExpressionType.GreaterThan => " gt ",
                ExpressionType t when t == ExpressionType.GreaterThanOrEqual => " ge ",
                ExpressionType t when t == ExpressionType.LessThan => " lt ",
                ExpressionType t when t == ExpressionType.LessThanOrEqual => " le ",
                ExpressionType t when t == ExpressionType.OrElse => " or ",

                IEnumerable<PropertyName> p => string.Join("/", p.Select(pr => pr.Name)),

                MethodCall m when m.Method == "StartsWith" => $"startswith({m.Caller}, {Translate(m.Arguments[0])})",
                MethodCall m when m.Method == "EndsWith" => $"endswith({m.Caller}, {Translate(m.Arguments[0])})",

                _ => throw new InvalidOperationException($"Unable to translate object.")
            };
        }
    }
}
