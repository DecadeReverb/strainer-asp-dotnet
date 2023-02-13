using Fluorite.Extensions;
using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public class StrainerConfigurationBuilder : IStrainerConfigurationBuilder
    {
        private IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> propertyMetadata;
        private IReadOnlyDictionary<Type, IObjectMetadata> objectMetadata;
        private IReadOnlyDictionary<string, IFilterOperator> filterOperators;
        private IReadOnlyDictionary<Type, IPropertyMetadata> defaultMetadata;
        private IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> customSortMethods;
        private IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> customFilterMethods;

        public StrainerConfigurationBuilder()
        {
            propertyMetadata = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>().ToReadOnly();
            objectMetadata = new Dictionary<Type, IObjectMetadata>().ToReadOnly();
            filterOperators = new Dictionary<string, IFilterOperator>().ToReadOnly();
            defaultMetadata = new Dictionary<Type, IPropertyMetadata>().ToReadOnly();
            customSortMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>().ToReadOnly();
            customFilterMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>().ToReadOnly();
        }

        public IStrainerConfiguration Build()
        {
            return new StrainerConfiguration(
                customFilterMethods,
                customSortMethods,
                defaultMetadata,
                filterOperators,
                objectMetadata,
                propertyMetadata);
        }

        public IStrainerConfigurationBuilder WithPropertyMetadata(ICollection<IStrainerModule> modules)
        {
            propertyMetadata = modules
                .SelectMany(module => module
                .PropertyMetadata
                .Select(pair =>
                    new KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                        pair.Key, pair.Value.ToReadOnly())))
                .Merge()
                .ToReadOnly();

            return this;
        }

        public IStrainerConfigurationBuilder WithObjectMetadata(ICollection<IStrainerModule> modules)
        {
            objectMetadata = modules
                .SelectMany(module => module.ObjectMetadata.ToReadOnly())
                .Merge()
                .ToReadOnly();

            return this;
        }

        public IStrainerConfigurationBuilder WithFilterOperators(ICollection<IStrainerModule> modules)
        {
            filterOperators = modules
                .SelectMany(module => module.FilterOperators)
                .Union(FilterOperatorMapper.DefaultOperators)
                .Merge()
                .ToReadOnly();

            return this;
        }

        public IStrainerConfigurationBuilder WithDefaultMetadata(ICollection<IStrainerModule> modules)
        {
            defaultMetadata = modules
                .SelectMany(module => module.DefaultMetadata)
                .Merge()
                .ToReadOnly();

            return this;
        }

        public IStrainerConfigurationBuilder WithCustomSortMethods(ICollection<IStrainerModule> modules)
        {
            customSortMethods = modules
                .SelectMany(module => module
                .CustomSortMethods
                .Select(pair =>
                    new KeyValuePair<Type, IReadOnlyDictionary<string, ICustomSortMethod>>(
                        pair.Key, pair.Value.ToReadOnly())))
                .Merge()
                .ToReadOnly();

            return this;
        }

        public IStrainerConfigurationBuilder WithCustomFilterMethods(ICollection<IStrainerModule> modules)
        {
            customFilterMethods = modules
                .SelectMany(module => module
                    .CustomFilterMethods
                    .Select(pair =>
                        new KeyValuePair<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>(
                            pair.Key, pair.Value.ToReadOnly())))
                .Merge()
                .ToReadOnly();

            return this;
        }
    }
}
