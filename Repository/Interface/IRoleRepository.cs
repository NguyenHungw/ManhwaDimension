using ManhwaDimension.Models;
using ManhwaDimension.Models.ViewModel;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Repository.Interface
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<DTResult<Role>> ListServerSide(RoleDTParameters parameters);
        public int GetMaxRoleId();
        public Task<int> CheckDuplicatedRole<T>(T id );
    }
}
