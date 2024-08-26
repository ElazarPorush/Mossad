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
        private readonly ViewMissions _viewMissions;
        public ViewController(ViewMissions viewMissions)
        {
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

        //get all details on this projekt
        [HttpGet("mossad")]
        public async Task<IActionResult> GetDetailsMossad()
        {
            MossadView mossad = await _viewMissions.GetDetailsOfMossad();
            return Ok(mossad);
        }

        //get all details on agents
        [HttpGet("agents")]
        public async Task<IActionResult> GetAgents()
        {
            List<AgentView> agents = await _viewMissions.GetAgents();
            return Ok(agents);
        }

        //get all details on targets
        [HttpGet("targets")]
        public async Task<IActionResult> GetTargets()
        {
            List<TargetView> targets = await _viewMissions.GetTargets();
            return Ok(targets);
        }
    }
}
