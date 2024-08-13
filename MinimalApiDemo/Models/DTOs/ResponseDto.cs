namespace MinimalApiDemo.Models.DTOs;

public record ResponseDto(string Name,string Description, decimal Price, int Quantity, DateTime Date)
{
    public decimal TotalPrice => Price * Quantity;
}