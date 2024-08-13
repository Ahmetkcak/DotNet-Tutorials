namespace MinimalApiDemo.Models.DTOs;

public record UpdateRequestDTO(int Id,string Name, string Description, decimal Price, int Quantity);
