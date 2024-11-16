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
    public class ProductTypesController : ControllerBase
    {
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductTypesController(IGenericRepository<Product> productRepo,
                                      IGenericRepository<ProductType> productTypeRepo,
                                      IMapper mapper)
        {
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductTypeDto>>> GetProductTypes([FromQuery] ProductSpecParams productParams)
        {
            var specs = new ProductTypesSpecification(productParams);

            var countSpecs = new ProductTypesWithFilterCount(productParams);

            var totalItems = await _productTypeRepo.CountAsync(countSpecs);

            var productstypes = await _productTypeRepo.GetAllAsync(specs);

            var data = _mapper.Map< IReadOnlyList<ProductType>, IReadOnlyList<ProductTypeDto> >(productstypes);

            var reponse = new Pagination<ProductTypeDto>()
            {
                Count = totalItems,
                PageSize = productParams.PageSize,
                PageIndex = productParams.PageIndex,
                Data = data
            };

            return Ok(reponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductTypeDto>> GetProductType(int id)
        {
            var specs = new ProductTypesSpecification(id);

            var productType = await this._productTypeRepo.GetByIdAsync(id, specs);
            
            if (productType == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<ProductType, ProductTypeDto>(productType));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ProductType>> UpdateProduct(int id, ProductTypeDto productTypeDto)
        {
            if (id != productTypeDto.Id)
            {
                return BadRequest(new ApiResponse(400));
            }

            var productType = _mapper.Map<ProductTypeDto, ProductType>(productTypeDto);

            var checkProudct = await this._productTypeRepo.UpdateAsync(id, productType);

            if (checkProudct == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductTypeDto>> CreateProduct( ProductTypeDto productTypeDto )
        {
            var productType = _mapper.Map<ProductTypeDto, ProductType>(productTypeDto);

            await _productTypeRepo.AddAsync(productType);

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductTypeDto>> DeleteProduct(int id)
        {

            var checkProudct = await _productTypeRepo.DeleteAsync(id);

            if (checkProudct == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return NoContent();
        }

    }
}
