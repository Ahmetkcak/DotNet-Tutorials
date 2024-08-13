namespace MinimalApiDemo.Models.DTOs;

public record Response(bool Success = false, string? Message = null);
