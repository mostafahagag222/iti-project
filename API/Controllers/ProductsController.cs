using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> productRepo;
        private readonly IGenericRepository<Media> mediaRepo;
        private readonly IGenericRepository<ProductBrand> productBrandRepo;
        private readonly IGenericRepository<ProductType> productTypeRepo;
        private readonly IMapper mapper;
        private readonly IMediaHandler mediaHandler;

      

        public ProductsController(IGenericRepository<Product> productRepo,
            IGenericRepository<Media> mediaRepo,
            IGenericRepository<ProductBrand> productBrandRepo,
            IGenericRepository<ProductType> productTypeRepo,
            IMapper mapper,
            IMediaHandler mediaHandler
            
            )
        {
            this.productRepo = productRepo;
            this.mediaRepo = mediaRepo;
            this.productBrandRepo = productBrandRepo;
            this.productTypeRepo = productTypeRepo;
            this.mapper = mapper;
            this.mediaHandler = mediaHandler;
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var specs = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpecs = new ProductsWithFiltersCount(productParams);
            var totalItems = await productRepo.CountAsync(countSpecs);
            var products = await productRepo.GetAllAsync(specs);
            var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);



                // 1-1-2022
                // 5-10-2022
            var reponse = new Pagination<ProductDto>()
            {
                Count = totalItems,
                PageSize = productParams.PageSize,
                PageIndex = productParams.PageIndex,
                Data = data
            };

            return Ok(reponse);
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var specs = new ProductsWithTypesAndBrandsSpecification(id);
          
            var product=  await this.productRepo.GetByIdAsync(id,specs);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(mapper.Map<Product, ProductDto>(product));

        }

        [HttpPost("uploadimages")]
        public IActionResult UploadImages([FromForm] List<IFormFile> files)
        {

            for (int i = 0; i < files.Count; i++)
            {
                string imgPath = mediaHandler.UploadImage(files[i]);

            }
            return Ok(new ApiResponse(200));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromForm] DataWithImagesUpload obj)
        {

         
            var specs = new ProductsWithTypesAndBrandsSpecification(id);


            var checkProudct = await this.productRepo.GetByIdAsync(id, specs);

            if (checkProudct == null)
            {
                return NotFound(new ApiResponse(404));
            }
            else
            {
                foreach (var media in checkProudct.Pictures)
                {

                    mediaHandler.RemoveImage(media.ImageUrl);

                    await this.mediaRepo.DeleteAsync(media.Id);

                }
               

                Product product = JsonConvert.DeserializeObject<Product>(obj.product);
                
                for (int i = 0; i < obj.files.Count; i++)
                {
                    string imgPath = mediaHandler.UploadImage(obj.files[i]);

                    Media picture = new Media() { ProductId = id, ImageUrl = imgPath };
                   
                    await this.mediaRepo.AddAsync(picture);
                }
                product.Id= id;
                await this.productRepo.UpdateAsync(id, product);
                return Ok();


            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] DataWithImagesUpload obj)
        {


            Product product = JsonConvert.DeserializeObject<Product>(obj.product);
            

            await this.productRepo.AddAsync(product);

            int id=product.Id;

            for (int i = 0; i < obj.files.Count; i++)
            {
                string imgPath = mediaHandler.UploadImage(obj.files[i]);

                Media picture = new Media() {ProductId=id, ImageUrl = imgPath };

                await this.mediaRepo.AddAsync(picture);
            }

            return Ok(product);


        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {


            var specs = new ProductsWithTypesAndBrandsSpecification(id);

 
            var checkProudct= await this.productRepo.GetByIdAsync(id,specs);

            if (checkProudct == null)
            {
                return NotFound(new ApiResponse(404));
            }
            else
            {
                await this.productRepo.DeleteAsync(checkProudct.Id);
                if (checkProudct.Pictures != null)
                {
                    foreach (var media in checkProudct.Pictures)
                    {

                        mediaHandler.RemoveImage(media.ImageUrl);

                    }
                }
                return NoContent();
            }
        }
        
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            return Ok(await productBrandRepo.GetAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            return Ok(await productTypeRepo.GetAllAsync());
        }

        [HttpGet("types/count")]
        public async Task<ActionResult<List<TypesDto>>> GetTypesCount()
        {
            var param = new ProductSpecParams() { PageSize = 50 };
            var specs = new ProductsWithTypesAndBrandsSpecification(param);
            var result = await productRepo.GetAllAsync(specs);
            var count = result.GroupBy(p => p.ProductType).Select(g => new TypesDto{ Id = g.Key.Id, Name=g.Key.Name ,Count = g.Count() }).ToList();
            count.Add(new()
            {
                Name="All",
                Id = 0,
                Count = result.Count()
            });
            return Ok(count);
        }

        [HttpGet("colors")]
        public async Task<ActionResult<List<ColorDto>>> GetColors()
        {
            //var specs = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams());
            var result = await productRepo.GetAllAsync();
            var count = result.GroupBy(p => p.Color).Select(g => new ColorDto { Color = g.Key, Count = g.Count() }).ToList();
            
            return Ok(count);
        }

        
        [HttpGet("maxPrice")]
        public async Task<ActionResult<decimal>> GetMaxPrice()
        {
            //var specs = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams());
            var result = await productRepo.GetAllAsync();
            var maxPrice = result.Max(p => p.Price);

            return Ok(maxPrice);
        }

        //
        [HttpGet("trendy")]
        public async Task<ActionResult<List<ProductDto>>> GetTrendy()
        {

            var specs = new ProductsWithTypesAndBrandsSpecification(new ProductSpecParams());
            var result = await productRepo.GetAllAsync(specs);
            var trendy = result.Where(p => DateTime.Now.Subtract(p.ManufactureDate) <= new TimeSpan(30,0,0,0) )
                    .OrderByDescending(p => p.UnitsSold).Take(5).ToList();

            var data = mapper.Map<List<Product>, List<ProductDto>>(trendy);
            
            return data;
        }


    }
}
