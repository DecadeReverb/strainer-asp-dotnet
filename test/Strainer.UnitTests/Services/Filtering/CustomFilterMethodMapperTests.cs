using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.TestModels;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class CustomFilterMethodMapperTests
    {
        [Fact]
        public void Mapper_Adds_NewCustomMethod()
        {
            // Arrange
            var options = new StrainerOptions();
            var customFilterMethod = new CustomFilterMethod<Comment>
            {
                Function = context => context.Source.Where(c => c.DateCreated.Year > 2000),
                Name = "XXI-century-comments",
            };
            var mapper = new CustomFilterMethodMapper(options);

            // Act
            mapper.AddMap(customFilterMethod);
            var result = mapper.GetMethod<Comment>(customFilterMethod.Name);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(customFilterMethod);
        }
    }
}
