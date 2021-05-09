using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    /// <summary>
    /// Provides <see cref="PropertyInfo"/> and full member name when supplied
    /// with an <see cref="Expression{TDelegate}"/>.
    /// </summary>
    public class PropertyInfoProvider : IPropertyInfoProvider
    {
        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> and property full name
        /// (for nested property paths).
        /// </summary>
        /// <typeparam name="T">
        /// The base type expression is based on.
        /// </typeparam>
        /// <param name="expression">
        /// The lamda expression leading to property.
        /// </param>
        /// <returns>
        /// A tuple of <see cref="PropertyInfo"/> and <see cref="string"/>
        /// full property name.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="expression"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="expression"/> is invalid expression,
        /// not leading to a readable property.
        /// </exception>
        public (PropertyInfo PropertyInfo, string FullName) GetPropertyInfoAndFullName<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (expression.Body is not MemberExpression body)
            {
                var ubody = expression.Body as UnaryExpression;
                body = ubody?.Operand as MemberExpression;
            }

            if (body?.Member is not PropertyInfo propertyInfo)
            {
                throw new ArgumentException(
                    $"Expression for '{nameof(T)}' {expression} " +
                    $"is not a valid expression leading to a readable property.");
            }

            var stack = new Stack<string>();

            while (body != null)
            {
                stack.Push(body.Member.Name);
                body = body.Expression as MemberExpression;
            }

            var fullName = string.Join(".", stack.ToArray());

            return (propertyInfo, fullName);
        }
    }
}
