using System;
using System.Collections.Generic;
using SimpleEntityUpdater.Interfaces;
using SimpleEntityUpdater.Models;

namespace SimpleEntityUpdater.Logic
{
    internal class IdSelectorImpl<TSource, TProperty> : IIdSelector<TSource, TProperty> where TSource : class
    {
        private readonly Func<TSource, IEnumerable<TProperty>> _propertySelector;

        private readonly Action<PropertyMapperConfig> _callback;

        public IdSelectorImpl(Func<TSource, IEnumerable<TProperty>> propertySelector, Action<PropertyMapperConfig> callback)
        {
            _propertySelector = propertySelector;
            _callback = callback;
        }

        public void IdSelector<TId>(Func<TProperty, TId> idSelector)
        {
            return new MapConfigComparatorMany<TSource, IEnumerable<TProperty>, TId>(_propertySelector, _callback, idSelector);
        }
    }
}