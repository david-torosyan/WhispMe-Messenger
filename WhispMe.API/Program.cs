using AutoMapper;
using MongoDB.Driver;
using WhispMe.API.MiddlewareExtensions;
using WhispMe.BLL.AuthInterfaces;
using WhispMe.BLL.AuthServices;
using WhispMe.Configuration.Mappings;
using WhispMe.DAL.Data;
using WhispMe.DAL.Interfaces;
using WhispMe.DAL.Repositories;
using WhispMe.DTO;

var builder = WebApplication.CreateBuilder(args);

// NoSQL Connection
builder.Services.AddScoped<IMongoClient, MongoClient>(sp =>
     new MongoClient(builder.Configuration["MongoDb:ConnectionString"]));
builder.Services.AddScoped(sp =>
     new WhispMeDbContext(sp.GetRequiredService<IMongoClient>(), builder.Configuration["MongoDb:Database"]!));

// JWT Configuration
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddLogging();

builder.Services.AddSingleton(new MapperConfiguration(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
}).CreateMapper());

// Add services to the container.
builder.Services.AddControllers();

// Custom extension method for JwtBearer authentication
builder.AddAppAuthetication();
builder.Services.AddModSwaggerGen();

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

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

// Custom middleware for logging requests
app.ConfigureRequestLoggingMiddleware();

// Custom middleware for exception handling
app.ConfigureExceptionHandling();

// Custom middleware for logging requests
app.ConfigureExceptionLogging();

app.MapControllers();

app.Run();

