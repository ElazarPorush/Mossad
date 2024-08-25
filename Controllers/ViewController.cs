using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MossadAPI.Data;
using MossadAPI.Manegers;
using MossadAPI.Models;
using MossadAPI.Services;

namespace MossadAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ViewController : ControllerBase
    {
        private readonly MossadDBContext _context;
        private readonly ViewMissions _viewMissions;
        public ViewController(MossadDBContext context, ViewMissions viewMissions)
        {
            _context = context;
            _viewMissions = viewMissions;
        }

        // get's objects for MVC

        // get suggestions missions
        [HttpGet("suggestionsMissions")]
        public async Task<IActionResult> GetSuggestionsMissions()
        {
            List<SuggestionView> suggestions = await _viewMissions.GetSuggestions();
            return Ok(suggestions);
        }

        //get one suggestion bu ID
        [HttpGet("suggestionsMissions/{id}")]
        public async Task<IActionResult> GetSuggestionsMissionsByID(int id)
        {
            SuggestionView suggestion = await _viewMissions.GetSuggestionsByID(id);
            return Ok(suggestion);
        }
    }
}
