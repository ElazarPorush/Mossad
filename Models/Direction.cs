namespace MossadAPI.Models
{
    public class Direction
    {
        public string direction {  get; set; }
        public Dictionary<string, List<int>> dictionary = new Dictionary<string, List<int>>()
        {
            { "nw", [-1, 1] },
            { "n" , [0, 1] },
            { "ne" , [1, 1] },
            { "w" , [-1, 0] },
            { "e" , [1, 0] },
            { "sw" , [-1, -1] },
            { "s" , [0, -1] },
            { "se" , [1, -1] }
        };
    }
}
