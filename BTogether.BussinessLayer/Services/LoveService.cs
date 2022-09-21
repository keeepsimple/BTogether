using BTogether.BussinessLayer.BaseServices;
using BTogether.BussinessLayer.IServices;
using BTogether.Data.Infrastructure;
using BTogether.Models;
using Microsoft.EntityFrameworkCore;

namespace BTogether.BussinessLayer.Services
{
    public class LoveService : BaseServices<Love>, ILoveService
    {
        public LoveService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool CheckUserCreateLove(string userId)
        {
            var love = _unitOfWork.LoveRepository.GetQuery().Where(x => x.UserId == userId).ToList();
            if (love.Count > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<int> GetLoveIdByUserId(string userId)
        {
            var love = await _unitOfWork.LoveRepository.GetQuery(x => x.UserId == userId).FirstOrDefaultAsync();
            if (love == null)
            {
                return 0;
            }
            return love.Id;
        }
    }
}
