namespace Fluorite.Strainer.Models.Sorting;

public class CustomSortMethod : ICustomSortMethod
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomSortMethod"/>
    /// class.
    /// </summary>
    public CustomSortMethod()
    {

    }

    /// <inheritdoc/>
    public string Name { get; set; }
}
