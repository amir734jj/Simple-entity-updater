using Core.Tests.Models;
using SimpleEntityUpdater.Abstracts;

namespace Core.Tests.Profiles
{
    public class DummyClassProfile : AbstractProfile<DummyClass>
    {
        public DummyClassProfile()
        {
            Map(x => x.Firstname)
                .DefaultComparator()
                .Assignment((x, y) => x.Firstname = y);
            
            Map(x => x.Lastname)
                .DefaultComparator()
                .Assignment((x, y) => x.Lastname = y);
        }
    }
}