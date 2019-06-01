using System.Linq;
using Strainer.Services;
using Strainer.UnitTests.Entities;

namespace Strainer.UnitTests.Services
{
    public class StrainerCustomSortMethods : IStrainerCustomSortMethods
    {
        public IQueryable<Post> Popularity(IQueryable<Post> source, bool useThenBy, bool desc)
        {
            var result = useThenBy ?
                ((IOrderedQueryable<Post>)source).ThenBy(p => p.LikeCount) :
                source.OrderBy(p => p.LikeCount)
                .ThenBy(p => p.CommentCount)
                .ThenBy(p => p.DateCreated);

            return result;
        }
    }
}
