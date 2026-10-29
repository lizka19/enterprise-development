using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;
using EnterpriseDevelopment.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<DataSeeder>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/api/restaurants", (DataSeeder data) =>
{
    return Results.Ok(data.Restaurants);
});

app.MapGet("/api/restaurants/{id:int}", (int id, DataSeeder data) =>
{
    var restaurant = data.Restaurants.FirstOrDefault(r => r.Id == id);

    if (restaurant == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(restaurant);
});

app.MapPost("/api/restaurants", (Restaurant restaurant, DataSeeder data) =>
{
    if (string.IsNullOrWhiteSpace(restaurant.Name) ||
        string.IsNullOrWhiteSpace(restaurant.Address) ||
        restaurant.Rating < 0 ||
        restaurant.Rating > 5)
    {
        return Results.BadRequest("Некорректные данные ресторана");
    }

    restaurant.Id = data.Restaurants.Max(r => r.Id) + 1;

    data.Restaurants.Add(restaurant);

    return Results.Created($"/api/restaurants/{restaurant.Id}", restaurant);
});


app.MapPut("/api/restaurants/{id:int}", (int id, Restaurant updatedRestaurant, DataSeeder data) =>
{
    var restaurant = data.Restaurants.FirstOrDefault(r => r.Id == id);

    if (restaurant == null)
    {
        return Results.NotFound("Ресторан не найден");
    }

    if (string.IsNullOrWhiteSpace(updatedRestaurant.Name) ||
        string.IsNullOrWhiteSpace(updatedRestaurant.Address) ||
        updatedRestaurant.Rating < 0 ||
        updatedRestaurant.Rating > 5)
    {
        return Results.BadRequest("Некорректные данные ресторана");
    }

    restaurant.Name = updatedRestaurant.Name;
    restaurant.Address = updatedRestaurant.Address;
    restaurant.Rating = updatedRestaurant.Rating;
    restaurant.OpeningTime = updatedRestaurant.OpeningTime;
    restaurant.ClosingTime = updatedRestaurant.ClosingTime;

    return Results.Ok(restaurant);
});

app.MapDelete("/api/restaurants/{id:int}", (int id, DataSeeder data) =>
{
    var restaurant = data.Restaurants.FirstOrDefault(r => r.Id == id);

    if (restaurant == null)
    {
        return Results.NotFound("Ресторан не найден");
    }

    if (data.Dishes.Any(d => d.RestaurantId == id) ||
        data.Orders.Any(o => o.RestaurantId == id))
    {
        return Results.Conflict(
            "Нельзя удалить ресторан, у которого есть блюда или заказы");
    }

    data.Restaurants.Remove(restaurant);

    return Results.NoContent();
});

app.MapGet("/api/clients", (DataSeeder data) =>
{
    return Results.Ok(data.Clients);
});

app.MapGet("/api/clients/{id:int}", (int id, DataSeeder data) =>
{
    var client = data.Clients.FirstOrDefault(c => c.Id == id);

    if (client == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(client);
});

app.MapPost("/api/clients", (Client client, DataSeeder data) =>
{
    if (string.IsNullOrWhiteSpace(client.FullName) ||
        string.IsNullOrWhiteSpace(client.PhoneNumber) ||
        string.IsNullOrWhiteSpace(client.DeliveryAddress))
    {
        return Results.BadRequest("Некорректные данные клиента");
    }

    client.Id = data.Clients.Select(c => c.Id).DefaultIfEmpty(0).Max() + 1;

    data.Clients.Add(client);

    return Results.Created($"/api/clients/{client.Id}", client);
});

app.MapPut("/api/clients/{id:int}", (int id, Client updatedClient, DataSeeder data) =>
{
    var client = data.Clients.FirstOrDefault(c => c.Id == id);

    if (client == null)
    {
        return Results.NotFound("Клиент не найден");
    }

    if (string.IsNullOrWhiteSpace(updatedClient.FullName) ||
        string.IsNullOrWhiteSpace(updatedClient.PhoneNumber) ||
        string.IsNullOrWhiteSpace(updatedClient.DeliveryAddress))
    {
        return Results.BadRequest("Некорректные данные клиента");
    }

    client.FullName = updatedClient.FullName;
    client.PhoneNumber = updatedClient.PhoneNumber;
    client.DeliveryAddress = updatedClient.DeliveryAddress;

    return Results.Ok(client);
});

app.MapDelete("/api/clients/{id:int}", (int id, DataSeeder data) =>
{
    var client = data.Clients.FirstOrDefault(c => c.Id == id);

    if (client == null)
    {
        return Results.NotFound("Клиент не найден");
    }

    if (data.Orders.Any(o => o.ClientId == id))
    {
        return Results.Conflict(
            "Нельзя удалить клиента, у которого есть заказы");
    }

    data.Clients.Remove(client);

    return Results.NoContent();
});

app.MapGet("/api/dishes", (DataSeeder data) =>
{
    return Results.Ok(data.Dishes);
});

app.MapGet("/api/dishes/{id:int}", (int id, DataSeeder data) =>
{
    var dish = data.Dishes.FirstOrDefault(d => d.Id == id);

    if (dish == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(dish);
});

app.MapGet("/api/categories", (DataSeeder data) =>
{
    return Results.Ok(data.Categories);
});

app.MapPost("/api/dishes", (Dish dish, DataSeeder data) =>
{
    if (string.IsNullOrWhiteSpace(dish.Name) ||
        dish.Price <= 0 ||
        dish.Weight <= 0)
    {
        return Results.BadRequest("Некорректные данные блюда");
    }

    var category = data.Categories.FirstOrDefault(
        c => c.Id == dish.CategoryId);

    if (category == null)
    {
        return Results.BadRequest("Категория не найдена");
    }

    var restaurant = data.Restaurants.FirstOrDefault(
        r => r.Id == dish.RestaurantId);

    if (restaurant == null)
    {
        return Results.BadRequest("Ресторан не найден");
    }

    dish.Id = data.Dishes
        .Select(d => d.Id)
        .DefaultIfEmpty(0)
        .Max() + 1;

    dish.Category = category;
    dish.Restaurant = restaurant;

    data.Dishes.Add(dish);

    return Results.Created($"/api/dishes/{dish.Id}", dish);
});

app.MapPut("/api/dishes/{id:int}", (int id, Dish updatedDish, DataSeeder data) =>
{
    var dish = data.Dishes.FirstOrDefault(d => d.Id == id);

    if (dish == null)
    {
        return Results.NotFound("Блюдо не найдено");
    }

    if (string.IsNullOrWhiteSpace(updatedDish.Name) ||
        updatedDish.Price <= 0 ||
        updatedDish.Weight <= 0)
    {
        return Results.BadRequest("Некорректные данные блюда");
    }

    var category = data.Categories.FirstOrDefault(
        c => c.Id == updatedDish.CategoryId);

    if (category == null)
    {
        return Results.BadRequest("Категория не найдена");
    }

    var restaurant = data.Restaurants.FirstOrDefault(
        r => r.Id == updatedDish.RestaurantId);

    if (restaurant == null)
    {
        return Results.BadRequest("Ресторан не найден");
    }

    dish.Name = updatedDish.Name;
    dish.Weight = updatedDish.Weight;
    dish.Price = updatedDish.Price;
    dish.CategoryId = category.Id;
    dish.Category = category;
    dish.RestaurantId = restaurant.Id;
    dish.Restaurant = restaurant;

    return Results.Ok(dish);
});

app.MapDelete("/api/dishes/{id:int}", (int id, DataSeeder data) =>
{
    var dish = data.Dishes.FirstOrDefault(d => d.Id == id);

    if (dish == null)
    {
        return Results.NotFound("Блюдо не найдено");
    }

    if (data.Orders.Any(o => o.Dishes.Any(d => d.Id == id)))
    {
        return Results.Conflict(
            "Нельзя удалить блюдо, которое используется в заказах");
    }

    data.Dishes.Remove(dish);

    return Results.NoContent();
});

app.MapGet("/api/categories/{id:int}", (int id, DataSeeder data) =>
{
    var category = data.Categories.FirstOrDefault(c => c.Id == id);

    if (category == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(category);
});

app.MapPost("/api/categories", (DishCategory category, DataSeeder data) =>
{
    if (string.IsNullOrWhiteSpace(category.Name))
    {
        return Results.BadRequest("Название категории не может быть пустым");
    }

    category.Id = data.Categories
        .Select(c => c.Id)
        .DefaultIfEmpty(0)
        .Max() + 1;

    data.Categories.Add(category);

    return Results.Created($"/api/categories/{category.Id}", category);
});

app.MapPut("/api/categories/{id:int}", (int id, DishCategory updatedCategory, DataSeeder data) =>
{
    var category = data.Categories.FirstOrDefault(c => c.Id == id);

    if (category == null)
    {
        return Results.NotFound("Категория не найдена");
    }

    if (string.IsNullOrWhiteSpace(updatedCategory.Name))
    {
        return Results.BadRequest("Название категории не может быть пустым");
    }

    category.Name = updatedCategory.Name;

    return Results.Ok(category);
});

app.MapDelete("/api/categories/{id:int}", (int id, DataSeeder data) =>
{
    var category = data.Categories.FirstOrDefault(c => c.Id == id);

    if (category == null)
    {
        return Results.NotFound("Категория не найдена");
    }

    if (data.Dishes.Any(d => d.CategoryId == id))
    {
        return Results.Conflict(
            "Нельзя удалить категорию, в которой есть блюда");
    }

    data.Categories.Remove(category);

    return Results.NoContent();
});

app.MapGet("/api/orders", (DataSeeder data) =>
{
    return Results.Ok(data.Orders);
});

app.MapGet("/api/orders/{id:int}", (int id, DataSeeder data) =>
{
    var order = data.Orders.FirstOrDefault(o => o.Id == id);

    if (order == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(order);
});


app.MapPost("/api/orders", (CreateOrderRequest request, DataSeeder data) =>
{
    var client = data.Clients.FirstOrDefault(c => c.Id == request.ClientId);

    if (client == null)
    {
        return Results.BadRequest("Клиент не найден");
    }

    var restaurant = data.Restaurants.FirstOrDefault(r => r.Id == request.RestaurantId);

    if (restaurant == null)
    {
        return Results.BadRequest("Ресторан не найден");
    }

    if (request.DishIds == null || request.DishIds.Count == 0)
    {
        return Results.BadRequest("В заказе должны быть блюда");
    }

    var dishes = new List<Dish>();

    foreach (var dishId in request.DishIds)
    {
        var dish = data.Dishes.FirstOrDefault(d => d.Id == dishId);

        if (dish == null)
        {
            return Results.BadRequest($"Блюдо с ID {dishId} не найдено");
        }

        if (dish.RestaurantId != restaurant.Id)
        {
            return Results.BadRequest("Все блюда должны принадлежать выбранному ресторану");
        }

        dishes.Add(dish);
    }

    if (request.DeliveredAt < request.CreatedAt)
    {
        return Results.BadRequest("Время доставки не может быть раньше времени создания заказа");
    }

    var order = new Order
    {
        Id = data.Orders.Select(o => o.Id).DefaultIfEmpty(0).Max() + 1,
        ClientId = client.Id,
        Client = client,
        RestaurantId = restaurant.Id,
        Restaurant = restaurant,
        Dishes = dishes,
        CreatedAt = request.CreatedAt,
        DeliveredAt = request.DeliveredAt
    };

    data.Orders.Add(order);

    return Results.Created($"/api/orders/{order.Id}", order);
});

app.MapPut("/api/orders/{id:int}", (int id, CreateOrderRequest request, DataSeeder data) =>
{
    var order = data.Orders.FirstOrDefault(o => o.Id == id);

    if (order == null)
    {
        return Results.NotFound("Заказ не найден");
    }

    var client = data.Clients.FirstOrDefault(c => c.Id == request.ClientId);

    if (client == null)
    {
        return Results.BadRequest("Клиент не найден");
    }

    var restaurant = data.Restaurants.FirstOrDefault(r => r.Id == request.RestaurantId);

    if (restaurant == null)
    {
        return Results.BadRequest("Ресторан не найден");
    }

    if (request.DishIds == null || request.DishIds.Count == 0)
    {
        return Results.BadRequest("В заказе должны быть блюда");
    }

    var dishes = new List<Dish>();

    foreach (var dishId in request.DishIds)
    {
        var dish = data.Dishes.FirstOrDefault(d => d.Id == dishId);

        if (dish == null)
        {
            return Results.BadRequest($"Блюдо с ID {dishId} не найдено");
        }

        if (dish.RestaurantId != restaurant.Id)
        {
            return Results.BadRequest("Все блюда должны принадлежать выбранному ресторану");
        }

        dishes.Add(dish);
    }

    if (request.DeliveredAt < request.CreatedAt)
    {
        return Results.BadRequest("Время доставки не может быть раньше времени создания заказа");
    }

    order.ClientId = client.Id;
    order.Client = client;
    order.RestaurantId = restaurant.Id;
    order.Restaurant = restaurant;
    order.Dishes = dishes;
    order.CreatedAt = request.CreatedAt;
    order.DeliveredAt = request.DeliveredAt;

    return Results.Ok(order);
});

app.MapDelete("/api/orders/{id:int}", (int id, DataSeeder data) =>
{
    var order = data.Orders.FirstOrDefault(o => o.Id == id);

    if (order == null)
    {
        return Results.NotFound("Заказ не найден");
    }

    data.Orders.Remove(order);

    return Results.NoContent();
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
