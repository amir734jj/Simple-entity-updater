using System;
using SimpleEntityUpdater.Interfaces;

namespace SimpleEntityUpdater.Logic
{
    public class MapperFuncResolver
    {
        public static Action<object, object> Resolve(IUnTypedEntityProfile profile) => (source, destination) =>
        {
            foreach (var propertyMapperConfig in profile.Config.PropertyMapperConfigs)
            {
                // Resolve source/destination property values
                var sourceProperty = propertyMapperConfig.Member(source);
                var destinationProperty = propertyMapperConfig.Member(destination);

                if (propertyMapperConfig.Comparator(sourceProperty, destinationProperty))
                {
                    continue;
                }

                propertyMapperConfig.Assignment(source, destinationProperty);
            }
        };
    }
}