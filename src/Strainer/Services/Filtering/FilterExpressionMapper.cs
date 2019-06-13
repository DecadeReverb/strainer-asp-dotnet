using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterExpressionMapper : IFilterExpressionMapper
    {
        private readonly Dictionary<Type, Func<dynamic, dynamic, Expression>> _expressions;

        public FilterExpressionMapper()
        {
            _expressions = new Dictionary<Type, Func<dynamic, dynamic, Expression>>
            {
                // Equality operators
                {
                    typeof(EqualsOperator),
                    (filterValue, propertyValue) => Expression.Equal(propertyValue, filterValue)
                },
                {
                    typeof(EqualsCaseInsensitiveOperator),
                    (filterValue, propertyValue) => Expression.Equal(propertyValue, filterValue)
                },
                {
                    typeof(NotEqualsOperator),
                    (filterValue, propertyValue) => Expression.NotEqual(propertyValue, filterValue)
                },


                // Less/greater operators
                {
                    typeof(GreaterThanOperator),
                    (filterValue, propertyValue) => Expression.GreaterThan(propertyValue, filterValue)
                },
                {
                    typeof(GreaterThanOrEqualToOperator),
                    (filterValue, propertyValue) => Expression.GreaterThanOrEqual(propertyValue, filterValue)
                },
                {
                    typeof(LessThanOperator),
                    (filterValue, propertyValue) => Expression.LessThan(propertyValue, filterValue)
                },
                {
                    typeof(LessThanOrEqualToOperator),
                    (filterValue, propertyValue) => Expression.LessThanOrEqual(propertyValue, filterValue)
                },


                // Contains operators
                {
                    typeof(ContainsOperator),
                    (filterValue, propertyValue) => Expression.Call(
                        propertyValue,
                        typeof(string).GetMethod("Contains", new Type[]{ typeof(string) }),
                        filterValue)
                },
                {
                    typeof(ContainsCaseInsensitiveOperator),
                    (filterValue, propertyValue) => Expression.Call(
                        propertyValue,
                        typeof(string).GetMethod("Contains", new Type[]{ typeof(string) }),
                        filterValue)
                },
                {
                    typeof(DoesNotContainOperator),
                    (filterValue, propertyValue) => Expression.Call(
                        propertyValue,
                        typeof(string).GetMethod("Contains", new Type[]{ typeof(string) }),
                        filterValue)
                },
                {
                    typeof(DoesNotContainCaseInsensitiveOperator),
                    (filterValue, propertyValue) => Expression.Call(
                        propertyValue,
                        typeof(string).GetMethod("Contains", new Type[]{ typeof(string) }),
                        filterValue)
                },


                // Starts with operators
                {
                    typeof(StartsWithOperator),
                    (filterValue, propertyValue) => Expression.Call(
                        propertyValue,
                        typeof(string).GetMethod("StartsWith", new Type[]{ typeof(string) }),
                        filterValue)
                },
                {
                    typeof(StartsWithCaseInsensitiveOperator),
                    (filterValue, propertyValue) => Expression.Call(
                        propertyValue,
                        typeof(string).GetMethod("StartsWith", new Type[]{ typeof(string) }),
                        filterValue)
                },
                {
                    typeof(DoesNotStartWithOperator),
                    (filterValue, propertyValue) => Expression.Call(
                        propertyValue,
                        typeof(string).GetMethod("StartsWith", new Type[]{ typeof(string) }),
                        filterValue)
                },
                {
                    typeof(DoesNotStartWithCaseInsensitiveOperator),
                    (filterValue, propertyValue) => Expression.Call(
                        propertyValue,
                        typeof(string).GetMethod("StartsWith", new Type[]{ typeof(string) }),
                        filterValue)
                },
            };
        }

        public Expression GetDefaultExpression(dynamic filterValue, dynamic propertyValue)
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

        public Expression GetExpression(IFilterOperator filterOperator, dynamic filterValue, dynamic propertyValue)
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
                return _expressions[operatorType](filterValue, propertyValue);
            }
            else
            {
                return GetDefaultExpression(filterValue, propertyValue);
            }
        }
    }
}
