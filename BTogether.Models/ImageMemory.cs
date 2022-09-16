namespace BTogether.Models
{
    public class ImageMemory
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public int StoryId { get; set; }

        public Story Story { get; set; }
    }
}