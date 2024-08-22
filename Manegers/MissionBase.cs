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

        public async Task DeleteMissionIfIsNotRelevant(Guid id)
        {
            Mission? theMission = await _context.Missions.FindAsync(id);
            if (theMission != null)
            {
                foreach (Mission mission in _context.Missions)
                {
                    if (theMission.agentID == mission.agentID || theMission.targetID == mission.targetID)
                    {
                        if (mission.Status == StatusMission.Assigned)
                        {
                            _context.Missions.Remove(mission);
                            _context.SaveChanges();
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
                agentID = agent.ID,
                targetID = target.ID,
                Status = StatusMission.Suggestion
            };
            _context.Missions.Add(mission);
            _context.SaveChanges();
        }

        public async Task<bool> TheyMeet(Guid agentID, Guid targetID)
        {
            Agent? agent = await _context.Agents.FindAsync(agentID);
            Target? target = await _context.Targets.FindAsync(targetID);
            Location? agentLocation = await _context.Locations.FindAsync(agent.locationID);
            Location? targetLocation = await _context.Locations.FindAsync(target.locationID);
            if (agentLocation.X == targetLocation.X && agentLocation.Y == targetLocation.Y)
            {
                return true;
            }
            return false;
        }

        public async Task KillTarget(Mission mission)
        {
            Agent? agent = await _context.Agents.FindAsync(mission.agentID);
            Target? target = await _context.Targets.FindAsync(mission.targetID);
            agent.status = StatusAgent.Dormant;
            target.status = StatusTarget.Eliminated;
            mission.Status = StatusMission.Completed;
        }

        public async Task<Double> PutTimeLeft(Mission mission)
        {
            Agent? agent = await _context.Agents.FindAsync(mission.agentID);
            Target? target = await _context.Targets.FindAsync(mission.targetID);
            Location? agentLocation = await _context.Locations.FindAsync(agent.locationID);
            Location? targetLocation = await _context.Locations.FindAsync(target.locationID);
            return GetDistence(agentLocation, targetLocation);
        }
    }
}
