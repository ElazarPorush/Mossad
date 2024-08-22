using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MossadAPI.Data;
using MossadAPI.Manegers;
using MossadAPI.Models;
using System.ComponentModel;
using System.Reflection;

namespace MossadAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MissionsController : ControllerBase
    {
        private readonly MossadDBContext _context;
        private readonly MissionBase _missionManeger;
        public MissionsController(MossadDBContext context, MissionBase missionManeger)
        {
            _context = context;
            _missionManeger = missionManeger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Missions.ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus(Guid id, StatusMission statusMission)
        {
            await _missionManeger.DeleteOldMissions();
            await _missionManeger.DeleteMissionIfIsNotRelevant(id);
            Mission ? mission = await _context.Missions.FindAsync(id);
            if (mission == null)
            {
                return NotFound("The mission is not relevant");
            }
            mission.TimeLeft = await _missionManeger.PutTimeLeft(mission);
            mission.Status = statusMission;
            Agent? agent = await _context.Agents.FindAsync(mission.agentID);
            agent.status = StatusAgent.InActivity;
            _context.SaveChanges();
            return Ok(mission);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateMissions()
        {
            foreach (Mission mission in _context.Missions)
            {
                if (mission.Status == StatusMission.Assigned)
                {

                    if (await _missionManeger.TheyMeet(mission.agentID, mission.targetID))
                    {
                        await _missionManeger.KillTarget(mission);
                    }
                    else
                    {
                        Agent? agent = await _context.Agents.FindAsync(mission.agentID);
                        Target? target = await _context.Targets.FindAsync(mission.targetID);
                        Location? agentLocation = await _context.Locations.FindAsync(agent.locationID);
                        Location? targetLocation = await _context.Locations.FindAsync(target.locationID);
                        Location tmpLocation = ChangeLocation.GoToTarget(agentLocation, targetLocation);
                        agentLocation.X = tmpLocation.X;
                        agentLocation.Y = tmpLocation.Y;

                    }
                }
            }
        }

        
    }
}
