using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repository;
using Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
