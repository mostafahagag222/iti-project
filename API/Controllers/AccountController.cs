using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using JWTAuth.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Core.Entities.Enum;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IMediaHandler mediaHandler;
        private readonly IGenericRepository<WishList> wishListRepo;
        private readonly IGenericRepository<Product> productsRepo;
        private readonly IMapper mapper;

        public AccountController(
            UserManager<AppUser> user,
            IConfiguration config,
            IMediaHandler _mediaHandler,
            IMapper _mapper,
            IGenericRepository<WishList> _wishListRepo,
            IGenericRepository<Product> _productsRepo,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = user;
            _roleManager = roleManager;
            _config = config;
            mediaHandler = _mediaHandler;
            wishListRepo = _wishListRepo;
            productsRepo = _productsRepo;
            mapper = _mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AppUser newUser = new AppUser();
            newUser.Email = registerDTO.Email;
            newUser.UserName = registerDTO.Username;
            newUser.ProfilePicture = "https://localhost:7272/images/32190230-20c8-4efa-8a47-9ebb91f3cf0f_ktokatitmir0.jpg";
            newUser.Gender = registerDTO.Gender;

            IdentityResult result = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Customer");
                return Ok(new
                {
                    result,
                    userId = (await _userManager.FindByEmailAsync(newUser.Email)).Id,
                });
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return BadRequest(ModelState);
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // check
            AppUser user = await _userManager.FindByNameAsync(loginDTO.Username);

            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, loginDTO.Password))
                {
                    // create token based on claims
                    var claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, loginDTO.Username));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    // jti 
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
                    var token = new JwtSecurityToken(
                        audience: _config["JWT:ValidAudience"],
                        issuer: _config["JWT:ValidIssuer"],
                        expires: DateTime.Now.AddHours(1),
                        claims: claims,
                        signingCredentials:
                            new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                        );

                    var x = Enum.GetName(typeof(Gender), user.Gender);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        userId = user.Id,
                        username = loginDTO.Username,
                        profilePicture = user.ProfilePicture,
                        email = user.Email,
                        mobileNumber = user.PhoneNumber,
                        expiration = token.ValidTo,
                        roles = roles,
                        gender = Enum.GetName(typeof(Gender), user.Gender)
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse(400));
                }
            }

            return BadRequest(new ApiResponse(400));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getallusers")]
        public async Task<IActionResult> GetAll([FromQuery] UserSpecs userSpecs)
        {
            var role = await _roleManager.FindByNameAsync("Customer");
            if (role != null)
            {
                //var users = await _userManager.GetUsersInRoleAsync(role.Name);
                var usersList = (List<AppUser>) await _userManager.GetUsersInRoleAsync(role.Name);
                var users = usersList.AsQueryable();
                var total = users.Count();
                if (!string.IsNullOrEmpty(userSpecs.Search)) users = users.Where(u => u.UserName.ToLower().Contains(userSpecs.Search.ToLower()));
                users = users.Skip(userSpecs.PageSize * (userSpecs.PageIndex - 1)).Take(userSpecs.PageSize);

                return Ok(new Pagination<UserDto>()
                {
                    Count = total,
                    PageSize = userSpecs.PageSize,
                    PageIndex = userSpecs.PageIndex,
                    Data = mapper.Map<IReadOnlyList<AppUser>, IReadOnlyList<UserDto>>(users.ToList())
                });
            }

            return BadRequest(new ApiResponse(400));
        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getuser")]
        public async Task<IActionResult> getUser()
        {
            // check
            var tokenHandler = new JwtSecurityTokenHandler();
            var SecretKey = _config.GetValue<string>("JWT:SecretKey");
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                // create token based on claims
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                // jti 
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                var newKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
                var newToken = new JwtSecurityToken(
                    audience: _config["JWT:ValidAudience"],
                    issuer: _config["JWT:ValidIssuer"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials:
                        new SigningCredentials(newKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = token,
                    userId = user.Id,
                    profilePicture = user.ProfilePicture,
                    email = user.Email,
                    roles = roles,
                    mobileNumber = user.PhoneNumber,
                    username = user.UserName,
                    gender = Enum.GetName(typeof(Gender), user.Gender)
            });

            }

            return Unauthorized(new ApiResponse(401));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(UserProfileDto))]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return Ok(new UserProfileDto
            {
                id = user.Id,
                userName = user.UserName,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                profilePicture = user.ProfilePicture,
                gender = user.Gender,
            });
        }

        [HttpPut("updateProfile")]
        public async Task<IActionResult> UpdateUserProfile(UserProfileDto userProfileDto)
        {
            var user = await _userManager.FindByIdAsync(userProfileDto.id);

            if (user != null)
            {
                user.UserName = userProfileDto.userName;
                user.Email = userProfileDto.email;
                user.PhoneNumber = userProfileDto.phoneNumber;
                user.Gender = userProfileDto.gender;

                await _userManager.UpdateAsync(user);
                return Ok(new ApiResponse(200));
            }

            return BadRequest(new ApiResponse(400));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserProfileImage([FromForm] string id, [FromForm] IFormFile image)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (user.ProfilePicture == null)
                {
                    string profilePictureURL = mediaHandler.UploadImage(image);
                    user.ProfilePicture = profilePictureURL;
                    await _userManager.UpdateAsync(user);
                    Console.WriteLine(image);
                    Console.WriteLine(profilePictureURL);
                    return Ok(new ApiResponse(200));
                }
                else if (user.ProfilePicture != null)
                {
                    //mediaHandler.RemoveImage(user.ProfilePicture);

                    string profilePictureURL = mediaHandler.UploadImage(image);
                    Console.WriteLine(image);
                    Console.WriteLine(profilePictureURL);
                    user.ProfilePicture = profilePictureURL;
                    await _userManager.UpdateAsync(user);

                    return Ok(new ApiResponse(200));
                }
            }

            return BadRequest(new ApiResponse(400));
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("wishlist")]
        public async Task<IActionResult> GetUserWishList()
        {
            // check
            string userId = GetUserId();

            if (userId != null)
            {
                var user = await _userManager.Users
                    .Include(u => u.WishList.Products)
                    .ThenInclude(p => p.Pictures)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(user?.WishList?.Products);

                return Ok(new
                {
                    Id = user.WishListId,
                    Products = data
                });
            }

            return Unauthorized(new ApiResponse(401));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("wishlist/add")]
        public async Task<IActionResult> AddToWishList(AddToWishListDto data)
        {
            // check
            string userId = GetUserId();

            if (userId != null)
            {
                var user = _userManager.Users.Include(u => u.WishList.Products).FirstOrDefault(u => u.Id == userId);
                var product = await productsRepo.GetByIdAsync(data.productId);

                if (product != null)
                {
                    user.WishList.Products.Add(product);
                    await _userManager.UpdateAsync(user);

                    var userData = await _userManager.Users
                        .Include(u => u.WishList.Products)
                        .ThenInclude(p => p.Pictures)
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    var productsData = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(user.WishList.Products);

                    return Ok(new {
                        response = new ApiResponse(200, "Product Added to WishList Successfully"),
                        Id = user.WishListId,
                        Products = productsData
                    });

                }

                return NotFound(new ApiResponse(404, "Product Not Found"));
            }

            return Unauthorized(new ApiResponse(401));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("wishlist/remove")]
        public async Task<IActionResult> RemoveFromWishList(AddToWishListDto data)
        {
            // check
            string userId = GetUserId();

            if (userId != null)
            {
                var user = _userManager.Users
                    .Include(u => u.WishList.Products)
                    .ThenInclude(p => p.Pictures)
                    .FirstOrDefault(u => u.Id == userId);
                var product = await productsRepo.GetByIdAsync(data.productId);

                if (product != null)
                {
                    user.WishList.Products.Remove(product);
                    await _userManager.UpdateAsync(user);

                    var userData = await _userManager.Users
                        .Include(u => u.WishList.Products)
                        .ThenInclude(p => p.Pictures)
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    var productsData = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(user.WishList.Products);

                    return Ok(new
                    {
                        Id = user.WishListId,
                        Products = productsData
                    });
                }

                return NotFound(new ApiResponse(404, "Product Not Found"));
            }

            return Unauthorized(new ApiResponse(401));
        }

        [NonAction]
        public string GetUserId()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var SecretKey = _config.GetValue<string>("JWT:SecretKey");
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return userId;
        }
    }
}
