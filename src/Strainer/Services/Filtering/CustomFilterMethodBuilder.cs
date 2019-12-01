﻿using Fluorite.Strainer.Models.Filtering;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Filtering
{
    public class CustomFilterMethodBuilder<TEntity> : ICustomFilterMethodBuilder<TEntity>
    {
        public CustomFilterMethodBuilder(ICustomFilterMethodMapper mapper, string name)
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

        public Func<ICustomFilterMethodContext<TEntity>, IQueryable<TEntity>> Function { get; protected set; }

        public string Name { get; protected set; }

        protected ICustomFilterMethodMapper Mapper { get; }

        public ICustomFilterMethod<TEntity> Build() => new CustomFilterMethod<TEntity>
        {
            Function = Function,
            Name = Name,
        };

        public ICustomFilterMethodBuilder<TEntity> WithFunction(
            Func<ICustomFilterMethodContext<TEntity>, IQueryable<TEntity>> function)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));

            Mapper.AddMap(Build());

            return this;
        }
    }
}
