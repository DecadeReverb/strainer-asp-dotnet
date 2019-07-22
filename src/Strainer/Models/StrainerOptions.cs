namespace Fluorite.Strainer.Models
{
    /// <summary>
    /// Represents options used by Strainer.
    /// </summary>
	public class StrainerOptions
    {
        // TODO:
        // Add more options like:
        // - MinPageSize

        /// <summary>
        /// Initializes new instance of <see cref="StrainerOptions"/> class.
        /// </summary>
        public StrainerOptions()
        {

        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether
        /// Strainer should operatre in case sensitive mode.
        /// <para/>
        /// This affects for example the way of comparing value of incoming filters
        /// with names of properties marked as filterable.
        /// <para/>
        /// Defaults to <see langword="false"/>.
        /// </summary>
        public bool CaseSensitive { get; set; } = false;

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
        /// Gets or sets a maximum page number.
        /// <para/>
        /// Defaults to 50.
        /// </summary>
        public int MaxPageSize { get; set; } = 50;

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether
        /// Strainer should throw <see cref="Exceptions.StrainerException"/>
        /// and the like.
        /// <para/>
        /// Defaults to <see langword="false"/>.
        /// </summary>
        public bool ThrowExceptions { get; set; } = false;

        ///// <summary>
        ///// Gets or sets a <see cref="bool"/> value indictating whether
        ///// Strainer should check whether every entity with at least single
        ///// property marked as sortable has defined default sort expression
        ///// acting as a fallback ordering.
        ///// <para/>
        ///// When enabled an <see cref="Exceptions.StrainerSortingExpressionValidatorException"/>
        ///// is thrown for every entity without default sort expression.
        ///// <para/>
        ///// Defaults to <see langword="true"/>.
        ///// </summary>
        //public bool ValidateDefaultSortRules { get; set; } = true;
    }
}
