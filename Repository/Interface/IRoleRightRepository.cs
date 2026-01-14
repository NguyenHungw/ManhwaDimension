using ManhwaDimension.Models;
using ManhwaDimension.Models.ViewModel;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Repository.Interface
{
    public interface IRoleRightRepository : IBaseRepository<RoleMenu>
    {
        Task<DTResult<RoleMenuViewModel>> ListServerSide(RoleMenuDTParameters parameters);
    

    }
}
