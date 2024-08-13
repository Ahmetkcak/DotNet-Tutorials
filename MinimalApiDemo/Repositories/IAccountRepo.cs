using MinimalApiDemo.Models.DTOs;

namespace MinimalApiDemo.Repositories;

public interface IAccountRepo
{
    Task<Response> Register(RegisterDTO request);
    Task<LoginResponse> Login(LoginDTO request);
}
