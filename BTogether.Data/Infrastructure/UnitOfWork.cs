using BTogether.Data.Infrastructure.Repositories;
using BTogether.Models;

namespace BTogether.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BTogetherContext _dbContext;
        public BTogetherContext BTogetherContext => _dbContext;

        public UnitOfWork(BTogetherContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ICoreRepository<Love> _loveRepository;
        public ICoreRepository<Love> LoveRepository => _loveRepository ?? new CoreRepository<Love>(_dbContext);

        private ICoreRepository<Hobby> _hobbyRepository;
        public ICoreRepository<Hobby> HobbyRepository => _hobbyRepository ?? new CoreRepository<Hobby>(_dbContext);

        private ICoreRepository<ImageMemory> _imageMemoryRepository;
        public ICoreRepository<ImageMemory> ImageMemoryRepository => _imageMemoryRepository ?? new CoreRepository<ImageMemory>(_dbContext);

        private ICoreRepository<Story> _storyRepository;
        public ICoreRepository<Story> StoryRepository => _storyRepository ?? new CoreRepository<Story>(_dbContext);

        private ICoreRepository<Comment> _commentRepository;
        public ICoreRepository<Comment> CommentRepository => _commentRepository ?? new CoreRepository<Comment>(_dbContext);

        public ICoreRepository<T> CoreRepository<T>() where T : class
        {
            return new CoreRepository<T>(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
