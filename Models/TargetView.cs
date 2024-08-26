namespace MossadAPI.Models
{
    public class TargetView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string PhotoUrl { get; set; }
        public string Location { get; set; }
        public StatusTarget? StatusTarget { get; set; }
    }
}
