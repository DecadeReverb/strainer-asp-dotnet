using Fluorite.Extensions;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services
{
    /// <summary>
    /// Default implementation of Strainer main service taking care of filtering,
    /// sorting and pagination.
    /// </summary>
    public class StrainerProcessor : IStrainerProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrainerProcessor"/> class
        /// with specified context.
        /// </summary>
        /// <param name="context">
        /// The Strainer context.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context"/> is <see langword="null"/>.
        /// </exception>
        public StrainerProcessor(IStrainerContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            // TODO:
            // Move sort expression validation to service injection.
            //var properties = Context.Mapper.GetAllMetadata();

            //foreach (var type in properties.Keys)
            //{
            //    dynamic sortingExpressions = properties.Select(pair => pair.Key == type);
            //    //Context.Sorting.ExpressionValidator.Validate(sortingExpressions);
            //}
        }

        /// <summary>
        /// Gets the <see cref="IStrainerContext"/>.
        /// </summary>
        protected IStrainerContext Context { get; }

        /// <summary>
        /// Applies filtering, sorting and pagination (in that order)
        /// to source collection based on parameters found in provided model.
        /// <para/>
        /// Can throw <see cref="StrainerException"/> if <see cref="StrainerOptions.ThrowExceptions"/>
        /// is set to <see langword="true"/>.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity stored in source collection.
        /// </typeparam>
        /// <param name="model">
        /// The model containing filtering, sorting and pagination parameters.
        /// </param>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <param name="applyFiltering">
        /// A <see cref="bool"/> value indicating whether source collection
        /// should be filtered.
        /// <para/>
        /// Defaults to <see langword="true"/>.
        /// </param>
        /// <param name="applySorting">
        /// A <see cref="bool"/> value indicating whether source collection
        /// should be sorted.
        /// <para/>
        /// Defaults to <see langword="true"/>.
        /// </param>
        /// <param name="applyPagination">
        /// A <see cref="bool"/> value indicating whether source collection
        /// should be paginated.
        /// <para/>
        /// Defaults to <see langword="true"/>.
        /// </param>
        /// <returns>
        /// A transformed version of source collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
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
            catch (StrainerException) when (!Context.Options.ThrowExceptions)
            {
                return source;
            }
        }

        /// <summary>
        /// Applies filtering to source collection based on parameters
        /// found in provided model.
        /// <para/>
        /// Can throw <see cref="StrainerException"/> if <see cref="StrainerOptions.ThrowExceptions"/>
        /// is set to <see langword="true"/>.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity stored in source collection.
        /// </typeparam>
        /// <param name="model">
        /// The model containing filtering, sorting and pagination parameters.
        /// </param>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <returns>
        /// A filtered version of source collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
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

            var parsedTerms = Context.Filter.TermParser.GetParsedTerms(model.Filters);
            if (parsedTerms.Count == 0)
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
                    var metadata = Context.Metadata.GetMetadata<TEntity>(
                        isSortableRequired: false,
                        isFilterableRequired: true,
                        name: filterTermName);

                    try
                    {
                        if (metadata != null)
                        {
                            innerExpression = Context.Filter.ExpressionProvider.GetExpression(metadata, filterTerm, parameterExpression, innerExpression);
                        }
                        else
                        {
                            if (Context.CustomMethods.GetCustomFilterMethods().TryGetValue(typeof(TEntity), out var customFilterMethods)
                                && customFilterMethods.TryGetValue(filterTermName, out var customMethod))
                            {
                                source = (customMethod as ICustomFilterMethod<TEntity>).Function(source, filterTerm.Operator?.Symbol);
                            }
                            else
                            {
                                throw new StrainerMethodNotFoundException(
                                    filterTermName,
                                    $"Property or custom filter method '{filterTermName}' was not found.");
                            }
                        }
                    }
                    catch (StrainerException) when (!Context.Options.ThrowExceptions)
                    {
                        return source;
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

            if (outerExpression == null)
            {
                return source;
            }

            return source.Where(Expression.Lambda<Func<TEntity, bool>>(outerExpression, parameterExpression));
        }

        /// <summary>
        /// Applies pagination to source collection based on parameters
        /// found in provided model.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity stored in source collection.
        /// </typeparam>
        /// <param name="model">
        /// The model containing filtering, sorting and pagination parameters.
        /// </param>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <returns>
        /// A paginated version of source collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
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

            var page = model.Page ?? Context.Options.DefaultPageNumber;
            var pageSize = model.PageSize ?? Context.Options.DefaultPageSize;
            var maxPageSize = Context.Options.MaxPageSize > 0
                ? Context.Options.MaxPageSize
                : pageSize;

            if (page > 1)
            {
                source = source.Skip((page - 1) * pageSize);
            }

            if (pageSize > 0)
            {
                source = source.Take(Math.Min(pageSize, maxPageSize));
            }

            return source;
        }

        /// <summary>
        /// Applies sorting to source collection based on parameters
        /// found in provided model.
        /// <para/>
        /// Can throw <see cref="StrainerException"/> if <see cref="StrainerOptions.ThrowExceptions"/>
        /// is set to <see langword="true"/>.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity stored in source collection.
        /// </typeparam>
        /// <param name="model">
        /// The model containing filtering, sorting and pagination parameters.
        /// </param>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <returns>
        /// A sorted version of source collection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
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
            var isSubsequent = false;
            var sortingPerformed = false;

            foreach (var sortTerm in parsedTerms)
            {
                var metadata = Context.Metadata.GetMetadata<TEntity>(
                    isSortableRequired: true,
                    isFilterableRequired: false,
                    name: sortTerm.Name);

                if (metadata != null)
                {
                    var sortExpression = Context.Sorting.ExpressionProvider.GetExpression<TEntity>(metadata.PropertyInfo, sortTerm, isSubsequent);
                    if (sortExpression != null)
                    {
                        source = source.OrderWithSortExpression(sortExpression);
                        sortingPerformed = true;
                    }
                }
                else
                {
                    try
                    {
                        if (Context.CustomMethods.GetCustomSortMethods().TryGetValue(typeof(TEntity), out var customSortMethods)
                            && customSortMethods.TryGetValue(sortTerm.Name, out var customMethod))
                        {
                            source = (customMethod as ICustomSortMethod<TEntity>).Function(source, sortTerm.IsDescending, isSubsequent);
                            sortingPerformed = true;
                        }
                        else
                        {
                            throw new StrainerMethodNotFoundException(
                                sortTerm.Name,
                                $"Property or custom sorting method '{sortTerm.Name}' was not found.");
                        }
                    }
                    catch (StrainerException) when (!Context.Options.ThrowExceptions)
                    {
                        return source;
                    }
                }

                isSubsequent = true;
            }

            if (!sortingPerformed)
            {
                var defaultSortExpression = Context.Sorting.ExpressionProvider.GetDefaultExpression<TEntity>();
                if (defaultSortExpression != null)
                {
                    source = source.OrderWithSortExpression(defaultSortExpression);
                }
            }

            return source;
        }
    }
}
