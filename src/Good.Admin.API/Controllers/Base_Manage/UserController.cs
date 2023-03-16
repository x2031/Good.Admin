using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Util;
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
        [ApiPermission("Base_User.GetDataList")]
        // [IgnoreVaild]        
        public async Task<PageResult<Base_UserDTO>> GetDataList([FromBody] PageInput<Base_UsersInputDTO> input)
        {
            return await _userBus.GetDataListAsync(input);
        }
        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        [HttpPost]
        [ApiPermission("Base_User.GetDataById")]
        public async Task<Base_UserDTO> GetDataByIdAsync(IdInputDTO input)
        {
            return await _userBus.GetTheDataAsync(input.id);
        }

        public async Task<bool> ExistByName(NameInputDTO input)
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
        [ApiPermission("Base_User.Update")]
        public async Task UpdateData(UserEditInputDTO input)
        {
            if (!input.newPwd.IsNullOrEmpty())
                input.Password = input.newPwd.ToMD5String();

            if (input.Id.IsNullOrEmpty())
            {
                throw new BusException("更新用户数据必须传入用户标识字符串!", 511);
            }
            else
            {
                UpdateInitEntity(input);
                await _userBus.UpdateDataAsync(input);
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiPermission("Base_User.Add")]
        public async Task AddData(UserEditInputDTO input)
        {
            if (!input.newPwd.IsNullOrEmpty())
                input.Password = input.newPwd.ToMD5String();

            if (input.Id.IsNullOrEmpty())
            {
                InitEntity(input);
                await _userBus.AddDataAsync(input);
            }
            else
            {
                throw new BusException("新增数据不允许传入用户ID!", 510);
            }
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiPermission("Base_User.ChangePwd")]
        public async Task ChangePwdAsync(ChangePwdInputDTO inputDTO)
        {
            await _userBus.ChangePwdAsync(inputDTO);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ApiPermission("Base_User.Delete")]
        public async Task DeleteData(List<string> ids)
        {
            await _userBus.DeleteDataAsync(ids);
        }
        #endregion
    }
}
