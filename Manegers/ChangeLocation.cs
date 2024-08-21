using Microsoft.Identity.Client;
using MossadAPI.Models;

namespace MossadAPI.Manegers
{
    public static class ChangeLocation
    {
        public static Location Move(Direction direction, Location location)
        {
            switch (direction)
            {
                case Direction.nw:
                {
                    location.X = Reduce(location.X);
                    location.Y = Reduce(location.Y);
                    break;
                }
                case Direction.n:
                {
                    location.Y = Reduce(location.Y);
                    break;
                }
                case Direction.ne:
                {
                    location.X = Add(location.X);
                    location.Y = Reduce(location.Y);
                    break;
                }
                case Direction.w:
                {
                    location.X = Reduce(location.X);
                    break;
                }
                case Direction.e:
                {
                    location.X = Add(location.X);
                    break;
                }
                case Direction.sw:
                {
                    location.X = Reduce(location.X);
                    location.Y = Add(location.Y);
                    break;
                }
                case Direction.s:
                {
                    location.Y = Add(location.Y);
                    break;
                }
                case Direction.se:
                {
                    location.X = Add(location.X);
                    location.Y = Add(location.Y);
                    break;
                }
            }
            return location;

        }
        private static bool InRange(int num)
        {
            if (num > 0 && num < 1000)
            {
                return true;
            }
            return false;
        }

        private static int Add(int num)
        {
            if (InRange(num))
            {
                num += 1;
            }
            return num;
        }
        private static int Reduce(int num)
        {
            if (InRange(num))
            {
                num -= 1;
            }
            return num;
        }


    }
}
