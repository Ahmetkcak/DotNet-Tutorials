using MinimalApiDemo.Models.DTOs;

namespace MinimalApiDemo.Repositories;

public interface IProductRepo
{
    Task<Response> Add(AddRequestDTO request);
    Task<Response> Update(UpdateRequestDTO request);
    Task<List<ResponseDto>> GetAll();
    Task<ResponseDto> GetById(int id);
    Task<Response> Delete(int id);
}
