namespace Sieve.Models
{
	public class SieveOptions
    {
        // TODO:
        // Add more options like:
        // - DefaultPageNumber
        // - MinPageSize
        // - DefaultFilterOperator

        public bool CaseSensitive { get; set; } = false;

        public int DefaultPageSize { get; set; } = 0;

        public int MaxPageSize { get; set; } = 0;

        public bool ThrowExceptions { get; set; } = false;
    }
}
