
using Good.Admin.Common;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Mapster;
using SqlSugar;

namespace Good.Admin.Business
{
    public class Base_DepartmentBusiness : IBase_DepartmentBusiness, ITransientDependency
    {
        private readonly ISqlSugarClient _db;
        public Base_DepartmentBusiness(ISqlSugarClient sqlSugarClient)
        {
            _db = sqlSugarClient;
        }

        #region 修改
        public async Task AddAsync(Base_Department model)
        {
            var existName = await ExistByName(model.Name);
            if (existName)
            {
                throw new BusException($"{model.Name}已存在", 500);
            }
            await _db.Insertable(model).ExecuteCommandAsync();
        }
        public async Task DeleteAsync(List<string> ids)
        {
            var existFlag = await _db.Queryable<Base_Department>().AnyAsync(x => ids.Contains(x.ParentId));
            if (existFlag)
            {
                throw new BusException("禁止删除！请先删除所有子级！");
            }
            await _db.Deleteable<Base_Department>().Where(x => ids.Contains(x.Id)).ExecuteCommandAsync();
        }

        public async Task UpdateAsync(Base_Department model)
        {
            await _db.Updateable(model).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }
        #endregion
        #region 查询

        public Task<List<string>> GetChildrenIdsAsync(string departmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<Base_Department> GetTheDataAsync(string id)
        {
            var result = await _db.Queryable<Base_Department>()
                .Where(x => x.Id == id)
                .Where(x => x.Deleted == 0)
                .FirstAsync();
            return result;
        }

        public Task<List<DepartmentTreeDTO>> GetTreeListAsync(DepartmentsTreeDTO input)
        {
            throw new NotImplementedException();
        }
        public async Task<List<DepartmentDto>> GetListByParentId(string id)
        {
            var reuslt = await _db.Queryable<Base_Department>()
                 .WhereIF(!id.IsNullOrEmpty(), x => x.ParentId == id)
                 .Where(x => x.Deleted == 0)
                 .Select(x => new DepartmentDto { Id = x.Id, Name = x.Name, ParentId = x.ParentId, CreateTime = x.CreateTime })
                 .ToListAsync();

            return reuslt;
        }
        public async Task<PageResult<DepartmentDto>> GetList(PageInput<NameInputDTO> input)
        {
            RefAsync<int> total = 0;
            var search = input.Search;
            //构建查询条件
            var expable = Expressionable.Create<Base_Department>();
            expable.AndIF(!search.name.IsNullOrEmpty(), x => x.Name.Contains(search.name));

            var result = await _db.Queryable<Base_Department>()
                    .Where(x => x.Deleted == 0)
                    .Where(expable.ToExpression())
                    .ToPageListAsync(input.PageIndex, input.PageSize, total);

            var pageResult = new PageResult<Base_Department>(input.PageIndex, total.Value, input.PageSize, result);

            return pageResult.Adapt<PageResult<DepartmentDto>>();
        }
        public async Task<bool> ExistByName(string name)
        {
            var existName = await _db.Queryable<Base_Department>().AnyAsync(x => x.Name == name);
            return existName;
        }
        #endregion

    }
}
