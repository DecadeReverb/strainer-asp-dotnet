using Fluorite.Strainer.ExampleWebApi.Entities;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Modules;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.ExampleWebApi.Services
{
    public class ExampleWebApiStrainerModule : StrainerModule
    {
        public ExampleWebApiStrainerModule()
        {

        }

        public override void Load()
        {
            AddCustomFilterMethod<Post>(nameof(IsNew))
                .WithFunction(IsNew);

            AddCustomSortMethod<Post>(nameof(Popularity))
                .WithFunction(Popularity);

            AddFilterOperator(symbol: "%")
                .HasName("modulo equal zero")
                .HasExpression((context) => Expression.Equal(
                    Expression.Modulo(context.PropertyValue, context.FilterValue),
                    Expression.Constant(0)));

            AddProperty<Post>(p => p.Comments.Count)
                .IsFilterable()
                .IsSortable();
        }

        private IQueryable<Post> IsNew(ICustomFilterMethodContext<Post> context)
            => context.Source.Where(p => EF.Functions.DateDiffDay(DateTime.Now, p.DateCreated) < 7);

        private IOrderedQueryable<Post> Popularity(ICustomSortMethodContext<Post> context)
        {
            return context.IsSubsequent
                ? context.OrderedSource.ThenBy(p => p.LikeCount)
                : context.Source.OrderBy(p => p.LikeCount)
                    .ThenBy(p => p.Comments.Count)
                    .ThenByDescending(p => p.DateCreated);
        }
    }
}
