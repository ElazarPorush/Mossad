using Microsoft.AspNetCore.Components.Routing;
using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Target
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string PhotoUrl { get; set; }
        public int? locationID { get; set; }
        public StatusTarget? status { get; set; }
    }
}
