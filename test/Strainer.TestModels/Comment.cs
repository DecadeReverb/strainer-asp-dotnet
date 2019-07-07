using Fluorite.Strainer.Attributes;
using System;

namespace Fluorite.Strainer.TestModels
{
    public class Comment
    {
        [Strainer(IsFilterable = true, IsSortable = true)]
        public DateTimeOffset DateCreated { get; set; }

        public int Id { get; set; }

        [Strainer(IsFilterable = true)]
        public string Text { get; set; }
    }
}
