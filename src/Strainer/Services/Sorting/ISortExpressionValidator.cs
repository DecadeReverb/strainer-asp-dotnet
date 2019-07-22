using Fluorite.Strainer.Models.Sorting;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortExpressionValidator
    {
        void Validate<TEntity>(IEnumerable<ISortExpression<TEntity>> sortExpressions);
    }
}
