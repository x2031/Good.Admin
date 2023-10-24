using Good.Admin.Common;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Common;
using Mapster;
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
        public async Task<PageResult<RoleInfoDTO>> GetListAsync([FromBody] PageInput<RolesInputDTO> input)
        {
            return await _roleBus.GetListAsync(input);
        }
        /// <summary>
        /// 根据id查询单个角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RoleInfoDTO> GetTheData(IdInputDTO input)
        {
            return await _roleBus.GetTheRoleInfoAsync(input.id) ?? new RoleInfoDTO();
        }
        /// <summary>
        /// 查询是否存在角色名称
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistByRoleName(NameInputNoNullDTO input)
        {
            return await _roleBus.ExistByRoleName(input.name);
        }
        #endregion
        #region 修改
        /// <summary>
        /// 保存数据-实现更新和新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Save(RoleSaveDto input)
        {
            if (input.Id.IsNullOrEmpty())
            {
                var role = input.Adapt<Base_Role>();
                InitEntity(role);
                await _roleBus.AddAsync(role, input.Actions);
            }
            else
            {
                var editrole = await _roleBus.GetTheDataAsync(input.Id);
                if (editrole == null)
                {
                    throw new BusException("查询不到该数据", 500);
                }
                editrole.RoleName = input.RoleName;
                UpdateInitEntity(editrole);
                await _roleBus.UpdateAsync(editrole, input.Actions);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task Delete(List<string> ids)
        {
            await _roleBus.DeleteAsync(ids);
        }
        #endregion
    }
}
