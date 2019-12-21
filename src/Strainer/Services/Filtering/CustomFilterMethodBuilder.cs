using Fluorite.Strainer.Models.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Services.Filtering
{
    public class CustomFilterMethodBuilder<TEntity> : ICustomFilterMethodBuilder<TEntity>
    {
        private readonly IDictionary<Type, IDictionary<string, ICustomFilterMethod>> _customMethods;

        public CustomFilterMethodBuilder(
            IDictionary<Type, IDictionary<string, ICustomFilterMethod>> customFilterMethodsDictionary,
            string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            _customMethods = customFilterMethodsDictionary
                ?? throw new ArgumentNullException(nameof(customFilterMethodsDictionary));
            Name = name;
            Function = context => context.Source;
        }

        protected Func<ICustomFilterMethodContext<TEntity>, IQueryable<TEntity>> Function { get; set; }

        protected string Name { get; set; }

        public ICustomFilterMethod<TEntity> Build() => new CustomFilterMethod<TEntity>
        {
            Function = Function,
            Name = Name,
        };

        public ICustomFilterMethodBuilder<TEntity> WithFunction(
            Func<ICustomFilterMethodContext<TEntity>, IQueryable<TEntity>> function)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));

            Save(Build());

            return this;
        }

        protected void Save(ICustomFilterMethod<TEntity> customFilterMethod)
        {
            if (customFilterMethod == null)
            {
                throw new ArgumentNullException(nameof(customFilterMethod));
            }

            if (!_customMethods.ContainsKey(typeof(TEntity)))
            {
                _customMethods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
            }

            _customMethods[typeof(TEntity)][customFilterMethod.Name] = customFilterMethod;
        }
    }
}
