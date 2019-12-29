using System;
using System.Collections.Generic;
using SimpleEntityUpdater.Interfaces;
using SimpleEntityUpdater.Models;

namespace SimpleEntityUpdater.Abstracts
{
    public abstract class AbstractProfile<TSource> : IEntityProfile<TSource> where TSource : class
    {
        private readonly List<PropertyMapperConfig> _propertyMapperConfigs = new List<PropertyMapperConfig>();
        
        public IMapConfigComparator<TSource, TProperty> Map<TProperty>(Func<TSource, TProperty> propertySelector)
        {
            void Callback(PropertyMapperConfig config) => _propertyMapperConfigs.Add(config);

            return new MapConfigComparator<TSource, TProperty>(propertySelector, Callback);
        }
        
        public IIdSelector<TSource, TProperty> MapMany<TProperty>(Func<TSource, IEnumerable<TProperty>> propertySelector)
        {
            void Callback(PropertyMapperConfig config) => _propertyMapperConfigs.Add(config);

            return new IdSelectorImpl(propertySelector, Callback);
        }

        public Type Type => typeof(TSource);

        public EntityMapperConfig Config => new EntityMapperConfig {PropertyMapperConfigs = _propertyMapperConfigs};
    }

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

    internal class IdSelectorImpl<TSource, TProperty> : IIdSelector<TSource, TProperty> where TSource : class
    {
        private readonly Func<object, IEnumerable<TProperty>> _propertySelector;

        private readonly Action<PropertyMapperConfig> _callback;

        public IdSelectorImpl(Func<object, IEnumerable<object>> propertySelector, Action<PropertyMapperConfig> callback)
        {
            _propertySelector = propertySelector;
            _callback = callback;
        }

        public IMapConfigComparator<TSource, IEnumerable<TProperty>> IdSelector<TId>(Func<TProperty, TId> idSelector)
        {
            return new MapConfigComparator<TSource, IEnumerable<TProperty>>(_propertySelector, _callback, idSelector);
        }
    }

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

            var config = new PropertyMapperConfig
            {
                Member = x => x switch
                {
                    TSource source => _propertySelector(source),
                    _ => null
                },
                Assignment = (x, y) =>
                {
                    if (x is TSource source && y is TProperty property)
                    {
                        _assign(source, property);
                    }
                },
                Comparator = (x, y) => x switch
                {
                    TProperty property1 when y is TProperty property2 => _compare(property1, property2),
                    _ => false
                }
            };
            
            // Bubble-up the config
            _callback(config);
        }
    }
}