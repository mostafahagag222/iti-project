using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBrandsController : ControllerBase
    {
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IMapper _mapper;

        public ProductBrandsController(IGenericRepository<ProductBrand> productBrandRepo, IMapper mapper)
        {
            _productBrandRepo = productBrandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductBrandDto>>> GetProductBrands([FromQuery] ProductSpecParams productBrandParams)
        {
            var specs = new ProductBrandsSpecification(productBrandParams);

            var countSpecs = new ProductsBrandsWithFiltersCount(productBrandParams);

            var totalItems = await _productBrandRepo.CountAsync(countSpecs);

            var productsbrands = await _productBrandRepo.GetAllAsync(specs);

            var data = _mapper.Map<IReadOnlyList<ProductBrand>, IReadOnlyList<ProductBrandDto>>(productsbrands);

            var reponse = new Pagination<ProductBrandDto>()
            {
                Count = totalItems,
                PageSize = productBrandParams.PageSize,
                PageIndex = productBrandParams.PageIndex,
                Data = data
            };

            return Ok(reponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductBrandDto>> GetProductBrand(int id)
        {
            var specs = new ProductBrandsSpecification(id);

            var productBrand = await _productBrandRepo.GetByIdAsync(id, specs);

            if (productBrand == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<ProductBrand, ProductBrandDto>(productBrand));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ProductBrand>> UpdateProduct(int id, ProductBrandDto productBrandDto)
        {
            if (id != productBrandDto.Id)
            {
                return BadRequest(new ApiResponse(400));
            }

            var productBrand = _mapper.Map<ProductBrandDto, ProductBrand>(productBrandDto);

            var checkProudct = await this._productBrandRepo.UpdateAsync(id, productBrand);

            if (checkProudct == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductBrandDto>> CreateProductBrand(ProductBrandDto productBrandDto)
        {
            var productBrand = _mapper.Map<ProductBrandDto, ProductBrand>(productBrandDto);

            await _productBrandRepo.AddAsync(productBrand);

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductBrandDto>> DeleteProductBrand(int id)
        {

            var checkProudct = await _productBrandRepo.DeleteAsync(id);

            if (checkProudct == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return NoContent();
        }
    }
}
