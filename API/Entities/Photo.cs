namespace API.Entities
{
    public class Photo
    {
        public string Id { get; set; }
        public string PhotoUrl { get; set; }
        public string PublicId { get; set; }
        public string MemoryId { get; set; }
        public Memory Memory { get; set; }
    }
}