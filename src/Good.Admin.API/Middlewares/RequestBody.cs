using Good.Admin.Common.DI;

namespace Good.Admin.API
{
    public class RequestBody : IScopedDependency
    {
        public string Body { get; set; }
    }
}
