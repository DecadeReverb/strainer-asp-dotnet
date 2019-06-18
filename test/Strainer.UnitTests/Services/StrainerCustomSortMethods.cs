using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.UnitTests.Entities;
using System.Linq;

namespace Fluorite.Strainer.UnitTests.Services
{
    public class ApplicationCustomSortMethodProvider : CustomSortMethodProvider
    {
        public ApplicationCustomSortMethodProvider(ICustomSortMethodMapper mapper) : base(mapper)
        {

        }

        //public IQueryable<Post> Popularity(IQueryable<Post> source, bool useThenBy, bool desc)
        //{
        //    var result = useThenBy
        //        ? ((IOrderedQueryable<Post>)source).ThenBy(p => p.LikeCount)
        //        : source.OrderBy(p => p.LikeCount)
        //            .ThenBy(p => p.CommentCount)
        //            .ThenBy(p => p.DateCreated);

        //    return result;
        //}

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
