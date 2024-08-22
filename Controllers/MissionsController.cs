using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MossadAPI.Data;
using MossadAPI.Models;

namespace MossadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionsController : ControllerBase
    {
        private readonly MossadDBContext _context;
        public MissionsController(MossadDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Missions.ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus(Guid id, StatusMission statusMission)
        {
            Mission? mission = await _context.Missions.FindAsync(id);
            if (mission == null)
            {
                return NotFound();
            }
            mission.Status = statusMission;
            return Ok(mission);
        }

        [HttpPost("/update")]
        public async Task<IActionResult> UpdateMission()
        {

        }
    }
}
