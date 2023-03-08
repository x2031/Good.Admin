using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Util;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Good.Admin.API.Controllers.Base_Manage
{
    [OpenApiTag("角色管理")]
    [Route("api/[controller]/[action]")]

    public class RoleController : BaseApiController
    {
        readonly IBase_RoleBusiness _roleBus;

        public RoleController(IBase_RoleBusiness roleBusiness)
        {
            _roleBus = roleBusiness;
        }

        #region 查询
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_RoleInfoDTO>> GetDataListAsync([FromBody] PageInput<RolesInputDTO> input)
        {
            return await _roleBus.GetDataListAsync(input);
        }
        /// <summary>
        /// 根据id查询单个角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Base_RoleInfoDTO> GetTheData(string id)
        {
            return await _roleBus.GetTheDataAsync(id) ?? new Base_RoleInfoDTO();
        }

        #endregion
        #region 修改
        /// <summary>
        /// 保存数据-实现更新和新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SaveData(Base_RoleInfoDTO input)
        {
            if (input.Id.IsNullOrEmpty())
            {
                InitEntity(input);

                await _roleBus.AddDataAsync(input);
            }
            else
            {
                UpdateInitEntity(input);
                await _roleBus.UpdateDataAsync(input);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteData(List<string> ids)
        {
            await _roleBus.DeleteDataAsync(ids);
        }
        #endregion
    }
}
