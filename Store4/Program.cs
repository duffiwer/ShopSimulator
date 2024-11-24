using Store4.BLL;
using Store4.DAL;
using Microsoft.EntityFrameworkCore;
using Store4.Pages;

var builder = WebApplication.CreateBuilder(args);

var dalImplementation = builder.Configuration["DAL:Implementation"];
var connectionString = builder.Configuration.GetConnectionString("Database");

if (dalImplementation == "Database")
{
    builder.Services.AddDbContext<StoreSimulatorContext>(options =>
    options.UseSqlServer(connectionString));
    builder.Services.AddScoped<IShopRepository, DatabaseShopRepository>();
    builder.Services.AddScoped<IProductRepository, DatabaseProductRepository>();
    builder.Services.AddScoped<IInventoryRepository, DatabaseInventoryRepository>();

}
else
{
    builder.Services.AddScoped<IProductRepository, FileProductRepository>();
    builder.Services.AddScoped<IShopRepository, FileShopRepository>();
    builder.Services.AddScoped<IInventoryRepository, FileInventoryRepository>();

}

builder.Services.AddScoped<ShopService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<InventoryService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();