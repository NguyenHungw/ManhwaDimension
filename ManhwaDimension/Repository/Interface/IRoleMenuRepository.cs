using ManhwaDimension.Models;
using ManhwaDimension.Models.ViewModel;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Repository.Interface
{
    public interface IRoleMenuRepository : IBaseRepository<RoleMenu>
    {
        Task<DTResult<RoleMenuViewModel>> ListServerSide(RoleMenuDTParameters parameters);
        Task<List<RoleMenu>> ListByRole(int roleId);
        Task UpdateRoleMenusAsync(long roleId, IEnumerable<long> checkedMenuIds, IEnumerable<long> uncheckedMenuIds);

    }
}
