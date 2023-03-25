using Good.Admin.Util;

namespace Good.Admin.Entity
{
    public class RoleInfoDTO : Base_Role
    {
        public RoleTypes? RoleType { get => RoleName?.ToEnum<RoleTypes>(); }
        public List<string> Actions { get; set; } = new List<string>();
    }
}
