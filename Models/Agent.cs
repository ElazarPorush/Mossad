using Microsoft.AspNetCore.Components.Routing;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Agent
    {
        [Key]
        public Guid? ID { get; set; }
        public string photo_url { get; set; }
        public string nickname { get; set; }
        public int? Location { get; set; }
        public StatusAgent? status { get; set; }
    }
}
