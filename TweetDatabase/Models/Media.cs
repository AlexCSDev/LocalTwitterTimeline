namespace TweetDatabase.Models
{
    public class Media
    {
        public long Id { get; set; }
        public MediaType MediaType { get; set; }
        public string Url { get; set; }
    }

    public enum MediaType
    {
        Photo,
        Video,
        AnimatedGIF
    }
}