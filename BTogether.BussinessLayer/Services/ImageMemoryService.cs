using BTogether.BussinessLayer.BaseServices;
using BTogether.BussinessLayer.IServices;
using BTogether.Data.Infrastructure;
using BTogether.Models;

namespace BTogether.BussinessLayer.Services
{
    public class ImageMemoryService : BaseServices<ImageMemory>, IImageMemoryService
    {
        public ImageMemoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
