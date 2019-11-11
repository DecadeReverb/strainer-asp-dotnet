﻿using Fluorite.Strainer.ExampleWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.ExampleWebApi.Data
{
    /// <summary>
    /// Provides means of database initialization.
    /// </summary>
    public static class DatabaseInitializer
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Creates the database if it does not exists and seeds it with
        /// initial data.
        /// </summary>
        public static void Initialize(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            AddPosts(context, postsCount: 120, upToCommentsPerPost: 10);

            context.SaveChanges();
        }

        private static void AddPosts(ApplicationDbContext context, int postsCount, int upToCommentsPerPost)
        {
            for (var i = 0; i < postsCount; i++)
            {
                var post = RandomizePost(upToCommentsPerPost);
                context.Posts.Add(post);
                context.SaveChanges();
            }
        }

        private static List<Comment> RandomizeComments(Post parentPost, int minComments, int maxComments)
        {
            var commentsCount = _random.Next(minComments, maxComments + 1);
            var comments = new List<Comment>(capacity: commentsCount);

            for (var i = 0; i < commentsCount; i++)
            {
                comments.Add(new Comment
                {
                    Message = RandomizeText(),
                    Post = parentPost,
                });
            }

            return comments;
        }

        private static Post RandomizePost(int upToCommentsPerPost)
        {
            var randomDateTime = RandomizeDateTime();

            var post = new Post
            {
                CategoryId = _random.Next(1, 3000),
                DateCreated = randomDateTime,
                DateLastViewed = randomDateTime.AddDays(_random.Next((DateTime.Today - randomDateTime).Days)),
                LikeCount = _random.Next(0, 48),
                Title = RandomizeText(),
            };

            post.Comments = RandomizeComments(post, 0, upToCommentsPerPost);

            return post;
        }

        private static DateTime RandomizeDateTime()
        {
            var start = new DateTime(2016, 1, 1);
            var range = (DateTime.Today - start).Days;

            return start.AddDays(_random.Next(range));
        }

        private static string RandomizeText()
        {
            var words = new List<string>
            {
                "anemone", "wagstaff", "man", "the", "for",
                "and", "a", "with", "bird", "fox"
            };
            var sentence = new List<string>();

            while (sentence.Count != words.Count)
            {
                var index = _random.Next(0, words.Count);
                var word = words[index];
                if (!sentence.Contains(word))
                {
                    sentence.Add(word);
                }
            }

            return string.Join(" ", sentence).FirstCharToUpper();
        }

        private static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.Substring(1),
            };
    }
}
