namespace Good.Admin.API
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public int AccessExpireHours { get; set; }
        public int RefreshExpireHours { get; set; }
    }
}
