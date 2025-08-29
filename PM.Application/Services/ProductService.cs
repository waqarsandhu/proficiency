using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PM.Application.Dto;
using PM.Common.Common;
using PM.Common.Dto;
using PM.Common.Interfaces;
using PM.EntityFrameworkCore.Entities;

namespace PM.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product, long> _repository;
        private readonly IMapper _mapper;

        public ProductService(IRepository<Product, long> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateAsync(CreateOrUpdateProductDto product)
        {
            var obj = _mapper.Map<Product>(product);

            var result = await _repository.CreateAndGetAsync(obj);

            return _mapper.Map<ProductDto>(result);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var find = await _repository.GetAsync(e=>e.Id == id);
            if(find == null)
            {
                return false;
            }

            await _repository.DeleteAsync(find);

            return true;
        }

        public async Task<PagedResult<ProductDto>> GetAllAsync(GetProductInputDto input)
        {
            var search = input.Search?.Trim().ToLower();

            var query = _repository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(search),
                                  sl => sl.Name.ToLower().Contains(search));

            var pagedAndFiltered = await query.OrderBy(input).PageBy(input).ToListAsync();

            return new PagedResult<ProductDto>(query.Count(), _mapper.Map<List<ProductDto>>(pagedAndFiltered));
        }

        public async Task<bool> UpdateAsync(long id, CreateOrUpdateProductDto product)
        {
            var find = await _repository.GetAsync(e => e.Id == id);
            if (find == null)
            {
                return false;
            }

            _mapper.Map(product, find);

            find.UpdatedOn = DateTime.Now;

            await _repository.UpdateAsync(find);

            return true;
        }

        public async Task<ProductDto?> GetByIdAsync(long id)
        {
            var find = await _repository.GetAsync(e => e.Id == id);
            if (find == null)
            {
                return null;
            }

            return _mapper.Map<ProductDto>(find);
        }
    }
}
