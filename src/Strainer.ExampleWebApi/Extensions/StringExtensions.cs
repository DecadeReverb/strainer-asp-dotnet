using System;

namespace Fluorite.Extensions;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => input,
            _ => input[..1].ToString().ToUpper() + input[1..],
        };

}
