
using Good.Admin.Entity;

namespace Good.Admin.IBusiness
{
    public interface IBase_DepartmentBusiness
    {
        /// <summary>
        /// 获取部门树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<Base_DepartmentTreeDTO>> GetTreeListAsync(DepartmentsTreeInputDTO input);
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Base_Department> GetTheDataAsync(string id);
        /// <summary>
        /// 获取子级
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        Task<List<string>> GetChildrenIdsAsync(string departmentId);
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="newData"></param>
        /// <returns></returns>
        Task AddAsync(Base_Department newData);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="theData"></param>
        /// <returns></returns>
        Task UpdateAsync(Base_Department theData);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteAsync(List<string> ids);
        /// <summary>
        /// 根据父级id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Base_DepartmentDto>> GetListByParentId(string id);
    }
}
