using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Repository;
using Good.Admin.Util;

namespace Good.Admin.Business
{
    public class Base_DepartmentBusiness : BaseRepository<Base_Department>, IBase_DepartmentBusiness, ITransientDependency
    {
        public Base_DepartmentBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 修改
        public async Task AddDataAsync(Base_Department model)
        {
            await InsertAsync(model);
        }
        public async Task DeleteDataAsync(List<string> ids)
        {
            if (await ExistsAsync(x => ids.Contains(x.ParentId)))
            {
                throw new BusException("禁止删除！请先删除所有子级！");
            }
            await DeleteByIdAsync(ids);
        }

        public async Task UpdateDataAsync(Base_Department model)
        {
            await UpdateDataAsync(model);
        }
        #endregion
        #region 查询

        public Task<List<string>> GetChildrenIdsAsync(string departmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<Base_Department> GetTheDataAsync(string id)
        {
            return await QueryByIdAsync(id);
        }

        public Task<List<Base_DepartmentTreeDTO>> GetTreeDataListAsync(DepartmentsTreeInputDTO input)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
