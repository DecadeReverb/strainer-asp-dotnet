using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Conversion;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterExpressionWorkflowContext
{
    public object FilterTermConstant { get; set; }

    public string FilterTermValue { get; set; }

    public Expression FinalExpression { get; set; }

    public IPropertyMetadata PropertyMetadata { get; set; }

    public Expression PropertyValue { get; set; }

    public IFilterTerm Term { get; set; }

    public ITypeConverter TypeConverter { get; set; }
}
