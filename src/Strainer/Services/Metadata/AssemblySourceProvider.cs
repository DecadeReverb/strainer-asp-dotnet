using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata;

public class AssemblySourceProvider : IMetadataAssemblySourceProvider
{
    private readonly Assembly[] _assemblies;

    public AssemblySourceProvider(Assembly[] assemblies)
    {
        _assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
    }

    public Assembly[] GetAssemblies() => _assemblies;
}
