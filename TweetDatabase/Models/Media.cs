namespace TweetDatabase.Models
{
    public class Media
    {
        public long Id { get; set; }
        public MediaType MediaType { get; set; }
        public string Url { get; set; }
        
        public long TweetId { get; set; }
        public Tweet Tweet { get; set; }
    }

    public enum MediaType
    {
        Photo,
        Video,
        AnimatedGIF
    }
}