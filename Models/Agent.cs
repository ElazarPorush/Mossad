using Microsoft.AspNetCore.Components.Routing;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Agent
    {
        [Key]
        public int ID { get; set; }
        public string photoUrl { get; set; }
        public string nickname { get; set; }
        public Location? location { get; set; } = new Location();
        public StatusAgent? status { get; set; }
    }
}
