using System.Collections.Generic;

namespace Core.Tests.Models
{
    public class DummyClass
    {
        public string Firstname { get; set; }
        
        public string Lastname { get; set; }
        
        public DummyNestedClass NestedClass { get; set; }
        
        public List<DummyNestedClass> NestedClasses { get; set; }
    }
}