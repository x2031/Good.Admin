using Good.Admin.Entity;

namespace Good.Admin.IBusiness
{
    public interface IBase_ActionBusines
    {
        Task<List<Base_Action>> GetListAsync(ActionsInputDTO input);
        Task<List<ActionDTO>> GetTreeListAsync(ActionsInputDTO input);
        Task<Base_Action> GetTheDataAsync(string id);
        Task AddAsync(ActionEditDTO input);
        Task UpdateAsync(ActionEditDTO input);
        Task DeleteAsync(List<string> ids);
    }
}
