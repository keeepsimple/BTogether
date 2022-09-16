using BTogether.BussinessLayer.BaseServices;
using BTogether.BussinessLayer.IServices;
using BTogether.Data.Infrastructure;
using BTogether.Models;

namespace BTogether.BussinessLayer.Services
{
    public class HobbyService : BaseServices<Hobby>, IHobbyService
    {
        public HobbyService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
