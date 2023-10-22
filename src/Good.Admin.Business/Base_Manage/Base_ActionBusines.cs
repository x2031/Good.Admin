using Good.Admin.Common.Cache;
using Good.Admin.Common.DI;
using Good.Admin.Common.Helper;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.IBusiness.Base_Manage;
using Good.Admin.Repository;
using Good.Admin.Common;
using Mapster;

namespace Good.Admin.Business.Base_Manage
{
    public class Base_ActionBusines : BaseRepository<Base_Action>, IBase_ActionBusines, ITransientDependency
    {
        public Base_ActionBusines(IUnitOfWork unitOfWork, IOperator @operator, IRedisBasketRepository rediscache) : base(unitOfWork)
        {
            _operator = @operator;
            _rediscache = rediscache;
        }
        readonly IOperator _operator;
        readonly IRedisBasketRepository _rediscache;

        #region 查询
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Base_Action>> GetListAsync(ActionsInputDTO input)
        {
            var where = LinqHelper.True<Base_Action>();
            where.AndIf(!input.parentId.IsNullOrEmpty(), x => x.ParentId == input.parentId)
               .AndIf(input.types?.Length > 0, x => input.types.Contains(x.Type))
               .AndIf(input.ActionIds?.Count > 0, x => input.ActionIds.Contains(x.Id));

            return await QueryListByClauseAsync(where, "Sort desc");
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Base_Action> GetTheDataAsync(string id)
        {
            return await QueryByIdAsync(id);
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
                var allPermissions = await QueryListByClauseAsync(x => ids.Contains(x.ParentId) && (int)x.Type == 2);
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
            await UpdateAsync(input.Adapt<Base_Action>());
            await SavePermissionAsync(input.Id, input.permissionList);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AddAsync(ActionEditDTO input)
        {
            await InsertAsync(input.Adapt<Base_Action>());
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
            await DeleteAsync(x => x.ParentId == parentId && (int)x.Type == 2);
            //新增
            await InsertAsync(permissionList);
            //权限值必须唯一
            var repeatValues = Query()
                .GroupBy(x => x.Value)
                .Where(x => !string.IsNullOrEmpty(x.Key) && x.Count() > 1)
                .Select(x => x.Key)
                .ToList();
            if (repeatValues.Count > 0)
                throw new Exception($"以下权限值重复:{string.Join(",", repeatValues)}");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteAsync(List<string> ids)
        {
            await DeleteByIdsAsync(ids);
            await DeleteAsync(x => ids.Contains(x.ParentId));
        }
        #endregion
    }
}
