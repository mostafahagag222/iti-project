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
using System.Net;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IGenericRepository<Review> reviewRepo;
        private readonly IMapper mapper;
        private readonly IConfiguration config;
        public ReviewController(IMapper _mapper, IGenericRepository<Review> _reviewRepo, IConfiguration _config)
        {
            mapper = _mapper;
            reviewRepo = _reviewRepo;
            config = _config;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> getReviews(int productId)
        {
            var specs = new ReviewSpecification(productId);
            var reviews = await reviewRepo.GetAllAsync(specs);
            var data = mapper.Map<IReadOnlyList<Review>, IReadOnlyList<ReviewDto>>(reviews);

            specs.GetTotalReviews(reviews);
            var rating = specs.CalculateRating();

            return Ok(new
            {
                reviews= data,
                rating = rating,
                stars = specs.getStars(),
            });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewDto addReviewDto)
        {
            var userId = TokenExtractor.GetUserId(config, HttpContext);


            await reviewRepo.AddAsync(new Review
            {
                ProductId = addReviewDto.productId,
                Body = addReviewDto.body,
                UserId = userId,
                Date = DateTime.Now,
                Stars = addReviewDto.stars,
            });

            var specs = new ReviewSpecification(addReviewDto.productId);

            var reviews = await reviewRepo.GetAllAsync(specs);

            var data = mapper.Map<IReadOnlyList<Review>, IReadOnlyList<ReviewDto>>(reviews);

            specs.GetTotalReviews(reviews);
            var rating = specs.CalculateRating();

            return Ok(new
            {
                reviews = data,
                rating = rating,
                stars = specs.getStars(),
            });
        }
    }
}
