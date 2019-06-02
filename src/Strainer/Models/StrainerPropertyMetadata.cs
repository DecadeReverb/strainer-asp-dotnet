namespace Fluorite.Strainer.Models
{
	public class StrainerPropertyMetadata : IStrainerPropertyMetadata
    {
        public bool CanFilter { get; set; }
        public bool CanSort { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
    }
}
