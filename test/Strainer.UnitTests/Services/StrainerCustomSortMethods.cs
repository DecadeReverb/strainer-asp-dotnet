using System.Linq;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.UnitTests.Entities;

namespace Fluorite.Strainer.UnitTests.Services
{
    public class StrainerCustomSortMethods : IStrainerCustomSortMethods
    {
        public IQueryable<Post> Popularity(IQueryable<Post> source, bool useThenBy, bool desc)
        {
            var result = useThenBy
                ? ((IOrderedQueryable<Post>)source).ThenBy(p => p.LikeCount)
                : source.OrderBy(p => p.LikeCount)
                    .ThenBy(p => p.CommentCount)
                    .ThenBy(p => p.DateCreated);

            return result;
        }
    }
}
