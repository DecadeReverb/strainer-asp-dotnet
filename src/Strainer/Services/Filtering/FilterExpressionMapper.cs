using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterExpressionMapper : IFilterExpressionMapper
    {
        private readonly Dictionary<Type, Func<IFilterExpressionContext, Expression>> _expressions;

        public FilterExpressionMapper()
        {
            _expressions = new Dictionary<Type, Func<IFilterExpressionContext, Expression>>
            {
                // Equality operators
                {
                    typeof(EqualsOperator),
                    (context) => Expression.Equal(context.PropertyValue, context.FilterValue)
                },
                {
                    typeof(EqualsCaseInsensitiveOperator),
                    (context) => Expression.Equal(context.PropertyValue, context.FilterValue)
                },
                {
                    typeof(NotEqualsOperator),
                    (context) => Expression.NotEqual(context.PropertyValue, context.FilterValue)
                },


                // Less/greater operators
                {
                    typeof(GreaterThanOperator),
                    (context) => Expression.GreaterThan(context.PropertyValue, context.FilterValue)
                },
                {
                    typeof(GreaterThanOrEqualToOperator),
                    (context) => Expression.GreaterThanOrEqual(context.PropertyValue, context.FilterValue)
                },
                {
                    typeof(LessThanOperator),
                    (context) => Expression.LessThan(context.PropertyValue, context.FilterValue)
                },
                {
                    typeof(LessThanOrEqualToOperator),
                    (context) => Expression.LessThanOrEqual(context.PropertyValue, context.FilterValue)
                },


                // Contains operators
                {
                    typeof(ContainsOperator),
                    (context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.Contains), new Type[]{ typeof(string) }),
                        context.FilterValue)
                },
                {
                    typeof(ContainsCaseInsensitiveOperator),
                    (context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.Contains), new Type[]{ typeof(string) }),
                        context.FilterValue)
                },
                {
                    typeof(DoesNotContainOperator),
                    (context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.Contains), new Type[]{ typeof(string) }),
                        context.FilterValue)
                },
                {
                    typeof(DoesNotContainCaseInsensitiveOperator),
                    (context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.Contains), new Type[]{ typeof(string) }),
                        context.FilterValue)
                },


                // Starts with operators
                {
                    typeof(StartsWithOperator),
                    (context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.StartsWith), new Type[]{ typeof(string) }),
                        context.FilterValue)
                },
                {
                    typeof(StartsWithCaseInsensitiveOperator),
                    (context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.StartsWith), new Type[]{ typeof(string) }),
                        context.FilterValue)
                },
                {
                    typeof(DoesNotStartWithOperator),
                    (context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.StartsWith), new Type[]{ typeof(string) }),
                        context.FilterValue)
                },
                {
                    typeof(DoesNotStartWithCaseInsensitiveOperator),
                    (context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.StartsWith), new Type[]{ typeof(string) }),
                        context.FilterValue)
                },
            };
        }

        public Expression GetDefaultExpression(Expression filterValue, Expression propertyValue)
        {
            if (filterValue == null)
            {
                throw new ArgumentNullException(nameof(filterValue));
            }

            if (propertyValue == null)
            {
                throw new ArgumentNullException(nameof(propertyValue));
            }

            return Expression.Equal(propertyValue, filterValue);
        }

        public Expression GetExpression(IFilterOperator filterOperator, Expression filterValue, Expression propertyValue)
        {
            if (filterOperator == null)
            {
                throw new ArgumentNullException(nameof(filterOperator));
            }

            if (filterValue == null)
            {
                throw new ArgumentNullException(nameof(filterValue));
            }

            if (propertyValue == null)
            {
                throw new ArgumentNullException(nameof(propertyValue));
            }

            var operatorType = filterOperator.GetType();
            if (_expressions.ContainsKey(operatorType))
            {
                return _expressions[operatorType](new FilterExpressionContext(filterValue, propertyValue));
            }
            else
            {
                return GetDefaultExpression(filterValue, propertyValue);
            }
        }
    }
}
