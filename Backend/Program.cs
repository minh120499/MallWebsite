using System.Text.Json.Serialization;
using Backend;
using Backend.Exceptions;
using Backend.Repository;
using Backend.Repository.Implements;
using Backend.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MyDbConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Singleton);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => { options.SignIn.RequireConfirmedAccount = true; })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ExceptionMiddleware>();

// Banner
builder.Services.AddScoped<IBannersRepository, BannersRepository>();
builder.Services.AddScoped<BannersService>();
// Category
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<CategoriesService>();
// Employee
builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
builder.Services.AddScoped<EmployeesService>();
// Facility
builder.Services.AddScoped<IFacilitiesRepository, FacilitiesRepository>();
builder.Services.AddScoped<FacilitiesService>();
// Feedback
builder.Services.AddScoped<IFeedbacksRepository, FeedbacksRepository>();
builder.Services.AddScoped<FeedbacksService>();
// Floor
builder.Services.AddScoped<IFloorsRepository, FloorsRepository>();
builder.Services.AddScoped<FloorsService>();
// Order
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<OrdersService>();
// OrderLineItem
builder.Services.AddScoped<IOrderLineItemsRepository, OrderLineItemsRepository>();
builder.Services.AddScoped<OrderLineItemsService>();
// Product
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<ProductsService>();
// Store
builder.Services.AddScoped<IStoresRepository, StoresRepository>();
builder.Services.AddScoped<StoresService>();
// StoreProduct
builder.Services.AddScoped<IStoreProductsRepository, StoreProductsRepository>();
builder.Services.AddScoped<StoreProductsService>();
// Variant
builder.Services.AddScoped<IVariantsRepository, VariantsRepository>();
builder.Services.AddScoped<VariantsService>();

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "R_Mall API", Description = "A Mall you love", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "R_Mall API V1"); });

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.UseCors();
app.Run();