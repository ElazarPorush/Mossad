using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        public int agentID { get; set; }
        public int targetID { get; set; }
        [Range(0, 40)]
        public Double? TimeLeft { get; set; }
        public StatusMission Status { get; set; }
        public Double? ExecutionTime { get; set; }
    }
}
