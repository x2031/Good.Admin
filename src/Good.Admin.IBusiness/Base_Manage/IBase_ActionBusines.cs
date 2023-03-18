using Good.Admin.Entity;

namespace Good.Admin.IBusiness.Base_Manage
{
    public interface IBase_ActionBusines
    {
        Task<List<Base_Action>> GetListAsync(Base_ActionsInputDTO input);
        Task<List<Base_ActionDTO>> GetTreeListAsync(Base_ActionsInputDTO input);
        Task<Base_Action> GetTheDataAsync(string id);
        Task AddAsync(ActionEditInputDTO input);
        Task UpdateAsync(ActionEditInputDTO input);
        Task DeleteAsync(List<string> ids);
    }
}
