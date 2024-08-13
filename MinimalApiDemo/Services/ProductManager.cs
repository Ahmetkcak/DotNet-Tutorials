using MinimalApiDemo.Models.DTOs;
using MinimalApiDemo.Repositories;

namespace MinimalApiDemo.Services;

public class ProductManager(IProductRepo productRepo) : IProductService
{
    public async Task<Response> Add(AddRequestDTO request) => await productRepo.Add(request);
    public async Task<Response> Delete(int id) => await productRepo.Delete(id);
    public async Task<List<ResponseDto>> GetAll() => await productRepo.GetAll();
    public async Task<ResponseDto> GetById(int id) => await productRepo.GetById(id);
    public async Task<Response> Update(UpdateRequestDTO request) => await productRepo.Update(request);
}
