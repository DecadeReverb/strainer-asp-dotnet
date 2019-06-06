using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorParser : IFilterOperatorParser
    {
        public FilterOperatorParser(IFilterOperatorProvider operatorProvider)
        {
            OperatorProvider = operatorProvider;
        }

        protected IFilterOperatorProvider OperatorProvider { get; }

        public virtual IFilterOperator GetParsedOperator(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return OperatorProvider.GetDefaultOperator();
            }

            input = input.Trim();

            // TODO:
            // Store somewhere info about case insensitivity asterisk suffix
            // and negation exclamation mark prefix.
            return OperatorProvider.FirstOrDefault(f => f.Operator == input.TrimEnd('*')) // Case sensivity variations;
                ?? OperatorProvider.GetDefaultOperator();
        }

        public virtual IFilterOperator GetParsedOperatorAsUnnegated(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(
                    $"{nameof(input)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(input));
            }

            input = input.Trim();

            // TODO:
            // Store somewhere info about case insensitivity asterisk suffix
            // and negation exclamation mark prefix.
            return OperatorProvider.FirstOrDefault(f =>
            {
                return f.Operator == input.TrimEnd('*')     // Case sensivity variations
                    || f.Operator == input.TrimStart('!');  // Negated variations
            }) ?? OperatorProvider.GetDefaultOperator();
        }
    }
}
