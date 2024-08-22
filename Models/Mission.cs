using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        public Guid agentID { get; set; }
        public Guid targetID { get; set; }
        [Range(0, 40)]
        public Double? TimeLeft { get; set; }
        public StatusMission Status { get; set; }
        public Double? ExecutionTime { get; set; }
    }
}
