using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public interface ICustomFilterMethodBuilder<TEntity>
{
    ICustomFilterMethod<TEntity> Build();

    ICustomFilterMethodBuilder<TEntity> HasFunction(Expression<Func<TEntity, bool>> expression);

    ICustomFilterMethodBuilder<TEntity> HasFunction(Func<IFilterTerm, Expression<Func<TEntity, bool>>> filterTermExpression);
}
