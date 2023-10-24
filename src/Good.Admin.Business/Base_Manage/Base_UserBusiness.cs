using Good.Admin.Common;
using Good.Admin.Common.Cache;
using Good.Admin.Common.DI;
using Good.Admin.Common.Helper;
using Good.Admin.Common;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Repository;
using Good.Admin.Common;
using Mapster;
using SqlSugar;

namespace Good.Admin.Business
{
    public class Base_UserBusiness : BaseRepository<Base_User>, IBase_UserBusiness, ISingletonDependency
    {
        //readonly IBaseRepository1<Base_User> _dal;
        public Base_UserBusiness(IUnitOfWork unitOfWork, IOperator @operator, IRedisBasketRepository rediscache) : base(unitOfWork)
        {
            _operator = @operator;
            _rediscache = rediscache;
            // _dal = dal;
        }
        readonly IOperator _operator;
        readonly IRedisBasketRepository _rediscache;

        #region 查询      
        public async Task<UserDTO> GetTheDataAsync(string id)
        {
            UserDTO result = new UserDTO();

            if (id.IsNullOrEmpty())
            {
                return null;
            }

            var userresult = await QueryByIdAsync(id);

            return userresult.Adapt(result);
        }
        /// <summary>
        ///  获取用户列表信息-分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>      
        public async Task<PageResult<UserDTO>> GetListAsync(PageInput<UsersDTO> input)
        {
            var search = input.Search;
            //构建查询条件
            var expable = Expressionable.Create<Base_User, Base_Department>();
            expable.AndIF(!search.Id.IsNullOrEmpty(), (x, y) => x.Id == search.Id);
            expable.AndIF(!search.Username.IsNullOrEmpty(), (x, y) => x.UserName.Contains(search.Username));
            expable.AndIF(!search.RealName.IsNullOrEmpty(), (x, y) => x.RealName.Contains(search.RealName));
            expable.AndIF(!search.DepartmentId.IsNullOrEmpty(), (x, y) => x.DepartmentId == search.DepartmentId);

            //构建查询func 
            var db_result = await QueryMuchPageAsync<Base_User, Base_Department, UserDTO>(
                 //关联表
                 (x, y) => new object[]
                 {
                    JoinType.Left, x.DepartmentId==y.Id
                 },
                 //返回值拼装
                 (x, y) => new UserDTO(),
                 //where条件
                 expable,
                 input.PageIndex,
                 input.PageSize
                 );

            await SetProperty(db_result.data);

            return db_result;

            async Task SetProperty(List<UserDTO> users)
            {
                var expable = Expressionable.Create<Base_UserRole, Base_Role>();
                List<string> userIds = users.Select(x => x.Id).ToList();
                List<string> departments = users.Select(x => x.DepartmentId).ToList();
                expable.And((x, y) => userIds.Contains(x.UserId));
                //补充用户角色属性             
                var userRoles = await Db.Queryable<Base_UserRole>()
                      .LeftJoin<Base_Role>((x, y) => x.RoleId == y.Id)
                       .Where(expable.ToExpression())
                       .Select((x, y) => new {
                           x.UserId,
                           RoleId = y.Id,
                           y.RoleName
                       }).ToListAsync();

                users.ForEach(aUser =>
                {
                    var roleList = userRoles.Where(x => x.UserId == aUser.Id);
                    aUser.RoleIdList = roleList.Select(x => x.RoleId).ToList();
                    aUser.RoleNameList = roleList.Select(x => x.RoleName).ToList();
                });


                var departmentnames = await Db.Queryable<Base_Department>()
                    .Where(x => departments.Contains(x.Id))
                    .Select(x => new {
                        x.Id,
                        x.Name
                    }).ToListAsync();

                users.ForEach(aUser =>
              {
                  var departList = departmentnames.Where(x => x.Id == aUser.DepartmentId);
                  aUser.DepartmentName = departList.Select(x => x.Name).First();
              });
            }
        }
        public async Task<bool> ExistByName(string name)
        {
            return await ExistsAsync(x => x.UserName == name);
        }

        public async Task<string> LoginAsync(LoginInputDTO input, bool pwdtomd5 = false)
        {
            if (pwdtomd5)
            {
                input.password = input.password.ToMD5String();
            }
            var theUser = await QueryByClauseAsync(x => x.UserName.ToLower() == input.userName.ToLower() && x.Password == input.password);
            if (theUser.IsNullOrEmpty())
                throw new BusException("账号或密码不正确！");

            return theUser.Id;
        }
        #endregion
        #region 修改
        public async Task AddAsync(Base_User user, List<string> roleIdList)
        {
            var existName = await ExistsAsync((x) => x.UserName == user.UserName);
            if (existName)
            {
                throw new BusException($"用户名{user.UserName}已存在", 500);
            }
            await InsertAsync(user);
            await SetUserRoleAsync(user.Id, roleIdList);
        }
        public async Task UpdateAsync(Base_User user, List<string> roleIdList)
        {
            if (user.Id == GlobalAssemblies.ADMINID && _operator?.UserId != user.Id)
                throw new BusException("禁止更改超级管理员！");

            await UpdateIgnoreNullAsync(user);
            await SetUserRoleAsync(user.Id, roleIdList);
            //TODO 缓存更新
            //await _userCache.UpdateCacheAsync(input.Id);
        }
        public async Task DeleteAsync(List<string> ids)
        {
            if (ids.Count == 0)
            {
                throw new BusException("请输入删除ID");
            }
            if (ids.Contains(GlobalAssemblies.ADMINID))
                throw new BusException("超级管理员是内置账号,禁止删除！");

            await DeleteByIdsAsync(ids);
            //TODO 缓存更新
            //  await _userCache.UpdateCacheAsync(ids);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task ChangePwdAsync(ChangePwdDTO input)
        {
            var theUser = _operator.UserProperty;
            if (theUser.Password != input.oldPwd?.ToMD5String())
                throw new BusException("账号或密码不正确!", 500);

            theUser.Password = input.newPwd.ToMD5String();
            var mpdto = theUser.Adapt<Base_User>();
            //更新数据
            await UpdateAsync(mpdto);
            //更新缓存
            await _rediscache.RemoveAsync(theUser.Id);
        }

        #endregion

        #region 私有成员

        private async Task SetUserRoleAsync(string userId, List<string> roleIds)
        {
            roleIds = roleIds ?? new List<string>();
            if (roleIds.Count == 0)
            {
                return;
            }
            var userRoleList = roleIds.Select(x => new Base_UserRole
            {
                Id = IdHelper.NextId(),
                CreateTime = DateTime.Now,
                UserId = userId,
                RoleId = x
            }).ToList();

            await Db.Deleteable<Base_UserRole>().Where(x => x.UserId == userId).ExecuteCommandAsync();
            await Db.Insertable<Base_UserRole>(userRoleList).ExecuteCommandAsync();
        }

        #endregion
    }

}
