using System;

namespace SimpleEntityUpdater.Models
{
    public class PropertyMapperConfig
    {
        public Type Type { get; private set; }

        public Action<object, object> Assignment { get; private set; }

        public Func<object, object, bool> Comparator { get; private set; }

        public Func<object, object> Member { get; private set; }

        public static PropertyMapperConfig Build<TSource, TProperty>(
            Func<TSource, TProperty> propertySelector, 
            Action<TSource, TProperty> assignment,
            Func<TProperty, TProperty, bool> comparator
        )
        {
            return new PropertyMapperConfig
            {
                // Member access un-typed
                Member = sourceUntyped => sourceUntyped switch
                {
                    TSource source => propertySelector(source),
                    _ => null
                },
                // Assignment un-typed
                Assignment = (sourceUntyped, valueUntyped) =>
                {
                    if (sourceUntyped is TSource source && valueUntyped is TProperty value)
                    {
                        assignment(source, value);
                    }
                },
                // Comparator un-typed
                Comparator = (sourceUntyped1, sourceUntyped2) => sourceUntyped1 switch
                {
                    TProperty property1 when sourceUntyped2 is TProperty property2 => comparator(property1, property2),
                    _ => false
                }
            };
        }
    }
}