using BTogether.BussinessLayer.BaseServices;
using BTogether.BussinessLayer.IServices;
using BTogether.Data.Infrastructure;
using BTogether.Models;

namespace BTogether.BussinessLayer.Services
{
    public class StoryService : BaseServices<Story>, IStoryService
    {
        public StoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
