using API.Middleware;
using Core.Entities;
using Core.Interface;
using Infrastructure.Data;
using Infrastructure.Repository;
using Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddCors();

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var conString = builder.Configuration.GetConnectionString("Redis")
    ?? throw new Exception("Redis Connection string not found");
    var configuration = ConfigurationOptions.Parse(conString, true);
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddSingleton<ICartService, CartService>();

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None; // Set to None
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
   .WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.MapControllers();
app.MapGroup("api").MapIdentityApi<AppUser>();
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
        await SeedData.SeedAsync(context);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}
app.Run();
