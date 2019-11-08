using Fluorite.Strainer.ExampleWebApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Sieve.Example.Data
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

            AddPosts(context, postsCount: 120);
            context.SaveChanges();
        }

        private static void AddPosts(ApplicationDbContext context, int postsCount)
        {
            for (var i = 0; i < postsCount; i++)
            {
                var post = RandomizePost();
                context.Posts.Add(post);
                context.SaveChanges();
            }
        }

        private static Post RandomizePost()
        {
            var randomDateTime = RandomizeDateTime();

            return new Post
            {
                CategoryId = _random.Next(1, 3000),
                CommentCount = _random.Next(0, 38),
                DateCreated = randomDateTime,
                DateLastViewed = randomDateTime.AddDays(_random.Next((DateTime.Today - randomDateTime).Days)),
                LikeCount = _random.Next(0, 48),
                Title = RandomizeTitle(),
            };
        }

        private static DateTime RandomizeDateTime()
        {
            var start = new DateTime(2016, 1, 1);
            var range = (DateTime.Today - start).Days;

            return start.AddDays(_random.Next(range));
        }

        private static string RandomizeTitle()
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

        private static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
