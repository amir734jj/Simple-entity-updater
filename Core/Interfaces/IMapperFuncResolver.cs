using System;

namespace SimpleEntityUpdater.Interfaces
{
    public interface IMapperFuncResolver
    {
        Action<object, object> Resolve();
    }
}