﻿using Fluorite.Strainer.Models.Filtering.Operators;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorBuilder
    {
        IFilterOperator Build();
        IFilterOperatorBuilder Default();
        IFilterOperatorBuilder HasName(string name);
        IFilterOperatorBuilder Operator(string @operator);
    }
}
