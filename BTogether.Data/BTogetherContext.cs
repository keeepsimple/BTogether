using BTogether.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTogether.Data {
    public class BTogetherContext : IdentityDbContext<User> {
        public BTogetherContext(DbContextOptions<BTogetherContext> opt) : base(opt) {

        }

        public DbSet<Love> Loves { get; set; }

        public DbSet<ImageMemory> ImageMemories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Story> Stories { get; set; }

        public DbSet<Hobby> Hobbies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {

            base.OnModelCreating(builder);

            builder.Entity<User>().Ignore(x => x.PhoneNumber)
                .Ignore(x => x.PhoneNumberConfirmed);
        }
    }
}