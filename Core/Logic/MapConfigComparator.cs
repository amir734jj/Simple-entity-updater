using System;
using SimpleEntityUpdater.Interfaces;
using SimpleEntityUpdater.Models;

namespace SimpleEntityUpdater.Logic
{
    internal class MapConfigComparator<TSource, TProperty> : IMapConfigComparator<TSource, TProperty> where TSource : class
    {
        private readonly Func<TSource, TProperty> _propertySelector;
        
        private readonly Action<PropertyMapperConfig> _callback;

        public MapConfigComparator(Func<TSource, TProperty> propertySelector, Action<PropertyMapperConfig> callback)
        {
            _propertySelector = propertySelector;
            _callback = callback;
        }

        public IMapConfigAssignment<TSource, TProperty> DefaultComparator()
        {
            return new MapConfigAssignment<TSource, TProperty>(_propertySelector, (x, y) => Equals(x, y), _callback);
        }

        public IMapConfigAssignment<TSource, TProperty> Comparator(Func<TProperty, TProperty, bool> compare)
        {
            return new MapConfigAssignment<TSource, TProperty>(_propertySelector, compare, _callback);
        }
    }
}