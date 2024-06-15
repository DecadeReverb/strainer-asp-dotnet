using Fluorite.Strainer.ExampleWebApi.Entities;
using Fluorite.Strainer.Services.Modules;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.ExampleWebApi.Modules;

public class ExampleWebApiStrainerModule : StrainerModule<Post>
{
    public override void Load(IStrainerModuleBuilder<Post> builder)
    {
        builder.AddCustomFilterMethod("IsNew")
            .HasFunction(p => EF.Functions.DateDiffDay(DateTime.UtcNow, p.DateCreated) < 7);

        builder.AddCustomSortMethod("Popularity")
            .HasFunction(p => p.LikeCount);

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
}
