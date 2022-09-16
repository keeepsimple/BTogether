namespace BTogether.Models
{
    public class Story
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int LoveId { get; set; }

        public Love Love { get; set; }

        public virtual ICollection<ImageMemory> ImageMemories { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}