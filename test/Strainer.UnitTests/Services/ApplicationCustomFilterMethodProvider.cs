using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.UnitTests.Entities;
using System;
using System.Linq;

namespace Fluorite.Strainer.UnitTests.Services
{
    public class ApplicationCustomFilterMethodProvider : CustomFilterMethodProvider
    {
        public ApplicationCustomFilterMethodProvider(ICustomFilterMethodMapper mapper) : base(mapper)
        {

        }

        public override void MapMethods(ICustomFilterMethodMapper mapper)
        {
            mapper.CustomMethod<Post>(nameof(IsNew))
                .WithFunction(IsNew);
            mapper.CustomMethod<Post>(nameof(HasInTitle))
                .WithFunction(HasInTitle);
            mapper.CustomMethod<Comment>(nameof(IsNew))
                .WithFunction(IsNew);
            mapper.CustomMethod<Comment>(nameof(TestComment))
                .WithFunction(TestComment);
        }

        private IQueryable<Post> IsNew(ICustomFilterMethodContext<Post> context)
        {
            return context.Source.Where(p => p.LikeCount < 100);
        }

        private IQueryable<Post> HasInTitle(ICustomFilterMethodContext<Post> context)
        {
            return context.Source.Where(p => p.Title.Contains(context.Term.Values[0]));
        }

        private IQueryable<Comment> IsNew(ICustomFilterMethodContext<Comment> context)
        {
            return context.Source.Where(c => c.DateCreated > DateTimeOffset.UtcNow.AddDays(-2));
        }

        private IQueryable<Comment> TestComment(ICustomFilterMethodContext<Comment> context)
        {
            return context.Source;
        }
    }
}
