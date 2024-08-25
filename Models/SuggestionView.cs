using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class SuggestionView
    {
        public int MissionID { get; set; }
        public string AgentNickname { get; set; }
        public string AgentLocation { get; set; }
        public string TargetName { get; set; }
        public string TargetPosition { get; set; }
        public string TargetLocation { get; set; }
        public double Distance { get; set; }
        public double? TimeLeft { get; set; }

    }
}
