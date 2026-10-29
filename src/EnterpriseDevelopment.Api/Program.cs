using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Application.Services;
using EnterpriseDevelopment.Api.Endpoints;
using EnterpriseDevelopment.Domain;

// Создание и настройка веб-приложения
var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddOpenApi();
builder.Services.AddSingleton<DataSeeder>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

var app = builder.Build();

// Настройка OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Перенаправление HTTP-запросов на HTTPS
app.UseHttpsRedirection();

// Регистрация маршрутов API
app.MapRestaurantEndpoints();

app.MapClientEndpoints();

app.MapDishEndpoints();

app.MapCategoryEndpoints();

app.MapOrderEndpoints();

app.MapAnalyticsEndpoints();

// Запуск приложения
app.Run();
