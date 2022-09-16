namespace BTogether.BussinessLayer.BaseServices
{
    public interface IBaseServices<T>
    {
        int Add(T entity);

        Task<int> AddAsync(T entity);

        bool Update(T entity);

        Task<bool> UpdateAsync(T entity);

        bool Delete(int id);

        Task<bool> DeleteAsync(int id);

        bool Delete(T entity);

        Task<bool> DeleteAsync(T entity);

        T GetById(int id);

        Task<T> GetByIdAsync(int id);

        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();
    }
}
