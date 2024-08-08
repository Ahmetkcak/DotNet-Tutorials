namespace EFCoreIdentityImplementation.Dtos;

public sealed record LoginDto(
    string UserNameOrEmail,
    string Password
    );
