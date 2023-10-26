
using Good.Admin.Common;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Mapster;
using SqlSugar;

namespace Good.Admin.Business
{
    public class Base_RoleBusiness : IBase_RoleBusiness, ITransientDependency
    {
        private readonly ISqlSugarClient _db;
        public Base_RoleBusiness(ISqlSugarClient sqlSugarClient)
        {
            _db = sqlSugarClient;
        }

        #region 查询
        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PageResult<RoleInfoDTO>> GetListAsync(PageInput<RolesInputDTO> input)
        {
            RefAsync<int> total = 0;
            var search = input.Search;
            //构建查询条件
            var expable = Expressionable.Create<Base_Role>();
            expable.AndIF(!search.roleName.IsNullOrEmpty(), x => x.RoleName.Contains(search.roleName));

            var dbResult = await _db.Queryable<Base_Role>()
                  .Where(x => x.Deleted == 0)
                  .Where(expable.ToExpression())
                  .ToPageListAsync(input.PageIndex, input.PageSize, total);


            var pageResult = new PageResult<Base_Role>(input.PageIndex, total.Value, input.PageSize, dbResult);

            var result = pageResult.Adapt<PageResult<RoleInfoDTO>>();

            await SetProperty(result.data);

            return result;

            async Task SetProperty(List<RoleInfoDTO> _list)
            {

                var allActionIds = await _db.Queryable<Base_Action>()
                    .Where(x => x.Deleted == 0)
                    .Select(x => x.Id).ToListAsync();

                var ids = _list.Select(x => x.Id).ToList();

                var roleActions = await _db.Queryable<Base_RoleAction>()
                    .Where(x => x.Deleted == 0)
                    .Where(x => ids.Contains(x.Id))
                    .ToListAsync();

                _list.ForEach(aData =>
                {
                    if (aData.RoleName == RoleTypes.超级管理员.ToString())
                    {
                        aData.Actions = allActionIds;
                    }
                    else
                    {
                        aData.Actions = roleActions.Where(x => x.RoleId == aData.Id).Select(x => x.ActionId).ToList();
                    }
                });
            }
        }

        public async Task<RoleInfoDTO> GetTheRoleInfoAsync(string id)
        {
            return (await GetListAsync(new PageInput<RolesInputDTO> { Search = new RolesInputDTO { roleId = id } })).data.FirstOrDefault();
        }
        public async Task<bool> ExistByRoleName(string name)
        {
            return await _db.Queryable<Base_Role>()
                .Where(x => x.Deleted == 0)
                .AnyAsync(x => x.RoleName == name);
        }
        public async Task<Base_Role> GetTheDataAsync(string id)
        {
            var result = await _db.Queryable<Base_Role>()
                 .Where(x => x.Deleted == 0)
                 .Where(x => x.Id == id)
                 .FirstAsync();
            return result;
        }

        #endregion
        #region 修改
        public async Task AddAsync(Base_Role role, List<string> actions)
        {
            var existName = await ExistByRoleName(role.RoleName);
            if (existName)
            {
                throw new BusException($"{role.RoleName}已存在");
            }

            await _db.Insertable(role).ExecuteCommandAsync();
            if (actions != null && actions.Count > 0)
            {
                await SetRoleActionAsync(role.Id, actions);
            }

        }


        public async Task DeleteAsync(List<string> ids)
        {
            try
            {
                _db.Ado.BeginTran();
                await _db.Deleteable<Base_Role>()
                     .Where(x => ids.Contains(x.Id))
                     .ExecuteCommandAsync();

                await _db.Deleteable<Base_RoleAction>()
                    .Where(x => ids.Contains(x.RoleId))
                    .ExecuteCommandAsync();
                _db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _db.Ado.RollbackTran();
                throw;
            }
        }

        public async Task UpdateAsync(Base_Role role, List<string> actions)
        {
            await _db.Updateable(role).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            if (actions != null && actions.Count > 0)
            {
                await SetRoleActionAsync(role.Id, actions);
            }
        }
        #endregion

        #region 私有
        private async Task SetRoleActionAsync(string roleId, List<string> actions)
        {


            var roleActions = (actions ?? new List<string>())
                .Select(x => new Base_RoleAction
                {
                    Id = IdHelper.NextId(),
                    ActionId = x,
                    CreateTime = DateTime.Now,
                    RoleId = roleId
                }).ToList();

            try
            {
                _db.Ado.BeginTran();

                await _db.Deleteable<Base_RoleAction>(x => x.RoleId == roleId).ExecuteCommandHasChangeAsync();
                await _db.Deleteable<Base_RoleAction>(roleActions).ExecuteCommandHasChangeAsync();

                _db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _db.Ado.RollbackTran();
                throw;
            }

        }
        #endregion


    }
}
