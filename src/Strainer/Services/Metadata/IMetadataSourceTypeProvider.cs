using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata;

public interface IMetadataSourceTypeProvider
{
    IList<Type> GetSourceTypes(Assembly[] assemblies);
}
