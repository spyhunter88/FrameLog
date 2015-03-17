using System;
using System.Linq.Expressions;
using System.Reflection;
using FrameLog.Exceptions;

namespace FrameLog.Helpers
{
    public static class ExpressionHelper
    {
        public static string GetPropertyName<TModel, TProperty>(this Expression<Func<TModel, TProperty>> lambda)
        {
            string result = GetPropertyName(lambda.Body);
            if (result == null)
            {
                throw new InvalidPropertyExpressionException(lambda);
            }
            return result;
        }

        private static string GetPropertyName(Expression expression)
        {
            MemberExpression member = expression as MemberExpression;
            if (member == null)
                return null;

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                return null;

            string parent = GetPropertyName(member.Expression);
            return Join(parent, propInfo.Name);
        }

        public static string Join(string a, string b)
        {
            if (a == null && b == null)
            {
                return null;
            }
            else if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
            {
                return a + b;
            }
            else
            {
                return a + "." + b;
            }
        }
    }
}
