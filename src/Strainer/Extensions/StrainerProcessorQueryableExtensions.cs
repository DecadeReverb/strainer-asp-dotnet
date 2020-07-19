using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using System;
using System.Linq;

namespace Fluorite.Extensions
{
    /// <summary>
    /// Provides extension methods for appling Strainer processing to <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class StrainerProcessorQueryableExtensions
    {
        /// <summary>
        /// Processes current <see cref="IQueryable{T}"/> (applies filtering,
        /// sorting and pagination) using provided processor and model data.
        /// </summary>
        /// <typeparam name="T">
        /// The type of entity being processed.
        /// </typeparam>
        /// <param name="source">
        /// The current <see cref="IQueryable{T}"/> instance.
        /// </param>
        /// <param name="strainerModel">
        /// The <see cref="IStrainerModel"/> containing i.a. filtering data.
        /// </param>
        /// <param name="strainerProcessor">
        /// The see <see cref="IStrainerProcessor"/> which will apply filtering.
        /// </param>
        /// <param name="applyFiltering">
        /// A <see cref="bool"/> value indicating whether provided
        /// <see cref="IQueryable{T}"/> should be filtered.
        /// </param>
        /// <param name="applySorting">
        /// A <see cref="bool"/> value indicating whether provided
        /// <see cref="IQueryable{T}"/> should be ordered.
        /// </param>
        /// <param name="applyPagination">
        /// A <see cref="bool"/> value indicating whether provided
        /// <see cref="IQueryable{T}"/> should be paginated.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IQueryable{T}"/> with filtering applied.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="strainerProcessor"/> is <see langword="null"/>.
        /// </exception>
        public static IQueryable<T> Apply<T>(
            this IQueryable<T> source,
            IStrainerModel strainerModel,
            IStrainerProcessor strainerProcessor,
            bool applyFiltering = true,
            bool applySorting = true,
            bool applyPagination = true)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (strainerProcessor is null)
            {
                throw new ArgumentNullException(nameof(strainerProcessor));
            }

            if (strainerModel is null)
            {
                return source;
            }

            source = strainerProcessor.Apply(strainerModel, source, applyFiltering, applySorting, applyPagination);

            return source;
        }

        /// <summary>
        /// Applies filtering to current <see cref="IQueryable{T}"/> using
        /// provided processor and model data.
        /// </summary>
        /// <typeparam name="T">
        /// The type of entity being processed.
        /// </typeparam>
        /// <param name="source">
        /// The current <see cref="IQueryable{T}"/> instance.
        /// </param>
        /// <param name="strainerModel">
        /// The <see cref="IStrainerModel"/> containing i.a. filtering data.
        /// </param>
        /// <param name="strainerProcessor">
        /// The see <see cref="IStrainerProcessor"/> which will apply filtering.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IQueryable{T}"/> with filtering applied.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="strainerProcessor"/> is <see langword="null"/>.
        /// </exception>
        public static IQueryable<T> ApplyFiltering<T>(
            this IQueryable<T> source,
            IStrainerModel strainerModel,
            IStrainerProcessor strainerProcessor)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (strainerProcessor is null)
            {
                throw new ArgumentNullException(nameof(strainerProcessor));
            }

            if (strainerModel is null)
            {
                return source;
            }

            return strainerProcessor.ApplyFiltering(strainerModel, source);
        }

        /// <summary>
        /// Applies pagination to current <see cref="IQueryable{T}"/> using
        /// provided processor and model data.
        /// </summary>
        /// <typeparam name="T">
        /// The type of entity being processed.
        /// </typeparam>
        /// <param name="source">
        /// The current <see cref="IQueryable{T}"/> instance.
        /// </param>
        /// <param name="strainerModel">
        /// The <see cref="IStrainerModel"/> containing i.a. pagination data.
        /// </param>
        /// <param name="strainerProcessor">
        /// The see <see cref="IStrainerProcessor"/> which will apply pagination.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IQueryable{T}"/> with pagination applied.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="strainerProcessor"/> is <see langword="null"/>.
        /// </exception>
        public static IQueryable<T> ApplyPagination<T>(
            this IQueryable<T> source,
            IStrainerModel strainerModel,
            IStrainerProcessor strainerProcessor)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (strainerProcessor is null)
            {
                throw new ArgumentNullException(nameof(strainerProcessor));
            }

            if (strainerModel is null)
            {
                return source;
            }

            return strainerProcessor.ApplyPagination(strainerModel, source);
        }

        /// <summary>
        /// Applies sorting to current <see cref="IQueryable{T}"/> using
        /// provided processor and model data.
        /// </summary>
        /// <typeparam name="T">
        /// The type of entity being processed.
        /// </typeparam>
        /// <param name="source">
        /// The current <see cref="IQueryable{T}"/> instance.
        /// </param>
        /// <param name="strainerModel">
        /// The <see cref="IStrainerModel"/> containing i.a. sorting.
        /// </param>
        /// <param name="strainerProcessor">
        /// The see <see cref="IStrainerProcessor"/> which will apply sorting.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IQueryable{T}"/> with sorting applied.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="strainerProcessor"/> is <see langword="null"/>.
        /// </exception>
        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> source,
            IStrainerModel strainerModel,
            IStrainerProcessor strainerProcessor)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (strainerProcessor is null)
            {
                throw new ArgumentNullException(nameof(strainerProcessor));
            }

            if (strainerModel is null)
            {
                return source;
            }

            return strainerProcessor.ApplySorting(strainerModel, source);
        }
    }
}
