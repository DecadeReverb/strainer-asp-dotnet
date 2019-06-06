﻿using System.Linq;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.ExampleWebApi.Entities;

namespace Fluorite.Strainer.ExampleWebApi.Services
{
    public class StrainerCustomSortMethods : IStrainerCustomSortMethods
    {
        public IQueryable<Post> Popularity(IQueryable<Post> source, bool useThenBy) => useThenBy
            ? ((IOrderedQueryable<Post>)source).ThenBy(p => p.LikeCount)
            : source.OrderBy(p => p.LikeCount)
                .ThenBy(p => p.CommentCount)
                .ThenBy(p => p.DateCreated);
    }
}