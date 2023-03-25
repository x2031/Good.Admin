using Good.Admin.Util;

namespace Good.Admin.Entity
{
    public class DepartmentTreeDTO : TreeModel
    {
        public object children { get => Children; }
        public string title { get => Text; }
        public string value { get => Id; }
        public string key { get => Id; }
    }
}
