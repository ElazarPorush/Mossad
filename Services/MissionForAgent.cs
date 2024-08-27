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
            
            Location? agentLocation = agent.location;
            var targets = await _context.Targets.Include(target => target.location).ToListAsync();
            foreach (Target target in targets)
            {
                if (agentLocation == null) { break; }
                Location? targetLocation = target.location;
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
            var missions = await _context.Missions.Include(mission => mission.target).ToListAsync();
            foreach (Mission mission in missions)
            {
                if ((mission.target.ID == target.ID && mission.Status != StatusMission.Suggestion) || target.status == StatusTarget.Eliminated)
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}
