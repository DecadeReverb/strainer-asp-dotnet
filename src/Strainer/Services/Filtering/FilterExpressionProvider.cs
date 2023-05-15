using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterExpressionProvider : IFilterExpressionProvider
    {
        private readonly IStrainerOptionsProvider _strainerOptionsProvider;
        private readonly ITypeConverterProvider _typeConverterProvider;

        public FilterExpressionProvider(
            IStrainerOptionsProvider strainerOptionsProvider,
            ITypeConverterProvider typeConverterProvider)
        {
            _strainerOptionsProvider = strainerOptionsProvider;
            _typeConverterProvider = typeConverterProvider;
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

            if (filterTerm.Values == null)
            {
                return null;
            }

            Expression propertyValueExpresssion = parameterExpression;

            foreach (var part in metadata.Name.Split('.'))
            {
                propertyValueExpresssion = Expression.PropertyOrField(propertyValueExpresssion, part);
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
            var typeConverter = _typeConverterProvider.GetTypeConverter(metadata.PropertyInfo.PropertyType);

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
                    if (typeConverter.CanConvertFrom(typeof(string))
                        && typeof(string) != metadata.PropertyInfo.PropertyType)
                    {
                        try
                        {
                            constantVal = typeConverter.ConvertFrom(filterTermValue);
                        }
                        catch (Exception ex)
                        {
                            throw new StrainerConversionException(
                                $"Failed to convert filter value '{filterTermValue}' " +
                                $"to type '{metadata.PropertyInfo.PropertyType.FullName}'.",
                                ex,
                                filterTermValue,
                                metadata.PropertyInfo.PropertyType);
                        }
                    }
                    else
                    {
                        try
                        {
                            constantVal = Convert.ChangeType(filterTermValue, metadata.PropertyInfo.PropertyType);
                        }
                        catch (Exception ex)
                        {
                            throw new StrainerConversionException(
                                $"Failed to change type of filter value '{filterTermValue}' " +
                                $"to type '{metadata.PropertyInfo.PropertyType.FullName}'.",
                                ex,
                                filterTermValue,
                                metadata.PropertyInfo.PropertyType);
                        }
                    }
                }

                var constantClosureType = filterTerm.Operator.IsStringBased
                    ? typeof(string)
                    : metadata.PropertyInfo.PropertyType;
                var filterValue = GetClosureOverConstant(
                    constantVal,
                    constantClosureType);

                var options = _strainerOptionsProvider.GetStrainerOptions();

                if ((filterTerm.Operator.IsCaseInsensitive
                    || (!filterTerm.Operator.IsCaseInsensitive && options.IsCaseInsensitiveForValues))
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

                Expression expression = null;

                try
                {
                    expression = filterTerm.Operator.Expression(filterOperatorContext);
                }
                catch (Exception ex)
                {
                    throw new StrainerUnsupportedOperatorException(
                        $"Failed to use operator '{filterTerm.Operator}' " +
                        $"for filter value '{filterTermValue}' on property " +
                        $"'{metadata.PropertyInfo.DeclaringType.FullName}.{metadata.PropertyInfo.Name}' " +
                        $"of type '{metadata.PropertyInfo.PropertyType.FullName}'\n." +
                        "Please ensure that this operator is supported by type " +
                        $"'{metadata.PropertyInfo.PropertyType.FullName}'.",
                        ex,
                        filterTerm.Operator,
                        metadata.PropertyInfo,
                        filterTermValue);
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
    }
}
