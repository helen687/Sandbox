using Microsoft.EntityFrameworkCore;
using CatalogDBContext;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text.Json.Serialization;
using System.Text.Json;
using CatalogWeb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(
            new JsonDateOnlyConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DBContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
          b => b.MigrationsAssembly("CatalogWeb")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Catalog}/{action=getList}/{id?}");

app.MapControllers();

app.Run();
