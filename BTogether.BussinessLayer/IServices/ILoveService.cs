using BTogether.BussinessLayer.BaseServices;
using BTogether.Models;

namespace BTogether.BussinessLayer.IServices
{
    public interface ILoveService : IBaseServices<Love>
    {
        bool CheckUserCreateLove(string userId);

        Task<int> GetLoveIdByUserId(string userId);
    }
}
