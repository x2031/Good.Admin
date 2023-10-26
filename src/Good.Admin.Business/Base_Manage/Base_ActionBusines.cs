using Good.Admin.Common;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Mapster;
using SqlSugar;

namespace Good.Admin.Business.Base_Manage
{
    public class Base_ActionBusines : IBase_ActionBusines, ITransientDependency
    {
        private readonly IOperator _operator;
        private readonly IRedisBasketRepository _rediscache;
        private readonly ISqlSugarClient _db;

        public Base_ActionBusines(ISqlSugarClient sqlSugarClient, IOperator nowOperator, IRedisBasketRepository rediscache)
        {
            _operator = nowOperator;
            _rediscache = rediscache;
            _db = sqlSugarClient;
        }


        #region 查询
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Base_Action>> GetListAsync(ActionsInputDTO input)
        {
            var result = await _db.Queryable<Base_Action>()
                 .WhereIF(!input.parentId.IsNullOrEmpty(), x => x.ParentId == input.parentId)
                 .WhereIF(input.types?.Length > 0, x => input.types.Contains(x.Type))
                 .WhereIF(input.ActionIds?.Count > 0, x => input.ActionIds.Contains(x.Id))
                 .Where(x => x.Deleted == 0)
                 .OrderBy(x => x.Sort, OrderByType.Asc)
                 .ToListAsync();

            return result;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Base_Action> GetTheDataAsync(string id)
        {
            return await _db.Queryable<Base_Action>().Where(x => x.Id == id).Where(x => x.Deleted == 0).FirstAsync();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ActionDTO>> GetTreeListAsync(ActionsInputDTO input)
        {
            var qList = await GetListAsync(input);
            var treeList = qList.Select(x => new ActionDTO
            {
                Id = x.Id,
                NeedAction = x.NeedAction,
                Text = x.Name,
                ParentId = x.ParentId,
                Type = x.Type,
                Url = x.Url,
                Value = x.Id,
                Icon = x.Icon,
                Sort = x.Sort,
                selectable = input.selectable
            }).ToList();


            //菜单节点中,若子节点为空则移除父节点
            if (input.checkEmptyChildren)
                treeList = treeList.Where(x => x.Type != 0 || TreeHelper.GetChildren(treeList, x, false).Count > 0).ToList();

            await SetProperty(treeList);

            return TreeHelper.BuildTree(treeList);

            async Task SetProperty(List<ActionDTO> _list)
            {
                var ids = _list.Select(x => x.Id).ToList();
                var allPermissions = await _db.Queryable<Base_Action>()
                    .Where(x => ids.Contains(x.ParentId))
                    .Where(x => (int)x.Type == 2)
                    .ToListAsync();

                _list.ForEach(aData =>
                {
                    var permissionValues = allPermissions
                        .Where(x => x.ParentId == aData.Id)
                        .Select(x => $"{x.Name}({x.Value})")
                        .ToList();

                    aData.PermissionValues = permissionValues;
                });
            }
        }
        #endregion
        #region 修改
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateAsync(ActionEditDTO input)
        {
            try
            {
                _db.Ado.BeginTran();
                await _db.Updateable(input.Adapt<Base_Action>()).ExecuteCommandAsync();
                await SavePermissionAsync(input.Id, input.permissionList);
                _db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _db.Ado.RollbackTran();
                throw;
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AddAsync(ActionEditDTO input)
        {
            await _db.Insertable(input.Adapt<Base_Action>()).ExecuteCommandAsync();
        }
        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="permissionList"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SavePermissionAsync(string parentId, List<Base_Action> permissionList)
        {
            permissionList.ForEach(aData =>
            {
                aData.Id = IdHelper.NextId();
                aData.CreateTime = DateTime.Now;
                aData.CreatorId = null;
                aData.ParentId = parentId;
                aData.NeedAction = true;
            });
            //删除原来          
            await _db.Deleteable<Base_Action>().Where(x => x.ParentId == parentId)
                 .Where(x => (int)x.Type == 2)
                 .ExecuteCommandAsync();
            //新增
            await _db.Insertable(permissionList).ExecuteCommandAsync();
            //权限值必须唯一            
            var repeatValues = await _db.Queryable<Base_Action>()
                .Where(x => x.Deleted == 0)
                .GroupBy(x => x.Value)
                .Having(x => SqlFunc.RowCount() > 0)
                .ToListAsync();

            if (repeatValues.Count > 0)
            {
                throw new Exception($"以下权限值重复:{string.Join(",", repeatValues)}");
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteAsync(List<string> ids)
        {
            try
            {
                _db.Ado.BeginTran();
                await _db.Deleteable<Base_Action>()
                       .Where(x => ids.Contains(x.Id))
                       .ExecuteCommandAsync();

                await _db.Deleteable<Base_Action>()
                    .Where(x => ids.Contains(x.ParentId))
                    .ExecuteCommandAsync();
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
