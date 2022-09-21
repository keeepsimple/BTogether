using BTogether.BussinessLayer.BaseServices;
using BTogether.Models;

namespace BTogether.BussinessLayer.IServices
{
    public interface IHobbyService : IBaseServices<Hobby>
    {
        Task<IEnumerable<Hobby>> GetHobbiesByUserId(string userId);
    }
}
