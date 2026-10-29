using EnterpriseDevelopment.Domain;
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

app.MapGet("/api/categories/{id:int}", (int id, DataSeeder data) =>
{
    var category = data.Categories.FirstOrDefault(c => c.Id == id);

    if (category == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(category);
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

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
