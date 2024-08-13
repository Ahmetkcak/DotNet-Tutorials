using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalApiDemo.DataAccess;
using MinimalApiDemo.Models.DTOs;
using MinimalApiDemo.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalApiDemo.Repositories;

public class AccountRepo(AppDbContext dbContext, IMapper mapper, IConfiguration configuration) : IAccountRepo
{
    public async Task<LoginResponse> Login(LoginDTO request)
    {
        var user = FindUserByEmail(request.Email).Result;
        if (user != null)
        {
            bool verifyPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!verifyPassword)
            {
                return new LoginResponse(false, "Invalid credentials", null);
            }
            else
            {
                string token = GenerateToken(user);
                return new LoginResponse(true, "Login successful", token);
            }
        }
        return new LoginResponse(false, "User does not exist", null);
    }

    public async Task<Response> Register(RegisterDTO request)
    {
        var user = await FindUserByEmail(request.Email);
        if (user != null)
        {
            return new Response(false, "User already exists");
        }
        else
        {
            var newUser = mapper.Map<User>(request);
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            dbContext.Users.Add(newUser);
            await dbContext.SaveChangesAsync();
            return new Response(true, "User registered successfully");
        }
    }



    private async Task<User?> FindUserByEmail(string email)
    {
        email = email.ToLower();
        return await dbContext.Users.FirstOrDefaultAsync(_ => _.Email.ToLower() == email);
    }

    private string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim("Fullname", user.Name),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
