using Microsoft.AspNetCore.Mvc;
using TrackingModule.BLL.Abstractions;
using TrackingModule.BLL.Models.ActivityModels;

namespace TrackingModule.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrackingController : Controller
{
    private readonly ITrackingManager _trackingManager;

    public TrackingController(ITrackingManager trackingManager)
    {
        _trackingManager = trackingManager;
    }

    [HttpGet("date")]
    [ProducesResponseType(typeof(IEnumerable<ActivityViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid id, DateTime fromDate, DateTime toDate)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _trackingManager.GetTimeByIdAndDate(id, fromDate, toDate));
    }
    [HttpGet("week")]
    [ProducesResponseType(typeof(IEnumerable<ActivityViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid id, int weekNumber)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _trackingManager.GetTimeByIdAndWeek(id, weekNumber));
    }

}
