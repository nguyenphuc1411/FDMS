using FDMS_API.Data;
using FDMS_API.Mappings;
using FDMS_API.Repositories;
using FDMS_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Khai báo DbContext

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDB"));
});

// Khai báo các DI

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IFlightService, FlightService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ISystemSettingService, SystemSettingService>();

builder.Services.AddScoped<IUploadImageService, UploadImageService>();
// Khai báo sử dụng JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, 
        ValidateAudience = true, 
        ValidateLifetime = true, 
        ValidateIssuerSigningKey = true, 
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


// Khai báo để có thể truy cập HttpContext từ các Class service
builder.Services.AddHttpContextAccessor();

// Khai báo AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Khai bao su dung Static Files

app.UseStaticFiles();

app.UseHttpsRedirection();

// Khai báo middleware xác thực JWT
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
