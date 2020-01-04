using System;

namespace SimpleEntityUpdater.Models
{
    public class PropertyMapperManyConfig
    {
        public Type Type { get; private set; }

        public Action<object, object> Assignment { get; private set; }

        public Func<object, object, bool> Comparator { get; private set; }

        public Func<object, object> Member { get; private set; }

        public static PropertyMapperConfig Build<TSource, TProperty, TId>(
            Func<TSource, TProperty> propertySelector, 
            Action<TSource, TProperty> assignment,
            Func<TProperty, TId> idSelector
        )
        {
            return new PropertyMapperConfig
            {
                Member = x => x switch
                {
                    TSource source => propertySelector(source),
                    _ => null
                },
                Assignment = (x, y) =>
                {
                    if (x is TSource source && y is TProperty property)
                    {
                        assignment(source, property);
                    }
                },
                Comparator = (x, y) => x switch
                {
                    TProperty property1 when y is TProperty property2 => comparator(property1, property2),
                    _ => false
                }
            };
        }
    }
}