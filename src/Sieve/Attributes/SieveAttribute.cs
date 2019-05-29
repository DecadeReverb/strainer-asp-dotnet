using Sieve.Models;
using System;

namespace Sieve.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SieveAttribute : Attribute, ISievePropertyMetadata
    {
        public string Name { get; set; }
        public string FullName => Name;
        public bool CanSort { get; set; }
        public bool CanFilter { get; set; }
    }
}
