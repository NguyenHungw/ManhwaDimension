using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT;

namespace ManhwaDimension.Repository
{
    public class AccountRepository : BaseRepository<User>, IAccountRepository
    {
        public AccountRepository(BookwormDbContext _db) : base(_db)
        {
        }

        public Task<DTResult<User>> ListServerSide(AccountDTParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
