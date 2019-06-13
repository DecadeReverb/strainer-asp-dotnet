using Fluorite.Strainer.Models.Filtering.Operators;

namespace Fluorite.Strainer.ExampleWebApi.Services
{
    public class NotEqualsCaseInsensitiveOperator : FilterOperator
    {
        public NotEqualsCaseInsensitiveOperator() : base(name: "not equals (case insensitive)", symbol: "!=*")
        {

        }

        public override bool IsCaseInsensitive => true;

        public override bool IsNegated => false;
    }
}
