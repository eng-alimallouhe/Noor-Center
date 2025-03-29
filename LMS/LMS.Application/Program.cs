using LMS.Application.Services;
using LMS.Application.Services.Users;
using LMS.Domain.Entities.Financial;
using LMS.Domain.Entities.Users;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.DbContexts;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Repositories.Financial;
using LMS.Infrastructure.Repositories.Users;
using LMS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Error()
            .WriteTo.File("logs\\logger.txt", rollingInterval: RollingInterval.Month)
            .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//get JWT section from appsettigns.json:
var jwtSettings = builder.Configuration.GetSection("Jwt");


//adding authentication by JWTBearer:
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        
        
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
    };
});


// Inject AutoMapper and assign the Profiles:
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Inject the dbcontext(AppDbContext): 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);



// Inject the Repositories:
// Users Repositories:
builder.Services.AddScoped<IRepository<Role>, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepositroy>();
builder.Services.AddScoped<IRepository<Department>, DepartmentRepository>();
builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();
builder.Services.AddScoped<IRepository<EmployeeDepartment>, EmployeeDepartmentRepository>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IRepository<Address>, AddressRepository>();
builder.Services.AddScoped<IRepository<OtpCode>, OtpCodeRepository>();
builder.Services.AddScoped<IRepository<Notification>, NotificationRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

//Financial Repositories: 
builder.Services.AddScoped<IRepository<LoyaltyLevel>, LevelRepository>();

//Inject Email Service:
builder.Services.AddScoped<IEmailService, EmailService>(prvider =>
{ 
    var config = builder.Configuration;
    return new EmailService(
        email: config["EmailSettings:Email"]!,
        password: config["EmailSettings:Password"]!,
        host: config["EmailSettings:Host"]!,
        port: int.Parse(config["EmailSettings:Port"]!)
    );
});


//Inject Application Services:
builder.Services.AddScoped<CodeService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<LoginService>();



builder.Host.UseSerilog();

var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
