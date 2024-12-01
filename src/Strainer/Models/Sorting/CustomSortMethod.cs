namespace Fluorite.Strainer.Models.Sorting;

public class CustomSortMethod : ICustomSortMethod
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomSortMethod"/> class.
    /// </summary>
    /// <param name="name">
    /// The custom method name.
    /// </param>
    public CustomSortMethod(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }

    /// <inheritdoc/>
    public string Name { get; }
}
