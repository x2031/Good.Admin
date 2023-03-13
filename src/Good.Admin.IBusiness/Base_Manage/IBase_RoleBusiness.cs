using Good.Admin.Entity;
using Good.Admin.Util;

namespace Good.Admin.IBusiness
{
    public interface IBase_RoleBusiness
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResult<Base_RoleInfoDTO>> GetDataListAsync(PageInput<RolesInputDTO> input);
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Base_Role> GetTheDataAsync(string id);
        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Base_RoleInfoDTO> GetTheDataRoleInfoAsync(string id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddDataAsync(Base_Role role, List<string> actions);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateDataAsync(Base_Role role, List<string> actions);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteDataAsync(List<string> ids);
    }

}
