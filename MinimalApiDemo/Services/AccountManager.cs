using MinimalApiDemo.Models.DTOs;
using MinimalApiDemo.Repositories;

namespace MinimalApiDemo.Services;

public class AccountManager(IAccountRepo accountRepo) : IAccountService
{
    public Task<LoginResponse> Login(LoginDTO request) => accountRepo.Login(request);

    public Task<Response> Register(RegisterDTO request) => accountRepo.Register(request);
}
