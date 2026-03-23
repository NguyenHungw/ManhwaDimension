using ManhwaDimension.Models;
using ManhwaDimension.Models.ViewModel;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Repository.Interface
{
    public interface IRoleRightRepository : IBaseRepository<RoleRight>
    {
        Task<DTResult<RoleRightViewModel>> ListServerSide(RoleRightDTParameters parameters);
    

    }
}
