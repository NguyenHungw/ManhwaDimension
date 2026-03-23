using ManhwaDimension.Models;
using ManhwaDimension.Models.DTO.Comic;
using ManhwaDimension.Repository;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;
using ManhwaDimension.Util.Entities;
using ManhwaDimension.Util.Extentions;
using NPOI.SS.Formula.Functions;
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
        public async Task Add(RoleDTO roledto)
        {
            //int lastId = repositorRole.GetMaxRoleId();  
            //int newId = lastId + 1;

            int resultId = await repositorRole.CheckDuplicatedRole(roledto.Id);
            int resultName = await repositorRole.CheckDuplicatedRole(roledto.Name);

            if (resultId > 0)
            {
                throw new Exception("Role ID "+roledto.Id+" already exists");
            }
            if (resultName > 0)
            {
                throw new Exception("Role Name " + roledto.Name + " already exists");
            }

            Role role = new Role
            {
                
                Id = roledto.Id,
                Name = char.ToUpper(roledto.Name[0]) + roledto.Name.Substring(1),
                Code = "ROLE_"+roledto.Name.ToUpper(),
                Active = true,
                CreatedAt = DateTime.Now
               
            };
            await repositorRole.Add(role);

        }

    }
}
