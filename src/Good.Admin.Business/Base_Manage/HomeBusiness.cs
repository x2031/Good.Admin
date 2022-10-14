using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Repository;
using Good.Admin.Util;

namespace Good.Admin.Business
{
    public class HomeBusiness : BaseRepository<Base_User>, IHomeBusiness, ITransientDependency
    {

        public HomeBusiness(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task ChangePwdAsync(ChangePwdInputDTO input)
        {
            //var theUser = _operator.Property;
            //if (theUser.Password != input.oldPwd?.ToMD5String())
            //    throw new BusException("原密码错误!");
            //theUser.Password = input.newPwd.ToMD5String();
            //await UpdateAsync(_mapper.Map<Base_User>(theUser));
            ////更新缓存
            //await _base_UserCache.UpdateCacheAsync(theUser.Id);
            throw new BusException("账号或密码不正确！");
        }
        public async Task<string> LoginAsync(LoginInputDTO input)
        {
            input.password = input.password.ToMD5String();
            var theUser = await QueryByClauseAsync(x => x.UserName == input.userName && x.Password == input.password);

            if (theUser.IsNullOrEmpty())
                throw new BusException("账号或密码不正确！");

            return theUser.Id;
        }
    }
}
