using Microsoft.Identity.Client;
using MossadAPI.Models;

namespace MossadAPI.Manegers
{
    public static class ChangeLocation
    {
        public static Location Move(Direction direction, Location location)
        {
            location.X += direction.dictionary[direction.direction][0];
            location.Y += direction.dictionary[direction.direction][1];
            return location;

        }

        public static Location GoToTarget(Location agentLocation, Location targetLocation)
        {
            if (agentLocation.X > targetLocation.X)
            {
                agentLocation.X -= 1;
            }
            else if (agentLocation.X < targetLocation.X)
            {
                agentLocation.X += 1;
            }
            if (agentLocation.Y > targetLocation.Y)
            {
                agentLocation.Y -= 1;
            }
            else if (agentLocation.Y < targetLocation.Y)
            {
                agentLocation.Y += 1;
            }
            return agentLocation;
        }

        public static bool InRange(int num)
        {
            if (num >= 0 && num <= 1000)
            {
                return true;
            }
            return false;
        }
    }
}
