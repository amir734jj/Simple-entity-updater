using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleEntityUpdater.Interfaces;
using SimpleEntityUpdater.Logic;

namespace SimpleEntityUpdater
{
    public static class EntityUpdater
    {
        public static ISimpleEntityUpdaterMapper New(Action<ISimpleEntityUpdaterConfig> config)
        {
            var configRef = new SimpleEntityUpdaterConfig();

            config(configRef);

            return new SimpleEntityUpdaterMapper(configRef.Profiles);
        }
    }

    internal class SimpleEntityUpdaterMapper : ISimpleEntityUpdaterMapper
    {
        private readonly Dictionary<Type, Action<object, object>> _handlers;

        public SimpleEntityUpdaterMapper(IEnumerable<IUnTypedEntityProfile> profiles)
        {
            var utility = new MapperFuncResolver();

            _handlers = profiles.ToDictionary(x => x.Type, MapperFuncResolver.Resolve);
        }

        public void Map<T>(T source, T destination)
        {
            if (_handlers.ContainsKey(typeof(T)))
            {
                _handlers[typeof(T)](source, destination);
            }
            else
            {
                throw new Exception($"Failed to find a profile for type {typeof(T).Name}");
            }
        }
    }

    internal class SimpleEntityUpdaterConfig : ISimpleEntityUpdaterConfig
    {
        public List<IUnTypedEntityProfile> Profiles { get; set; } = new List<IUnTypedEntityProfile>();

        public ISimpleEntityUpdaterConfig Assembly(Assembly assembly)
        {
            var profiles = assembly.DefinedTypes
                .Where(x => x.IsClass &&
                            !x.IsAbstract &&
                            typeof(IUnTypedEntityProfile).IsAssignableFrom(x) &&
                            x.GetConstructor(Type.EmptyTypes) != null)
                .Select(Activator.CreateInstance)
                .Cast<IUnTypedEntityProfile>()
                .ToArray();

            return Profile(profiles);
        }

        public ISimpleEntityUpdaterConfig Profile<T>(params T[] profiles) where T : class, IUnTypedEntityProfile
        {
            Profiles.AddRange(profiles);

            return this;
        }
    }
}