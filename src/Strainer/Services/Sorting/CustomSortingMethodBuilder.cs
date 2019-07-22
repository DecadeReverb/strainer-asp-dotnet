using Fluorite.Strainer.Models.Sorting;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public class CustomSortingMethodBuilder<TEntity> : ICustomSortingMethodBuilder<TEntity>
    {
        public CustomSortingMethodBuilder(ICustomSortingMethodMapper mapper, string name)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Function = context => context.Source;
        }

        public Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> Function { get; protected set; }
        public string Name { get; protected set; }

        protected ICustomSortingMethodMapper Mapper { get; }

        public ICustomSortMethod<TEntity> Build() => new CustomSortMethod<TEntity>
        {
            Function = Function,
            Name = Name,
        };

        public ICustomSortingMethodBuilder<TEntity> WithFunction(Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> function)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));

            Mapper.AddMap(Build());

            return this;
        }
    }
}