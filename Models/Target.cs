using Microsoft.AspNetCore.Components.Routing;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Target
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public Location Location { get; set; }
        public StatusTarget Status { get; set; }
    }
}
