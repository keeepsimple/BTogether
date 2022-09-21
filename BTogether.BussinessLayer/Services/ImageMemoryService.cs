using BTogether.BussinessLayer.BaseServices;
using BTogether.BussinessLayer.IServices;
using BTogether.Data.Infrastructure;
using BTogether.Models;
using Microsoft.EntityFrameworkCore;

namespace BTogether.BussinessLayer.Services
{
    public class ImageMemoryService : BaseServices<ImageMemory>, IImageMemoryService
    {
        public ImageMemoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<ImageMemory>> GetImageMemoriesByStoryIdAsync(int storyId)
        {
            return await _unitOfWork.ImageMemoryRepository.GetQuery(x => x.StoryId == storyId).ToListAsync();
        }
    }
}
