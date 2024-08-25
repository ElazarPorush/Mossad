using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAPI.Data;
using MossadAPI.Manegers;
using MossadAPI.Models;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace MossadAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        
        private readonly MossadDBContext _context;
        private readonly MissionForAgent MissionForAgent;
        public AgentsController(MossadDBContext context, MissionForAgent missionForAgent)
        {
            _context = context;
            MissionForAgent = missionForAgent;
        }
        

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Create(Agent agent)
        {
            agent.status = StatusAgent.Dormant;
            _context.Agents.Add(agent);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, agent.ID);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Agents.ToListAsync());
        }

        [HttpPut("{id}/pin")]
        public async Task< IActionResult> PutLocation(Location location, int id)
        {
            Agent? agent = _context.Agents.FirstOrDefault(agent => agent.ID == id);
            if (agent != null)
            {
                _context.Locations.Add(location);
                _context.SaveChanges();
                agent.locationID = location.Id;
                _context.SaveChanges();
                await MissionForAgent.DeleteOldMissions();
                await MissionForAgent.SearchMissions(agent);
                return Ok();
            }
            else
            {
                return NotFound();
            }  
        }


        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move(Direction direction , int id)
        {
            Agent? agent = _context.Agents.FirstOrDefault(agent => agent.ID == id);
            if (agent != null)
            {
                if (agent.status != StatusAgent.Dormant)
                {
                    return NotFound("The Agent is in active mission");
                }
                Location? location = _context.Locations.FirstOrDefault(location => location.Id == agent.locationID);
                if (location != null)
                {
                    Location tmpLocation = ChangeLocation.Move(direction, location);
                    location.X = tmpLocation.X;
                    location.Y = tmpLocation.Y;
                    _context.Update(location);
                    _context.SaveChanges();
                    await MissionForAgent.DeleteOldMissions();
                    await MissionForAgent.SearchMissions(agent);
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }

        }

    }
}
