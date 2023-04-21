using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using TrackingModele.DAL.Entities;
using TrackingModele.DAL.Settings;
using TrackingModule.BLL.Models.AuthModels;
using TrackingModule.BLL.Models.EmployeeModels;

namespace TrackingModule.BLL.Managers
{
    public class AuthManager
    {
        private readonly UserManager<Employee> _userManager;
        private readonly AccessTokenSettings _authSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthManager(UserManager<Employee> userManager, IOptions<AccessTokenSettings> authSettings, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _authSettings = authSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<bool> Register(EmployeeCreateModel model)
        {
            Employee employee = model.Adapt<Employee>();
            IdentityResult res = await _userManager.CreateAsync(employee, model.Password);
            if (!res.Succeeded)
            {
                throw new Exception(res.Errors.FirstOrDefault().Description);
            }

            var encodedToken = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(employee));
            var encodedEmail = HttpUtility.UrlEncode(employee.Email);

            string host = _httpContextAccessor.HttpContext.Request.Host.Value;

            var confirmationlink = "https://" + host + "/api/Auth/ConfirmEmail?email=" + encodedEmail + "&token=" + encodedToken;

            if (!res.Succeeded)
            {
                throw new Exception("Unable to assign employee to role 'Employee'");
            }
            return true;
        }

        public virtual async Task<bool> ConfirmEmail(string email, string token)
        {
            var employee = await _userManager.FindByEmailAsync(email);
            if (employee == null)
            {
                throw new Exception("The Employee with such email doesn't exist");
            }
            var result = await _userManager.ConfirmEmailAsync(employee, token);
            if (!result.Succeeded)
            {
                throw new Exception("Unable to confirm email");
            }
            return true;
        }

        public virtual async Task<TokenPairModel> Login(LoginModel loginModel)
        {
            TokenPairModel tokenPair = new TokenPairModel();
            Employee employee = await _userManager.FindByEmailAsync(loginModel.Email);
            if (employee == null)
            {
                throw new Exception("The Employee with such email doesn't exist");
            }
            if (!await _userManager.CheckPasswordAsync(employee, loginModel.Password))
            {
                throw new Exception("Wrong password");
            }
            if (!await _userManager.IsEmailConfirmedAsync(employee))
            {
                throw new Exception("Email is not confirmed");
            }

            string accessToken = GenerateJwtToken(employee);

            var refreshToken = await _userManager.GenerateUserTokenAsync(employee, "TrackingModule", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(employee, "TrackingModule", "RefreshToken", refreshToken);
            tokenPair.RefreshToken = refreshToken;
            tokenPair.AccessToken = accessToken;

            return tokenPair;
        }

        private ClaimsIdentity GetClaims(Employee employee)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, employee.Id.ToString())
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "AccessToken", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        public async Task<TokenPairModel> Refresh(TokenPairModel tokenPair)
        {
            string accessToken = tokenPair.AccessToken;
            string refreshToken = tokenPair.RefreshToken;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SigningKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            if (principal == null)
            {
                throw new Exception("Not existing access token.");
            }
            string id = principal.Identity.Name;

            var employee = await _userManager.FindByIdAsync(id);
            var employeeToken = await _userManager.GetAuthenticationTokenAsync(employee, "TrackingModule", "RefreshToken");
            var isValid = await _userManager.VerifyUserTokenAsync(employee, "TrackingModule", "RefreshToken", refreshToken);

            if (employee == null || employeeToken != refreshToken || !isValid)
            {
                return null;
            }

            var newAccessToken = GenerateJwtToken(employee);

            await _userManager.RemoveAuthenticationTokenAsync(employee, "TrackingModule", "RefreshToken");

            var newRefreshToken = await _userManager.GenerateUserTokenAsync(employee, "TrackingModule", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(employee, "TrackingModule", "RefreshToken", newRefreshToken);

            tokenPair.AccessToken = newAccessToken;
            tokenPair.RefreshToken = newRefreshToken;

            return tokenPair;
        }

        public string GenerateJwtToken(Employee employee)
        {
            var identity = GetClaims(employee);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            var key = Encoding.ASCII.GetBytes(_authSettings.SigningKey);

            var jwt = new JwtSecurityToken(
                        issuer: _authSettings.Issuer,
                        audience: _authSettings.Audience,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(15)),
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}
