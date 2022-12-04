namespace Good.Admin.Entity
{
    public class UserEditInputDTO : Base_User
    {
        public string newPwd { get; set; }
        public List<string> RoleIdList { get; set; }
    }
}
