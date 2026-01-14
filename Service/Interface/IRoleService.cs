using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Service.Interface
{
    public interface IRoleService : IBaseService<Role>
    {
        Task<DTResult<Role>> ListServerSide(RoleDTParameters parameters);

    }
}
