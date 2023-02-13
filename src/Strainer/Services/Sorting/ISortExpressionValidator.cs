using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortExpressionValidator
    {
        void Validate<TEntity>(IEnumerable<ISortExpression<TEntity>> sortExpressions);
    }
}
