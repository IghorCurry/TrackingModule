using Microsoft.AspNetCore.Mvc;
using TrackingModule.BLL.Abstractions;
using TrackingModule.BLL.Models.ActivityModels;
using TrackingModule.BLL.Models.ProjectModels;

namespace TrackingModule.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectManager _projectManager;
        public ProjectController(IProjectManager projectManager)
        {
            _projectManager = projectManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjectViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _projectManager.GetProjectsAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!await _projectManager.IsExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _projectManager.GetProjectAsync(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProjectViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(ProjectCreateModel project)
        {
            if (project == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _projectManager.CreateProjectAsync(project));
        }

        [HttpPut]
        [ProducesResponseType(typeof(ProjectViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, ProjectUpdateModel project)
        {

            if (id == default || project is null || id != project.Id)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _projectManager.UpdateProjectAsync(project));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _projectManager.IsExists(id))
            {
                return NotFound();
            }
            else
            {
                var activityToDelete = _projectManager.GetProjectAsync(id);
                return Ok(await _projectManager.DeleteProjectAsync(id));
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        }
    }
}