namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortingMethodProvider
    {
        ICustomSortingMethodMapper Mapper { get; }
    }
}
