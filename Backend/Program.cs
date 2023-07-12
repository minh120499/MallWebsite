using Backend;
using Backend.Exceptions;
using Backend.Repository;
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

builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
builder.Services.AddScoped<EmployeesService>();
builder.Services.AddScoped<IBannersRepository, BannersRepository>();
builder.Services.AddScoped<BannersService>();
builder.Services.AddScoped<IStoresRepository, StoresRepository>();
builder.Services.AddScoped<StoresService>();
builder.Services.AddScoped<IStoreProductsRepository, StoreProductsRepository>();
builder.Services.AddScoped<StoreProductsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "PizzaStore API", Description = "Making the Pizzas you love", Version = "v1" });
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
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1"); });

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.UseCors();
app.Run();