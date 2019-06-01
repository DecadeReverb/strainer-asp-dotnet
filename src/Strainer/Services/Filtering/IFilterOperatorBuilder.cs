﻿using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorBuilder
    {
        IFilterOperator Build();
        IFilterOperatorBuilder CaseInsensitive();
        IFilterOperatorBuilder Default();
        IFilterOperatorBuilder HasCaseSensitiveVersion(IFilterOperator filterOperator);
        IFilterOperatorBuilder HasName(string name);
        IFilterOperatorBuilder HasUnnegatedVersion(IFilterOperator filterOperator);
        IFilterOperatorBuilder Operator(string @operator);
    }
}