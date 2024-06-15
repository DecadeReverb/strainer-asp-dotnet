using Fluorite.Extensions;

namespace Fluorite.Strainer.UnitTests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void TrimEnd_Returns_String_Without_TrimmedValue()
    {
        // Arrange
        var value = "foo";
        var subValue = "o";

        // Act
        var result = value.TrimEnd(subValue);

        // Assert
        result.Should().Be("f");
    }

    [Fact]
    public void TrimEnd_WorksWithEmptyStrings()
    {
        // Arrange
        var value = string.Empty;
        var subValue = string.Empty;

        // Act
        var result = value.TrimEnd(subValue);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void TrimEnd_ThrowsException_WhenCurrentInstanceIsNull()
    {
        // Arrange
        string value = null;
        var subValue = "foo";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => value.TrimEnd(subValue));
    }

    [Fact]
    public void TrimEnd_ThrowsException_WhenArgumentInstanceIsNull()
    {
        // Arrange
        var value = "foo";
        string subValue = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => value.TrimEnd(subValue));
    }

    [Fact]
    public void TrimEndOnce_Returns_String_Without_TrimmedValue()
    {
        // Arrange
        var value = "foo";
        var subValue = "o";

        // Act
        var result = value.TrimEndOnce(subValue);

        // Assert
        result.Should().Be("fo");
    }

    [Fact]
    public void TrimEndOnce_WorksWithEmptyStrings()
    {
        // Arrange
        var value = string.Empty;
        var subValue = string.Empty;

        // Act
        var result = value.TrimEndOnce(subValue);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void TrimEndOnce_ThrowsException_WhenCurrentInstanceIsNull()
    {
        // Arrange
        string value = null;
        var subValue = "foo";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => value.TrimEndOnce(subValue));
    }

    [Fact]
    public void TrimEndOnce_ThrowsException_WhenArgumentInstanceIsNull()
    {
        // Arrange
        var value = "foo";
        string subValue = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => value.TrimEndOnce(subValue));
    }


    [Fact]
    public void TrimStart_Returns_String_Without_TrimmedValue()
    {
        // Arrange
        var value = "ffo";
        var subValue = "f";

        // Act
        var result = value.TrimStart(subValue);

        // Assert
        result.Should().Be("o");
    }

    [Fact]
    public void TrimStart_WorksWithEmptyStrings()
    {
        // Arrange
        var value = string.Empty;
        var subValue = string.Empty;

        // Act
        var result = value.TrimStart(subValue);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void TrimStart_ThrowsException_WhenCurrentInstanceIsNull()
    {
        // Arrange
        string value = null;
        var subValue = "foo";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => value.TrimStart(subValue));
    }

    [Fact]
    public void TrimStart_ThrowsException_WhenArgumentInstanceIsNull()
    {
        // Arrange
        var value = "foo";
        string subValue = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => value.TrimStart(subValue));
    }

    [Fact]
    public void TrimStartOnce_Returns_String_Without_TrimmedValue()
    {
        // Arrange
        var value = "ffoo";
        var subValue = "f";

        // Act
        var result = value.TrimStartOnce(subValue);

        // Assert
        result.Should().Be("foo");
    }

    [Fact]
    public void TrimStartOnce_WorksWithEmptyStrings()
    {
        // Arrange
        var value = string.Empty;
        var subValue = string.Empty;

        // Act
        var result = value.TrimStartOnce(subValue);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void TrimStartOnce_ThrowsException_WhenCurrentInstanceIsNull()
    {
        // Arrange
        string value = null;
        var subValue = "foo";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => value.TrimStartOnce(subValue));
    }

    [Fact]
    public void TrimStartOnce_ThrowsException_WhenArgumentInstanceIsNull()
    {
        // Arrange
        var value = "foo";
        string subValue = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => value.TrimStartOnce(subValue));
    }
}
