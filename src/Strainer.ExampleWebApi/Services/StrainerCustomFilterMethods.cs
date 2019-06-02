using Fluorite.Strainer.Services;
using Fluorite.Strainer.ExampleWebApi.Entities;
using System.Linq;

namespace Fluorite.Strainer.ExampleWebApi.Services
{
	public class StrainerCustomFilterMethods : IStrainerCustomFilterMethods
    {
        public IQueryable<Post> IsNew(IQueryable<Post> source, string op, string[] values)
            => source.Where(p => p.LikeCount < 100 && p.CommentCount < 5);
    }
}
