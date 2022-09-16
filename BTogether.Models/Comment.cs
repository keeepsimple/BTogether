namespace BTogether.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string CommentText { get; set; }

        public string Username { get; set; }

        public int StoryId { get; set; }

        public Story Story { get; set; }
    }
}
