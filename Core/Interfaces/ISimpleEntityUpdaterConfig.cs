using System.Collections.Generic;
using System.Reflection;

namespace SimpleEntityUpdater.Interfaces
{
    public interface ISimpleEntityUpdaterConfig
    {
        List<IUnTypedEntityProfile> Profiles { get; }
        
        ISimpleEntityUpdaterConfig Assembly(Assembly assembly);

        ISimpleEntityUpdaterConfig Profile<T>(params T[] profiles) where T : class, IUnTypedEntityProfile;
    }
}