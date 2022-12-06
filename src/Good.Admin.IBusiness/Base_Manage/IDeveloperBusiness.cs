using Good.Admin.Entity;
using Good.Admin.Util;

namespace Good.Admin.IBusiness
{
    public interface IDeveloperBusiness
    {
        List<Base_DbLink> GetAllDbLink();

        List<DbTableInfo> GetDbTableList(string linkId);

        void Build(BuildInputDTO input);
    }
}
