using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAPI.Data;
using MossadAPI.Manegers;
using MossadAPI.Models;

namespace MossadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private readonly MossadDBContext _context;
        public TargetsController(MossadDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Create(Target target)
        {
            target.ID = Guid.NewGuid();
            target.status = StatusTarget.Live;
            _context.Targets.Add(target);
            _context.SaveChanges();
            return Ok(target.ID);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var targets = _context.Targets.ToArray();
            return Ok(
                targets
                );
        }

        [HttpPut("{id}/pin")]
        public IActionResult PutLocation(Location location, Guid id)
        {
            Target? target = _context.Targets.FirstOrDefault(target => target.ID == id);
            if (target != null)
            {
                _context.Locations.Add(location);
                _context.SaveChanges();
                target.Location = location.Id;
                _context.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/move")]
        public IActionResult Move(Direction direction, Guid id)
        {
            Target? target = _context.Targets.FirstOrDefault(target => target.ID == id);
            if (target != null)
            {
                Location? location = _context.Locations.FirstOrDefault(location => location.Id == target.Location);
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
