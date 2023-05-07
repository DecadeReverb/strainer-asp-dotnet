using Fluorite.Strainer.ExampleWebApi.Entities;
using Fluorite.Strainer.Services.Modules;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.ExampleWebApi.Modules
{
    public class ExampleWebApiStrainerModule : StrainerModule<Post>
    {
        public override void Load(IStrainerModuleBuilder<Post> builder)
        {
            builder.AddCustomFilterMethod(nameof(IsNew))
                .HasFunction(IsNew);
            builder.AddCustomFilterMethod(nameof(HasInTitleFilterOperator))
                .HasFunction(HasInTitleFilterOperator);

            builder.AddCustomSortMethod(nameof(Popularity))
                .HasFunction(Popularity);

            builder.AddFilterOperator(symbol: "%")
                .HasName("modulo equal zero")
                .HasExpression((context) => Expression.Equal(
                    Expression.Modulo(context.PropertyValue, context.FilterValue),
                    Expression.Constant(0)));

            builder.AddProperty(p => p.Comments.Count)
                .IsFilterable()
                .IsSortable();

            builder.AddProperty(p => p.LikeCount)
                .IsFilterable()
                .IsSortable()
                .IsDefaultSort(isDescending: true);
        }

        private IQueryable<Post> HasInTitleFilterOperator(IQueryable<Post> source, string filterOperator)
            => source.Where(p => p.Title.Contains(filterOperator));

        private IQueryable<Post> IsNew(IQueryable<Post> source, string filterOperator)
            => source.Where(p => EF.Functions.DateDiffDay(DateTime.UtcNow, p.DateCreated) < 7);

        private IOrderedQueryable<Post> Popularity(IQueryable<Post> source, bool isDescending, bool isSubsequent)
        {
            return isSubsequent
                ? (source as IOrderedQueryable<Post>).ThenByDescending(p => p.LikeCount)
                : source.OrderByDescending(p => p.LikeCount)
                    .ThenByDescending(p => p.DateCreated);
        }
    }
}
