namespace EFCoreIdentityImplementation.Dtos;

public sealed record ReqisterDto(
    string Email,
    string UserName,
    string FirsName,
    string LastName,
    string Password
    );
