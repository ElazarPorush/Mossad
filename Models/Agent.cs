using Microsoft.AspNetCore.Components.Routing;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Agent
    {
        [Key]
        public Guid? ID { get; set; }
        public string Photo_url { get; set; }
        public string Nickname { get; set; }
        public Location? Location { get; set; }
        public StatusAgent? Status { get; set; }
    }
}
