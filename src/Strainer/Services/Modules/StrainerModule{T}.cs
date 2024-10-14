using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Modules;

/// <summary>
/// Provides configuration information about metadata, filter operators
/// and custom methods for Strainer.
/// </summary>
/// <typeparam name="T">
/// The type of model for which this module is for.
/// </typeparam>
public abstract class StrainerModule<T> : IStrainerModule<T>, IStrainerModule
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StrainerModule{T}"/> class.
    /// </summary>
    protected StrainerModule()
    {
        CustomFilterMethods = new Dictionary<Type, IDictionary<string, ICustomFilterMethod>>();
        CustomSortMethods = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
        DefaultMetadata = new Dictionary<Type, IPropertyMetadata>();
        FilterOperators = new Dictionary<string, IFilterOperator>();
        ExcludedBuiltInFilterOperators = new HashSet<string>();
        ObjectMetadata = new Dictionary<Type, IObjectMetadata>();
        PropertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
    }

    /// <inheritdoc/>
    public IDictionary<Type, IDictionary<string, ICustomFilterMethod>> CustomFilterMethods { get; }

    /// <inheritdoc/>
    public IDictionary<Type, IDictionary<string, ICustomSortMethod>> CustomSortMethods { get; }

    /// <inheritdoc/>
    public IDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

    /// <inheritdoc/>
    public IDictionary<string, IFilterOperator> FilterOperators { get; }

    /// <inheritdoc/>
    public ISet<string> ExcludedBuiltInFilterOperators { get; }

    /// <inheritdoc/>
    public IDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

    /// <inheritdoc/>
    public IDictionary<Type, IDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }

    /// <summary>
    /// Loads configuration in module.
    /// <para/>
    /// Override this method to specify configuration for this module.
    /// </summary>
    /// <param name="builder">
    /// The Strainer Module builder.
    /// </param>
    public abstract void Load(IStrainerModuleBuilder<T> builder);

    /// <inheritdoc/>
    public void Load(IStrainerModuleBuilder builder)
    {
        Guard.Against.Null(builder);

        if (builder is not IStrainerModuleBuilder<T> typedBuilder)
        {
            throw new NotSupportedException("Only strongly typed module builder is supported.");
        }

        Load(typedBuilder);
    }
}
