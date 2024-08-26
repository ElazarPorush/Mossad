using Microsoft.AspNetCore.Components.Routing;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Target
    {
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string photoUrl { get; set; }
        public Location? location { get; set; } = new Location();
        public StatusTarget? status { get; set; }
    }
}
