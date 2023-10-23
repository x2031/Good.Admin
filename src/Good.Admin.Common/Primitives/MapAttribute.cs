using System;

namespace Good.Admin.Common.Primitives
{
    public class MapAttribute : Attribute
    {
        public MapAttribute(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }
        public Type[] TargetTypes { get; }
    }
}
