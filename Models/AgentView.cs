namespace MossadAPI.Models
{
    public class AgentView
    {
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
        public string Nickname { get; set; }
        public StatusAgent? StatusAgent { get; set; }
        public string Location { get; set; }
        public int Kills { get; set; } = 0;
        public int? MissionID { get; set; }
        public double? TimeLeft { get; set; }
    }
}
