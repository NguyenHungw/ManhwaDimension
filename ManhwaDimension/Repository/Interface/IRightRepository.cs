using ManhwaDimension.Models;
using ManhwaDimension.Models.ViewModel;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Repository.Interface
{
    public interface IRightRepository : IBaseRepository<Role>
    {
        Task<DTResult<Right>> ListServerSide(RightDTParameters parameters);

    }
}
