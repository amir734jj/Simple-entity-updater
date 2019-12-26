namespace SimpleEntityUpdater.Interfaces
{
    public interface ISimpleEntityUpdaterMapper
    {
        void Map<T>(T source, T destination);
    }
}