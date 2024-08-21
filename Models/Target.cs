using Microsoft.AspNetCore.Components.Routing;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Target
    {
        [Key]
        public Guid? ID { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string photo_url { get; set; }
        public int? Location { get; set; }
        public StatusTarget? status { get; set; }
    }
}
