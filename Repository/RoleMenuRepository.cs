using ManhwaDimension.Models;
using ManhwaDimension.Models.ViewModel;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT;

namespace ManhwaDimension.Repository
{
    public class RoleMenuRepository : BaseRepository<RoleMenu>, IRoleMenuRepository
    {
        public RoleMenuRepository(BookwormDbContext _db) : base(_db)
        {
        }

        public Task<List<RoleMenu>> ListByRole(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<DTResult<RoleMenuViewModel>> ListServerSide(RoleMenuDTParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRoleMenusAsync(long roleId, IEnumerable<long> checkedMenuIds, IEnumerable<long> uncheckedMenuIds)
        {
            throw new NotImplementedException();
        }
    }
}
