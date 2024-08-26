using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MossadAPI.Data;
using MossadAPI.Manegers;
using MossadAPI.Models;
using System.Reflection;

namespace MossadAPI.Services
{
    public class ViewMissions: MissionBase
    {
        private readonly MossadDBContext _context;
        public ViewMissions(MossadDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SuggestionView>> GetSuggestions()
        {
            List<SuggestionView> suggestions = new List<SuggestionView>();
            var missions = await _context.Missions.Include(mission => mission.agent).Include(mission => mission.target).Include(mission => mission.agent.location).Include(mission => mission.target.location).ToListAsync();
            foreach (Mission mission in missions)
            {
                if (mission.Status == StatusMission.Suggestion)
                {
                    Location? agentLocation = mission.agent.location;
                    Location? targetLocation = mission.target.location;
                    SuggestionView suggestion = new SuggestionView
                    {
                        MissionID = mission.Id,
                        AgentNickname = mission.agent.nickname,
                        AgentLocation = $"X: {agentLocation.X}, Y: {agentLocation.Y}",
                        TargetName = mission.target.name,
                        TargetPosition = mission.target.position,
                        TargetLocation = $"X: {targetLocation.X}, Y: {targetLocation.Y}",
                        Distance = GetDistence(agentLocation, targetLocation),
                        TimeLeft = mission.TimeLeft,
                    };
                    suggestions.Add(suggestion);
                }
            }
            return suggestions;
        }

        public async Task<SuggestionView> GetSuggestionsByID(int id)
        {
            Mission? mission = await _context.Missions.Include(mission => mission.agent).Include(mission => mission.target).Include(mission => mission.agent.location).Include(mission => mission.target.location).FirstOrDefaultAsync(mission => mission.Id == id);
            if (mission == null) { return null; }

            Location? agentLocation = mission.agent.location;
            Location? targetLocation = mission.target.location;

            SuggestionView suggestion = new SuggestionView
            {
                MissionID = mission.Id,
                AgentNickname = mission.agent.nickname,
                AgentLocation = $"X: {agentLocation.X}, Y: {agentLocation.Y}",
                TargetName = mission.target.name,
                TargetPosition = mission.target.position,
                TargetLocation = $"X: {targetLocation.X}, Y: {targetLocation.Y}",
                Distance = GetDistence(agentLocation, targetLocation),
                TimeLeft = mission.TimeLeft,
            };
            return suggestion;
        }

        public async Task<MossadView> GetDetailsOfMossad()
        {
            MossadView mossadView = new MossadView
            {
                TotalAgents = await _context.Agents.CountAsync(),
                TotalAgentsActivate = await _context.Agents.CountAsync(agent => agent.status == StatusAgent.InActivity),
                TotalTargets = await _context.Targets.CountAsync(),
                TotalTargetsDead = await _context.Targets.CountAsync(target => target.status == StatusTarget.Eliminated),
                TotalMissions = await _context.Missions.CountAsync(),
                TotalMissionsActivate = await _context.Missions.CountAsync(mission => mission.Status == StatusMission.Assigned)
            };
            mossadView.AgentsToTargets = mossadView.TotalAgents / mossadView.TotalTargets;
            
            var agents = await _context.Agents.ToArrayAsync();
            int count = 0;
            foreach (Agent agent in agents)
            {
                if (agent.status == StatusAgent.Dormant)
                {
                    Mission? mission = await _context.Missions.FirstOrDefaultAsync(mission => mission.agent.ID == agent.ID);
                    if (mission != null)
                    {
                        count++;
                    }
                }
            }
            mossadView.SAgentsToTargets = count / mossadView.TotalTargets;
            return mossadView;
        }

        public async Task<List<AgentView>> GetAgents()
        {
            List<AgentView> agentsView = new List<AgentView>();
            var agents = _context.Agents.Include(agent => agent.location).ToArray();
            foreach (Agent agent in agents)
            {
                AgentView agentView = new AgentView
                {
                    ID = agent.ID,
                    PhotoUrl = agent.photoUrl,
                    Nickname = agent.nickname,
                    StatusAgent = agent.status,
                };
                Location? location = agent.location;
                if (location != null)
                {
                    agentView.Location = $"X: {location.X}, Y: {location.Y}";
                }
                if (agent.status == StatusAgent.InActivity)
                {
                    Mission? mission = await _context.Missions.FirstOrDefaultAsync(mission => mission.agent.ID == agent.ID && mission.Status == StatusMission.Assigned);
                    if (mission != null)
                    {
                        agentView.MissionID = mission.Id;
                        agentView.TimeLeft = mission.TimeLeft;
                    }
                }
                agentView.Kills = await _context.Missions.CountAsync(mission => mission.agent.ID == agent.ID && mission.Status == StatusMission.Completed);
                agentsView.Add(agentView);
            }
            return agentsView;
        }

        public async Task<List<TargetView>> GetTargets()
        {
            List<TargetView> targetsView = new List<TargetView>();
            var targets = _context.Targets.Include(target => target.location).ToArray();
            foreach (Target target in targets)
            {
                TargetView targetView = new TargetView
                {
                    Id = target.ID,
                    Name = target.name,
                    Position = target.position,
                    PhotoUrl = target.photoUrl,
                    StatusTarget = target.status,
                };
                Location? location = target.location;
                if (location != null)
                {
                    targetView.Location = $"X: {location.X}, Y: {location.Y}";
                }
                targetsView.Add(targetView);
            }
            return targetsView;
        }
    }
}
