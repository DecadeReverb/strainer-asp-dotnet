using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.UnitTests.Services.Configuration;

public class StrainerModuleFactoryTests
{
    private readonly StrainerModuleFactory _factory;

    public StrainerModuleFactoryTests()
    {
        _factory = new StrainerModuleFactory();
    }

    [Fact]
    public void Should_Throw_ForNullType()
    {
        // Action
        Action act = () => _factory.CreateModule(moduleType: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Should_Throw_ForNotModuleType()
    {
        // Action
        Action act = () => _factory.CreateModule(typeof(object));

        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage(
               $"Provider module type {typeof(object).FullName} is not implementing {nameof(IStrainerModule)}.*");
    }

    [Fact]
    public void Should_Throw_ForUnconstructableType()
    {
        // Action
        Action act = () => _factory.CreateModule(typeof(UnconstructableStrainerModule));

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(
                $"Unable to create instance of {typeof(UnconstructableStrainerModule).FullName}. " +
                $"Ensure that type provides parameterless constructor.");
    }

    [Fact]
    public void Should_Return_CreatedModule()
    {
        // Action
        var result = _factory.CreateModule(typeof(TestStrainerModule));

        // Assert
        result.Should().NotBeNull();
    }

    private class UnconstructableStrainerModule : StrainerModule
    {
        public UnconstructableStrainerModule(object _)
        {
        }

        public override void Load(IStrainerModuleBuilder builder)
        {
        }
    }

    private class TestStrainerModule : StrainerModule
    {
        public override void Load(IStrainerModuleBuilder builder)
        {
        }
    }
}
