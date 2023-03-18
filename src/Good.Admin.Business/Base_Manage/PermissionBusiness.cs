using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.IBusiness.Base_Manage;
using Good.Admin.Repository;
using Good.Admin.Util;
using LinqKit;
using SqlSugar;

namespace Good.Admin.Business.Base_Manage
{
    public class PermissionBusiness : BaseRepository<Base_Action>, IPermissionBusiness, ITransientDependency
    {
        public PermissionBusiness(IUnitOfWork unitOfWork, IOperator @operator, IRedisBasketRepository rediscache, IBase_UserBusiness userbus, IBase_ActionBusines actionBus) : base(unitOfWork)
        {
            _operator = @operator;
            _rediscache = rediscache;
            _userBus = userbus;
            _actionBus = actionBus;
        }

        readonly IOperator _operator;
        readonly IRedisBasketRepository _rediscache;
        readonly IBase_UserBusiness _userBus;
        readonly IBase_ActionBusines _actionBus;

        /// <summary>
        /// 根据用户id获取菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Base_ActionDTO>> GetUserMenuListAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new BusException("userId不能为空", 500);
            }

            var actionIds = await GetUserActionIds(userId);
            return await _actionBus.GetTreeListAsync(new Base_ActionsInputDTO
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
                .GetListAsync(new Base_ActionsInputDTO
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
                //构建查询条件
                var expable = Expressionable.Create<Base_UserRole, Base_RoleAction>();
                expable.And((x, y) => x.UserId == userId);
                //构建查询func 
                var actionIds = await QueryMuchAsync<Base_UserRole, Base_RoleAction, string>(
                     //关联表
                     (x, y) => new object[]
                     {
                         JoinType.Left, x.RoleId==y.RoleId
                     },
                     //返回值拼装
                     (x, y) => new string(y.ActionId),
                     //where条件
                     expable
                     );

                where = where.Or(x => actionIds.Contains(x.Id));
            }

            return await QueryListByClauseAsync<Base_Action, string>(where, (x) => x.Id);
        }
        #endregion
    }
}
