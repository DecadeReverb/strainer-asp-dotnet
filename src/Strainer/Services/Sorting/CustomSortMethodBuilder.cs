using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public class CustomSortMethodBuilder<TEntity> : ICustomSortMethodBuilder<TEntity>
    {
        private readonly IDictionary<Type, IDictionary<string, ICustomSortMethod>> _customMethods;

        public CustomSortMethodBuilder(
            IDictionary<Type, IDictionary<string, ICustomSortMethod>> customFilterMethodsDictionary,
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

        protected Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> Function { get; set; }

        protected string Name { get; set; }

        public ICustomSortMethod<TEntity> Build() => new CustomSortMethod<TEntity>
        {
            Function = Function,
            Name = Name,
        };

        public ICustomSortMethodBuilder<TEntity> HasExpression(
            Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> function)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));

            Save(Build());

            return this;
        }

        protected void Save(ICustomSortMethod<TEntity> customSortMethod)
        {
            if (customSortMethod == null)
            {
                throw new ArgumentNullException(nameof(customSortMethod));
            }

            if (!_customMethods.ContainsKey(typeof(TEntity)))
            {
                _customMethods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
            }

            _customMethods[typeof(TEntity)][customSortMethod.Name] = customSortMethod;
        }
    }
}