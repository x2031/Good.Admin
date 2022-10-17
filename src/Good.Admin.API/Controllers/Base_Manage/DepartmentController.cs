using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Good.Admin.API.Controllers.Base_Manage
{
    [OpenApiTag("主页")]
    [Route("api/[controller]/[action]")]
    public class DepartmentController : BaseApiController
    {
    }
}
