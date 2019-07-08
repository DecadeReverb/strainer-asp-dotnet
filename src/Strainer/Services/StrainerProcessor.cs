using Fluorite.Extensions;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Filtering;
using System;
using System.Linq;
using System.Linq.Expressions;

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
            bool applyFiltering = true,
            bool applySorting = true,
            bool applyPagination = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            try
            {
                // Filter
                if (applyFiltering)
                {
                    source = ApplyFiltering(model, source);
                }

                // Sort
                if (applySorting)
                {
                    source = ApplySorting(model, source);
                }

                // Paginate
                if (applyPagination)
                {
                    source = ApplyPagination(model, source);
                }

                return source;
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
                    return source;
                }
            }
        }

        public IQueryable<TEntity> ApplyFiltering<TEntity>(
            IStrainerModel model,
            IQueryable<TEntity> source)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var parsedTerms = Context.Filtering.TermParser.GetParsedTerms(model.Filters);
            if (parsedTerms == null)
            {
                return source;
            }

            Expression outerExpression = null;
            var parameterExpression = Expression.Parameter(typeof(TEntity), "e");
            foreach (var filterTerm in parsedTerms)
            {
                Expression innerExpression = null;
                foreach (var filterTermName in filterTerm.Names)
                {
                    var metadata = GetPropertyMetadata<TEntity>(
                        isSortingRequired: false,
                        isFilteringRequired: true,
                        name: filterTermName);

                    if (metadata != null)
                    {
                        innerExpression = Context.Filtering.ExpressionProvider.GetExpression(metadata, filterTerm, parameterExpression, innerExpression);
                    }
                    else
                    {
                        var customMethod = Context.CustomMethods.Filter?.Mapper.GetMethod<TEntity>(filterTermName);
                        if (customMethod != null)
                        {
                            var context = new CustomFilterMethodContext<TEntity>
                            {
                                Source = source,
                                Term = filterTerm,
                            };
                            source = customMethod.Function(context);
                        }
                        else
                        {
                            throw new StrainerMethodNotFoundException(
                                filterTermName,
                                $"{filterTermName} not found.");
                        }
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
                ? source
                : source.Where(Expression.Lambda<Func<TEntity, bool>>(outerExpression, parameterExpression));
        }

        public IQueryable<TEntity> ApplyPagination<TEntity>(
            IStrainerModel model,
            IQueryable<TEntity> source)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var page = model?.Page ?? Context.Options.DefaultPageNumber;
            var pageSize = model?.PageSize ?? Context.Options.DefaultPageSize;
            var maxPageSize = Context.Options.MaxPageSize > 0
                ? Context.Options.MaxPageSize
                : pageSize;

            source = source.Skip((page - 1) * pageSize);

            if (pageSize > 0)
            {
                source = source.Take(Math.Min(pageSize, maxPageSize));
            }

            return source;
        }

        public IQueryable<TEntity> ApplySorting<TEntity>(
            IStrainerModel model,
            IQueryable<TEntity> source)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var parsedTerms = Context.Sorting.TermParser.GetParsedTerms(model.Sorts);
            if (parsedTerms.Count == 0)
            {
                return source;
            }

            var isSubsequent = false;
            foreach (var sortTerm in parsedTerms)
            {
                var metadata = GetPropertyMetadata<TEntity>(
                    isSortingRequired: true,
                    isFilteringRequired: false,
                    name: sortTerm.Name);

                if (metadata != null)
                {
                    var sortExpression = Context.Sorting.ExpressionProvider.GetExpression<TEntity>(metadata.PropertyInfo, sortTerm, isSubsequent);
                    if (sortExpression != null)
                    {
                        source = source.OrderWithSortExpression(sortExpression);
                    }
                }
                else
                {
                    var customMethod = Context.CustomMethods.Sort?.Mapper.GetMethod<TEntity>(sortTerm.Name);
                    if (customMethod != null)
                    {
                        var context = new CustomSortMethodContext<TEntity>
                        {
                            IsDescending = sortTerm.IsDescending,
                            IsSubsequent = isSubsequent,
                            Source = source,
                            Term = sortTerm,
                        };
                        source = customMethod.Function(context);
                    }
                    else
                    {
                        throw new StrainerMethodNotFoundException(
                            sortTerm.Name,
                            $"{sortTerm.Name} not found.");
                    }
                }

                isSubsequent = true;
            }

            return source;
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

        // Workaround to ensure that the filter value gets passed as a parameter in generated SQL from EF Core
        // See https://github.com/aspnet/EntityFrameworkCore/issues/3361
        // Expression.Constant passed the target type to allow Nullable comparison
        // See http://bradwilson.typepad.com/blog/2008/07/creating-nullab.html
        private Expression GetClosureOverConstant<T>(T constant, Type targetType)
        {
            return Expression.Constant(constant, targetType);
        }

        private IStrainerPropertyMetadata GetPropertyMetadata<TEntity>(bool isSortingRequired, bool isFilteringRequired, string name)
        {
            var metadata = Context.Mapper.FindProperty<TEntity>(
                isSortingRequired,
                isFilteringRequired,
                name);

            if (metadata == null)
            {
                return Context.MetadataProvider.GetMetadataFromAttribute<TEntity>(isSortingRequired, isFilteringRequired, name);
            }

            return metadata;
        }
    }
}
