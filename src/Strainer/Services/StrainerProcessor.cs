using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Pipelines;
using System;
using System.Linq;

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

            var builder = new StrainerPipelineBuilder();

            try
            {
                // Filter
                if (applyFiltering)
                {
                    builder.Filter();
                }

                // Sort
                if (applySorting)
                {
                    builder.Sort();
                }

                // Paginate
                if (applyPagination)
                {
                    builder.Paginate();
                }

                return builder.Build().Run(model, source, Context);
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

            return new StrainerPipelineBuilder()
                .Filter()
                .Build()
                .Run(model, source, Context);
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

            return new StrainerPipelineBuilder()
                .Paginate()
                .Build()
                .Run(model, source, Context);
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

            return new StrainerPipelineBuilder()
                .Sort()
                .Build()
                .Run(model, source, Context);
        }
    }
}
