using Microsoft.AspNetCore.Identity;

namespace BTogether.Models
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }

        public int BirthYear { get; set; }

        public string? IsInLoveWith { get; set; }

        public virtual ICollection<Love> Loves { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
