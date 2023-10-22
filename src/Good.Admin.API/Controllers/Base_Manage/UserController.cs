using Good.Admin.Common.Primitives;
using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Common;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Good.Admin.API.Controllers.Base_Manage
{
    [OpenApiTag("用户管理")]
    [Route("api/[controller]/[action]")]
    public class UserController : BaseApiController
    {
        #region DI
        private readonly IHttpContextAccessor _accessor;
        private readonly IBase_UserBusiness _userBus;
        public UserController(IHttpContextAccessor accessor, IBase_UserBusiness userBus)
        {
            _accessor = accessor;
            _userBus = userBus;

        }
        #endregion
        #region 查询
        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        //[ApiPermission("Base_User.GetDataList")]
        public async Task<PageResult<UserDTO>> GetList([FromBody] PageInput<UsersDTO> input)
        {
            return await _userBus.GetListAsync(input);
        }
        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        [HttpPost]
        [ApiPermission("Base_User.GetDataById")]
        public async Task<UserDTO> GetDataByIdAsync(IdInputDTO input)
        {
            return await _userBus.GetTheDataAsync(input.id);
        }

        public async Task<bool> ExistByName(NameInputNoNullDTO input)
        {
            return await _userBus.ExistByName(input.name);
        }

        #endregion
        #region 修改
        /// <summary>
        /// 更新用户数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        //[ApiPermission("Base_User.Update")]
        public async Task Update(UserEditDTO input)
        {
            var user = input.Adapt<Base_User>();

            if (!input.newPwd.IsNullOrEmpty())
                user.Password = input.newPwd.ToMD5String();

            if (input.Id.IsNullOrEmpty())
            {
                throw new BusException("更新用户数据必须传入用户标识字符串!", 500);
            }
            else
            {
                UpdateInitEntity(user);
                await _userBus.UpdateAsync(user, input.RoleIdList);
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Add(UserEditDTO input)
        {
            var user = input.Adapt<Base_User>();

            if (!input.newPwd.IsNullOrEmpty())
            {
                user.Password = input.newPwd.ToMD5String();
            }
            else
            {
                //初始密码：用户名+123456
                input.newPwd = user.UserName + "123456";
                user.Password = input.newPwd.ToMD5String();
            }


            if (user.Id.IsNullOrEmpty())
            {

                InitEntity(user);
                await _userBus.AddAsync(user, input.RoleIdList);
            }
            else
            {
                throw new BusException("新增数据不允许传入用户ID!", 500);
            }
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiPermission("Base_User.ChangePwd")]
        public async Task ChangePwdAsync(ChangePwdDTO inputDTO)
        {
            await _userBus.ChangePwdAsync(inputDTO);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        //[ApiPermission("Base_User.Delete")]
        public async Task Delete(List<string> ids)
        {
            await _userBus.DeleteAsync(ids);
        }
        #endregion
    }
}
