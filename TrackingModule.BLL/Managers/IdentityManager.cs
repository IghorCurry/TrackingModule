using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TrackingModele.DAL.Entities;
using TrackingModule.BLL.Models.EmployeeModels;

namespace TrackingModule.BLL.Managers
{
    public class IdentityManager
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IdentityManager(UserManager<Employee> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;

        }

        public virtual async Task<EmployeeViewModel> WhoAmI()
        {
            string id = _httpContextAccessor.HttpContext.User.Identity.Name;
            Employee employee = await _userManager.FindByIdAsync(id);
            var model = employee.Adapt<EmployeeViewModel>();
            var roleNames = await _userManager.GetRolesAsync(employee);
            model.RoleNames = roleNames;
            return model;
        }
    }
}
