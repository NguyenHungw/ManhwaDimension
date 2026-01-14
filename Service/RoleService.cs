using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;
using ManhwaDimension.Util.Entities;
using NPOI.SS.Formula.Functions;
using ManhwaDimension.Util.Extentions;
namespace ManhwaDimension.Service
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        IRoleRepository repositorRole;
        public RoleService(IBaseRepository<Role> _repository, IRoleRepository _repositorRole) : base(_repository)
        {
            repositorRole = _repositorRole;
        }

        public async Task<DTResult<Role>> ListServerSide(RoleDTParameters parameters)
        {
            return await repositorRole.ListServerSide(parameters);
        }
   
 
    }
}
