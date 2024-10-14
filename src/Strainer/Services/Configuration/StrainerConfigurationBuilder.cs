using Fluorite.Extensions;
using Fluorite.Strainer.Collections;
using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public class StrainerConfigurationBuilder : IStrainerConfigurationBuilder
{
    private Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> propertyMetadata;
    private Dictionary<Type, IObjectMetadata> objectMetadata;
    private Dictionary<string, IFilterOperator> filterOperators;
    private Dictionary<Type, IPropertyMetadata> defaultMetadata;
    private Dictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> customSortMethods;
    private Dictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> customFilterMethods;
    private HashSet<string> excludedBuiltInFilterOperators;

    public StrainerConfigurationBuilder()
    {
        propertyMetadata = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();
        objectMetadata = new Dictionary<Type, IObjectMetadata>();
        filterOperators = new Dictionary<string, IFilterOperator>();
        defaultMetadata = new Dictionary<Type, IPropertyMetadata>();
        customSortMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>();
        customFilterMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>();
        excludedBuiltInFilterOperators = new HashSet<string>();
    }

    public IStrainerConfiguration Build()
    {
        var filterOperatorsWithAddedBuiltIn = AddBuiltInFilterOperators(filterOperators, excludedBuiltInFilterOperators, FilterOperatorMapper.DefaultOperators);

        return new StrainerConfiguration(
            customFilterMethods,
            customSortMethods,
            defaultMetadata,
            filterOperatorsWithAddedBuiltIn,
            new ReadOnlyHashSet<string>(excludedBuiltInFilterOperators),
            objectMetadata,
            propertyMetadata);
    }

    public IStrainerConfigurationBuilder WithPropertyMetadata(ICollection<IStrainerModule> modules)
    {
        Guard.Against.Null(modules);

        propertyMetadata = modules
            .SelectMany(module => module
            .PropertyMetadata
            .Select(pair =>
                new KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                    pair.Key, pair.Value.ToReadOnly())))
            .Merge();

        return this;
    }

    public IStrainerConfigurationBuilder WithObjectMetadata(ICollection<IStrainerModule> modules)
    {
        Guard.Against.Null(modules);

        objectMetadata = modules
            .SelectMany(module => module.ObjectMetadata.ToReadOnly())
            .Merge();

        return this;
    }

    public IStrainerConfigurationBuilder WithCustomFilterOperators(ICollection<IStrainerModule> modules)
    {
        Guard.Against.Null(modules);

        filterOperators = modules.SelectMany(module => module.FilterOperators).Merge();

        return this;
    }

    public IStrainerConfigurationBuilder WithDefaultMetadata(ICollection<IStrainerModule> modules)
    {
        Guard.Against.Null(modules);

        defaultMetadata = modules
            .SelectMany(module => module.DefaultMetadata)
            .Merge();

        return this;
    }

    public IStrainerConfigurationBuilder WithCustomSortMethods(ICollection<IStrainerModule> modules)
    {
        Guard.Against.Null(modules);

        customSortMethods = modules
            .SelectMany(module => module
            .CustomSortMethods
            .Select(pair =>
                new KeyValuePair<Type, IReadOnlyDictionary<string, ICustomSortMethod>>(
                    pair.Key, pair.Value.ToReadOnly())))
            .Merge();

        return this;
    }

    public IStrainerConfigurationBuilder WithCustomFilterMethods(ICollection<IStrainerModule> modules)
    {
        Guard.Against.Null(modules);

        customFilterMethods = modules
            .SelectMany(module => module
                .CustomFilterMethods
                .Select(pair =>
                    new KeyValuePair<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>(
                        pair.Key, pair.Value.ToReadOnly())))
            .Merge();

        return this;
    }

    public IStrainerConfigurationBuilder WithoutBuiltInFilterOperators(ICollection<IStrainerModule> modules)
    {
        Guard.Against.Null(modules);

        excludedBuiltInFilterOperators = new HashSet<string>(
            modules.SelectMany(module => module.ExcludedBuiltInFilterOperators));

        return this;
    }

    private IReadOnlyDictionary<string, IFilterOperator> AddBuiltInFilterOperators(
        IDictionary<string, IFilterOperator> customFilterOperators,
        ISet<string> excludedBuiltInFilterOperators,
        IReadOnlyDictionary<string, IFilterOperator> defaultOperators)
    {
        foreach (var pair in defaultOperators)
        {
            if (!excludedBuiltInFilterOperators.Contains(pair.Key))
            {
                if (customFilterOperators.ContainsKey(pair.Key))
                {
                    throw new InvalidOperationException(
                        $"A custom filter operator is conflicting with built-in filter operator on symbol {pair.Key}. " +
                        $"Either mark the built-in filter operator to be excluded or remove custom filter operator.");
                }
                else
                {
                    customFilterOperators.Add(pair.Key, pair.Value);
                }
            }
        }

        return customFilterOperators.OrderBy(x => x.Key.Length).ToReadOnlyDictionary();
    }
}
