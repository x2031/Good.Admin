using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Repository;
using Good.Admin.Util;
using SqlSugar;

namespace Good.Admin.Business
{
    public class Base_DepartmentBusiness : BaseRepository<Base_Department>, IBase_DepartmentBusiness, ITransientDependency
    {
        public Base_DepartmentBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 修改
        public async Task AddAsync(Base_Department model)
        {
            var existName = await ExistsAsync((x) => x.Name == model.Name);
            if (existName)
            {
                throw new BusException($"{model.Name}已存在", 500);
            }
            await InsertAsync(model);
        }
        public async Task DeleteAsync(List<string> ids)
        {
            if (await ExistsAsync(x => ids.Contains(x.ParentId)))
            {
                throw new BusException("禁止删除！请先删除所有子级！");
            }
            await DeleteByIdAsync(ids);
        }

        public async Task UpdateAsync(Base_Department model)
        {
            await UpdateAsync(model);
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

        public Task<List<Base_DepartmentTreeDTO>> GetTreeListAsync(DepartmentsTreeInputDTO input)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Base_DepartmentDto>> GetListByParentId(string id)
        {
            var expable = Expressionable.Create<Base_Department>();
            expable.AndIF(!id.IsNullOrEmpty(), x => x.ParentId == id);

            return await QueryListByClauseAsync<Base_Department, Base_DepartmentDto>(expable.ToExpression(), (x) => new Base_DepartmentDto { Id = x.Id, Name = x.Name });
        }

        #endregion

    }
}
