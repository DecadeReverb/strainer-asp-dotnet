﻿using Strainer.Models;
using System;

namespace Strainer.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StrainerAttribute : Attribute, IStrainerPropertyMetadata
    {
        public string Name { get; set; }
        public string FullName => Name;
        public bool CanSort { get; set; }
        public bool CanFilter { get; set; }
    }
}