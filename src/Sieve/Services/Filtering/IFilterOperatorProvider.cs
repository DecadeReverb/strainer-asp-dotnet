﻿using System.Collections.Generic;
using Sieve.Models;

namespace Sieve.Services.Filtering
{
    public interface IFilterOperatorProvider
    {
        IReadOnlyList<IFilterOperator> Operators { get; }

        void AddOperator(IFilterOperator @operator);
        IFilterOperator GetDefaultOperator();
        IFilterOperator GetFirstOrDefault(string @operator);
    }
}
