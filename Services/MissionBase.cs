using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            var missions = await _context.Missions.Include(mission => mission.agent).Include(mission => mission.target).Include(mission => mission.agent.location).Include(mission => mission.target.location).ToListAsync();
            foreach (Mission mission in missions)
            {
                Location? agentLocation = mission.agent.location;
                Location? targetLocation = mission.target.location;
                if (!IsNear(agentLocation, targetLocation))
                {
                    _context.Missions.Remove(mission);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteMissionIfIsNotRelevant(int id)
        {
            Mission? theMission = await _context.Missions.Include(mission => mission.agent).Include(mission => mission.target).FirstOrDefaultAsync(mission => mission.Id == id);
            if (theMission != null)
            {
                var missions = await _context.Missions.Include(mission => mission.agent).Include(mission => mission.target).ToListAsync();
                foreach (Mission mission in missions)
                {
                    if (theMission.agent.ID == mission.agent.ID || theMission.target.ID == mission.target.ID)
                    {
                        if (mission.Status == StatusMission.Assigned)
                        {
                            _context.Missions.Remove(mission);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }
        protected Double GetDistence(Location agentLocation, Location targetLocation)
        {
            return Math.Sqrt(Math.Pow(targetLocation.X - agentLocation.X, 2) + Math.Pow(targetLocation.Y - agentLocation.Y, 2));
        }

        protected bool IsNear(Location agentLocation, Location targetLocation)
        {
            if (GetDistence(agentLocation, targetLocation) <= 200)
            {
                return true;
            }
            return false;
        }

        protected async Task CreateMission(Agent agent, Target target)
        {
            Mission mission = new Mission()
            {
                agent = agent,
                target = target,
                Status = StatusMission.Suggestion,
            };
            await _context.Missions.AddAsync(mission);
            await _context.SaveChangesAsync();
            mission.TimeLeft = await PutTimeLeft(mission);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> TheyMeet(Agent agent, Target target)
        {
            Location? agentLocation = agent.location;
            Location? targetLocation = target.location;
            if (agentLocation.X == targetLocation.X && agentLocation.Y == targetLocation.Y)
            {
                return true;
            }
            return false;
        }

        public async Task KillTarget(Mission mission)
        {
            Agent? agent = mission.agent;
            Target? target = mission.target;
            agent.status = StatusAgent.Dormant;
            target.status = StatusTarget.Eliminated;
            mission.Status = StatusMission.Completed;
            //mission.ExecutionTime == 
        }

        public async Task<Double> PutTimeLeft(Mission mission)
        {
            Location? agentLocation = mission.agent.location;
            Location? targetLocation = mission.target.location;
            return GetDistence(agentLocation, targetLocation);
        }

        public async Task<bool> IsAlreadyExicte(Agent agent , Target target)
        {
            var missions = await _context.Missions.Include(mission => mission.agent).Include(mission => mission.target).ToListAsync();
            foreach (Mission mission in missions)
            {
                if (mission.agent.ID == agent.ID && mission.target.ID == target.ID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
