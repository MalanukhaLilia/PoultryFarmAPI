using PoultryFarmApi.Interfaces;
using PoultryFarmApi.Middleware;
using PoultryFarmApi.Repositories;
using PoultryFarmApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICoopRepository, CoopRepository>();
builder.Services.AddSingleton<IBirdRepository, BirdRepository>();
builder.Services.AddSingleton<IEggRepository, EggRepository>();

builder.Services.AddScoped<CoopService>();
builder.Services.AddScoped<BirdService>();
builder.Services.AddScoped<EggService>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
