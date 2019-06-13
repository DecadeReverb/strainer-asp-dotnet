using Microsoft.Extensions.DependencyInjection;

namespace Fluorite.Strainer.Models
{
	public class StrainerOptions
    {
        // TODO:
        // Add more options like:
        // - DefaultPageNumber
        // - MinPageSize
        // - DefaultFilterOperator

        public StrainerOptions()
        {
            ServiceLifetime = ServiceLifetime.Scoped;
        }

        public bool CaseSensitive { get; set; }

        public int DefaultPageSize { get; set; }

        public int MaxPageSize { get; set; }

        public ServiceLifetime ServiceLifetime { get; set; }

        public bool ThrowExceptions { get; set; }
    }
}
