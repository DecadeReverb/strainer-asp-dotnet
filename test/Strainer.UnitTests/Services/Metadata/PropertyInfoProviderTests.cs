﻿using FluentAssertions;
using Fluorite.Strainer.Services.Metadata;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Metadata
{
    public class PropertyInfoProviderTests
    {
        [Fact]
        public void Provider_Works_For_Property()
        {
            // Arrange
            Expression<Func<Stub, object>> expression = s => s.Property;
            var provider = new PropertyInfoProvider();

            // Act
            var (propertyInfo, fullName) = provider.GetPropertyInfoAndFullName(expression);

            // Assert
            fullName.Should().Be(nameof(Stub.Property));
            propertyInfo.Should().NotBeNull();
            propertyInfo.Should().BeSameAs(typeof(Stub).GetProperty(nameof(Stub.Property)));
        }

        [Fact]
        public void Provider_Works_For_Property_With_Only_Getter()
        {
            // Arrange
            Expression<Func<Stub, object>> expression = s => s.PropertyOnlyGetter;
            var provider = new PropertyInfoProvider();

            // Act
            var (propertyInfo, fullName) = provider.GetPropertyInfoAndFullName(expression);

            // Assert
            fullName.Should().Be(nameof(Stub.PropertyOnlyGetter));
            propertyInfo.Should().NotBeNull();
            propertyInfo.Should().BeSameAs(typeof(Stub).GetProperty(nameof(Stub.PropertyOnlyGetter)));
        }

        [Fact]
        public void Provider_Does_Not_Work_For_Field()
        {
            // Arrange
            Expression<Func<Stub, object>> expression = (Stub v) => v.Field;
            var provider = new PropertyInfoProvider();

            // Act & Assert
            Action result = () => provider.GetPropertyInfoAndFullName(expression);
            result.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Provider_Does_Not_Work_For_Method()
        {
            // Arrange
            Expression<Func<Stub, object>> expression = (Stub v) => v.Method();
            var provider = new PropertyInfoProvider();

            // Act & Assert
            Action result = () => provider.GetPropertyInfoAndFullName(expression);
            result.Should().ThrowExactly<ArgumentException>();
        }

        private class Stub
        {
            public readonly int Field = 0;

            public int Property { get; set; }

            public int PropertyOnlyGetter { get; }

            public int Method() => default;
        }
    }
}
