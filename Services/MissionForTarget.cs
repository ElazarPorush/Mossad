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
            Location? targetLocation = target.location;
            var agents = await _context.Agents.Include(agent => agent.location).ToListAsync();
            foreach (Agent agent in agents)
            {
                if (targetLocation == null) { break; }
                Location? agentLocation = agent.location;
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
            var missions = await _context.Missions.Include(mission => mission.agent).ToListAsync();
            foreach (Mission mission in missions)
            {
                if (mission.agent.ID == agent.ID && mission.Status != StatusMission.Suggestion || agent.status == StatusAgent.InActivity)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

