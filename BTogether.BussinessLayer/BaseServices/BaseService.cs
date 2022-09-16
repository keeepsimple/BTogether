using BTogether.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BTogether.BussinessLayer.BaseServices
{
    public class BaseServices<T> : IBaseServices<T> where T : class
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual int Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Add(entity);
            return _unitOfWork.SaveChanges();
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Add(entity);
            return await _unitOfWork.SaveChangesAsync();
        }

        public bool Delete(int id)
        {
            var entity = _unitOfWork.CoreRepository<T>().GetById(id);
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Delete(entity);
            return _unitOfWork.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = _unitOfWork.CoreRepository<T>().GetById(id);
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Delete(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public bool Delete(T entity)
        {
            _unitOfWork.CoreRepository<T>().Delete(entity);
            return _unitOfWork.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _unitOfWork.CoreRepository<T>().Delete(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _unitOfWork.CoreRepository<T>().GetQuery().ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _unitOfWork.CoreRepository<T>().GetQuery().ToListAsync();
        }

        public virtual T GetById(int id)
        {
            return _unitOfWork.CoreRepository<T>().GetById(id);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _unitOfWork.CoreRepository<T>().GetByIdAsync(id);
        }

        public virtual bool Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Update(entity);
            return _unitOfWork.SaveChanges() > 0;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
