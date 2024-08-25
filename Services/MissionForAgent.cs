using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MossadAPI.Data;
using MossadAPI.Models;

namespace MossadAPI.Manegers
{
    public class MissionForAgent: MissionBase
    {
        private readonly MossadDBContext _context;
        public MissionForAgent(MossadDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task SearchMissions(Agent agent)
        {
            
            Location? agentLocation = await _context.Locations.FindAsync(agent.locationID);
            foreach (Target target in _context.Targets)
            {
                if (agentLocation == null) { break; }
                Location? targetLocation = await _context.Locations.FindAsync(target.locationID);
                if (targetLocation == null)
                {
                    continue;
                }
                if (IsNear(agentLocation, targetLocation) && await IsAvailble(target))
                {
                    if (await IsAlreadyExicte(agent, target) == false)
                    {
                        await CreateMission(agent, target);
                    }
                }
            }
        }

   
        private async Task<bool> IsAvailble(Target target)
        {
            foreach (Mission mission in _context.Missions)
            {
                if (mission.targetID == target.ID && mission.Status != StatusMission.Suggestion || target.status == StatusTarget.Eliminated)
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}
