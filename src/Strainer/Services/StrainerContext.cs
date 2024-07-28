using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Pipelines;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services;

/// <summary>
/// Represents wrapper over main services used by Strainer.
/// </summary>
public class StrainerContext : IStrainerContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StrainerContext"/> class.
    /// </summary>
    public StrainerContext(
        IConfigurationCustomMethodsProvider customMethodsConfigurationProvider,
        IStrainerOptionsProvider optionsProvider,
        IFilterContext filteringContext,
        ISortingContext sortingContext,
        IMetadataFacade metadataProvidersFacade,
        IPipelineContext pipelineContext)
    {
        CustomMethods = Guard.Against.Null(customMethodsConfigurationProvider);
        Filter = Guard.Against.Null(filteringContext);
        Sorting = Guard.Against.Null(sortingContext);
        Metadata = Guard.Against.Null(metadataProvidersFacade);
        Pipeline = Guard.Against.Null(pipelineContext);
        Options = Guard.Against.Null(optionsProvider).GetStrainerOptions();
    }

    /// <inheritdoc/>
    public IConfigurationCustomMethodsProvider CustomMethods { get; }

    /// <inheritdoc/>
    public IFilterContext Filter { get; }

    /// <inheritdoc/>
    public IMetadataFacade Metadata { get; }

    /// <inheritdoc/>
    public StrainerOptions Options { get; }

    /// <inheritdoc/>
    public ISortingContext Sorting { get; }

    /// <inheritdoc/>
    public IPipelineContext Pipeline { get; }
}
