using Fluorite.Strainer.Models.Filtering;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public class CustomFilterMethodMapper : ICustomFilterMethodMapper
    {
        public CustomFilterMethodMapper()
        {
            Methods = new Dictionary<Type, IDictionary<string, ICustomFilterMethod>>();
        }

        public IDictionary<Type, IDictionary<string, ICustomFilterMethod>> Methods { get; }

        public void AddMap<TEntity>(ICustomFilterMethod<TEntity> customMethod)
        {
            if (customMethod == null)
            {
                throw new ArgumentNullException(nameof(customMethod));
            }

            if (!Methods.ContainsKey(typeof(TEntity)))
            {
                Methods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
            }

            Methods[typeof(TEntity)][customMethod.Name] = customMethod;
        }

        public ICustomFilterMethodBuilder<TEntity> CustomMethod<TEntity>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characters.",
                    nameof(name));
            }

            if (!Methods.ContainsKey(typeof(TEntity)))
            {
                Methods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
            }

            return new CustomFilterMethodBuilder<TEntity>(Methods, name);
        }
    }
}