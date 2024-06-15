using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Sorting;

/// <summary>
/// Provides means of tranlating <see cref="ISortTerm"/> into
/// <see cref="Expression{TDelegate}"/> of <see cref="Func{T, TResult}"/>.
/// <para/>
/// In other words - provides list of expressions which later can be used
/// as arguments for ordering <see cref="IQueryable{T}"/>.
/// </summary>
public class SortExpressionProvider : ISortExpressionProvider
{
    private readonly IMetadataFacade _metadataProvidersFacade;

    /// <summary>
    /// Initializes a new instance of the <see cref="SortExpressionProvider"/> class.
    /// </summary>
    public SortExpressionProvider(IMetadataFacade metadataProvidersFacade)
    {
        _metadataProvidersFacade = Guard.Against.Null(metadataProvidersFacade);
    }

    public ISortExpression<TEntity> GetDefaultExpression<TEntity>()
    {
        var propertyMetadata = _metadataProvidersFacade.GetDefaultMetadata<TEntity>();

        if (propertyMetadata == null)
        {
            return null;
        }

        var sortTerm = new SortTerm
        {
            IsDescending = propertyMetadata.IsDefaultSortingDescending,
            Name = propertyMetadata.DisplayName ?? propertyMetadata.Name,
        };

        return GetExpression<TEntity>(propertyMetadata.PropertyInfo, sortTerm, isSubsequent: false);
    }

    public ISortExpression<TEntity> GetExpression<TEntity>(
        PropertyInfo propertyInfo,
        ISortTerm sortTerm,
        bool isSubsequent)
    {
        Guard.Against.Null(propertyInfo);
        Guard.Against.Null(sortTerm);

        var metadata = _metadataProvidersFacade.GetMetadata<TEntity>(
            isSortableRequired: true,
            isFilterableRequired: false,
            name: sortTerm.Name);

        if (metadata == null)
        {
            return null;
        }

        var parameter = Expression.Parameter(typeof(TEntity), "p");
        Expression propertyValue = parameter;

        if (metadata.Name.Contains("."))
        {
            var parts = metadata.Name.Split('.');

            for (var i = 0; i < parts.Length - 1; i++)
            {
                propertyValue = Expression.PropertyOrField(propertyValue, parts[i]);
            }
        }

        var propertyAccess = Expression.MakeMemberAccess(propertyValue, propertyInfo);
        var conversion = Expression.Convert(propertyAccess, typeof(object));
        var orderExpression = Expression.Lambda<Func<TEntity, object>>(conversion, parameter);

        return new SortExpression<TEntity>
        {
            Expression = orderExpression,
            IsDefault = metadata.IsDefaultSorting,
            IsDescending = sortTerm.IsDescending,
            IsSubsequent = isSubsequent,
        };
    }

    /// <summary>
    /// Gets a list of <see cref="ISortExpression{TEntity}"/> from
    /// list of sort terms used to associate names from <see cref="IMetadataMapper"/>.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity for which the expression is for.
    /// </typeparam>
    /// <param name="sortTerms">
    /// A list of property info paired with sort terms.
    /// </param>
    /// <returns>
    /// A list of <see cref="ISortExpression{TEntity}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="sortTerms"/> is <see langword="null"/>.
    /// </exception>
    public IReadOnlyCollection<ISortExpression<TEntity>> GetExpressions<TEntity>(
        IEnumerable<KeyValuePair<PropertyInfo, ISortTerm>> sortTerms)
    {
        Guard.Against.Null(sortTerms);

        var expressions = new List<ISortExpression<TEntity>>();
        var isSubqequent = false;

        foreach (var pair in sortTerms)
        {
            var sortExpression = GetExpression<TEntity>(pair.Key, pair.Value, isSubqequent);
            if (sortExpression != null)
            {
                expressions.Add(sortExpression);
            }
            else
            {
                continue;
            }

            isSubqequent = true;
        }

        return expressions.AsReadOnly();
    }
}
