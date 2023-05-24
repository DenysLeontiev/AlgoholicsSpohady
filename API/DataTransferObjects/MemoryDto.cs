namespace API.DataTransferObjects
{
    public class MemoryDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MemoryUrl { get; set; }
        public string MemoryQrCode { get; set; }
        public string OwnerId{ get; set; }
        public DateTime DateCreated { get; set; }
        // public List<UserDto> Users { get; set; } = new();
        public List<PhotoDto> Photos { get; set; } = new();
        public List<MessageDto> Messages { get; set; } = new();
    }
}