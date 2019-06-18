using Fluorite.Strainer.ExampleWebApi.Entities;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Services.Filtering;
using System.Linq;

namespace Fluorite.Strainer.ExampleWebApi.Services
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
        }

        private IQueryable<Post> IsNew(IQueryable<Post> source, string op, string[] values)
            => source.Where(p => p.LikeCount < 100 && p.CommentCount < 5);

        private IQueryable<Post> IsNew(ICustomFilterMethodContext<Post> context)
        {
            return context.Source.Where(p => p.LikeCount < 100 && p.CommentCount < 5);
        }
    }
}
