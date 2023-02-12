using Fluorite.Strainer.Models.Configuration;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Configuration
{
    public interface IStrainerConfigurationBuilder
    {
        IStrainerConfiguration Build(IReadOnlyCollection<Type> moduleTypes);
    }
}
