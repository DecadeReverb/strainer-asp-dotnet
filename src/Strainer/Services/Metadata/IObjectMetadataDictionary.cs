using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IObjectMetadataDictionary :
        IReadOnlyDictionary<Type, IObjectMetadata>,
        IReadOnlyCollection<KeyValuePair<Type, IObjectMetadata>>,
        IEnumerable<KeyValuePair<Type, IObjectMetadata>>,
        IEnumerable
    {

    }
}
