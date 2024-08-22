using Microsoft.EntityFrameworkCore;
using MossadAPI.Data;
using MossadAPI.Models;

namespace MossadAPI.Manegers
{
    public class MissionBase
    {
        private readonly MossadDBContext _context;
        public MissionBase(MossadDBContext context)
        {
            _context = context;
        }

        public async Task DeleteOldMissions()
        {
            foreach (Mission mission in _context.Missions)
            {
                Location? agentLocation = await _context.Locations.FindAsync(mission.agentID);
                Location? targetLocation = await _context.Locations.FindAsync(mission.targetID);
                if (!IsNear(agentLocation, targetLocation))
                {
                    _context.Missions.Remove(mission);
                    _context.SaveChanges();
                }
            }
        }

        protected bool IsNear(Location agentLocation, Location targetLocation)
        {
            if (Math.Sqrt(Math.Pow(targetLocation.X - agentLocation.X, 2) + Math.Pow(targetLocation.Y - agentLocation.Y, 2)) <= 200)
            {
                return true;
            }
            return false;
        }

        protected async Task CreateMission(Agent agent, Target target)
        {
            Mission mission = new Mission()
            {
                agentID = agent.ID,
                targetID = target.ID,
                Status = StatusMission.Suggestion
            };
            _context.Missions.Add(mission);
            _context.SaveChanges();
        }
    }
}
