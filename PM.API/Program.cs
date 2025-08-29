using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using PM.Application.Mappings;
using PM.Application.Services;
using PM.Common.Interfaces;
using PM.Common.Repositories;
using PM.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add ProblemDetails Middleware
builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();
    options.MapToStatusCode<ArgumentException>(StatusCodes.Status400BadRequest);
    options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
    options.MapToStatusCode<KeyNotFoundException>(StatusCodes.Status404NotFound);
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
});

// Register Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// AutoMapper
builder.Services.AddAutoMapper(typeof(CustomMapper));

builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProblemDetails();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
