using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface ICustomFilterMethodProvider
    {
        ICustomFilterMethodMapper Mapper { get; }

        void MapMethods(ICustomFilterMethodMapper mapper);
    }
}
