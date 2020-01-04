using System;
using SimpleEntityUpdater.Interfaces;
using SimpleEntityUpdater.Models;

namespace SimpleEntityUpdater.Logic
{
    internal class MapConfigAssignment<TSource, TProperty> : IMapConfigAssignment<TSource, TProperty> where TSource : class
    {
        private readonly Func<TProperty, TProperty, bool> _compare;

        private Action<TSource, TProperty> _assign;
        
        private readonly Func<TSource, TProperty> _propertySelector;
        
        private readonly Action<PropertyMapperConfig> _callback;

        public MapConfigAssignment(Func<TSource, TProperty> propertySelector, Func<TProperty, TProperty, bool> compare,
            Action<PropertyMapperConfig> callback)
        {
            _propertySelector = propertySelector;
            _compare = compare;
            _callback = callback;
        }

        public void Assignment(Action<TSource, TProperty> assign)
        {
            _assign = assign;

            var config = PropertyMapperConfig.Build(_propertySelector, _assign, _compare);
            
            // Bubble-up the config
            _callback(config);
        }
    }
}