using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MossadAPI.Data;
using MossadAPI.Models;

namespace MossadAPI.Manegers
{
    public class MissionForTarget: MissionBase
    {
        private readonly MossadDBContext _context;
        public MissionForTarget(MossadDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task SearchMissions(Target target)
        {
            Location? targetLocation = await _context.Locations.FindAsync(target.locationID);
            foreach (Agent agent in _context.Agents)
            {
                if (targetLocation == null) { break; }
                Location? agentLocation = await _context.Locations.FindAsync(agent.locationID);
                if (agentLocation == null)
                {
                    continue;
                }
                if (IsNear(targetLocation, agentLocation) && await IsAvailble(agent))
                {
                    if (await IsAlreadyExicte(agent, target) == false)
                    {
                        await CreateMission(agent, target);
                    }
                }
            }
        }
        private async Task<bool> IsAvailble(Agent agent)
        {
            foreach (Mission mission in _context.Missions)
            {
                if (mission.agentID == agent.ID && mission.Status != StatusMission.Suggestion || agent.status == StatusAgent.InActivity)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

