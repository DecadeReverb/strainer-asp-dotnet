namespace Strainer.Models
{
	public class StrainerPropertyMetadata : IStrainerPropertyMetadata
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool CanFilter { get; set; }
        public bool CanSort { get; set; }
    }
}
