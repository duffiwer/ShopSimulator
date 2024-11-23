
using Store4.BLL;
using Store4.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, FileProductRepository>();
builder.Services.AddScoped<IShopRepository, FileShopRepository>();
builder.Services.AddScoped<IInventoryRepository, FileInventoryRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ShopService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); 

app.Run();
