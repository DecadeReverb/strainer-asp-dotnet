using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata;

public class MetadataSourceTypeProvider : IMetadataSourceTypeProvider
{
    public IList<Type> GetSourceTypes(Assembly[] assemblies)
    {
        Guard.Against.Null(assemblies);

        // TODO: Check if TraceDataCollector is still causing an issue.
        return assemblies
            .Where(assembly => !assembly.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector"))
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass || type.IsValueType)
            .ToList();
    }
}
