using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class AppDomainAssemblySourceProvider : IMetadataAssemblySourceProvider
    {
        public Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
