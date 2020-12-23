using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Models
{
    /// <summary>
    /// Represents options used by Strainer.
    /// </summary>
	public class StrainerOptions
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerOptions"/> class.
        /// </summary>
        public StrainerOptions()
        {

        }

        /// <summary>
        /// Gets or sets a default page number.
        /// <para/>
        /// Defaults to 1.
        /// </summary>
        public int DefaultPageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets a default page size.
        /// <para/>
        /// Defaults to 10.
        /// </summary>
        public int DefaultPageSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets a default sorting way used when applying default
        /// sorting.
        /// <para/>
        /// Defaults to <see cref="SortingWay.Ascending"/>.
        /// </summary>
        public SortingWay DefaultSortingWay { get; set; } = SortingWay.Ascending;

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether
        /// Strainer should operatre in case insensitive mode when comparing values.
        /// <para/>
        /// This for example affects the way of comparing value of incoming filter
        /// with actual property value.
        /// <para/>
        /// Defaults to <see langword="false"/>.
        /// </summary>
        public bool IsCaseInsensitiveForValues { get; set; } = false;

        /// <summary>
        /// Gets or sets a maximum page number.
        /// <para/>
        /// Defaults to 50.
        /// </summary>
        public int MaxPageSize { get; set; } = 50;

        /// <summary>
        /// Defines the type of sources Strainer will look through
        /// when obtaining property metadata.
        /// <para/>
        /// Note: narrowing down type of metadata source can speed up Strainer
        /// processing performance.
        /// <para/>
        /// Defaults to <see cref="MetadataSourceType.All"/>.
        /// </summary>
        public MetadataSourceType MetadataSourceType { get; set; } = MetadataSourceType.All;

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether
        /// Strainer should throw <see cref="Exceptions.StrainerException"/>
        /// and the like.
        /// <para/>
        /// Defaults to <see langword="false"/>.
        /// </summary>
        public bool ThrowExceptions { get; set; } = false;
    }
}
