using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IPropertyMetadataDictionary :
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>,
        IReadOnlyCollection<KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>>,
        IEnumerable<KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>>,
        IEnumerable
    {

    }
}
