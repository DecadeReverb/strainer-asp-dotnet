using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IDefaultMetadataDictionary :
        IReadOnlyDictionary<Type, IPropertyMetadata>,
        IReadOnlyCollection<KeyValuePair<Type, IPropertyMetadata>>,
        IEnumerable<KeyValuePair<Type, IPropertyMetadata>>,
        IEnumerable
    {

    }
}
