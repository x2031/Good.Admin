using Good.Admin.Common.Primitives;
using Good.Admin.Entity;

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
