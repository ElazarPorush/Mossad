using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MossadAPI.Data;
using MossadAPI.Services;
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
        public async Task<IActionResult> PutStatus(int id, StatusMission statusMission)
        {
            await _missionManeger.DeleteOldMissions();
            await _missionManeger.DeleteMissionIfIsNotRelevant(id);
            Mission? mission = await _context.Missions.Include(mission => mission.agent).Include(mission => mission.target).Include(mission => mission.agent.location).Include(mission => mission.target.location).FirstOrDefaultAsync(mission => mission.Id == id);
            if (mission == null)
            {
                return NotFound("The mission is not relevant");
            }
            mission.TimeLeft = await _missionManeger.PutTimeLeft(mission);
            mission.Status = StatusMission.Assigned;
            Agent? agent = mission.agent;
            agent.status = StatusAgent.InActivity;
            _context.SaveChanges();
            return Ok(mission);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateMissions()
        {
            var missions = await _context.Missions.Include(mission => mission.agent).Include(mission => mission.target).Include(mission => mission.agent.location).Include(mission => mission.target.location).ToListAsync();
            foreach (Mission mission in missions)
            { 
                if (mission.Status == StatusMission.Assigned)
                {
                    mission.TimeLeft = await _missionManeger.PutTimeLeft(mission);
                    if (await _missionManeger.TheyMeet(mission.agent, mission.target))
                    {
                        await _missionManeger.KillTarget(mission);
                        _context.SaveChanges();
                        return Ok("mission completed");
                    }
                    else
                    {
                        Location? agentLocation = mission.agent.location;
                        Location? targetLocation = mission.target.location;
                        Location tmpLocation = ChangeLocation.GoToTarget(agentLocation, targetLocation);
                        agentLocation.X = tmpLocation.X;
                        agentLocation.Y = tmpLocation.Y;
                        _context.Update(agentLocation);
                        _context.SaveChanges();
                    }
                }
            }
            return Ok();
        }
    }
}
