using Fluorite.Strainer.Attributes;
using System;

namespace Fluorite.Strainer.TestModels
{
    public class Comment
    {
        [StrainerProperty]
        public DateTimeOffset DateCreated { get; set; }

        public int Id { get; set; }

        [StrainerProperty]
        public string Text { get; set; }
    }
}
