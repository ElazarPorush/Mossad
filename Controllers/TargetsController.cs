using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //create new target
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Create(Target target)
        {
            target.status = StatusTarget.Live;
            _context.Targets.Add(target);
            _context.SaveChanges();
            return Ok(target.ID);
        }

        //get all targets
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Targets.ToListAsync());
        }

        //update target's location and create new mission
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> PutLocation(Location location, int id)
        {
            Target? target = _context.Targets.FirstOrDefault(target => target.ID == id);
            if (target != null)
            {
                _context.Locations.Add(location);
                _context.SaveChanges();
                target.locationID = location.Id;
                _context.SaveChanges();
                //delete from DB old missions before create new mission
                await MissionForTarget.DeleteOldMissions();
                //search for agent in the area and create new missions if you find one relevante
                await MissionForTarget.SearchMissions(target);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move(Direction direction, int id)
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
