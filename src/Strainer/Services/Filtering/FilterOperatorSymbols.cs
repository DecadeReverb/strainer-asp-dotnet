namespace Fluorite.Strainer.Services.Filtering;

public static class FilterOperatorSymbols
{
    public const string EqualsSymbol = "==";
    public const string DoesNotEqual = "!=";
    public const string GreaterThan = ">";
    public const string LessThan = "<";
    public const string GreaterThanOrEqualTo = ">=";
    public const string LessThanOrEqualTo = "<=";
    public const string Contains = "@=";
    public const string StartsWith = "_=";
    public const string EndsWith = "=_";
    public const string DoesNotContain = "!@=";
    public const string DoesNotStartWith = "!_=";
    public const string DoesNotEndWith = "!=_";
    public const string ContainsCaseInsensitive = "@=*";
    public const string StartsWithCaseInsensitive = "_=*";
    public const string EndsWithCaseInsensitive = "=_*";
    public const string EqualsCaseInsensitive = "==*";
    public const string DoesNotEqualCaseInsensitive = "!=*";
    public const string DoesNotContainCaseInsensitive = "!@=*";
    public const string DoesNotStartWithCaseInsensitive = "!_=*";
    public const string DoesNotEndWithCaseInsensitive = "!=_*";
}
