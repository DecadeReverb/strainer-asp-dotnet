using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Extensions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Filtering;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services
{
    public class StrainerProcessor : IStrainerProcessor
    {
        public StrainerProcessor(IStrainerContext context)
        {
            Context = context;

            MapFilterOperators(context.Filtering.OperatorMapper);
            Context.Filtering.OperatorValidator.Validate(Context.Filtering.OperatorMapper.Operators);

            MapProperties(context.Mapper);
        }

        /// <summary>
        /// Gets the <see cref="IStrainerContext"/>.
        /// </summary>
        protected IStrainerContext Context { get; }

        /// <summary>
        /// Apply filtering, sorting, and pagination parameters found in `model` to `source`
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model">An instance of IStrainerModel</param>
        /// <param name="source">Data source</param>
        /// <param name="dataForCustomMethods">Additional data that will be passed down to custom methods</param>
        /// <param name="applyFiltering">Should the data be filtered? Defaults to true.</param>
        /// <param name="applySorting">Should the data be sorted? Defaults to true.</param>
        /// <param name="applyPagination">Should the data be paginated? Defaults to true.</param>
        /// <returns>Returns a transformed version of `source`</returns>
        public IQueryable<TEntity> Apply<TEntity>(
            IStrainerModel model,
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
                    if (ex is StrainerException)
                    {
                        throw;
                    }

                    throw new StrainerException(ex.Message, ex);
                }
                else
                {
                    return result;
                }
            }
        }

        protected virtual IFilterOperatorMapper MapFilterOperators(IFilterOperatorMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return mapper;
        }

        protected virtual IStrainerPropertyMapper MapProperties(IStrainerPropertyMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return mapper;
        }

        private IQueryable<TEntity> ApplyFiltering<TEntity>(
            IStrainerModel model,
            IQueryable<TEntity> result,
            object[] dataForCustomMethods = null)
        {
            var parsedFilters = Context.Filtering.TermParser.GetParsedTerms(model.Filters);
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
                    var (fullName, property) = GetStrainerProperty<TEntity>(
                        isSortingRequired: false,
                        ifFileringRequired: true,
                        name: filterTermName);

                    if (property != null)
                    {
                        var converter = TypeDescriptor.GetConverter(property.PropertyType);
                        Expression propertyValue = parameterExpression;
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

                            var constantVal = converter.CanConvertFrom(typeof(string))
                                ? converter.ConvertFrom(filterTermValue)
                                : Convert.ChangeType(filterTermValue, property.PropertyType);

                            var filterValue = GetClosureOverConstant(constantVal, property.PropertyType);

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
                    }
                    else
                    {
                        result = ApplyCustomMethod(
                            result,
                            filterTermName,
                            Context.CustomMethods.FilterMethods,
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

        // Workaround to ensure that the filter value gets passed as a parameter in generated SQL from EF Core
        // See https://github.com/aspnet/EntityFrameworkCore/issues/3361
        // Expression.Constant passed the target type to allow Nullable comparison
        // See http://bradwilson.typepad.com/blog/2008/07/creating-nullab.html
        private Expression GetClosureOverConstant<T>(T constant, Type targetType)
        {
            return Expression.Constant(constant, targetType);
        }

        private IQueryable<TEntity> ApplySorting<TEntity>(
            IStrainerModel model,
            IQueryable<TEntity> result,
            object[] dataForCustomMethods = null)
        {
            var parsedTerms = Context.Sorting.TermParser.GetParsedTerms(model.Sorts);
            if (parsedTerms.Count == 0)
            {
                return result;
            }

            var useThenBy = false;
            foreach (var sortTerm in parsedTerms)
            {
                var (fullName, property) = GetStrainerProperty<TEntity>(
                    isSortingRequired: true,
                    ifFileringRequired: false,
                    name: sortTerm.Name);

                if (property != null)
                {
                    result = result.OrderByDynamic(fullName, property, sortTerm.IsDescending, useThenBy);
                }
                else
                {
                    result = ApplyCustomMethod(
                        result,
                        sortTerm.Name,
                        Context.CustomMethods.SortMethods,
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
            IStrainerModel model,
            IQueryable<TEntity> result)
        {
            var page = model?.Page ?? Context.Options.DefaultPageNumber;
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

        private (string, PropertyInfo) GetStrainerProperty<TEntity>(
            bool isSortingRequired,
            bool ifFileringRequired,
            string name)
        {
            var property = Context.Mapper.FindProperty<TEntity>(
                isSortingRequired,
                ifFileringRequired,
                name,
                Context.Options.CaseSensitive);

            if (property.Item1 == null)
            {
                var prop = FindPropertyByStrainerAttribute<TEntity>(
                    isSortingRequired,
                    ifFileringRequired,
                    name,
                    Context.Options.CaseSensitive);

                return (prop?.Name, prop);
            }

            return property;
        }

        private PropertyInfo FindPropertyByStrainerAttribute<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name,
            bool isCaseSensitive)
        {
            var stringComparisonMethod = isCaseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;
            var properties = typeof(TEntity).GetProperties();

            return Array.Find(properties, propertyInfo =>
            {
                var strainerAttribute = propertyInfo.GetCustomAttribute<StrainerAttribute>(inherit: true);

                return strainerAttribute != null
                    && (isSortingRequired ? strainerAttribute.IsSortable : true)
                    && (isFilteringRequired ? strainerAttribute.IsFilterable : true)
                    && ((strainerAttribute.DisplayName ?? propertyInfo.Name).Equals(name, stringComparisonMethod));
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

                    throw new StrainerIncompatibleMethodException(
                        name,
                        expected,
                        actual,
                            $"{name} failed. Expected a custom method for type " +
                            $"{expected} but only found for type {actual}");
                }
                else
                {
                    throw new StrainerMethodNotFoundException(name, $"{name} not found.");
                }
            }

            return result;
        }
    }
}
