namespace Good.Admin.Entity
{
    public class ActionEditDTO : Base_Action
    {
        public List<Base_Action> permissionList { get; set; } = new List<Base_Action>();
    }
}
