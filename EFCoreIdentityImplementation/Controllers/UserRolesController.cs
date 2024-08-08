using EFCoreIdentityImplementation.Context;
using EFCoreIdentityImplementation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreIdentityImplementation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class UserRolesController(ApplicationDbContext dbContext,UserManager<AppUser> userManager,RoleManager<AppRole> roleManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Create(Guid userId,Guid roleId, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByIdAsync(userId.ToString());
            if (appUser is null)
            {
                return BadRequest("User not found");
            }

            AppRole? appRole = await roleManager.FindByIdAsync(roleId.ToString());
            if (appRole is null)
            {
                return BadRequest("Role not found");
            }

            AppUserRole appUserRole = new()
            {
                UserId = userId,
                RoleId = roleId
            };

            dbContext.UserRoles.Add(appUserRole);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Ok(StatusCodes.Status201Created);
        }
    }
}
