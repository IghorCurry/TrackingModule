using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackingModule.BLL.Managers;
using TrackingModule.BLL.Models.AuthModels;
using TrackingModule.BLL.Models.EmployeeModels;

namespace TrackingModule.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthManager _authManager;
        private readonly IdentityManager _identityManager;

        public AuthController(AuthManager authManager, IdentityManager identityManager)
        {
            _authManager = authManager;
            _identityManager = identityManager;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(EmployeeCreateModel model)
        {
            try
            {
                if (await _authManager.Register(model) == true)
                {
                    return StatusCode(201);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest("ll");
        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            try
            {
                if (await _authManager.ConfirmEmail(email, token) == true) ;
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {
                TokenPairModel tokenPair = await _authManager.Login(loginModel);
                return Ok(tokenPair);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Refresh")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh(TokenPairModel tokenPair)
        {
            try
            {
                TokenPairModel newTokenPair = await _authManager.Refresh(tokenPair);
                if (newTokenPair != null)
                {
                    return Ok(newTokenPair);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("WhoAmI")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> WhoAmI()
        {
            EmployeeViewModel user = await _identityManager.WhoAmI();
            return Ok(user);
        }
    }
}