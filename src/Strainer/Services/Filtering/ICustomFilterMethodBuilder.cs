using Fluorite.Strainer.Models.Filtering;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface ICustomFilterMethodBuilder<TEntity>
    {
        ICustomFilterMethod<TEntity> Build();

        ICustomFilterMethodBuilder<TEntity> HasFunction(
            Func<IQueryable<TEntity>, string, IQueryable<TEntity>> function);
    }
}
