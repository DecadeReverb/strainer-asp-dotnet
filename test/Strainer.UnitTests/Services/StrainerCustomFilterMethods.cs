using System;
using System.Collections.Generic;
using System.Linq;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.UnitTests.Entities;

namespace Fluorite.Strainer.UnitTests.Services
{
    public class StrainerCustomFilterMethods : IStrainerCustomFilterMethods
    {
        public IQueryable<Post> IsNew(IQueryable<Post> source, string op, IList<string> values)
        {
            var result = source.Where(p => p.LikeCount < 100);

            return result;
        }

        public IQueryable<Post> HasInTitle(IQueryable<Post> source, string op, IList<string> values)
        {
            var result = source.Where(p => p.Title.Contains(values[0]));

            return result;
        }

        public IQueryable<Comment> IsNew(IQueryable<Comment> source, string op, IList<string> values)
        {
            var result = source.Where(c => c.DateCreated > DateTimeOffset.UtcNow.AddDays(-2));

            return result;
        }

        public IQueryable<Comment> TestComment(IQueryable<Comment> source, string op, IList<string> values)
        {
            return source;
        }
    }
}
