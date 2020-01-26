using FluentAssertions;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Sorting;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class CustomSortMethodMapperTests
    {
        [Fact]
        public void Mapper_Adds_NewCustomMethod()
        {
            // Arrange
            var customSortMethod = new CustomSortMethod<Uri>
            {
                Function = (source, isDescending, isSubsequent) => source
                    .OrderBy(uri => uri.Port)
                    .ThenBy(uri => uri.Host),
                Name = "PortThenHost",
            };
            var mapper = new CustomSortMethodMapper();

            // Act
            mapper.AddMap(customSortMethod);

            // Assert
            mapper.Methods.ContainsKey(typeof(Uri)).Should().BeTrue();
            mapper.Methods[typeof(Uri)].Should().NotBeEmpty();
            mapper.Methods[typeof(Uri)].ContainsKey(customSortMethod.Name).Should().BeTrue();
            mapper.Methods[typeof(Uri)][customSortMethod.Name].Should().Be(customSortMethod);
        }
    }
}
