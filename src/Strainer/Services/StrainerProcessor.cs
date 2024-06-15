﻿using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Pipelines;

namespace Fluorite.Strainer.Services;

/// <summary>
/// Default implementation of Strainer main service taking care of filtering,
/// sorting and pagination.
/// </summary>
public class StrainerProcessor : IStrainerProcessor
{
    private readonly IStrainerPipelineBuilderFactory _strainerPipelineBuilderFactory;
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="StrainerProcessor"/> class
    /// with specified context.
    /// </summary>
    /// <param name="strainerPipelineBuilderFactory"></param>
    /// <param name="strainerOptionsProvider"></param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="strainerPipelineBuilderFactory"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="strainerOptionsProvider"/> is <see langword="null"/>.
    /// </exception>
    public StrainerProcessor(
        IStrainerPipelineBuilderFactory strainerPipelineBuilderFactory,
        IStrainerOptionsProvider strainerOptionsProvider)
    {
        _strainerPipelineBuilderFactory = Guard.Against.Null(strainerPipelineBuilderFactory);
        _strainerOptionsProvider = Guard.Against.Null(strainerOptionsProvider);
    }

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
        Guard.Against.Null(model);
        Guard.Against.Null(source);

        var builder = _strainerPipelineBuilderFactory.CreateBuilder();

        if (applyFiltering)
        {
            builder.Filter();
        }

        if (applySorting)
        {
            builder.Sort();
        }

        if (applyPagination)
        {
            builder.Paginate();
        }

        var pipeline = builder.Build();

        return RunPipeline(model, source, pipeline);
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
        Guard.Against.Null(model);
        Guard.Against.Null(source);

        var pipeline = _strainerPipelineBuilderFactory
            .CreateBuilder()
            .Filter()
            .Build();

        return RunPipeline(model, source, pipeline);
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
        Guard.Against.Null(model);
        Guard.Against.Null(source);

        var pipeline = _strainerPipelineBuilderFactory
            .CreateBuilder()
            .Paginate()
            .Build();

        return RunPipeline(model, source, pipeline);
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
        Guard.Against.Null(model);
        Guard.Against.Null(source);

        var pipeline = _strainerPipelineBuilderFactory
            .CreateBuilder()
            .Sort()
            .Build();

        return RunPipeline(model, source, pipeline);
    }

    private IQueryable<TEntity> RunPipeline<TEntity>(
        IStrainerModel model,
        IQueryable<TEntity> source,
        IStrainerPipeline pipeline)
    {
        var options = _strainerOptionsProvider.GetStrainerOptions();

        try
        {
            return pipeline.Run(model, source);
        }
        catch (StrainerException) when (!options.ThrowExceptions)
        {
            return source;
        }
    }
}
