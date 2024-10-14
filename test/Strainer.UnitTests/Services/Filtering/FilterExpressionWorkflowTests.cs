using Fluorite.Strainer.Services.Filtering;
using System.Linq.Expressions;

namespace Fluorite.Strainer.UnitTests.Services.Filtering;

public class FilterExpressionWorkflowTests
{
    [Fact]
    public void Should_Execute_EveryStep()
    {
        // Arrange
        var stepMock = Substitute.For<IFilterExpressionWorkflowStep>();
        var steps = new[] { stepMock };
        var workflow = new FilterExpressionWorkflow(steps);
        var context = new FilterExpressionWorkflowContext
        {
            FinalExpression = Expression.Constant(true),
        };

        // Act
        var expression = workflow.Run(context);

        // Assert
        expression.Should().Be(context.FinalExpression);

        stepMock
            .Received(1)
            .Execute(context);
    }
}
