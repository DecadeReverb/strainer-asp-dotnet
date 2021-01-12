using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class PropertyInfoProvider : IPropertyInfoProvider
    {
        public (string, PropertyInfo) GetPropertyInfoAndFullName<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!(expression.Body is MemberExpression body))
            {
                var ubody = expression.Body as UnaryExpression;
                body = ubody.Operand as MemberExpression;
            }

            var propertyInfo = body?.Member as PropertyInfo;
            var stack = new Stack<string>();
            while (body != null)
            {
                stack.Push(body.Member.Name);
                body = body.Expression as MemberExpression;
            }

            return (string.Join(".", stack.ToArray()), propertyInfo);
        }
    }
}
