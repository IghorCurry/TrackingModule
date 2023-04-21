using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackingModele.DAL.Entities;
using TrackingModule.BLL.Managers;
using TrackingModule.BLL.Models.EmployeeModels;

namespace TrackingModule.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private UserManager<Employee> _userManager { get; set; }
        private RoleManager<IdentityRole<Guid>> _roleManager { get; set; }
        private IdentityManager _identityManager { get; set; }

        public EmployeeController(UserManager<Employee> userManager, RoleManager<IdentityRole<Guid>> roleManager, IdentityManager identityManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityManager = identityManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(EmployeeCreateModel model)
        {
            IdentityResult res = await _userManager.CreateAsync(model.Adapt<Employee>(), model.Password);

            if (res.Succeeded)
            {
                return StatusCode(201);
            }
            return NotFound();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IQueryable<EmployeeViewModel> GetAll()
        {
            var users = _userManager.Users.ProjectToType<EmployeeViewModel>();
            return users;
        }

        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            var model = user.Adapt<EmployeeViewModel>();
            var roleNames = await _userManager.GetRolesAsync(user);
            model.RoleNames = roleNames;
            return Ok(model);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            IdentityResult res = await _userManager.DeleteAsync(user);
            if (res.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("{userId}/Role/{roleId}/assign-role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignRoleToUser(Guid roleId, Guid userId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (role == null || user == null)
            {
                return NotFound();
            }
            if (!await _userManager.IsInRoleAsync(user, role.Name))
            {
                IdentityResult res = await _userManager.AddToRoleAsync(user, role.Name);
                if (res.Succeeded)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpDelete("{userId}/Role/{roleId}/assign-role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveRoleFromUser(Guid roleId, Guid userId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (role == null || user == null)
            {
                return NotFound();
            }
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                IdentityResult res = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (res.Succeeded)
                {
                    return Ok();
                }
                else return NotFound();
            }
            return BadRequest();
        }
    }
}