using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filter.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterExpressionProvider : IFilterExpressionProvider
    {
        private readonly StrainerOptions _options;

        public FilterExpressionProvider(IStrainerOptionsProvider optionsProvider)
        {
            _options = (optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider)))
                .GetStrainerOptions();
        }

        public Expression GetExpression(
            IPropertyMetadata metadata,
            IFilterTerm filterTerm,
            ParameterExpression parameterExpression,
            Expression innerExpression)
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

            Expression propertyValueExpresssion = parameterExpression;

            foreach (var part in metadata.Name.Split('.'))
            {
                propertyValueExpresssion = Expression.PropertyOrField(propertyValueExpresssion, part);
            }

            if (filterTerm.Values == null)
            {
                return null;
            }

            return CreateInnerExpression(
                metadata,
                filterTerm,
                innerExpression,
                propertyValueExpresssion);
        }

        private Expression CreateInnerExpression(
            IPropertyMetadata metadata,
            IFilterTerm filterTerm,
            Expression innerExpression,
            Expression propertyValue)
        {
            var typeConverter = GetTypeConverter(metadata);

            foreach (var filterTermValue in filterTerm.Values)
            {
                object constantVal = filterTermValue;

                if (filterTerm.Operator.IsStringBased)
                {
                    if (metadata.PropertyInfo.PropertyType != typeof(string))
                    {
                        propertyValue = ConvertToStringValue(propertyValue);
                    }
                }
                else
                {
                    if (typeConverter.CanConvertFrom(typeof(string)))
                    {
                        try
                        {
                            constantVal = typeConverter.ConvertFrom(filterTermValue);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    else
                    {
                        constantVal = Convert.ChangeType(filterTermValue, metadata.PropertyInfo.PropertyType);
                    }
                }

                var filterValue = GetClosureOverConstant(
                    constantVal,
                    filterTerm.Operator.IsStringBased
                        ? (typeof(string))
                        : metadata.PropertyInfo.PropertyType);

                if ((filterTerm.Operator.IsCaseInsensitive
                    || (!filterTerm.Operator.IsCaseInsensitive && _options.IsCaseInsensitiveForValues))
                    && metadata.PropertyInfo.PropertyType == typeof(string))
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

        private Expression ConvertToStringValue(Expression expressionToConvert)
        {
            return Expression.Call(expressionToConvert, typeof(object).GetMethod(nameof(object.ToString)));
        }

        // Workaround to ensure that the filter value gets passed as a parameter in generated SQL from EF Core
        // See https://github.com/aspnet/EntityFrameworkCore/issues/3361
        // Expression.Constant passed the target type to allow Nullable comparison
        // See http://bradwilson.typepad.com/blog/2008/07/creating-nullab.html
        private Expression GetClosureOverConstant<T>(T constant, Type targetType)
        {
            return Expression.Constant(constant, targetType);
        }

        private static TypeConverter GetTypeConverter(IPropertyMetadata metadata)
        {
            return TypeDescriptor.GetConverter(metadata.PropertyInfo.PropertyType);
        }
    }
}
