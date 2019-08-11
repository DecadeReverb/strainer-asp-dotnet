//using System;

//namespace Fluorite.Strainer.Services.Sorting
//{
//    public class CustomSortMethodProvider : ICustomSortMethodProvider
//    {
//        public CustomSortMethodProvider(ICustomSortMethodMapper mapper)
//        {
//            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

//            MapMethods(mapper);
//        }

//        public ICustomSortMethodMapper Mapper { get; }

//        protected virtual void MapMethods(ICustomSortMethodMapper mapper)
//        {
//            if (mapper == null)
//            {
//                throw new ArgumentNullException(nameof(mapper));
//            }
//        }
//    }
//}
