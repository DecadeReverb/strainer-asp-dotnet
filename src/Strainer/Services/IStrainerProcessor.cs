using System.Linq;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    /// <summary>
    /// Strainer main service taking care of filtering, sorting and pagination.
    /// </summary>
    public interface IStrainerProcessor
    {
        /// <summary>
        /// Applies filtering, sorting and pagination (in that order)
        /// to source collection based on parameters found in provided model.
        /// <para/>
        /// Can throw <see cref="Exceptions.StrainerException"/>
        /// if <see cref="StrainerOptions.ThrowExceptions"/> is set to
        /// <see langword="true"/>.
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
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="model"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        IQueryable<TEntity> Apply<TEntity>(
            IStrainerModel model,
            IQueryable<TEntity> source,
            bool applyFiltering = true,
            bool applySorting = true,
            bool applyPagination = true);

        /// <summary>
        /// Applies filtering to source collection based on parameters
        /// found in provided model.
        /// <para/>
        /// Can throw <see cref="Exceptions.StrainerException"/>
        /// if <see cref="StrainerOptions.ThrowExceptions"/> is set to
        /// <see langword="true"/>.
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
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="model"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        IQueryable<TEntity> ApplyFiltering<TEntity>(IStrainerModel model, IQueryable<TEntity> source);

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
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="model"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        IQueryable<TEntity> ApplyPagination<TEntity>(IStrainerModel model, IQueryable<TEntity> source);

        /// <summary>
        /// Applies sorting to source collection based on parameters
        /// found in provided model.
        /// <para/>
        /// Can throw <see cref="Exceptions.StrainerException"/>
        /// if <see cref="StrainerOptions.ThrowExceptions"/> is set to
        /// <see langword="true"/>.
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
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="model"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        IQueryable<TEntity> ApplySorting<TEntity>(IStrainerModel model, IQueryable<TEntity> source);
    }
}
