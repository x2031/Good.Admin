using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Good.Admin.API.Controllers.Base_Manage
{
    [OpenApiTag("开发者")]
    [Route("api/[controller]/[action]")]
    public class DevelopController : BaseApiController
    {
    }
}
