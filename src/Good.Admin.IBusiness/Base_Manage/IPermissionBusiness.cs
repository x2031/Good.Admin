using Good.Admin.Entity;

namespace Good.Admin.IBusiness
{
    public interface IPermissionBusiness
    {
        Task<List<string>> GetUserPermissionValuesAsync(string userId);
        Task<List<ActionDTO>> GetUserMenuListAsync(string userId);
    }
}
