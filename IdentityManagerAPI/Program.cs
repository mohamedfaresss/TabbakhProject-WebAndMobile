using DataAcess.Repos.IRepos;
using DataAcess.Repos;
using IdentityManager.Services.ControllerService.IControllerService;
using IdentityManager.Services.ControllerService;
using IdentityManagerAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Domain;
using Models.DTOs.Mapper;
using Scalar.AspNetCore;
using System.Text;
using DataAcess.DbContexts;
using Graduation_project_APIs;
using DataAcess.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // توحيد الـ keys
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache(); // إضافة IMemoryCache للـ proxy

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ArabicDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ArabicConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddAutoMapper(typeof(RecipeProfile));

// Add Identity Managers
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

// Add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin() // أو WithOrigins("http://127.0.0.1:5500", "https://yourfrontend.com")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Configure JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["ApiSettings:Secret"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.FromDays(7)
    };
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Graduation project APIs ", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    c.OperationFilter<AddAcceptLanguageHeaderParameter>();
});

// Exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger";
});

// Scalar API
app.MapScalarApiReference();

// Global exception handler
app.UseExceptionHandler();

// HTTPS redirect
app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

// Auth middlewares
app.UseAuthentication();
app.UseAuthorization();

// Static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

// Controllers
app.MapControllers();

app.Run();