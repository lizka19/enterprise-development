using EnterpriseDevelopment.Application.Interfaces;
using EnterpriseDevelopment.Application.Services;
using EnterpriseDevelopment.Api.Endpoints;
using EnterpriseDevelopment.Domain;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddOpenApi();
builder.Services.AddSingleton<DataSeeder>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();

var app = builder.Build();

// Настройка OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

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
