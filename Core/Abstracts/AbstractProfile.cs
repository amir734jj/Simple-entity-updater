using System;
using System.Collections.Generic;
using SimpleEntityUpdater.Interfaces;
using SimpleEntityUpdater.Logic;
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
            void Callback(PropertyMapperManyConfig config)
            {
                
            }

            return new IdSelectorImpl<TSource, TProperty>(propertySelector, Callback);
        }

        public Type Type => typeof(TSource);

        public EntityMapperConfig Config => new EntityMapperConfig {PropertyMapperConfigs = _propertyMapperConfigs};
    }
}