﻿using System.ComponentModel;

namespace Fluorite.Strainer.Services.Conversion;

public class ComponentModelTypeConverter : ITypeConverter
{
    private readonly TypeConverter _typeConverter;

    public ComponentModelTypeConverter(TypeConverter typeConverter)
    {
        _typeConverter = Guard.Against.Null(typeConverter);
    }

    public bool CanConvertFrom(Type type) => _typeConverter.CanConvertFrom(type);

    public object ConvertFrom(object value) => _typeConverter.ConvertFrom(value);
}
