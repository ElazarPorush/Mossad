using System.ComponentModel.DataAnnotations;

namespace MossadAPI.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Range(0, 1000)]
        public int X { get; set; }
        [Range(0, 1000)]
        public int Y { get; set; }

        public Location() { }
        public Location(int x, int y) { X = x; Y = y; }
    }
}
