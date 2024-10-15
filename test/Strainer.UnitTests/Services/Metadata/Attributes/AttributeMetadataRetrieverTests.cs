using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Metadata.Attributes;
using NSubstitute.ReturnsExtensions;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Metadata.Attributes;

public class AttributeMetadataRetrieverTests
{
    private readonly IMetadataSourceChecker _metadataSourceCheckerMock = Substitute.For<IMetadataSourceChecker>();
    private readonly IAttributePropertyMetadataBuilder _attributePropertyMetadataBuilderMock = Substitute.For<IAttributePropertyMetadataBuilder>();
    private readonly IPropertyMetadataDictionaryProvider _propertyMetadataDictionaryProviderMock = Substitute.For<IPropertyMetadataDictionaryProvider>();
    private readonly IStrainerAttributeProvider _strainerAttributeProviderMock = Substitute.For<IStrainerAttributeProvider>();
    private readonly IPropertyInfoProvider _propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
    private readonly IAttributeCriteriaChecker _attributeCriteriaCheckerMock = Substitute.For<IAttributeCriteriaChecker>();

    private readonly AttributeMetadataRetriever _retriever;

    public AttributeMetadataRetrieverTests()
    {
        _retriever = new AttributeMetadataRetriever(
            _metadataSourceCheckerMock,
            _attributePropertyMetadataBuilderMock,
            _propertyMetadataDictionaryProviderMock,
            _strainerAttributeProviderMock,
            _propertyInfoProviderMock,
            _attributeCriteriaCheckerMock);
    }

    [Fact]
    public void Retriever_Returns_NullDefaultMetadata_ForObject_WhenObjectAttributeMetadataIsDisabled()
    {
        // Arrange
        var modelType = typeof(string);

        // Act
        var result = _retriever.GetDefaultMetadataFromObjectAttribute(modelType);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Retriever_Throws_WhenDefaultMetadata_ForObject_DoesNotExist()
    {
        // Arrange
        var modelType = typeof(string);
        var defaultSortingPropertyName = nameof(string.Length);
        var objectAttribute = new StrainerObjectAttribute(defaultSortingPropertyName)
        {
            IsSortable = false,
        };
        var propertyInfo = modelType.GetProperty(defaultSortingPropertyName);
        var defaultPropertyMetadata = new PropertyMetadata();

        _metadataSourceCheckerMock
            .IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes)
            .Returns(true);
        _strainerAttributeProviderMock
            .GetObjectAttribute(modelType)
            .Returns(objectAttribute);
        _propertyInfoProviderMock
            .GetPropertyInfo(modelType, objectAttribute.DefaultSortingPropertyName)
            .ReturnsNull();

        // Act
        Action act = () => _retriever.GetDefaultMetadataFromObjectAttribute(modelType);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Retriever_Returns_DefaultMetadata_ForObject()
    {
        // Arrange
        var modelType = typeof(string);
        var defaultSortingPropertyName = nameof(string.Length);
        var objectAttribute = new StrainerObjectAttribute(defaultSortingPropertyName);
        var propertyInfo = modelType.GetProperty(defaultSortingPropertyName);
        var defaultPropertyMetadata = new PropertyMetadata();

        _metadataSourceCheckerMock
            .IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes)
            .Returns(true);
        _strainerAttributeProviderMock
            .GetObjectAttribute(modelType)
            .Returns(objectAttribute);
        _propertyInfoProviderMock
            .GetPropertyInfo(modelType, objectAttribute.DefaultSortingPropertyName)
            .Returns(propertyInfo);
        _attributePropertyMetadataBuilderMock
            .BuildDefaultPropertyMetadata(objectAttribute, propertyInfo)
            .Returns(defaultPropertyMetadata);

        // Act
        var result = _retriever.GetDefaultMetadataFromObjectAttribute(modelType);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(defaultPropertyMetadata);
    }

    [Fact]
    public void Retriever_Returns_NullDefaultMetadata_ForProperty_WhenPropertyAttributeMetadataIsDisabled()
    {
        // Arrange
        var modelType = typeof(string);

        // Act
        var result = _retriever.GetDefaultMetadataFromPropertyAttribute(modelType);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Retriever_Throws_WhenGettingDefaultMetadata_ForProperty_ThatIsNotSortable()
    {
        // Arrange
        var modelType = typeof(string);
        var propertyInfo = modelType.GetProperty(nameof(string.Length));
        var propertyAttribute = new StrainerPropertyAttribute()
        {
            IsDefaultSorting = true,
            IsSortable = false,
            PropertyInfo = propertyInfo,
        };
        var propertyInfos = new[] { propertyInfo };

        _metadataSourceCheckerMock
            .IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes)
            .Returns(true);
        _propertyInfoProviderMock
            .GetPropertyInfos(modelType)
            .Returns(propertyInfos);
        _strainerAttributeProviderMock
            .GetPropertyAttribute(propertyInfo)
            .Returns(propertyAttribute);

        // Act
        Action act = () => _retriever.GetDefaultMetadataFromPropertyAttribute(modelType);

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage(
                $"Property * " +
                $"is declared as {nameof(IPropertyMetadata.IsDefaultSorting)} " +
                $"but not as {nameof(IPropertyMetadata.IsSortable)}. " +
                $"Set the {nameof(IPropertyMetadata.IsSortable)} to true " +
                $"in order to use the property as a default sortable property.");
    }

    [Fact]
    public void Retriever_Returns_DefaultMetadata_ForProperty()
    {
        // Arrange
        var modelType = typeof(string);
        var defaultSortingPropertyName = nameof(string.Length);
        var propertyAttribute = new StrainerPropertyAttribute()
        {
            IsDefaultSorting = true,
            IsSortable = true,
        };
        var propertyInfo = modelType.GetProperty(defaultSortingPropertyName);
        var propertyInfos = new[] { propertyInfo };

        _metadataSourceCheckerMock
            .IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes)
            .Returns(true);
        _propertyInfoProviderMock
            .GetPropertyInfos(modelType)
            .Returns(propertyInfos);
        _strainerAttributeProviderMock
            .GetPropertyAttribute(propertyInfo)
            .Returns(propertyAttribute);

        // Act
        var result = _retriever.GetDefaultMetadataFromPropertyAttribute(modelType);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(propertyAttribute);
    }

    [Fact]
    public void Retriever_Returns_MetadataDictionary_FromObjects()
    {
        // Arrange
        var validType = typeof(string);
        var invalidType = typeof(object);
        var types = new[] { validType, invalidType };
        var defaultSortingPropertyName = nameof(string.Length);
        var objectAttribute = new StrainerObjectAttribute(defaultSortingPropertyName);
        var propertyMetadata = new PropertyMetadata();
        var metadataDictionary = new Dictionary<string, IPropertyMetadata>
        {
            [defaultSortingPropertyName] = propertyMetadata,
        };

        _strainerAttributeProviderMock
            .GetObjectAttribute(validType)
            .Returns(objectAttribute);
        _strainerAttributeProviderMock
            .GetObjectAttribute(invalidType)
            .ReturnsNull();
        _propertyMetadataDictionaryProviderMock
            .GetMetadata(validType, objectAttribute)
            .Returns(metadataDictionary);
        _metadataSourceCheckerMock
            .IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes)
            .Returns(true);

        // Act
        var result = _retriever.GetMetadataDictionaryFromObjectAttributes(types);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Keys.Should().BeEquivalentTo(new[] { validType });
        result.Values.Single().Should().BeSameAs(metadataDictionary);
    }

    [Fact]
    public void Retriever_Returns_MetadataDictionary_FromProperties()
    {
        // Arrange
        var validType = typeof(string);
        var invalidType = typeof(object);
        var types = new[] { validType, invalidType };
        var defaultSortingPropertyName = nameof(string.Length);
        var propertyMetadata = new PropertyMetadata();
        var metadataDictionary = new Dictionary<string, IPropertyMetadata>
        {
            [defaultSortingPropertyName] = propertyMetadata,
        };

        _propertyMetadataDictionaryProviderMock
            .GetMetadata(validType)
            .Returns(metadataDictionary);
        _propertyMetadataDictionaryProviderMock
            .GetMetadata(invalidType)
            .Returns(new Dictionary<string, IPropertyMetadata>());
        _metadataSourceCheckerMock
            .IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes)
            .Returns(true);

        // Act
        var result = _retriever.GetMetadataDictionaryFromPropertyAttributes(types);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Keys.Should().BeEquivalentTo(new[] { validType });
        result.Values.Single().Should().BeSameAs(metadataDictionary);
    }

    [Fact]
    public void Retriever_Returns_NullPropertyMetadata_WhenAttributeMetadataSourceIsDisabled()
    {
        // Arrange
        var name = "foo";
        var isSortableRequired = false;
        var isFilterableRequired = false;

        _metadataSourceCheckerMock
            .IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes)
            .Returns(false);

        // Act
        var result = _retriever.GetMetadataFromPropertyAttribute(typeof(string), isSortableRequired, isFilterableRequired, name);

        // Assert
        result.Should().BeNull();

        _metadataSourceCheckerMock
            .Received(1)
            .IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes);
    }

    [Fact]
    public void Retriever_Returns_NullPropertyMetadata_When_PropertyInfoProvider_ReturnsNoResults()
    {
        // Arrange
        var name = "foo";
        var isSortableRequired = false;
        var isFilterableRequired = false;

        _metadataSourceCheckerMock
            .IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes)
            .Returns(true);
        _propertyInfoProviderMock
            .GetPropertyInfos(typeof(string))
            .Returns(Array.Empty<PropertyInfo>());

        // Act
        var result = _retriever.GetMetadataFromPropertyAttribute(typeof(string), isSortableRequired, isFilterableRequired, name);

        // Assert
        result.Should().BeNull();

        _metadataSourceCheckerMock
            .Received(1)
            .IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes);
    }

    [Fact]
    public void Retriever_Returns_NullPropertyMetadata_When_CriteriaCheckerMatchesNoResults()
    {
        // Arrange
        var name = "foo";
        var isSortableRequired = false;
        var isFilterableRequired = false;
        var propertyInfoMock = Substitute.For<PropertyInfo>();
        var propertyInfos = new[] { propertyInfoMock };
        var attributeMock = Substitute.For<StrainerPropertyAttribute>();

        _metadataSourceCheckerMock
            .IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes)
            .Returns(true);
        _propertyInfoProviderMock
            .GetPropertyInfos(typeof(string))
            .Returns(propertyInfos);
        _strainerAttributeProviderMock
            .GetPropertyAttribute(propertyInfoMock)
            .Returns(attributeMock);
        _attributeCriteriaCheckerMock
            .CheckIfPropertyAttributeIsMatching(attributeMock, propertyInfoMock, isSortableRequired, isFilterableRequired, name)
            .Returns(false);

        // Act
        var result = _retriever.GetMetadataFromPropertyAttribute(typeof(string), isSortableRequired, isFilterableRequired, name);

        // Assert
        result.Should().BeNull();

        _metadataSourceCheckerMock
            .Received(1)
            .IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes);
        _propertyInfoProviderMock
            .Received(1)
            .GetPropertyInfos(typeof(string));
        _strainerAttributeProviderMock
            .Received(1)
            .GetPropertyAttribute(propertyInfoMock);
        _attributeCriteriaCheckerMock
            .Received(1)
            .CheckIfPropertyAttributeIsMatching(attributeMock, propertyInfoMock, isSortableRequired, isFilterableRequired, name);
    }

    [Fact]
    public void Retriever_Returns_PropertyMetadata()
    {
        // Arrange
        var name = "foo";
        var isSortableRequired = false;
        var isFilterableRequired = false;
        var propertyInfoMock = Substitute.For<PropertyInfo>();
        var propertyInfos = new[] { propertyInfoMock };
        var attributeMock = Substitute.For<StrainerPropertyAttribute>();

        _metadataSourceCheckerMock
            .IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes)
            .Returns(true);
        _propertyInfoProviderMock
            .GetPropertyInfos(typeof(string))
            .Returns(propertyInfos);
        _strainerAttributeProviderMock
            .GetPropertyAttribute(propertyInfoMock)
            .Returns(attributeMock);
        _attributeCriteriaCheckerMock
            .CheckIfPropertyAttributeIsMatching(attributeMock, propertyInfoMock, isSortableRequired, isFilterableRequired, name)
            .Returns(true);

        // Act
        var result = _retriever.GetMetadataFromPropertyAttribute(typeof(string), isSortableRequired, isFilterableRequired, name);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(attributeMock);

        _metadataSourceCheckerMock
            .Received(1)
            .IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes);
        _propertyInfoProviderMock
            .Received(1)
            .GetPropertyInfos(typeof(string));
        _strainerAttributeProviderMock
            .Received(1)
            .GetPropertyAttribute(propertyInfoMock);
        _attributeCriteriaCheckerMock
            .Received(1)
            .CheckIfPropertyAttributeIsMatching(attributeMock, propertyInfoMock, isSortableRequired, isFilterableRequired, name);
    }
}
