﻿using Good.Admin.Entity;
using Good.Admin.Util;

namespace Good.Admin.IBusiness
{
    public interface IBase_UserBusiness
    {
        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResult<Base_UserDTO>> GetDataListAsync(PageInput<Base_UsersInputDTO> input);
        //Task<List<SelectOption>> GetOptionListAsync(OptionListInputDTO input);
        /// <summary>
        /// 根据id获取单个用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Base_UserDTO> GetTheDataAsync(string id);
        /// <summary>
        /// 根据用户名查询是否存在同名数据
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        Task<bool> ExistByName(string name);
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddDataAsync(UserEditInputDTO input);
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateDataAsync(UserEditInputDTO input);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids">id数据</param>
        /// <returns></returns>
        Task DeleteDataAsync(List<string> ids);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input">用户名密码</param>
        /// <param name="pwdtomd5">是否转md5</param>
        /// <returns></returns>
        Task<string> LoginAsync(LoginInputDTO input, bool pwdtomd5 = false);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="iqnput"></param>
        /// <returns></returns>
        Task ChangePwdAsync(ChangePwdInputDTO iqnput);
    }
}
