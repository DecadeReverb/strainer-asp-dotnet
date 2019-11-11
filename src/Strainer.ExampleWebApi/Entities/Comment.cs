using Fluorite.Strainer.Attributes;

namespace Fluorite.Strainer.ExampleWebApi.Entities
{
    [StrainerObject(nameof(Id))]
    public class Comment
    {
        public Comment()
        {

        }

        public int Id { get; set; }

        public string Message { get; set; }

        public Post Post { get; set; }

        public int PostId { get; set; }
    }
}
