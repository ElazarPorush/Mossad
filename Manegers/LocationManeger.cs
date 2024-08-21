using MossadAPI.Models;

namespace MossadAPI.Manegers
{
    public static class LocationManeger
    {
        public static int Random()
        {
            Random random = new Random();
            int num = random.Next(0, 1001);
            return num;
        }

        public static Location GetLocation()
        {
            int x = Random();
            int y = Random();
            Location location = new Location(x, y);
            return location;
        }
    }
}
