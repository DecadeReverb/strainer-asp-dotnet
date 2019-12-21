using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluorite.Strainer.Services.Modules
{
    public abstract class StrainerModule
    {
        protected StrainerModule()
        {

        }

        public IDictionary<Type, IDictionary<string, ICustomFilterMethod>> CustomFilterMethods;

        public IDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

        public IDictionary<string, IFilterOperator> FilterOperators { get; }

        public IDictionary<Type, IDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }

        public IDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

        public StrainerOptions Options { get; }

        protected IStrainerContext StrainerContext { get; }

        public ICustomFilterMethodBuilder<TEntity> AddCustomFitlerMethod<TEntity>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            if (!CustomFilterMethods.ContainsKey(typeof(TEntity)))
            {
                CustomFilterMethods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
            }

            return new CustomFilterMethodBuilder<TEntity>(CustomFilterMethods, name);
        }
    }
}
