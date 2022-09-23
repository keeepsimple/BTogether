using BTogether.BussinessLayer.BaseServices;
using BTogether.BussinessLayer.IServices;
using BTogether.Data.Infrastructure;
using BTogether.Models;
using Microsoft.EntityFrameworkCore;

namespace BTogether.BussinessLayer.Services
{
    public class HobbyService : BaseServices<Hobby>, IHobbyService
    {
        public HobbyService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<Hobby>> GetHobbiesByUserId(string userId)
        {
            var love = await _unitOfWork.LoveRepository.GetQuery(x => x.UserId == userId || x.PartnerId == userId).FirstOrDefaultAsync();
            return await _unitOfWork.HobbyRepository.GetQuery(x => x.LoveId == love.Id).ToListAsync();
        }
    }
}
