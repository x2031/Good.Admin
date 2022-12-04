using Good.Admin.Entity;

namespace Good.Admin.IBusiness.Base_Manage
{
    public interface IBase_ActionBusines
    {
        Task<List<Base_Action>> GetDataListAsync(Base_ActionsInputDTO input);
        Task<List<Base_ActionDTO>> GetTreeDataListAsync(Base_ActionsInputDTO input);
        Task<Base_Action> GetTheDataAsync(string id);
        Task AddDataAsync(ActionEditInputDTO input);
        Task UpdateDataAsync(ActionEditInputDTO input);
        Task DeleteDataAsync(List<string> ids);
    }
}
