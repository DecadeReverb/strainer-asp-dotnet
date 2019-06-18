namespace Fluorite.Strainer.Services.Filtering
{
    public abstract class CustomFilterMethodProvider : ICustomFilterMethodProvider
    {
        public CustomFilterMethodProvider(ICustomFilterMethodMapper mapper)
        {
            Mapper = mapper;

            MapMethods(mapper);
        }

        public ICustomFilterMethodMapper Mapper { get; }

        public abstract void MapMethods(ICustomFilterMethodMapper mapper);
    }
}
