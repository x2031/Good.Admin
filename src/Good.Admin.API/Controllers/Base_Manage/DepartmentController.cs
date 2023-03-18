using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Good.Admin.API.Controllers.Base_Manage
{
    [OpenApiTag("部门管理")]
    [Route("api/[controller]/[action]")]

    public class DepartmentController : BaseApiController
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IBase_DepartmentBusiness _bus;
        public DepartmentController(IHttpContextAccessor accessor, IBase_DepartmentBusiness bus)
        {
            _accessor = accessor;
            _bus = bus;

        }
        [HttpPost]
        public async Task<List<Base_DepartmentDto>> GetListByParentId([FromBody] IdInputNullDTO input)
        {
            return await _bus.GetListByParentId(input.id);
        }
    }
}
