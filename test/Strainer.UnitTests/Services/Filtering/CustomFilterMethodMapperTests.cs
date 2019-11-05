using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Moq;
using System;
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
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var customFilterMethod = new CustomFilterMethod<Uri>
            {
                Function = context => context.Source.Where(uri => uri.Port == 443),
                Name = "URIs with HTTP port",
            };
            var mapper = new CustomFilterMethodMapper(optionsProvider);

            // Act
            mapper.AddMap(customFilterMethod);
            var result = mapper.GetMethod<Uri>(customFilterMethod.Name);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(customFilterMethod);
        }
    }
}
