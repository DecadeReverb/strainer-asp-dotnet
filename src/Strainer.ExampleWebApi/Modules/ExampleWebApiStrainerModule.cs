using Fluorite.Strainer.ExampleWebApi.Entities;
using Fluorite.Strainer.Services.Modules;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.ExampleWebApi.Modules
{
    public class ExampleWebApiStrainerModule : StrainerModule
    {
        public ExampleWebApiStrainerModule()
        {

        }

        public override void Load(IStrainerModuleBuilder builder)
        {
            builder.AddCustomFilterMethod<Post>(nameof(IsNew))
                .HasFunction(IsNew);
            builder.AddCustomFilterMethod<Post>(nameof(HasInTitleFilterOperator))
                .HasFunction(HasInTitleFilterOperator);

            builder.AddCustomSortMethod<Post>(nameof(Popularity))
                .HasFunction(Popularity);

            builder.AddFilterOperator(symbol: "%")
                .HasName("modulo equal zero")
                .HasExpression((context) => Expression.Equal(
                    Expression.Modulo(context.PropertyValue, context.FilterValue),
                    Expression.Constant(0)));

            builder.AddProperty<Post>(p => p.Comments.Count)
                .IsFilterable()
                .IsSortable();
        }

        private IQueryable<Post> HasInTitleFilterOperator(IQueryable<Post> source, string filterOperator)
            => source.Where(p => p.Title.Contains(filterOperator));

        private IQueryable<Post> IsNew(IQueryable<Post> source, string filterOperator)
            => source.Where(p => EF.Functions.DateDiffDay(DateTime.Now, p.DateCreated) < 7);

        private IOrderedQueryable<Post> Popularity(IQueryable<Post> source, bool isDescending, bool isSubsequent)
        {
            return isSubsequent
                ? (source as IOrderedQueryable<Post>).ThenByDescending(p => p.LikeCount)
                : source.OrderByDescending(p => p.LikeCount)
                    .ThenByDescending(p => p.DateCreated);
        }
    }
}
