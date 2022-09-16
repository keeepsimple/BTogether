using BTogether.Data.Infrastructure.Repositories;
using BTogether.Models;

namespace BTogether.Data.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        BTogetherContext BTogetherContext { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();

        ICoreRepository<T> CoreRepository<T>() where T : class;

        #region Master Data

        ICoreRepository<Love> LoveRepository { get; }

        ICoreRepository<Hobby> HobbyRepository { get; }

        ICoreRepository<ImageMemory> ImageMemoryRepository { get; }

        ICoreRepository<Story> StoryRepository { get; }

        ICoreRepository<Comment> CommentRepository { get; }

        #endregion
    }
}
