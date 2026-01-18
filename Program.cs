using BluntServe;
using BluntServe.Models;
using BluntServe.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<PgDbContext>(options =>
{
    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
});

// 将 DbContextFactory 注册为 Scoped 而不是 Singleton
builder.Services.AddDbContextFactory<PgDbContext>((services, options) =>
{
    var config = services.GetRequiredService<IConfiguration>();
    options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Scoped);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 允许接收大小写不敏感的 JSON
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        // 保持属性原名（不转换为 camelCase）
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));


// 3. 配置 Authentication（必须添加！）
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

// 4. 添加授权服务
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
