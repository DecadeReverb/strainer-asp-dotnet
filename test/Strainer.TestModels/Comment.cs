using Fluorite.Strainer.Attributes;
using System;

namespace Fluorite.Strainer.TestModels
{
    public class Comment
    {
        [StrainerProperty(IsFilterable = true, IsSortable = true)]
        public DateTimeOffset DateCreated { get; set; }

        public int Id { get; set; }

        [StrainerProperty(IsFilterable = true)]
        public string Text { get; set; }
    }
}
