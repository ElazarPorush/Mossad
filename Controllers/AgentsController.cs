using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAPI.Data;
using MossadAPI.Manegers;
using MossadAPI.Models;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

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

        //create new agent
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task< IActionResult> Create(Agent agent)
        {
            agent.status = StatusAgent.Dormant;
            await _context.Agents.AddAsync(agent);
            await _context.SaveChangesAsync();
            return Ok( new { agent.ID });
        }

        //get all agents
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Agents.ToListAsync());
        }

        //update agent's location and create new mission 
        [HttpPut("{id}/pin")]
        public async Task< IActionResult> PutLocation(Location location, int id)
        {
            Agent? agent = _context.Agents.Include(agent => agent.location).FirstOrDefault(agent => agent.ID == id);
            if (agent != null)
            {
                agent.location = location;
                await _context.Locations.AddAsync(location);
                await _context.SaveChangesAsync();
                //delete from DB old missions before create new mission
                await MissionForAgent.DeleteOldMissions();
                //search for target in the area and create new missions if you find one relevante
                await MissionForAgent.SearchMissions(agent);
                return Ok();
            }
            else
            {
                return NotFound();
            }  
        }
        
        //move agent to random spot and create new mission
        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move([FromBody] Direction direction, int id)
        {
            Agent? agent = _context.Agents.Include(agent => agent.location).FirstOrDefault(agent => agent.ID == id);
            if (agent != null)
            {
                //chack if the agent not already in active mission
                if (agent.status != StatusAgent.Dormant)
                {
                    return NotFound("The Agent is in active mission");
                }
                Location? location = agent.location;
                if (location != null)
                {
                    Location tmpLocation = ChangeLocation.Move(direction, location);
                    
                    if (!ChangeLocation.InRange(tmpLocation.X) || !ChangeLocation.InRange(tmpLocation.Y))
                    {
                        return BadRequest(new {location.X, location.Y});
                    }
                    location.X = tmpLocation.X;
                    location.Y = tmpLocation.Y;
                    _context.Update(location);
                    _context.SaveChanges();
                    //delete from DB old missions before create new mission
                    await MissionForAgent.DeleteOldMissions();
                    //search for target in the area and create new missions if you find one relevante
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
