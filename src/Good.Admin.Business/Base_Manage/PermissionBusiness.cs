using Good.Admin.Common;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using LinqKit;
using SqlSugar;

namespace Good.Admin.Business.Base_Manage
{
    public class PermissionBusiness : IPermissionBusiness, ITransientDependency
    {

        private readonly IOperator _operator;
        private readonly IRedisBasketRepository _rediscache;
        private readonly IBase_UserBusiness _userBus;
        private readonly IBase_ActionBusines _actionBus;
        private readonly ISqlSugarClient _db;

        public PermissionBusiness(ISqlSugarClient sqlSugarClient, IOperator @operator, IRedisBasketRepository rediscache, IBase_UserBusiness userbus, IBase_ActionBusines actionBus)
        {
            _operator = @operator;
            _rediscache = rediscache;
            _userBus = userbus;
            _actionBus = actionBus;
            _db = sqlSugarClient;
        }


        /// <summary>
        /// 根据用户id获取菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ActionDTO>> GetUserMenuListAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new BusException("userId不能为空", 500);
            }

            var actionIds = await GetUserActionIds(userId);
            return await _actionBus.GetTreeListAsync(new ActionsInputDTO
            {
                types = new ActionType[] { ActionType.菜单, ActionType.页面 },
                ActionIds = actionIds,
                checkEmptyChildren = true
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<string>> GetUserPermissionValuesAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new BusException("userId不能为空", 500);
            }

            var actionIds = await GetUserActionIds(userId);
            return (await _actionBus
                .GetListAsync(new ActionsInputDTO
                {
                    types = new ActionType[] { ActionType.权限 },
                    ActionIds = actionIds
                }))
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .Select(x => x.Value)
                .ToList();
        }
        #region 私有方法
        async Task<List<string>> GetUserActionIds(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new BusException("userId不能为空", 500);
            }

            var where = LinqHelper.False<Base_Action>();
            var theUser = await _userBus.GetTheDataAsync(userId);
            //不需要权限的菜单
            where = where.Or(x => x.NeedAction == false);

            if (userId == GlobalAssemblies.ADMINID || theUser.RoleType.HasFlag(RoleTypes.超级管理员))
                where = where.Or(x => true);
            else
            {
                //构建查询func               
                var actionIds = await _db.Queryable<Base_UserRole>()
                   .LeftJoin<Base_RoleAction>((x, y) => x.RoleId == y.RoleId)
                   .Where((x, y) => x.UserId == userId)
                   .Select((x, y) => y.ActionId)
                   .ToListAsync();

                where = where.Or(x => actionIds.Contains(x.Id));
            }
            var result = await _db.Queryable<Base_Action>().Where(where).Select(x => x.Id).ToListAsync();

            return result;
        }
        #endregion
    }
}
