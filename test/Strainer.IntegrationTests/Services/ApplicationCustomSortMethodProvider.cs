using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.TestModels;
using System.Linq;

namespace Fluorite.Strainer.IntegrationTests.Services
{
    public class ApplicationCustomSortMethodProvider : CustomSortMethodProvider
    {
        public ApplicationCustomSortMethodProvider(ICustomSortMethodMapper mapper) : base(mapper)
        {

        }

        public override void MapMethods(ICustomSortMethodMapper mapper)
        {
            mapper.CustomMethod<Post>(nameof(Popularity))
                .WithFunction(Popularity);
        }

        private IOrderedQueryable<Post> Popularity(ICustomSortMethodContext<Post> context)
        {
            return context.IsSubsequent
                ? (context.Source as IOrderedQueryable<Post>).ThenBy(p => p.LikeCount)
                : context.Source.OrderBy(p => p.LikeCount)
                    .ThenBy(p => p.CommentCount)
                    .ThenBy(p => p.DateCreated);
        }
    }
}
