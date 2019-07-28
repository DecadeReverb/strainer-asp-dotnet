using Fluorite.Strainer.Models.Sorting;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public class CustomSortMethodBuilder<TEntity> : ICustomSortMethodBuilder<TEntity>
    {
        public CustomSortMethodBuilder(ICustomSortMethodMapper mapper, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Name = name;
            Function = context => context.Source;
        }

        public Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> Function { get; protected set; }
        public string Name { get; protected set; }

        protected ICustomSortMethodMapper Mapper { get; }

        public ICustomSortMethod<TEntity> Build() => new CustomSortMethod<TEntity>
        {
            Function = Function,
            Name = Name,
        };

        public ICustomSortMethodBuilder<TEntity> WithFunction(Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> function)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));

            Mapper.AddMap(Build());

            return this;
        }
    }
}