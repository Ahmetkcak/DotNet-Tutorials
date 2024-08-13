using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.AutoMapper;
using MinimalApiDemo.DataAccess;
using MinimalApiDemo.Models.DTOs;
using MinimalApiDemo.Models.Entities;

namespace MinimalApiDemo.Repositories;

public class ProductRepo(AppDbContext dbContext, IMapper mapper) : IProductRepo
{
    public async Task<Response> Add(AddRequestDTO request)
    {
        dbContext.Products.Add(mapper.Map<Product>(request));
        await dbContext.SaveChangesAsync();
        return new Response(true, "Product added successfully");
    }

    public async Task<Response> Delete(int id)
    {
        dbContext.Products.Remove(await dbContext.Products.FindAsync(id));
        await dbContext.SaveChangesAsync();
        return new Response(true, "Product deleted successfully");
    }

    public async Task<List<ResponseDto>> GetAll() => mapper.Map<List<ResponseDto>>(await dbContext.Products.ToListAsync());

    public async Task<ResponseDto> GetById(int id) => mapper.Map<ResponseDto>(await dbContext.Products.FindAsync(id));

    public async Task<Response> Update(UpdateRequestDTO request)
    {
        dbContext.Products.Update(mapper.Map<Product>(request));
        await dbContext.SaveChangesAsync();
        return new Response(true, "Product updated successfully");
    }
}
