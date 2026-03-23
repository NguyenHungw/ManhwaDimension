using ManhwaDimension.Models;
using ManhwaDimension.ULT;

namespace ManhwaDimension.Repository.Interface
{
    public interface IAccountRepository : IBaseRepository<User>
    {
        Task<DTResult<User>> ListServerSide(AccountDTParameters parameters);    
    }
}
