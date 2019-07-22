using System;

namespace Fluorite.Strainer.Services.Sorting
{
    public class CustomSortingMethodProvider : ICustomSortingMethodProvider
    {
        public CustomSortingMethodProvider(ICustomSortingMethodMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            MapMethods(mapper);
        }

        public ICustomSortingMethodMapper Mapper { get; }

        protected virtual void MapMethods(ICustomSortingMethodMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
        }
    }
}
