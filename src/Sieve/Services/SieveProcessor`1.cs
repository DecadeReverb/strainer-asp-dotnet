using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sieve.Attributes;
using Sieve.Exceptions;
using Sieve.Extensions;
using Sieve.Models;
using Sieve.Models.Filtering.Operators;

namespace Sieve.Services
{
    public class SieveProcessor<TSieveModel> : ISieveProcessor<TSieveModel>
           where TSieveModel : class, ISieveModel
    {
        public SieveProcessor(ISieveContext context)
        {
            Context = context;

            MapProperties(context.Mapper);
        }

        /// <summary>
        /// Gets the <see cref="ISieveContext"/>.
        /// </summary>
        protected ISieveContext Context { get; }

        /// <summary>
        /// Apply filtering, sorting, and pagination parameters found in `model` to `source`
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model">An instance of ISieveModel</param>
        /// <param name="source">Data source</param>
        /// <param name="dataForCustomMethods">Additional data that will be passed down to custom methods</param>
        /// <param name="applyFiltering">Should the data be filtered? Defaults to true.</param>
        /// <param name="applySorting">Should the data be sorted? Defaults to true.</param>
        /// <param name="applyPagination">Should the data be paginated? Defaults to true.</param>
        /// <returns>Returns a transformed version of `source`</returns>
        public IQueryable<TEntity> Apply<TEntity>(
            TSieveModel model,
            IQueryable<TEntity> source,
            object[] dataForCustomMethods = null,
            bool applyFiltering = true,
            bool applySorting = true,
            bool applyPagination = true)
        {
            var result = source;

            if (model == null)
            {
                return result;
            }

            try
            {
                // Filter
                if (applyFiltering)
                {
                    result = ApplyFiltering(model, result, dataForCustomMethods);
                }

                // Sort
                if (applySorting)
                {
                    result = ApplySorting(model, result, dataForCustomMethods);
                }

                // Paginate
                if (applyPagination)
                {
                    result = ApplyPagination(model, result);
                }

                return result;
            }
            catch (Exception ex)
            {
                if (Context.Options.ThrowExceptions)
                {
                    if (ex is SieveException)
                    {
                        throw;
                    }

                    throw new SieveException(ex.Message, ex);
                }
                else
                {
                    return result;
                }
            }
        }

        protected virtual ISievePropertyMapper MapProperties(ISievePropertyMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return mapper;
        }

        private IQueryable<TEntity> ApplyFiltering<TEntity>(
            TSieveModel model,
            IQueryable<TEntity> result,
            object[] dataForCustomMethods = null)
        {
            var parsedFilters = Context.FilterTermContext.Parser.GetParsedTerms(model.Filters);
            if (parsedFilters == null)
            {
                return result;
            }

            Expression outerExpression = null;
            var parameterExpression = Expression.Parameter(typeof(TEntity), "e");
            foreach (var filterTerm in parsedFilters)
            {
                Expression innerExpression = null;
                foreach (var filterTermName in filterTerm.Names)
                {
                    var (fullName, property) = GetSieveProperty<TEntity>(false, true, filterTermName);
                    if (property != null)
                    {
                        var converter = TypeDescriptor.GetConverter(property.PropertyType);

                        dynamic propertyValue = parameterExpression;
                        foreach (var part in fullName.Split('.'))
                        {
                            propertyValue = Expression.PropertyOrField(propertyValue, part);
                        }

                        if (filterTerm.Values == null)
                        {
                            continue;
                        }

                        foreach (var filterTermValue in filterTerm.Values)
                        {

                            dynamic constantVal = converter.CanConvertFrom(typeof(string))
                                ? converter.ConvertFrom(filterTermValue)
                                : Convert.ChangeType(filterTermValue, property.PropertyType);

                            var filterValue = GetClosureOverConstant(constantVal, property.PropertyType);

                            if (filterTerm.OperatorIsCaseInsensitive)
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

                            var expression = GetExpression(filterTerm, filterValue, propertyValue);

                            if (filterTerm.OperatorIsNegated)
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
                    }
                    else
                    {
                        result = ApplyCustomMethod(
                            result,
                            filterTermName,
                            Context.CustomMethodsContext.FilterMethods,
                            new object[]
                            {
                                result,
                                filterTerm.Operator,
                                filterTerm.Values
                            },
                            dataForCustomMethods);

                    }
                }

                if (outerExpression == null)
                {
                    outerExpression = innerExpression;
                    continue;
                }

                if (innerExpression == null)
                {
                    continue;
                }

                outerExpression = Expression.And(outerExpression, innerExpression);
            }

            return outerExpression == null
                ? result
                : result.Where(Expression.Lambda<Func<TEntity, bool>>(outerExpression, parameterExpression));
        }

        // TODO:
        // This method should be public and encapsulated by a dedicated
        // service, for example some kind of filter operator to expression
        // translator. It should be possible to add or remove translations,
        // or even supply custom filter operator to expression translator.
        private static Expression GetExpression(IFilterTerm filterTerm, dynamic filterValue, dynamic propertyValue)
        {
            switch (filterTerm.OperatorParsed)
            {
                case EqualsOperator equalsOperator:
                    return Expression.Equal(propertyValue, filterValue);
                case NotEqualsOperator notEqualsOperator:
                    return Expression.NotEqual(propertyValue, filterValue);
                case GreaterThanOperator greaterThanOperator:
                    return Expression.GreaterThan(propertyValue, filterValue);
                case LessThanOperator lessThanOperator:
                    return Expression.LessThan(propertyValue, filterValue);
                case GreaterThanOrEqualToOperator greaterThanOrEqualToOperator:
                    return Expression.GreaterThanOrEqual(propertyValue, filterValue);
                case LessThanOrEqualToOperator lessThanOrEqualToOperator:
                    return Expression.LessThanOrEqual(propertyValue, filterValue);
                case ContainsOperator containsOperator:
                    return Expression.Call(
                        propertyValue,
                        typeof(string).GetMethods().First(m => m.Name == "Contains" && m.GetParameters().Length == 1),
                        filterValue);
                case StartsWithOperator startsWithOperator:
                    return Expression.Call(
                        propertyValue,
                        typeof(string).GetMethods().First(m => m.Name == "StartsWith" && m.GetParameters().Length == 1),
                        filterValue);
                default:
                    // TODO:
                    // Call defined default filter operator, instead of calling
                    // hardcoded equal expression.
                    return Expression.Equal(propertyValue, filterValue);
            }
        }

        // Workaround to ensure that the filter value gets passed as a parameter in generated SQL from EF Core
        // See https://github.com/aspnet/EntityFrameworkCore/issues/3361
        // Expression.Constant passed the target type to allow Nullable comparison
        // See http://bradwilson.typepad.com/blog/2008/07/creating-nullab.html
        private Expression GetClosureOverConstant<T>(T constant, Type targetType)
        {
            return Expression.Constant(constant, targetType);
        }

        private IQueryable<TEntity> ApplySorting<TEntity>(
            TSieveModel model,
            IQueryable<TEntity> result,
            object[] dataForCustomMethods = null)
        {
            var parsedTerms = Context.SortingContext.TermParser.GetParsedTerms(model.Sorts);
            if (parsedTerms.Count == 0)
            {
                return result;
            }

            var useThenBy = false;
            foreach (var sortTerm in parsedTerms)
            {
                var (fullName, property) = GetSieveProperty<TEntity>(true, false, sortTerm.Name);

                if (property != null)
                {
                    result = result.OrderByDynamic(fullName, property, sortTerm.IsDescending, useThenBy);
                }
                else
                {
                    result = ApplyCustomMethod(
                        result,
                        sortTerm.Name,
                        Context.CustomMethodsContext.SortMethods,
                        new object[]
                        {
                            result,
                            useThenBy,
                            sortTerm.IsDescending
                        },
                        dataForCustomMethods);
                }
                useThenBy = true;
            }

            return result;
        }

        private IQueryable<TEntity> ApplyPagination<TEntity>(
            TSieveModel model,
            IQueryable<TEntity> result)
        {
            var page = model?.Page ?? 1;
            var pageSize = model?.PageSize ?? Context.Options.DefaultPageSize;
            var maxPageSize = Context.Options.MaxPageSize > 0
                ? Context.Options.MaxPageSize
                : pageSize;

            result = result.Skip((page - 1) * pageSize);

            if (pageSize > 0)
            {
                result = result.Take(Math.Min(pageSize, maxPageSize));
            }

            return result;
        }

        private (string, PropertyInfo) GetSieveProperty<TEntity>(
            bool canSortRequired,
            bool canFilterRequired,
            string name)
        {
            var property = Context.Mapper.FindProperty<TEntity>(
                canSortRequired,
                canFilterRequired,
                name,
                Context.Options.CaseSensitive);

            if (property.Item1 == null)
            {
                var prop = FindPropertyBySieveAttribute<TEntity>(
                    canSortRequired,
                    canFilterRequired,
                    name,
                    Context.Options.CaseSensitive);

                return (prop?.Name, prop);
            }

            return property;

        }

        private PropertyInfo FindPropertyBySieveAttribute<TEntity>(
            bool canSortRequired,
            bool canFilterRequired,
            string name,
            bool isCaseSensitive)
        {
            return Array.Find(typeof(TEntity).GetProperties(), p =>
            {
                return p.GetCustomAttribute(typeof(SieveAttribute)) is SieveAttribute sieveAttribute
                && (canSortRequired ? sieveAttribute.CanSort : true)
                && (canFilterRequired ? sieveAttribute.CanFilter : true)
                && ((sieveAttribute.Name ?? p.Name).Equals(
                        name,
                        isCaseSensitive
                            ? StringComparison.Ordinal
                            : StringComparison.OrdinalIgnoreCase
                        )
                    );
            });
        }

        private IQueryable<TEntity> ApplyCustomMethod<TEntity>(
            IQueryable<TEntity> result,
            string name,
            object parent,
            object[] parameters,
            object[] optionalParameters = null)
        {
            var customMethod = parent?.GetType()
                .GetMethodExt(
                    name,
                    Context.Options.CaseSensitive
                        ? BindingFlags.Default
                        : BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance,
                    typeof(IQueryable<TEntity>));

            if (customMethod != null)
            {
                try
                {
                    result = customMethod.Invoke(parent, parameters) as IQueryable<TEntity>;
                }
                catch (TargetParameterCountException)
                {
                    if (optionalParameters != null)
                    {
                        result = customMethod.Invoke(parent, parameters.Concat(optionalParameters).ToArray())
                            as IQueryable<TEntity>;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                var incompatibleCustomMethod = parent?.GetType()
                    .GetMethod(
                        name,
                        Context.Options.CaseSensitive
                            ? BindingFlags.Default
                            : BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (incompatibleCustomMethod != null)
                {
                    var expected = typeof(IQueryable<TEntity>);
                    var actual = incompatibleCustomMethod.ReturnType;
                    throw new SieveIncompatibleMethodException(name, expected, actual,
                        $"{name} failed. Expected a custom method for type {expected} but only found for type {actual}");
                }
                else
                {
                    throw new SieveMethodNotFoundException(name, $"{name} not found.");
                }
            }

            return result;
        }
    }
}
