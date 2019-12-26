using System;
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
}