using BTogether.BussinessLayer.BaseServices;
using BTogether.BussinessLayer.IServices;
using BTogether.Data.Infrastructure;
using BTogether.Models;
using Microsoft.EntityFrameworkCore;

namespace BTogether.BussinessLayer.Services
{
    public class StoryService : BaseServices<Story>, IStoryService
    {
        public StoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<Story>> GetStoryByLoveIdAsync(int loveId)
        {
            return await _unitOfWork.StoryRepository.GetQuery(x => x.LoveId == loveId).ToListAsync();
        }
    }
}
