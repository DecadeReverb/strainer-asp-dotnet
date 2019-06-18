namespace Fluorite.Strainer.Services.Sorting
{
    public abstract class CustomSortMethodProvider : ICustomSortMethodProvider
    {
        public CustomSortMethodProvider(ICustomSortMethodMapper mapper)
        {
            Mapper = mapper;

            MapMethods(mapper);
        }

        public ICustomSortMethodMapper Mapper { get; }

        public abstract void MapMethods(ICustomSortMethodMapper mapper);
    }
}
