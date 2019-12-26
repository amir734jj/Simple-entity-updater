using System;

namespace SimpleEntityUpdater.Models
{
    public class PropertyMapperConfig
    {
        public Type Type { get; set; }
        
        public Action<object, object> Assignment { get; set; }
        
        public Func<object, object, bool> Comparator { get; set; }
        
        public Func<object, object> Member { get; set; }
    }
}