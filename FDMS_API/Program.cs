using FDMS_API.Configurations.CustomAuthorize.Admin;
using FDMS_API.Configurations.Mappings;
using FDMS_API.Configurations.Middlewares;
using FDMS_API.Data;
using FDMS_API.Services.Implementations;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

builder.Services.AddScoped<IFileUploadService, FileUploadService>();

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddScoped<ITypeService, TypeService>();

builder.Services.AddScoped<IDocumentService, DocumentService>();

// Đăng ký custom authorization handler
builder.Services.AddScoped<IAuthorizationHandler, AdminAuthorizationHandler>();

// Khai báo các Custom Authorization
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("RequireAdmin", policy =>
    {
        policy.AddRequirements(new AdminRequirement());
    });
});


// Khai báo sử dụng JWT

var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,

        ValidateIssuerSigningKey = true, 
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
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

// Middleware su ly ngoai le
app.UseMiddleware<GlobalExceptionHandler>();

// Khai bao sử dung Static Files
app.UseStaticFiles();

app.UseHttpsRedirection();

// Khai báo middleware xác thực JWT
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
