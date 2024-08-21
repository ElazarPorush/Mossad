using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        public Agent Agent { get; set; }
        public Target Target { get; set; }
        [Range(0, 40)]
        public Double? TimeLeft { get; set; }
        public StatusMission Status { get; set; }
        public DateTime? ExecutionTime { get; set; }
    }
}
