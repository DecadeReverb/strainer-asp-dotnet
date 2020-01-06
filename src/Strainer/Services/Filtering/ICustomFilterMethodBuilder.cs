﻿using Fluorite.Strainer.Models.Filtering;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface ICustomFilterMethodBuilder<TEntity>
    {
        ICustomFilterMethod<TEntity> Build();

        ICustomFilterMethodBuilder<TEntity> HasExpression(
            Func<ICustomFilterMethodContext<TEntity>, IQueryable<TEntity>> function);
    }
}
