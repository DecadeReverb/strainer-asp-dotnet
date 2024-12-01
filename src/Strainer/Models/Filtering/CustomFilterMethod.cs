namespace Fluorite.Strainer.Models.Filtering;

public class CustomFilterMethod : ICustomFilterMethod
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFilterMethod"/>
    /// class.
    /// </summary>
    public CustomFilterMethod(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }

    /// <summary>
    /// Gets the custom method name.
    /// </summary>
    public string Name { get; }
}
