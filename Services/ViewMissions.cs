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
            var missions = await _context.Missions.ToListAsync();
            foreach (Mission mission in missions)
            {
                if (mission.Status == StatusMission.Suggestion)
                {
                    Agent? agent = await _context.Agents.FindAsync(mission.agentID);
                    Target? target = await _context.Targets.FindAsync(mission.targetID);
                    Location? agentLocation = await _context.Locations.FindAsync(agent.locationID);
                    Location? targetLocation = await _context.Locations.FindAsync(target.locationID);
                    SuggestionView suggestion = new SuggestionView
                    {
                        MissionID = mission.Id,
                        AgentNickname = agent.Nickname,
                        AgentLocation = $"X: {agentLocation.X}, Y: {agentLocation.Y}",
                        TargetName = target.Name,
                        TargetPosition = target.Position,
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
            Mission? mission = await _context.Missions.FindAsync(id);
            Agent? agent = await _context.Agents.FindAsync(mission.agentID);
            Target? target = await _context.Targets.FindAsync(mission.targetID);
            Location? agentLocation = await _context.Locations.FindAsync(agent.locationID);
            Location? targetLocation = await _context.Locations.FindAsync(target.locationID);
            SuggestionView suggestion = new SuggestionView
            {
                MissionID = mission.Id,
                AgentNickname = agent.Nickname,
                AgentLocation = $"X: {agentLocation.X}, Y: {agentLocation.Y}",
                TargetName = target.Name,
                TargetPosition = target.Position,
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
            var missions = await _context.Missions.ToArrayAsync();
            int suggestionsAgentsToTargets = 0;
            foreach (Agent agent in agents)
            {
                if (agent.status == StatusAgent.Dormant)
                {
                    Mission? mission = await _context.Missions.FirstOrDefaultAsync(mission => mission.agentID == agent.ID);
                    if (mission != null)
                    {
                        suggestionsAgentsToTargets++;
                    }
                }
            }
            mossadView.SuggestionsAgentsToTargets = suggestionsAgentsToTargets / mossadView.TotalTargets;
            return mossadView;
        }
    }
}
