using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MossadAPI.Data;
using MossadAPI.Manegers;
using MossadAPI.Models;

namespace MossadAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private readonly MossadDBContext _context;
        private readonly MissionForTarget MissionForTarget;
        public TargetsController(MossadDBContext context, MissionForTarget missionForTarget)
        {
            _context = context;
            MissionForTarget = missionForTarget;
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
        public async Task<IActionResult> PutLocation(Location location, Guid id)
        {
            Target? target = _context.Targets.FirstOrDefault(target => target.ID == id);
            if (target != null)
            {
                _context.Locations.Add(location);
                _context.SaveChanges();
                target.locationID = location.Id;
                _context.SaveChanges();
                await MissionForTarget.DeleteOldMissions();
                await MissionForTarget.SearchMissions(target);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move(Direction direction, Guid id)
        {
            Target? target = _context.Targets.FirstOrDefault(target => target.ID == id);
            if (target != null)
            {
                Location? location = _context.Locations.FirstOrDefault(location => location.Id == target.locationID);
                if (location != null)
                {
                    Location tmpLocation = ChangeLocation.Move(direction, location);
                    location.X = tmpLocation.X;
                    location.Y = tmpLocation.Y;
                    _context.Update(location);
                    _context.SaveChanges();
                    await MissionForTarget.DeleteOldMissions();
                    await MissionForTarget.SearchMissions(target);
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
