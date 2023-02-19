using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class AppDomainaAssemblySourceProvider : IMetadataAssemblySourceProvider
    {
        public Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
