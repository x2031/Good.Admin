using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Util;
using Mapster;
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
        #region 查询
        /// <summary>
        /// 根据父级id获取list
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<DepartmentDto>> GetListByParentId([FromBody] IdInputNullDTO input)
        {
            return await _bus.GetListByParentId(input.id);
        }
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<DepartmentDto>> GetList([FromBody] PageInput<NameInputDTO> input)
        {

            return await _bus.GetList(input);
        }
        /// <summary>
        /// 查询是否存在指定名称的部门
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistByName(NameInputNoNullDTO input)
        {
            return await _bus.ExistByName(input.name);
        }
        #endregion
        #region 修改
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="BusException"></exception>
        [HttpPost]

        public async Task Add([FromBody] DepartmentEditDto input)
        {
            var model = input.Adapt<Base_Department>();

            if (!model.Id.IsNullOrEmpty())
            {
                throw new BusException("新增数据不允许传入ID!", 500);
            }

            if (await _bus.ExistByName(input.Name))
            {
                throw new BusException($"{input.Name}已存在", 500);
            }

            InitEntity(model);
            await _bus.AddAsync(model);
        }
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task Delete(List<string> ids)
        {
            await _bus.DeleteAsync(ids);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="BusException"></exception>
        [HttpPost]
        public async Task Update([FromBody] DepartmentDto input)
        {
            var model = input.Adapt<Base_Department>();

            if (model.Id.IsNullOrEmpty())
            {
                throw new BusException("更新数据必须传入ID标识字符串", 500);
            }

            UpdateInitEntity(model);
            await _bus.UpdateAsync(model);
        }

        #endregion
    }
}
