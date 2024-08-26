using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        public Agent agent { get; set; } = new Agent();
        public Target target { get; set; } = new Target();
        [Range(0, 40)]
        public Double? TimeLeft { get; set; }
        public StatusMission Status { get; set; }
        public Double? ExecutionTime { get; set; }
    }
}
