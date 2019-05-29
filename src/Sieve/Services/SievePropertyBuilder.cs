using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Sieve.Models;

namespace Sieve.Services
{
    public class SievePropertyBuilder<TEntity> : ISievePropertyBuilder<TEntity>
    {
        public SievePropertyBuilder(SievePropertyMapper sievePropertyMapper, Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            Mapper = sievePropertyMapper ?? throw new ArgumentNullException(nameof(sievePropertyMapper));
            (FullName, PropertyInfo) = GetPropertyInfo(expression);
            Name = FullName;
            IsFilterable = false;
            IsSortable = false;
        }

        public string FullName { get; protected set; }
        public bool IsFilterable { get; protected set; }
        public bool IsSortable { get; protected set; }
        public string Name { get; protected set; }

        protected PropertyInfo PropertyInfo { get; }
        protected SievePropertyMapper Mapper { get; }

        public ISievePropertyBuilder<TEntity> CanFilter()
        {
            IsFilterable = true;
            UpdateMap();

            return this;
        }

        public ISievePropertyBuilder<TEntity> CanSort()
        {
            IsSortable = true;
            UpdateMap();

            return this;
        }

        public ISievePropertyBuilder<TEntity> HasName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            Name = name;
            UpdateMap();

            return this;
        }

        private void UpdateMap()
        {
            var metadata = new SievePropertyMetadata()
            {
                Name = Name,
                FullName = FullName,
                CanFilter = IsFilterable,
                CanSort = IsSortable
            };

            Mapper.AddMap<TEntity>(PropertyInfo, metadata);
        }

        private static (string, PropertyInfo) GetPropertyInfo(Expression<Func<TEntity, object>> expression)
        {
            if (!(expression.Body is MemberExpression body))
            {
                var ubody = expression.Body as UnaryExpression;
                body = ubody.Operand as MemberExpression;
            }

            var propertyInfo = body?.Member as PropertyInfo;
            var stack = new Stack<string>();
            while (body != null)
            {
                stack.Push(body.Member.Name);
                body = body.Expression as MemberExpression;
            }

            return (string.Join(".", stack.ToArray()), propertyInfo);
        }
    }
}
