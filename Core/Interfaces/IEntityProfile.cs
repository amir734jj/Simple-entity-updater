using System;
using System.Collections.Generic;
using SimpleEntityUpdater.Abstracts;
using SimpleEntityUpdater.Models;

namespace SimpleEntityUpdater.Interfaces
{
    
    public interface IUnTypedEntityProfile { 
        
        Type Type { get; }
        
        EntityMapperConfig Config { get; }
    }
    
    public interface IEntityProfile<out TSource> : IUnTypedEntityProfile where TSource: class
    {
        IMapConfigComparator<TSource, TProperty> Map<TProperty>(Func<TSource, TProperty> propertySelector);

        IIdSelector<TSource, TProperty> MapMany<TProperty>(Func<TSource, IEnumerable<TProperty>> propertySelector);
    }

    public interface IMapConfigComparator<out TSource, out TProperty> where TSource: class
    {
        IMapConfigAssignment<TSource, TProperty> DefaultComparator();

        IMapConfigAssignment<TSource, TProperty> Comparator(Func<TProperty, TProperty, bool> compare);
    }
    
    public interface IMapConfigAssignment<out TSource, out TProperty> where TSource: class
    {
        void Assignment(Action<TSource, TProperty> assign);
    }

    public interface IIdSelector<out TSource, out TProperty> where TSource: class
    {
        public IMapConfigComparator<TSource, IEnumerable<TProperty>> IdSelector<TId>(Func<TProperty, TId> idSelector);
    }
}