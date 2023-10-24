﻿
using Good.Admin.Common;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Repository;
using Mapster;
using SqlSugar;

namespace Good.Admin.Business
{
    public class Base_RoleBusiness : BaseRepository<Base_Role>, IBase_RoleBusiness, ITransientDependency
    {
        public Base_RoleBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 查询
        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PageResult<RoleInfoDTO>> GetListAsync(PageInput<RolesInputDTO> input)
        {
            var search = input.Search;
            //构建查询条件
            var expable = Expressionable.Create<Base_Role>();
            expable.AndIF(!search.roleName.IsNullOrEmpty(), x => x.RoleName.Contains(search.roleName));

            var db_result = await QueryPageListByClauseAsync(expable, pageIndex: input.PageIndex, pagesize: input.PageSize);
            var result = db_result.Adapt<PageResult<RoleInfoDTO>>();

            await SetProperty(result.data);

            return result;

            async Task SetProperty(List<RoleInfoDTO> _list)
            {
                var allActionIds = await Db.Queryable<Base_Action>().Select(x => x.Id).ToListAsync();
                var ids = _list.Select(x => x.Id).ToList();
                var roleActions = await Db.Queryable<Base_RoleAction>()
                    .Where(x => ids.Contains(x.RoleId))
                    .ToListAsync();

                _list.ForEach(aData =>
              {
                  if (aData.RoleName == RoleTypes.超级管理员.ToString())
                      aData.Actions = allActionIds;
                  else
                      aData.Actions = roleActions.Where(x => x.RoleId == aData.Id).Select(x => x.ActionId).ToList();
              });
            }
        }

        public async Task<RoleInfoDTO> GetTheRoleInfoAsync(string id)
        {
            return (await GetListAsync(new PageInput<RolesInputDTO> { Search = new RolesInputDTO { roleId = id } })).data.FirstOrDefault();
        }
        public async Task<bool> ExistByRoleName(string name)
        {
            return await ExistsAsync(x => x.RoleName == name);
        }
        public async Task<Base_Role> GetTheDataAsync(string id)
        {
            return await QueryByIdAsync(id);
        }

        #endregion
        #region 修改
        public async Task AddAsync(Base_Role role, List<string> actions)
        {
            var existName = await ExistsAsync((x) => x.RoleName == role.RoleName);
            if (existName)
            {
                throw new BusException($"{role.RoleName}已存在");
            }

            await InsertAsync(role);
            if (actions != null && actions.Count > 0)
            {
                await SetRoleActionAsync(role.Id, actions);
            }

        }


        public async Task DeleteAsync(List<string> ids)
        {
            await DeleteByIdsAsync(ids);
            await Db.Deleteable<Base_RoleAction>(x => ids.Contains(x.RoleId)).ExecuteCommandHasChangeAsync();
        }

        public async Task UpdateAsync(Base_Role role, List<string> actions)
        {
            await UpdateIgnoreNullAsync(role);
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
            await Db.Deleteable<Base_RoleAction>(x => x.RoleId == roleId).ExecuteCommandHasChangeAsync();
            await Db.Insertable<Base_RoleAction>(roleActions).ExecuteCommandAsync();
        }
        #endregion


    }
}
