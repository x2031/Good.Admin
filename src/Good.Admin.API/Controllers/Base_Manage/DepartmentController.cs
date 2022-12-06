using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Good.Admin.API.Controllers.Base_Manage
{
    [OpenApiTag("部门管理")]
    [Route("api/[controller]/[action]")]
    [ApiPermission("Department.Use")]
    public class DepartmentController : BaseApiController
    {

    }
}
