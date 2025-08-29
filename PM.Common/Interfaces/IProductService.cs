using PM.Application.Dto;
using PM.Common.Common;
using PM.Common.Dto;

namespace PM.Common.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<ProductDto>> GetAllAsync(GetProductInputDto input);
        Task<ProductDto?> GetByIdAsync(long id);
        Task<ProductDto> CreateAsync(CreateOrUpdateProductDto product);
        Task<bool> UpdateAsync(long id, CreateOrUpdateProductDto product);
        Task<bool> DeleteAsync(long id);
    }
}
