using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorDictionary :
        IFilterOperatorDictionary,
        IReadOnlyDictionary<string, IFilterOperator>,
        IReadOnlyCollection<KeyValuePair<string, IFilterOperator>>,
        IEnumerable<KeyValuePair<string, IFilterOperator>>,
        IEnumerable
    {
        private readonly IDictionary<string, IFilterOperator> _filterOperators;

        public FilterOperatorDictionary(IDictionary<string, IFilterOperator> filterOperators)
        {
            _filterOperators = filterOperators ?? throw new ArgumentNullException(nameof(filterOperators));
        }

        public IFilterOperator this[string key] => _filterOperators[key];

        public int Count => _filterOperators.Count;

        public IEnumerable<string> Keys => _filterOperators.Keys;

        public IEnumerable<IFilterOperator> Values => _filterOperators.Values;

        public bool ContainsKey(string key)
        {
            return _filterOperators.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, IFilterOperator>> GetEnumerator()
        {
            return _filterOperators.GetEnumerator();
        }

        public bool TryGetValue(string key, out IFilterOperator value)
        {
            return _filterOperators.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _filterOperators.GetEnumerator();
        }
    }
}
