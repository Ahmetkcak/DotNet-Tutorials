namespace MinimalApiDemo.Models.DTOs;

public record AddRequestDTO(string Name, string Description, decimal Price, int Quantity);