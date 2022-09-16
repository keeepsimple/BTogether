using System.Linq.Expressions;

namespace BTogether.Data.Infrastructure.Repositories
{
    public interface ICoreRepository<Entity>
    {
        Task<Entity> GetByIdAsync(int id);

        Entity GetById(int id);

        void Add(Entity entity);

        void Delete(Entity entity);

        IQueryable<Entity> GetQuery();

        IQueryable<Entity> GetQuery(Expression<Func<Entity, bool>> where);

        void Update(Entity entity);
    }
}
