using System;

namespace Fluorite.Strainer.Services.Filter
{
    public class CustomFilterMethodProvider : ICustomFilterMethodProvider
    {
        public CustomFilterMethodProvider(ICustomFilterMethodMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            MapMethods(mapper);
        }

        public ICustomFilterMethodMapper Mapper { get; }

        protected virtual void MapMethods(ICustomFilterMethodMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
        }
    }
}
