using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Modules;

/// <summary>
/// Provides configuration information about metadata, filter operators
/// and custom methods for Strainer.
/// </summary>
public abstract class StrainerModule : IStrainerModule
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StrainerModule"/> class.
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

    /// <inheritdoc/>
    public abstract void Load(IStrainerModuleBuilder builder);
}
