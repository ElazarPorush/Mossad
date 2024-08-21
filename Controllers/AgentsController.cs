using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAPI.Data;
using MossadAPI.Manegers;
using MossadAPI.Models;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace MossadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly MossadDBContext _context;
        public AgentsController(MossadDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Create(Agent agent)
        {
            agent.ID = Guid.NewGuid();
            agent.Status = StatusAgent.Dormant;
            _context.Agents.Add(agent);
            _context.SaveChanges();
            return Ok(agent.ID);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var attacks = _context.Agents.ToArray();
            return Ok(
                attacks
                );
        }

        [HttpPut("{id}/pin")]
        public IActionResult PutLocation(Location location, Guid id)
        {
            Agent? agent = _context.Agents.FirstOrDefault(agent => agent.ID == id);
            if (agent != null)
            {
                if (agent.Location != null)
                {
                    Location? newLocation = _context.Locations.FirstOrDefault(newLocation => newLocation.Id == agent.Location.Id);
                    _context.Locations.Remove(newLocation);
                }
                agent.Location = location;
                _context.Locations.Add(location);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            
        }

    }
}
