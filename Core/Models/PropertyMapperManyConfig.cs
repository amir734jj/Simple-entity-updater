using System;

namespace SimpleEntityUpdater.Models
{
    public class PropertyMapperManyConfig
    {
        public Type Type { get; private set; }

        public Action<object, object> Assignment { get; private set; }

        public Func<object, object> IdSelector { get; private set; }

        public Func<object, object> Member { get; private set; }

        public static PropertyMapperManyConfig Build<TSource, TProperty, TId>(
            Func<TSource, TProperty> propertySelector, 
            Action<TSource, TProperty> assignment,
            Func<TProperty, TId> idSelector
        )
        {
            return new PropertyMapperManyConfig
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
                // Id selector
                IdSelector = (sourceUntyped) => sourceUntyped switch
                {
                    TProperty property => idSelector(property),
                    _ => false
                }
            };
        }
    }
}