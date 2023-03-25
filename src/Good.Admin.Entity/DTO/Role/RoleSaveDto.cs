using Good.Admin.Util;

namespace Good.Admin.Entity
{
    public class RoleSaveDto
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public RoleTypes? RoleType { get => RoleName?.ToEnum<RoleTypes>(); }
        public List<string> Actions { get; set; } = new List<string>();
    }
}
