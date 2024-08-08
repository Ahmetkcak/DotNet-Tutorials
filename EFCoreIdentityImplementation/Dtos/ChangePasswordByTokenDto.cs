namespace EFCoreIdentityImplementation.Dtos;

public sealed record ChangePasswordByTokenDto(
    string Email,
    string NewPassword,
    string Token
    );

