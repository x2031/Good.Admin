using Good.Admin.Entity;
using Good.Admin.Util;

namespace Good.Admin.IBusiness
{
    public interface IBase_UserBusiness
    {

        Task<PageResult<Base_UserDTO>> GetDataListAsync(PageInput<Base_UsersInputDTO> input);
        //Task<List<SelectOption>> GetOptionListAsync(OptionListInputDTO input);
        Task<Base_UserDTO> GetTheDataAsync(string id);
        Task AddDataAsync(UserEditInputDTO input);
        Task UpdateDataAsync(UserEditInputDTO input);
        Task DeleteDataAsync(List<string> ids);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input">用户名密码</param>
        /// <param name="pwdtomd5">是否转md5</param>
        /// <returns></returns>
        Task<string> LoginAsync(LoginInputDTO input, bool pwdtomd5 = false);
        Task ChangePwdAsync(ChangePwdInputDTO iqnput);
    }
}
