﻿using Fluorite.Strainer.Models.Filtering;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface ICustomFilterMethodMapper
    {
        void AddMap<TEntity>(ICustomFilterMethod<TEntity> sortMethod);
        ICustomFilterMethod<TEntity> GetMethod<TEntity>(string name);
        ICustomFilterMethodBuilder<TEntity> CustomMethod<TEntity>(string name);
    }
}
