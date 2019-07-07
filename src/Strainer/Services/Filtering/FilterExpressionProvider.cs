using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterExpressionProvider : IFilterExpressionProvider
    {
        public FilterExpressionProvider()
        {

        }

        public Expression GetExpression(IStrainerPropertyMetadata metadata, IFilterTerm filterTerm, ParameterExpression parameterExpression, Expression innerExpression)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (filterTerm == null)
            {
                throw new ArgumentNullException(nameof(filterTerm));
            }

            if (parameterExpression == null)
            {
                throw new ArgumentNullException(nameof(parameterExpression));
            }

            var converter = TypeDescriptor.GetConverter(metadata.PropertyInfo.PropertyType);
            Expression propertyValue = parameterExpression;
            foreach (var part in metadata.Name.Split('.'))
            {
                propertyValue = Expression.PropertyOrField(propertyValue, part);
            }

            if (filterTerm.Values == null)
            {
                return null;
            }

            foreach (var filterTermValue in filterTerm.Values)
            {
                object constantVal = null;
                if (converter.CanConvertFrom(typeof(string)))
                {
                    constantVal = converter.ConvertFrom(filterTermValue);
                }
                else
                {
                    constantVal = Convert.ChangeType(filterTermValue, metadata.PropertyInfo.PropertyType);
                }

                var filterValue = GetClosureOverConstant(constantVal, metadata.PropertyInfo.PropertyType);

                if (filterTerm.Operator.IsCaseInsensitive)
                {
                    propertyValue = Expression.Call(
                        propertyValue,
                        typeof(string).GetMethods()
                            .First(m => m.Name == "ToUpper" && m.GetParameters().Length == 0));

                    filterValue = Expression.Call(
                        filterValue,
                        typeof(string).GetMethods()
                            .First(m => m.Name == "ToUpper" && m.GetParameters().Length == 0));
                }

                var filterOperatorContext = new FilterExpressionContext(filterValue, propertyValue);
                var expression = filterTerm.Operator.Expression(filterOperatorContext);

                if (filterTerm.Operator.NegateExpression)
                {
                    expression = Expression.Not(expression);
                }

                if (innerExpression == null)
                {
                    innerExpression = expression;
                }
                else
                {
                    innerExpression = Expression.Or(innerExpression, expression);
                }
            }

            return innerExpression;
        }

        // Workaround to ensure that the filter value gets passed as a parameter in generated SQL from EF Core
        // See https://github.com/aspnet/EntityFrameworkCore/issues/3361
        // Expression.Constant passed the target type to allow Nullable comparison
        // See http://bradwilson.typepad.com/blog/2008/07/creating-nullab.html
        private Expression GetClosureOverConstant<T>(T constant, Type targetType)
        {
            return Expression.Constant(constant, targetType);
        }
    }
}
