using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Sorting;

public interface ICustomSortMethodBuilder<TEntity>
{
    ICustomSortMethod<TEntity> Build();

    ICustomSortMethodBuilder<TEntity> HasFunction(Expression<Func<TEntity, object>> expression);

    ICustomSortMethodBuilder<TEntity> HasFunction(Func<ISortTerm, Expression<Func<TEntity, object>>> expressionProvider);
}
