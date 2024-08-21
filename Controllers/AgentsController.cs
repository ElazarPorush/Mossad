using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAPI.Data;
using MossadAPI.Models;

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
        public IActionResult CreateAgent(Agent agent)
        {
            agent.ID = Guid.NewGuid();
            agent.Status = StatusAgent.Dormant;
            Location location =  new Location(5, 13);
            _context.Locations.Add(location);
            agent.Location = location;
            _context.Agents.Add(agent);
            _context.SaveChanges();
            return StatusCode(
                StatusCodes.Status201Created,
                new { success = true, agent = agent }
                );
        }
    }
}
