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
        Task<PageResult<RoleInfoDTO>> GetListAsync(PageInput<RolesInputDTO> input);
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Base_Role> GetTheDataAsync(string id);
        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        Task<RoleInfoDTO> GetTheRoleInfoAsync(string id);
        /// <summary>
        /// 查询是否存在指定名称的数据
        /// </summary>
        /// <param name="name">角色名</param>
        /// <returns></returns>
        Task<bool> ExistByRoleName(string name);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddAsync(Base_Role role, List<string> actions);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAsync(Base_Role role, List<string> actions);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">id 数组</param>
        /// <returns></returns>
        Task DeleteAsync(List<string> ids);
    }

}
