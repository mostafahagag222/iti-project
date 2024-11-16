using API.DTOs;
using API.Errors;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public RoleManager<IdentityRole> roleManager { get; }
        public UserManager<AppUser> userManager { get; }
        
        public RoleController(RoleManager<IdentityRole> _roleManager, UserManager<AppUser> _userManager)        
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleDto role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole() { Name = role.RoleName };
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                    return Ok(ModelState);
            }

            return BadRequest(new ApiResponse(400));
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRoleToUser(UserRoleDto userRole)
        {
            var role = await roleManager.FindByNameAsync(userRole.RoleName);

            if (role == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var user = await userManager.FindByIdAsync(userRole.UserId);

            IdentityResult result = null;

            if (userRole.Action == 1 && !(await userManager.IsInRoleAsync(user, role.Name)))
            {
                result = await userManager.AddToRoleAsync(user, role.Name);
            }
            else if (userRole.Action == 0 && await userManager.IsInRoleAsync(user, role.Name))
            {
                result = await userManager.RemoveFromRoleAsync(user, role.Name);
            }

            if (result.Succeeded)
            {
                return Ok(new ApiResponse(200));
            }

            return Unauthorized(new ApiResponse(401));
        }
    }
}
