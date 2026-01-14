using ManhwaDimension.Models;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Repository.Interface
{
    public interface IRoleMenuRepository : IBaseRepository<Role>
    {
        Task<DTResult<Role>> ListServerSide(RoleDTParameters parameters);
      
    }
}
