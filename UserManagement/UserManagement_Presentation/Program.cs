using Microsoft.EntityFrameworkCore;
using UserManagement_DataAccess;
using UserManagement_DataAccess.InterfacesImplementation;
using Usermanagement_Domain.Interfaces;
using Usermanagement_Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserManagementContext>(option => option.UseSqlServer(
        builder.Configuration.GetConnectionString("UsersConnection")
    ));
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost:6379";
});
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IUser, UserRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
