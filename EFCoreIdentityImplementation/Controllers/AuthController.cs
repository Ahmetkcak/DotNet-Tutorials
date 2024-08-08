using EFCoreIdentityImplementation.Dtos;
using EFCoreIdentityImplementation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EFCoreIdentityImplementation.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class AuthController(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(ReqisterDto request, CancellationToken cancellationToken)
    {
        AppUser appUser = new()
        {
            Email = request.Email,
            UserName = request.UserName,
            FirstName = request.FirsName,
            LastName = request.LastName
        };

        IdentityResult result = await userManager.CreateAsync(appUser, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }
        return Ok(StatusCodes.Status201Created);
    }


    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto request, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.FindByIdAsync(request.Id.ToString());
        if (appUser is null)
        {
            return BadRequest("User not found");
        }

        IdentityResult result = await userManager.ChangePasswordAsync(appUser, request.Password, request.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }
        return Ok(StatusCodes.Status200OK);
    }


    [HttpGet]
    public async Task<IActionResult> ForgotPassword(string email, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.FindByEmailAsync(email);
        if (appUser is null)
        {
            return BadRequest("User not found");
        }

        string token = await userManager.GeneratePasswordResetTokenAsync(appUser);
        return Ok(new { Token = token });
    }

    [HttpPost]
    public async Task<IActionResult> ChangePasswordByToken(ChangePasswordByTokenDto request, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.FindByEmailAsync(request.Email);
        if (appUser is null)
        {
            return BadRequest("User not found");
        }

        IdentityResult result = await userManager.ResetPasswordAsync(appUser, request.Token, request.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }
        return Ok(StatusCodes.Status200OK);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserNameOrEmail || u.Email == request.UserNameOrEmail, cancellationToken);
        if (appUser is null)
        {
            return BadRequest("User not found");
        }

        bool isPasswordValid = await userManager.CheckPasswordAsync(appUser, request.Password);
        if (!isPasswordValid)
        {
            return BadRequest("Invalid password");
        }

        return Ok(new { Token = "Token goes here" });
    }

    [HttpPost]
    public async Task<IActionResult> LoginWithSignInManager(LoginDto request, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserNameOrEmail || u.Email == request.UserNameOrEmail, cancellationToken);
        if (appUser is null)
        {
            return BadRequest("User not found");
        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(appUser, request.Password, lockoutOnFailure:true);
        if (signInResult.IsLockedOut)
        {
            TimeSpan? timeSpan = appUser.LockoutEnd - DateTime.UtcNow;
            if(timeSpan.HasValue)
            {
                return BadRequest($"User is locked out for {timeSpan.Value.Minutes} minutes");
            }
            else
            {
                return BadRequest("User is locked");
            }
        }

        if (!signInResult.Succeeded)
        {
            return BadRequest("Invalid password");
        }

        if (signInResult.IsNotAllowed)
        {
            return BadRequest("Email is not confirmed");
        }

        return Ok(new { Token = "Token goes here" });
    }
}
