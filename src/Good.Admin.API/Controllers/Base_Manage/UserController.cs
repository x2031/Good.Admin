using Good.Admin.Entity;
using Good.Admin.IBusiness;
using Good.Admin.Util;
using Mapster;
using Microsoft.AspNetCore.Http;
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
        [HttpGet]
        public async Task<Base_UserDTO> GetTheDataAsync(string id)
        {
            return await _userBus.GetTheDataAsync(id);
        }

        #endregion
        #region 修改
        /// <summary>
        /// 保存数据-实现更新和新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SaveData(UserEditInputDTO input)
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
                UpdateInitEntity(input);
                await _userBus.UpdateDataAsync(input);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _userBus.DeleteDataAsync(ids);
        }
        #endregion
    }
}
