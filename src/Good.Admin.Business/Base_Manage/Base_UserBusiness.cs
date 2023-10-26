using Good.Admin.Common;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Mapster;
using SqlSugar;

namespace Good.Admin.Business
{
    public class Base_UserBusiness : IBase_UserBusiness, ISingletonDependency
    {
        private readonly ISqlSugarClient _db;
        readonly IOperator _operator;
        readonly IRedisBasketRepository _rediscache;
        public Base_UserBusiness(ISqlSugarClient sqlSugarClient, IOperator nowOperator, IRedisBasketRepository rediscache)
        {
            _operator = nowOperator;
            _rediscache = rediscache;
            _db = sqlSugarClient;
        }


        #region 查询      
        public async Task<UserDTO> GetTheDataAsync(string id)
        {
            UserDTO result = new UserDTO();

            if (id.IsNullOrEmpty())
            {
                return null;
            }
            var userresult = await _db.Queryable<Base_User>().Where(x => x.Id == id).Where(x => x.Deleted == 0).FirstAsync();

            return userresult.Adapt(result);
        }
        /// <summary>
        ///  获取用户列表信息-分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>      
        public async Task<PageResult<UserDTO>> GetListAsync(PageInput<UsersDTO> input)
        {
            RefAsync<int> total = 0;
            var search = input.Search;
            //构建查询条件
            var expable = Expressionable.Create<Base_User, Base_Department>();
            expable.AndIF(!search.Id.IsNullOrEmpty(), (x, y) => x.Id == search.Id);
            expable.AndIF(!search.Username.IsNullOrEmpty(), (x, y) => x.UserName.Contains(search.Username));
            expable.AndIF(!search.RealName.IsNullOrEmpty(), (x, y) => x.RealName.Contains(search.RealName));
            expable.AndIF(!search.DepartmentId.IsNullOrEmpty(), (x, y) => x.DepartmentId == search.DepartmentId);



            var result = await _db.Queryable<Base_User>()
                    .LeftJoin<Base_Department>((x, y) => x.DepartmentId == y.Id)
                    .Where(x => x.Deleted == 0)
                    .Where(expable.ToExpression())
                    .ToPageListAsync(input.PageIndex, input.PageSize, total);

            var pageResult = new PageResult<UserDTO>(input.PageIndex, total.Value, input.PageSize, result.Adapt(new List<UserDTO>()));

            await SetProperty(pageResult.data);

            return pageResult;

            async Task SetProperty(List<UserDTO> users)
            {
                var expable = Expressionable.Create<Base_UserRole, Base_Role>();
                List<string> userIds = users.Select(x => x.Id).ToList();
                List<string> departments = users.Select(x => x.DepartmentId).ToList();
                expable.And((x, y) => userIds.Contains(x.UserId));
                //补充用户角色属性             
                var userRoles = await _db.Queryable<Base_UserRole>()
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


                var departmentnames = await _db.Queryable<Base_Department>()
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

            return await _db.Queryable<Base_User>().Where(x => x.UserName == name).AnyAsync();
        }

        public async Task<string> LoginAsync(LoginInputDTO input, bool pwdtomd5 = false)
        {
            if (pwdtomd5)
            {
                input.password = input.password.ToMD5String();
            }
            var theUser = await _db.Queryable<Base_User>()
                .Where(x => x.UserName.ToLower() == input.userName.ToLower())
                .Where(x => x.Password == input.password)
                .FirstAsync();

            if (theUser.IsNullOrEmpty())
                throw new BusException("账号或密码不正确！");

            return theUser.Id;
        }
        #endregion
        #region 修改
        public async Task AddAsync(Base_User user, List<string> roleIdList)
        {
            var existName = await ExistByName(user.UserName);
            if (existName)
            {
                throw new BusException($"用户名{user.UserName}已存在", 500);
            }
            await _db.Insertable(user).ExecuteCommandAsync();
            await SetUserRoleAsync(user.Id, roleIdList);
        }
        public async Task UpdateAsync(Base_User user, List<string> roleIdList)
        {
            if (user.Id == GlobalAssemblies.ADMINID && _operator?.UserId != user.Id)
                throw new BusException("禁止更改超级管理员！");

            await _db.Updateable(user).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
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

            await _db.Deleteable<Base_User>(x => ids.Contains(x.Id)).ExecuteCommandAsync();
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
            await _db.Updateable(mpdto).ExecuteCommandAsync();
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

            await _db.Deleteable<Base_UserRole>().Where(x => x.UserId == userId).ExecuteCommandAsync();
            await _db.Insertable(userRoleList).ExecuteCommandAsync();
        }

        #endregion
    }

}
