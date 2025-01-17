﻿using Fluorite.Strainer.Models.Filtering.Operators;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public interface IFilterOperatorBuilder
{
    IFilterOperator Build();

    IFilterOperatorBuilder HasExpression(Func<IFilterExpressionContext, Expression> expression);

    IFilterOperatorBuilder HasName(string name);

    IFilterOperatorBuilder HasSymbol(string symbol);

    IFilterOperatorBuilder IsCaseInsensitive();

    IFilterOperatorBuilder IsStringBased();
}
