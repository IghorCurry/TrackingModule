using Microsoft.AspNetCore.Mvc;
using TrackingModule.BLL.Abstractions;
using TrackingModule.BLL.Models.ActivityModels;

namespace TrackingModule.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivitiesManager _activityManager;
    public ActivitiesController(IActivitiesManager activitiesManager)
    {
        _activityManager = activitiesManager;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ActivityViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _activityManager.GetActivitiesAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ActivityViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid id)
    {
        if (!await _activityManager.IsExists(id))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _activityManager.GetActivityAsync(id));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ActivityViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(ActivityCreateModel activity)
    {
        if (activity == null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _activityManager.CreateActivityAsync(activity));
    }

    [HttpPut]
    [ProducesResponseType(typeof(ActivityViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, ActivityUpdateModel activity)
    {

        if (id == default || activity is null || id != activity.Id)
        {
            return BadRequest(ModelState);
        }
        return Ok(await _activityManager.UpdateActivityAsync(activity));
    }

    [HttpDelete]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _activityManager.IsExists(id))
        {
            return NotFound();
        }
        else
        {
            var activityToDelete = _activityManager.GetActivityAsync(id);
            return Ok(await _activityManager.DeleteActivityAsync(id));
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
    }
}
