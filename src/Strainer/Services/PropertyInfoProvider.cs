namespace Fluorite.Strainer.Services
{
    public class PropertyInfoProvider : IPropertyInfoProvider
    {
        private readonly IStrainerPropertyMapper _mapper;

        public PropertyInfoProvider(IStrainerPropertyMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
