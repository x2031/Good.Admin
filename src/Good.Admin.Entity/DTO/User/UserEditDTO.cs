namespace Good.Admin.Entity
{
    public class UserEditDTO
    {
        public string newPwd { get; set; }
        public List<string> RoleIdList { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public Sex Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string DepartmentId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
