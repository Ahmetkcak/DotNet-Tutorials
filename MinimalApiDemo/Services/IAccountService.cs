using MinimalApiDemo.Models.DTOs;

namespace MinimalApiDemo.Services;

public interface IAccountService
{
    Task<Response> Register(RegisterDTO request);
    Task<LoginResponse> Login(LoginDTO request);
}
