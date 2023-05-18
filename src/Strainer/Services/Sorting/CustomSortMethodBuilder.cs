using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using System.Linq.Expressions;

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
                    $"or contain only whitespace characters.",
                    nameof(name));
            }

            _customMethods = customFilterMethodsDictionary
                ?? throw new ArgumentNullException(nameof(customFilterMethodsDictionary));
            Name = name;
        }

        protected Expression<Func<TEntity, object>> Expression { get; set; }

        protected Func<ISortTerm, Expression<Func<TEntity, object>>> SortTermExpression { get; set; }

        protected string Name { get; set; }

        public ICustomSortMethod<TEntity> Build() => new CustomSortMethod<TEntity>
        {
            Expression = Expression,
            Name = Name,
            SortTermExpression = SortTermExpression,
        };

        public ICustomSortMethodBuilder<TEntity> HasFunction(
            Expression<Func<TEntity, object>> expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            SortTermExpression = null;

            Save(Build());

            return this;
        }

        public ICustomSortMethodBuilder<TEntity> HasFunction(Func<ISortTerm, Expression<Func<TEntity, object>>> sortTermExpression)
        {
            SortTermExpression = sortTermExpression ?? throw new ArgumentNullException(nameof(sortTermExpression));
            Expression = null;

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