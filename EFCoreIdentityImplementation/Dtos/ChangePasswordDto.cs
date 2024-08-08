namespace EFCoreIdentityImplementation.Dtos;

public sealed record ChangePasswordDto(
    Guid Id,
    string Password,
    string NewPassword);
