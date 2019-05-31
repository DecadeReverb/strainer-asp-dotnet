﻿using System.Collections.Generic;
using Sieve.Models;

namespace Sieve.Services.Filtering
{
    public interface IFilterTermParser
    {
        IList<IFilterTerm> GetParsedFilterTerms(string input);
    }
}