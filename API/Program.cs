using API.Errors;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using API.Helpers;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Core.Entities;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Database Service
builder.Services.AddDbContext<StoreContext>(options => options.
            UseSqlServer(builder.Configuration.
            GetConnectionString("Default")));

//Media Services

builder.Services.AddScoped<IMediaHandler, MediaHandler>();


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count() > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage)
            .ToArray();

        var errorResponse = new ApiValidationErrorResponse()
        {
            Errors = errors
        };

        return new BadRequestObjectResult(errorResponse);
    };
});


//Auto Mapper
builder.Services.AddAutoMapper(typeof(MappingProfiles));

//Generic Repostiory Service
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<StoreContext>();

//Basket Repository Service
builder.Services.AddScoped<IBasketRepository, BasketRepository>();


// Order Service
builder.Services.AddScoped<IOrderService, OrderService>();


//Payment Service
builder.Services.AddScoped<IPaymentService, PaymentService>();


//How To Use? 
/// <summary>
/// In the constructor you just need to specify the types... for example
/// public ProductsController(IGenericRepository<Product> productRepo,IGenericRepository<ProductBrand> productBrandRepo)
/// </summary>
/// 
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                      });
});

var app = builder.Build();


//Configure Database and Seed
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();
    var userManager = service.GetRequiredService <UserManager<AppUser>>();
    var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
    try
    {
        //Ensure Database Creation and Update to Latest Migration
        var context = service.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();
        await SeedData.SeedAsync(context, loggerFactory, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An Error Ocurred During Migration");
    }
}

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseCors(MyAllowSpecificOrigins);


app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
