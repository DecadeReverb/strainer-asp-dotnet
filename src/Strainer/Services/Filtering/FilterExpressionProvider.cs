using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Conversion;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterExpressionProvider : IFilterExpressionProvider
    {
        private readonly ITypeConverterProvider _typeConverterProvider;
        private readonly IFilterExpressionWorkflowBuilder _filterExpressionWorkflowBuilder;

        public FilterExpressionProvider(
            ITypeConverterProvider typeConverterProvider,
            IFilterExpressionWorkflowBuilder filterExpressionWorkflowBuilder)
        {
            _typeConverterProvider = typeConverterProvider;
            _filterExpressionWorkflowBuilder = filterExpressionWorkflowBuilder;
        }

        public Expression GetExpression(
            IPropertyMetadata metadata,
            IFilterTerm filterTerm,
            ParameterExpression parameterExpression,
            Expression innerExpression)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (filterTerm == null)
            {
                throw new ArgumentNullException(nameof(filterTerm));
            }

            if (parameterExpression == null)
            {
                throw new ArgumentNullException(nameof(parameterExpression));
            }

            if (filterTerm.Values == null)
            {
                return null;
            }

            var typeConverter = _typeConverterProvider.GetTypeConverter(metadata.PropertyInfo.PropertyType);
            var workflow = _filterExpressionWorkflowBuilder.BuildDefaultWorkflow();

            Expression propertyValueExpresssion = parameterExpression;

            foreach (var part in metadata.Name.Split('.'))
            {
                propertyValueExpresssion = Expression.PropertyOrField(propertyValueExpresssion, part);
            }

            return CreateInnerExpression(
                metadata,
                filterTerm,
                innerExpression,
                propertyValueExpresssion,
                workflow,
                typeConverter);
        }

        private Expression CreateInnerExpression(
            IPropertyMetadata metadata,
            IFilterTerm filterTerm,
            Expression innerExpression,
            Expression propertyValue,
            IFilterExpressionWorkflow workflow,
            ITypeConverter typeConverter)
        {
            foreach (var filterTermValue in filterTerm.Values)
            {
                var context = new FilterExpressionWorkflowContext
                {
                    FilterTermConstant = filterTermValue,
                    FilterTermValue = filterTermValue,
                    FinalExpression = null,
                    PropertyMetadata = metadata,
                    PropertyValue = propertyValue,
                    Term = filterTerm,
                    TypeConverter = typeConverter,
                };

                var expression = workflow.Run(context);

                if (innerExpression == null)
                {
                    innerExpression = expression;
                }
                else
                {
                    innerExpression = Expression.Or(innerExpression, expression);
                }
            }

            return innerExpression;
        }
    }
}
