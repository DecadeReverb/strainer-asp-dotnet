using Fluorite.Strainer.Models.Filtering.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public interface ICustomFilteringExpressionProvider
{
    public bool TryGetCustomExpression<T>(
        IFilterTerm filterTerm,
        string filterTermName,
        out Expression<Func<T, bool>> expression);
}
