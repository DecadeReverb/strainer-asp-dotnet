namespace Fluorite.Strainer.Models
{
	public class StrainerOptions
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
