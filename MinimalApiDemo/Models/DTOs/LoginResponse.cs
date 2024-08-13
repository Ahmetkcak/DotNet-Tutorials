namespace MinimalApiDemo.Models.DTOs;

public record LoginResponse(bool Success, string? Message, string? Token);
