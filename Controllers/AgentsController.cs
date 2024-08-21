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
            agent.status = StatusAgent.Dormant;
            _context.Agents.Add(agent);
            _context.SaveChanges();
            return Ok(agent.ID);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var agents = _context.Agents.ToArray();
            return Ok(
                agents
                );
        }

        [HttpPut("{id}/pin")]
        public IActionResult PutLocation(Location location, Guid id)
        {
            Agent? agent = _context.Agents.FirstOrDefault(agent => agent.ID == id);
            if (agent != null)
            {
                _context.Locations.Add(location);
                _context.SaveChanges();
                agent.Location = location.Id;
                _context.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }  
        }


        [HttpPut("{id}/move")]
        public IActionResult Move(Direction direction , Guid id)
        {
            Agent? agent = _context.Agents.FirstOrDefault(agent => agent.ID == id);
            if (agent != null)
            {
                Location? location = _context.Locations.FirstOrDefault(location => location.Id == agent.Location);
                if (location != null)
                {
                    Location tmpLocation = ChangeLocation.Move(direction, location);
                    location.X = tmpLocation.X;
                    location.Y = tmpLocation.Y;
                    _context.Update(location);
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }

        }

    }
}
