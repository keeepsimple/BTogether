using BTogether.BussinessLayer.BaseServices;
using BTogether.Models;

namespace BTogether.BussinessLayer.IServices
{
    public interface IStoryService : IBaseServices<Story>
    {
        Task<IEnumerable<Story>> GetStoryByLoveIdAsync(int loveId);
    }

}
