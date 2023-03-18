using Good.Admin.Entity;
using Good.Admin.Util;

namespace Good.Admin.IBusiness
{
    public interface IBase_DbLinkBusiness
    {
        Task<PageResult<Base_DbLink>> GetListAsync(PageInput input);
        Task<Base_DbLink> GetTheDataAsync(string id);
        Task AddAsync(Base_DbLink newData);
        Task UpdateAsync(Base_DbLink theData);
        Task DeleteAsync(List<string> ids);
    }
}
