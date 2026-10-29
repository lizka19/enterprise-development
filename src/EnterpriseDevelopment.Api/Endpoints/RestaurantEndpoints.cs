using EnterpriseDevelopment.Domain;
using EnterpriseDevelopment.Domain.Entities;
namespace EnterpriseDevelopment.Api.Endpoints;

public static class RestaurantEndpoints
{
    public static void MapRestaurantEndpoints(this WebApplication app)
    {
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
    }
}
