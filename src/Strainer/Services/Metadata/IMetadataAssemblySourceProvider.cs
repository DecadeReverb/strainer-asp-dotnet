using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IMetadataAssemblySourceProvider
    {
        Assembly[] GetAssemblies();
    }
}
