namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortMethodProvider
    {
        ICustomSortMethodMapper Mapper { get; }

        void MapMethods(ICustomSortMethodMapper mapper);
    }
}
